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
                file = new System.IO.StreamWriter(Path);
                return true;
            }
            catch (Exception ex) { 
                openError=ex.Message;
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
}
