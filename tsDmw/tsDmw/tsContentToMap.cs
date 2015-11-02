using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Convert;
using tsObjects;
using xmap;

namespace tsDmw
{
    class tsContentToMap : tsContentTo
    {

        const double DegToRad = Math.PI / 180;
        const double znil = -99999;

        xmap.Ixmap_auto fmap;
        xmap.IFeature obj;

        string fPath;
        int fCount;
        bool fEnabled;
        int fStage;

        public int Count { get { return fCount; } }

        public tsContentToMap(string Path)
		{
			fmap = new xmap_auto();
            obj = fmap.Feature;

            fPath = Path; fCount = 0; fStage = 0;
		}

        public bool Enabled() { return fEnabled;  }

        public void Close() 
        {
            fmap.Close();
        }

        public int GetStageCount() { return 4; }
        public void SetStage(int value) { fStage = value; }

        public bool Bound(double x1, double y1, double x2, double y2)
        {
            double b1 = Math.Truncate(x1) * DegToRad;
            x1 *= DegToRad; y1 *= DegToRad; x2 *= DegToRad; y2 *= DegToRad;

            double r = Math.Max(x2-x1,y2-y1)/16;
            r = Math.Max(r, (1 / 60 / 60) * DegToRad);

            fmap.NewMap(fPath, "s100", x1-r, y1-r, x2+r, y2+r, 9, 3, b1, 0, 0, 50000);
            fEnabled = fmap.Enabled == 1; fmap.IsWGS84 = 1;
            return fEnabled;
        }

        public void pointTo(tsPoint p) 
        {
            if (fStage == 0)
            if (fEnabled && (p.Key != null))
            {
                int id;
                fmap.New_node(p.Key, p.Lat * DegToRad, p.Lon * DegToRad, znil, out id);
            }
        }

        public void polylineTo(tsPolyline c) 
        {
            if (fStage == 1)
            if (fEnabled && c.Enabled())
            {
                obj.Reset();
                xmap.IPoly mf = obj.mf;

                List<Point2D> pp = c.internalPoints;

                if (pp != null)
                if (pp.Count > 0) {
                    foreach (var p in pp)
                    mf.AddPoint(p.lat * DegToRad, p.lon * DegToRad);
                    mf.endContour(1);
                }     

                int id;
                obj.New_edge(c.Key, c.startPoint, c.endPoint, out id);
            }

        }

        public void polygonTo(tsPolygon s) 
        {
            if (fStage == 2)
            if (fEnabled && s.Enabled())
            {
                obj.Reset();
                foreach (var r in s.rings)
                obj.Add_ref(r);

                int id;
                obj.New_path(s.Key, out id);
            }
        } 

        public void featureTo(tsFeature fe) 
        {  
            if (fStage == 3)
            if (fEnabled && fe.Enabled()) 
            {
                obj.Reset();

                if (convert.IsString(fe.attrs))
                    obj.Info = fe.attrs;

                foreach (var g in fe.Geometry)
                    obj.Add_geometry(g.lower, g.upper, g.key);

                int ptr;
                obj.end_Feature(fe.Key, fe.code, fe.guid, out ptr);

                foreach (var r in fe.Relations)
                fmap.Relation(r.code, fe.Key, r.dest);
            }
        }

        public void stat(tsObject o) { }

        public void writeKey(string Key, string Value) { }

    }
}
