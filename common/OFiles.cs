using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ofiles
{
    public class TTextWrite
    {

        StreamWriter file;

        string openError = "";
        string fPath = "";
        int flineCount = 0;

		public bool open(string Path)
		{
            openError=""; fPath = Path;
            flineCount = 0;

            try
            {
                file = new StreamWriter(Path);
                return true;
            }
            catch (Exception e) { 
                openError=e.Message;
                file = null;
            }

            return false;
		}

        public void close()
        {
            if (file != null) {
                file.Close();
                if (flineCount == 0)
                File.Delete(fPath);
            }

            file = null;
        }

        public void writeLine(string msg)
        {
            if (file != null) {
                file.WriteLine(msg);
                flineCount++;
            }
        }
    }

    public static class OFiles
    {
        public static string GetAppData()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%");
        }

        public static string GetIniDir()
        {
            return GetAppData() + "\\dmw\\ini\\";
        }

        public static string GetTmpDir()
        {
            return GetAppData() + "\\dmw\\tmp\\";
        }

        public static string GetTmpPath(string name)
        {
            return GetAppData() + "\\dmw\\tmp\\" + name;
        }

        public static string GetIniPath(string name)
        {
            return GetAppData() + "\\dmw\\ini\\" + name;
        }

        public static void dumpData(string dest, byte[] data)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(dest, FileMode.Create)))
            {
                bw.Write(data);
            }
        }

        public static void dumpTempData(string name, byte[] data)
        {
            dumpData(GetTmpPath(name), data);
        }

    }
}
