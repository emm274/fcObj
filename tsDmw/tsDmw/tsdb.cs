using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using otypes;
using ofiles;
using xjson;
using tsContent;

namespace tsDmw
{
    public delegate void tnotify(object sender, EventArgs e);
    public delegate void tnotifyTask(object sender, tnotify cb);
    
    class tsdb: XJson {

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

        List<string> fCommits;
        public List<string> Commits { get { return fCommits; } }

        List<string> fCodes = new List<string>();
        TTextWrite fdump = new TTextWrite();

        enum tquery { getVersion,
                      getBranches,
                      getCommits,
                      getCodes
                    }

        tquery fquery;

        delegate void tvalue(string k, int index, JsonToken typ, Object v);

        Stack<string> fkeys = new Stack<string>();

        string fdest;

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
            Login = "oosdb";
            Password = "chylm7UpBoucwoPin";
            fVersion = "";
            fBranches = new List<string>();
            fCommits = new List<string>();
        }

        protected override void value(string k, int index, JsonToken typ, Object v)
        {
            string s = v.ToString();

            switch (fquery)
            {
                case tquery.getVersion:
                    if (k == "API version")
                    fVersion = s; 
                    break;

                case tquery.getBranches:
                    if (k == "data")
                    fBranches.Add(s);
                    break;

                case tquery.getCommits:
                    if (k == "message") 
                    fCommits.Add(s);
                    break;

                case tquery.getCodes:
                    if (k == "code") 
                    if (fCodes.IndexOf(s) < 0) {
                        fCodes.Add(s);
                        fdump.writeLine(s);
                    }
                    break;
            }
        }

        public void getVersion(tnotify Anotify)
        {
            __beginTask("get version");
            Task.Run(() => __request("", tquery.getVersion, Anotify));
        }

        public void getBranches(tnotify Anotify)
        {
            fBranches.Clear(); __beginTask("get branches");
            Task.Run(() => __request("branches", tquery.getBranches, Anotify));
        }

        public void getCommits(string name, tnotify Anotify)
        {
            fCommits.Clear(); __beginTask("get commits");
            Task.Run(() => __request("branch/" + name + "/log", tquery.getCommits, Anotify));
        }

        public void getContent(string name, string dest)
        {
            File.Delete(dest);

            __beginTask("get content: "+name);
            Task.Run(() => task_requestData("branch/" + name + "/content", dest) );
        }

        public void stat_codes(string path)
        {
            string s = Path.ChangeExtension(path, ".log");
            if (fdump.open(s)) {
                string t = Path.GetFileName(path);
                fdump.writeLine("file: "+t);

                fCodes.Clear();
                StreamReader stream = new StreamReader(path);

                __beginTask(t);

                Task.Run(() => task_json(stream,tquery.getCodes) );
                fdump.close();
            }
        }

        public void json_to_text(string path, string dest)
        {
            __beginTask(Path.GetFileName(dest));
            Task.Run(() => task_json_to_text(path,dest));
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
            Task.Run(() => task_json_to_map(path, dest));
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

        void __request(string what, tquery query, tnotify notify)
        {
            WebRequest request = WebRequest.Create(Host + "/" + what);
            request.Credentials = new NetworkCredential(Login, Password);

            try
            {
                WebResponse response = request.GetResponse();

                StreamReader stream = new StreamReader(
                        response.GetResponseStream(), Encoding.UTF8);

                fquery = query; Parse(stream);

                if (notify != null)

                    if (fnotifyTask != null)
                        fnotifyTask(this, notify);
                    else
                        notify(this, null);

            }
            catch (Exception ex)
            {
                __error("http get", ex.Message);
            }

            __endTask(this, null);
        }

        bool __requestData(string what, string dest)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Login, Password);
            try
            {
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                client.DownloadFile(Host + "/" + what, dest);
                return true;
            }
            catch (Exception ex)
            {
                __error("http get", ex.Message);
            }

            return false;
        }

        void __requestAsync(string what, string dest)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Login, Password);
            Uri uri = new Uri(Host + "/" + what);
            try
            {
                fdest = dest;
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedCallback);
                client.DownloadFileAsync(uri, dest);
            }
            catch (Exception ex)
            {
                __error("http get", ex.Message);
            }

        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            string s = String.Format("{0}%...",e.ProgressPercentage);
            fProgress(this,s);
        }

        private void DownloadCompletedCallback(object sender, AsyncCompletedEventArgs e) 
        {
            __endTask(this, Path.GetFileName(fdest) + ".");
        }

        void task_requestData(string what, string dest)
        {
            if (fProgress != null)
                __requestAsync(what, dest);
            else {
                __requestData(what, dest);
                __endTask(this, Path.GetFileName(dest) + ".");
            }
        }
    }
}
