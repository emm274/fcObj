using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace xfiles
{
    public static class XFiles
    {

        public static bool dialFile(ref string dir,
                                    string filter,
                                    string title,
                                    bool exists,
                                    out string path)
        {
            path = "";

            OpenFileDialog dlg = new OpenFileDialog();

            if (Directory.Exists(dir))
                dlg.InitialDirectory = dir;

            dlg.Title = title;
            dlg.Filter = filter;
            dlg.RestoreDirectory = true;
            dlg.CheckFileExists = exists;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path = dlg.FileName;
                dir = Path.GetDirectoryName(path);

                if (!exists) return true;
                return File.Exists(path);
            }

            return false;
        }

        public static int dialSaveFile(ref string dir,
                                      string filter,
                                      string title,
                                      int filterIndex,   
                                      out string path)
        {
            path = "";

            SaveFileDialog dlg = new SaveFileDialog();

            if (Directory.Exists(dir))
                dlg.InitialDirectory = dir;

            dlg.Title = title;
            dlg.Filter = filter;
            dlg.RestoreDirectory = true;
            dlg.FilterIndex = filterIndex;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path = dlg.FileName;
                dir = Path.GetDirectoryName(path);
                return dlg.FilterIndex;
            }

            return -1;
        }

    }
}
