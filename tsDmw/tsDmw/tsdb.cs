using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Diagnostics;

using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using otypes;
using Convert;
using ofiles;
using xjson;
using tsContent;
using xmap;

namespace tsDmw
{
    public delegate void tnotify(object sender, EventArgs e);
    public delegate void tnotifyTask(object sender, tnotify cb);
    
    class tsdb: XJson {

        const int bufferSize = 0x4000;

        string Host = "http://oosdb-dev-v1.krontech.org/api";

        public string Login { get; set; }
        public string Password { get; set; }

        tmessage fbeginTask;
        public tmessage beginTask { set { fbeginTask = value; } }

        tmessage fendTask;
        public tmessage endTask { set { fendTask = value; } }

        tnotifyTask fnotifyTask = null;
        public tnotifyTask notifyTask { set { fnotifyTask = value; } }

        tmessage fProgress = null;
        public tmessage Progress { set { fProgress = value; } }

        string fVersion;
        public string version { get { return fVersion; } }

        List<string> fBranches;
        public List<string> branches { get { return fBranches; } }

        Dictionary<string,string> fCommits;
        public Dictionary<string,string> Commits { get { return fCommits; } }

        enum tquery { error,
                      getVersion,
                      getBranches,
                      getCommits,
                      post,
                      delete
                    }

        tquery fquery;

        string fdata;
        string fdest;

        string fcommitMsg = null;
        string fcommitID = null;

        Task task = null;

        void __error(string capt, string message)
        {
            __message(capt,message);
        }

        void __beginTask(string msg)
        {
            if (fbeginTask != null)
                fbeginTask(this, msg+"...");
        }

        void __endTask(object sender, string msg)
        {
            if (fendTask != null)
            fendTask(sender,msg);
        }

        void __parse_error(string capt)
        {
            __error(capt, "parse error");
        }

        public tsdb() {
            Login = "";
            Password = "";
            fVersion = "";
            fBranches = new List<string>();
            fCommits = new Dictionary<string,string>();
        }

        public void waitTask()
        {
            if (task != null) Task.WaitAny(task);
        }

        protected override void value(string k, int index, JsonToken typ, Object v)
        {
            string s = v.ToString();

            switch (fquery)
            {
                case tquery.error:
                    __message(null, s);
                    break;

                case tquery.getVersion:
                    if (k == "API version")
                    fVersion = s; 
                    break;

                case tquery.getBranches:
                    if (k == "data")
                    fBranches.Add(s);
                    break;

                case tquery.getCommits:
                    if (k == "message") fcommitMsg = s;
                    else
                    if (k == "commit") fcommitID = s;
                    break;

                case tquery.post:
                    if (v != null) 
                    __message(k,v.ToString());
                    break;
            }
        }

        protected override void endObject(string k)
        {
            if (fquery == tquery.getCommits)
            {
                if (convert.IsString(fcommitID))
                if (convert.IsString(fcommitMsg))
                fCommits.Add(fcommitID, fcommitMsg);
            }
        }

        public void getVersion(tnotify notify)
        {
            __beginTask("get version");
            task = Task.Run(() => __get("", tquery.getVersion, notify));
        }

        public void getBranches(tnotify notify)
        {
            fBranches.Clear(); __beginTask("get branches");
            task = Task.Run(() => __get("branches", tquery.getBranches, notify));
        }

        public void getCommits(string name, tnotify notify)
        {
            fCommits.Clear(); __beginTask("get commits");
            task = Task.Run(() => __get("branch/" + name + "/log", tquery.getCommits, notify));
        }

        public void getContent(string name, string dest)
        {
            File.Delete(dest); fdest = null;

            string path = dest;
            if (Path.GetExtension(dest) != "json")
            {
                fdest = dest;
                path = Path.ChangeExtension(dest, ".json");
            }

            __beginTask("get content: "+name);
            task = Task.Run(() => __download("branch/" + name + "/content", path));
        }

        public void commit(string name, string path, tnotify notify)
        {
            __beginTask("commit: " + name);
            task = Task.Run(() => __postFile("/commit", path, tquery.post, notify));
        }

        public void undo_commit(string branch, string commit, tnotify notify)
        {
            XJsonStringWriter sw = new XJsonStringWriter();
            JsonWriter writer = sw.writer;
            writer.WriteStartObject();
            writer.WritePropertyName("commit");
            writer.WriteValue(commit);
            writer.WriteEndObject();
            writer.Close();

            byte[] data = sw.Data();
            if (data != null)
            {
                string tmp = OFiles.GetTmpPath("send");
                OFiles.dumpData(tmp, data);

                __beginTask(String.Format("{0}: undo commit {1}", branch, commit));

                string what = "/branch/"+branch+"?force=true";

                task = Task.Run(() => xmap_put(what, tmp, tquery.delete, notify));

//              task = Task.Run(() => __sendData(what, "PUT", data, null, tquery.delete, notify));
            }
        }

        public void json_to_text(string path, string dest)
        {
            __beginTask(Path.GetFileName(dest));
            task = Task.Run(() => task_json_to_text(path,dest));
        }

        void task_json_to_text(string path, string dest)
        {
            tsContentToText text = new tsContentToText(dest);
            if (text.Enabled())
            {
                jsonContent json = new jsonContent(null,text);
                json.ParseFile(path,message);
                text.Close();
            }

            __endTask(this, Path.GetFileName(dest)+".");
        }

        public void json_to_map(string path, string dest)
        {
            __beginTask(Path.GetFileName(dest));
            task = Task.Run(() => task_json_to_map(path, dest));
        }

        void task_json_to_map(string path, string dest)
        {
            tsContentToMap map = new tsContentToMap(dest);

            jsonContent json = new jsonContent(null, map);
            json.ParseFile(path, message);
            map.Close();

            string msg = null;
            if (map.Count > 0)
            msg = Path.GetFileName(dest) + ".";

            __endTask(this,msg);
        }

        void task_json(StreamReader stream, tquery query)
        {
            fquery = query; Parse(stream);
            __endTask(this, null);
        }

        bool response_json(WebResponse response, tquery query) 
        {
            Stream stream = response.GetResponseStream();

            if (stream != null) { 
                StreamReader reader = new StreamReader(stream,Encoding.UTF8);
                fquery = query; Parse(reader); return true;
            }

            return false;
        }

        bool error_json(WebException e)
        {
            WebResponse response = e.Response;
            if (response != null) {
                bool rc = response_json(response, tquery.error);
                response.Close(); return rc;
            }

            return false;
        }

        void web_error(WebException e, string capt)
        {
            if (!error_json(e))
            __error(capt, e.Message);
        }

        void request_notify(tnotify notify)
        {
            if (notify != null)

                if (fnotifyTask != null)
                    fnotifyTask(this, notify);
                else
                    notify(this, null);
        }

        void request_json(WebRequest request, tquery query, tnotify notify)
        {
            try
            {
                WebResponse response = request.GetResponse();
                response_json(response, query);
                response.Close();

                request_notify(notify);
            }
            catch (WebException e)
            {
                web_error(e,request.Method);
            }
        }

        void __request(string what, string method, tquery query, tnotify notify)
        {
            WebRequest request = WebRequest.Create(Host + "/" + what);
            request.Credentials = new NetworkCredential(Login, Password);
            request.Method = method;
            request_json(request, query, notify);
            __endTask(this, null);
        }

        void __get(string what, tquery query, tnotify notify)
        {
            __request(what, "GET", query, notify);
        }

        void __sendData(string what, string method, byte[] data, string path, tquery query, tnotify notify)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Host + "/" + what);
            request.Credentials = new NetworkCredential(Login, Password);
            request.Method = method;
            request.Timeout = 1000 * 60 * 5;    // 5 min

            request.Accept = "*/*";
            request.ContentType = "application/json";

            if (data != null)
            {
                int cx = data.Length;
                request.ContentLength = cx;

                request.ServicePoint.ConnectionLimit = 1;
                request.ServicePoint.Expect100Continue = true; 

                Stream stream = request.GetRequestStream();

                if (cx < bufferSize)
                    stream.Write(data, 0, cx);
                else
                {
                    int bx = 0;
                    while (bx < cx)
                    {
                        int dx = cx - bx;
                        if (dx > bufferSize) dx = bufferSize;

                        stream.Write(data, bx, dx);
                        stream.Flush();

                        bx += dx;
                    }
                }
                
                stream.Close();
            }
            else            
            {
                byte[] buf = new byte[bufferSize];

                using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    int cx = (int)br.BaseStream.Length;
                    request.ContentLength = cx;
                    Stream stream = request.GetRequestStream();

                    int bx = 0; 
                    while (bx < cx)
                    {
                        int dx = cx - bx;
                        if (dx > bufferSize) dx = bufferSize;
                        br.Read(buf, 0, dx);
                        stream.Write(buf, 0, dx);
                        bx += dx;
                    }

                    stream.Close();
                }
            }

            if (request.ContentLength > 0)
            request_json(request, query, notify);

            __endTask(this, null);
        }

        void curl_post(string path, tquery query, tnotify notify)
        {
            string _auth = Login+":"+Password;
            string _enc = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(_auth));

            string dir = Path.GetDirectoryName(path); 
            string json = Path.GetFileName(path);

            string bin = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(dir);

            string arguments = "-s" +
                               " -H \"Content-Type: application/json\"" +
                               " -H \"Authorization: Basic " + _enc + "\"" +
                               " -X POST --data-binary @" + json +
                               " " + Host + "/" + "commit";

            __message("curl.exe", "commit...");

            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = bin + "\\curl.exe";
                proc.StartInfo.Arguments = arguments;

                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.CreateNoWindow = true;

                proc.Start();

                string ack = dir + "\\response";
                if (File.Exists(ack)) File.Delete(ack);

                using (StreamWriter sw = new StreamWriter(ack))
                {

                    while (!proc.StandardOutput.EndOfStream)
                    {
                        string line = proc.StandardOutput.ReadLine();
                        sw.WriteLine(line);
                    }

                    sw.Close();
                }

                if (File.Exists(ack))
                using (StreamReader reader = new StreamReader(ack))
                {
                    fquery = query; Parse(reader);
                    request_notify(notify);
                }
            }
            catch (Exception e)
            {
                __message("curl", e.Message);
            }

            Directory.SetCurrentDirectory(bin);
        }

        void xmap_post(string path, tquery query, tnotify notify)
        {
            xmap.Ixmap_auto fmap = new xmap_auto();

            string response = Path.GetDirectoryName(path) + "\\response";
            if (File.Exists(response)) File.Delete(response);

            int rc;
            fmap.HttpPost(Host + "/" + "commit", Login, Password, 
                          "application/json", path, response, out rc);

             if (rc == 1)
             if (File.Exists(response))
             using (StreamReader reader = new StreamReader(response))
             {
                 fquery = query; Parse(reader);
                 request_notify(notify);
             }
        }

        void xmap_put(string what, string path, tquery query, tnotify notify)
        {
            xmap.Ixmap_auto fmap = new xmap_auto();

            string response = Path.GetDirectoryName(path) + "\\response";
            if (File.Exists(response)) File.Delete(response);

            int rc;
            fmap.HttpPut(Host + "/" + what, Login, Password,
                         "application/json", path, response, out rc);

            if (rc == 1)
            if (File.Exists(response))
            using (StreamReader reader = new StreamReader(response))
            {
                fquery = query; Parse(reader);
                request_notify(notify);
            }
        }

        void __postJson(string what, string data, tquery query, tnotify notify)
        {
            __sendData(what,"POST", convert.GetBytes(data),null, query, notify);
        }

        void __postFile(string what, string path, tquery query, tnotify notify)
        {
            FileInfo inf = new FileInfo(path);
            if (inf.Length >= 0xffff)
                xmap_post(path,query,notify);
            else 
                __sendData(what,"POST",  File.ReadAllBytes(path), null, query, notify);
        }

        bool __download(string what, string path)
        {
            fdata = path;

            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Login, Password);
            Uri uri = new Uri(Host + "/" + what);
            try
            {
                if (fProgress == null)
                {
                    client.DownloadFile(uri, path);
                    __endTask(this, Path.GetFileName(path) + ".");
                }
                else
                {
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedCallback);
                    client.DownloadFileAsync(uri, path);
                }

                return true;
            }
            catch (WebException e)
            {
                web_error(e,"download");
            }

            return false;
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            string s = String.Format("{0}%...", e.ProgressPercentage);
            fProgress(this, s);
        }

        private void DownloadCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string ext = "";
            if (convert.IsString(fdest))
            ext = Path.GetExtension(fdest).ToLower();

            if (ext == ".txt") {
                __beginTask(Path.GetFileName(fdest));
                task_json_to_text(fdata, fdest);
            } else

            if (ext == ".dm")
            {
                __beginTask(Path.GetFileName(fdest));
                task_json_to_map(fdata, fdest);
            }
            else
                __endTask(this, Path.GetFileName(fdata) + ".");
        }

        bool __upload(string what, string path)
        {
            fdata = path;

            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Login, Password);
            Uri uri = new Uri(Host + "/" + what);

            client.Headers.Add("ContentType","application/json");

            try
            {
                if (fProgress == null)
                {
                    byte[] rc = client.UploadFile(uri, path);

                    if (rc != null)
                    {
                        string s = convert.GetString(rc);
                        __message(null,s);
                    }

                    __endTask(this, Path.GetFileName(path) + ".");
                }
                else
                {
                    client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                    client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadCompletedCallback);
                    client.UploadFileAsync(uri, path);
                }

                return true;
            }
            catch (WebException e)
            {
                web_error(e,"upload");
            }

            return false;
        }

        private void UploadProgressCallback(object sender, UploadProgressChangedEventArgs e)
        {
            string s = String.Format("{0}%...", e.ProgressPercentage);
            fProgress(this, s);
        }

        private void UploadCompletedCallback(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Cancelled)
                __message("upload", "cancelled");
            else
            if (e.Error != null)
            {
                if (e.Error is WebException)
                    web_error(e.Error as WebException, "upload");
                else
                    __message("upload", e.Error.Message);
            }
            else
                if (e.Result != null)
                    __message(null, Encoding.UTF8.GetString(e.Result));

            __endTask(this, Path.GetFileName(fdata) + ".");
        }

    }
}
