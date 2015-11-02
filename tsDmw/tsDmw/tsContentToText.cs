using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using tsObjects;
using Convert;

namespace tsDmw
{
    class tsContentToText : tsContentTo
    {

		StreamWriter file;
		string fPath;

        public tsContentToText(string Path)
		{
			file=new System.IO.StreamWriter(Path);
			fPath=Path;
		}

        public bool Enabled() {
            return (file != null);
        }

        public void Close()
        {
            if (file != null) file.Close();
            file = null;
        }

        public void stat(tsObject o)
        {
            if (o.Count > 0) {
                string s = String.Format("{0}s: {1}.", o.Capt, o.Count);
                file.WriteLine(s);
            }
        }

		public void writeKey(string Key,string Value)
		{
			file.WriteLine(Key+" "+Value);
		}

        public void writeKeyi(string Key, int Value)
        {
            file.WriteLine(Key + " " + Value.ToString());
        }

        string PosStr(double lat, double lon)
        {
            return String.Format("{0:0.0000000000} {1:0.0000000000}", lat, lon);
        }

        void beginObject(tsObject o, string msg)
        {
            string s = o.Name();
            if (msg != null) s += " " + msg;

            file.WriteLine("\n{");
            file.WriteLine(s);
        }

        void endObject()
        {
            file.WriteLine("}");
        }

        public bool Bound(double x1, double y1, double x2, double y2)
        {
            writeKey("extent", PosStr(x1, y1) + " " + PosStr(x2, y2));
            return true;
        }

        public void pointTo(tsPoint p)
        {
            beginObject(p,PosStr(p.Lat,p.Lon));
            endObject();
        }

        public void polylineTo(tsPolyline c)
        {
            beginObject(c,null);

            if (c.startPoint != null) writeKey("startPoint",c.startPoint);
            if (c.endPoint != null) writeKey("endPoint", c.endPoint);

            List<Point2D> pp = c.internalPoints;

            if (pp != null)
            if (pp.Count > 0)
            {
                writeKeyi("internalPoints", pp.Count);
                foreach (var p in pp)
                file.WriteLine(PosStr(p.lat,p.lon));
            }

            endObject();
        }

        public void polygonTo(tsPolygon s) 
        {
            beginObject(s,s.rings.Count.ToString());
            foreach(var c in s.rings) file.WriteLine(c);
            endObject();
        }

        public void featureTo(tsFeature fe)
        {
            beginObject(fe, null);

            if (convert.IsString(fe.guid)) writeKey("guid",fe.guid);
            if (convert.IsString(fe.code)) writeKey("code", fe.code);
            if (convert.IsString(fe.attrs)) writeKey("attrs", fe.attrs);

            if (fe.Geometry.Count > 0) {
                writeKeyi("geometry", fe.Geometry.Count);
                foreach (var g in fe.Geometry)
                file.WriteLine(g.key);
            }

            if (fe.Relations.Count > 0)
            {
                writeKeyi("relations", fe.Relations.Count);
                foreach (var r in fe.Relations)
                {
                    string s = String.Format("{0} {1} {2}",r.role,r.code,r.dest);
                    file.WriteLine(s);
                }
            }

            endObject();
        }

        public int GetStageCount() { return 1; }
        public void SetStage(int value) { }

    }
}
