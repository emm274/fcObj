using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using classMsg;
using xmap;

namespace tsDmw
{
    class tsLoaderMap : ClassMsg
    {
        const double RadToDeg = 180 / Math.PI;

        xmap.Ixmap_auto fmap;
        xmap.IFeature obj;

        tsdb fdb;
        bool IsUpdate;

        public tsLoaderMap()
        {
            fmap = new xmap_auto();
            obj = fmap.Feature;
        }

        public void workDir(string s)
        {
            if (s != null)
                fmap.WorkDir = s;
        }

        public void exec(string path, tsdb Adb, bool AIsUpdate)
        {
            fdb = Adb; IsUpdate = AIsUpdate; 
        }
    }
}
