using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using otypes;
using Convert;

namespace tsObjects
{
    public enum tsObjectRef {refNone,refPoint,refPolyline,refPolygon,refFeature};

    public static class tsConvert { 
    
        public static tsObjectRef StrToRef(string key, ref string id) {

            string s = key, t;
            if (s.Length >= 2) {
                t = s.Substring(0, 2);
                if (t == "O/") {
                    s = s.Substring(2);
                    if (s.Length >= 2) {
                        t = s.Substring(0, 2);
                        if (t == "F/") {
                            id = s.Substring(2);
                            return tsObjectRef.refFeature;
                        } else
                        if (t == "S/") {
                            s = s.Substring(2);
                            if (s.Length >= 2) {
                                t = s.Substring(0, 2);
                                if (t == "P/") {
                                    id = s.Substring(2);
                                    return tsObjectRef.refPoint;
                                } else
                                if (t == "C/") {
                                    id = s.Substring(2);
                                    return tsObjectRef.refPolyline;
                                } else
                                if (t == "S/") {
                                    id = s.Substring(2);
                                    return tsObjectRef.refPolygon;
                                } 

                            }
                        }
                    }
                }
            }

            return tsObjectRef.refNone;
        }

    }

    public interface tsContentTo
    {
        bool Enabled();
        void Close();

        bool Bound(double x1, double y1, double x2, double y2);

        void pointTo(tsPoint p);
        void polylineTo(tsPolyline c);
        void polygonTo(tsPolygon c);
        void featureTo(tsFeature f);

        void stat(tsObject o);

        void writeKey(string Key, string Value);

        int GetStageCount();
        void SetStage(int value);
    }

    public class tsContentBound : tsContentTo
    {
        double fminLat,fminLon,fmaxLat,fmaxLon;
        int fcount;

        public double minLat { get { return fminLat; } }
        public double minLon { get { return fminLon; } }
        public double maxLat { get { return fmaxLat; } }
        public double maxLon { get { return fmaxLon; } }
        public int count { get { return fcount; } }

        public tsContentBound()
        {
            reset();
        }

        void reset()
        {
            fminLat = 0; fminLon = 0; fmaxLat = 0; fmaxLon = 0;
            fcount = 0;
        }

        void point(double lat, double lon)
        {
            if (fcount == 0)
            {
                fminLat = lat; fminLon = lon; fmaxLat = lat; fmaxLon = lon;
            }
            else
            {
                fminLat = Math.Min(fminLat,lat); 
                fminLon = Math.Min(fminLon,lon); 
                fmaxLat = Math.Max(fmaxLat,lat); 
                fmaxLon = Math.Max(fmaxLon,lon);
            }

            fcount++;
        }

        public bool Enabled()
        {
            return true;
        }

        public void Close() {}

        public void stat(tsObject o) {}

        public void pointTo(tsPoint p)
        {
            point(p.Lat, p.Lon);
        }

        public void polylineTo(tsPolyline c)
        {
            foreach (var p in c.internalPoints)
            point(p.lat, p.lon);
        }

        public void polygonTo(tsPolygon c) { }

        public void featureTo(tsFeature fe) {}

        public bool Bound(double b1, double l1, double b2, double l2) { return true; }

        public void writeKey(string Key, string Value) {}

        public int GetStageCount() { return 1; }
        public void SetStage(int value) {}
    }

    public class tsObject
    {
        public string Capt { get; set; }
        public string Key { get; set; }

        protected int fcount;
        public int Count { get { return fcount; } }

        protected tsContentTo fdoc = null;

        public tsContentTo doc { set { fdoc = value; } }

        public tsObject(tsContentTo Adoc)
        {
            fdoc = Adoc; fcount = 0; Key = ""; 
        }

        public virtual bool Enabled() { return false; }

        public virtual void reset(string key)
        {
            if (key == null)
                fcount = 0;
            else {
                Key = key; fcount++;
            }
        }

        public string Name() 
        {
            string s = Capt;
            if (Key != null) s += String.Format("[{0}]", Key);
            return s;
        }

        public virtual void stat()
        {
            if (fdoc != null) fdoc.stat(this);
        }

        public void log(tmessage message)
        {
            if (fcount > 0) message(null, Capt + "s: " + fcount.ToString() + ".");
        }

    }

    public class tsPoint : tsObject
    {
        double flat, flon;
        int fbits;

        public double Lat {
            get { return flat; } 
            set { flat = value; fbits |= 1; }
        }
        public double Lon {
            get { return flon; } 
            set { flon = value; fbits |= 2; }
        }

        public tsPoint(tsContentTo Adoc) : base(Adoc) 
        {
            Capt = "Point";
        }

        public void close()
        {
            if (fdoc != null) fdoc.pointTo(this);
        }

        public override void reset(string key)
        {
            base.reset(key); fbits = 0; 
        }

        public override bool Enabled() 
        { 
            return (Key != null) && fbits == 3; 
        }
    }

    public struct Point2D
    {
        public double lat;
        public double lon;

        public Point2D(double v1, double v2) {
            lat = v1; lon = v2;
        }
    }

    public class tsPolyline : tsObject
    {
        public string startPoint { get; set; }
        public string endPoint { get; set; }

        List<Point2D> finternalPoints;
        public List<Point2D> internalPoints { get { return finternalPoints; } }

        public tsPolyline(tsContentTo Adoc) : base(Adoc) 
        {
            Capt = "Polyline";
            startPoint = null; 
            endPoint = null;
            finternalPoints = new List<Point2D>();
        }

        public override void reset(string key)
        {
            base.reset(key);

            startPoint = null;
            endPoint = null;
            internalPoints.Clear();
        }

        public override bool Enabled()
        {
            return (Key != null) &&
                   (startPoint != null) &&
                   (endPoint != null);
        }

        public void close()
        {
            if (fdoc != null) fdoc.polylineTo(this);
        }

        public void internalPoint(double lat, double lon) 
        {
            internalPoints.Add( new Point2D(lat, lon) );
        }
    }

    public class tsPolygon : tsObject
    {
        List<string> frings;
        public List<string> rings { get { return frings; } }

        public tsPolygon(tsContentTo Adoc) : base(Adoc) 
        {
            Capt = "Polygon";
            frings = new List<string>();
        }

        public override void reset(string key)
        {
            base.reset(key);
            rings.Clear();
        }

        public override bool Enabled()
        {
            return (Key != null) &&
                   (frings.Count > 0);
        }

        public void close()
        {
            if (fdoc != null) fdoc.polygonTo(this);
        }

    }

    public struct Relation
    {
        public string code;
        public string dest;

        public Relation(string acode, string adest)
        {
            code = acode; dest = adest;
        }

        public void Reset() {
            code = ""; dest = "";
        }

        public bool Enabled {
            get
            {
                return (convert.IsString(code) &&
                         convert.IsString(dest));
            }
        }
    }

    public struct Geometry
    {
        public double lower;
        public double upper;
        public string key;

        public Geometry(double alower, double aupper, string akey)
        {
            lower = alower; upper = aupper; key = akey;
        }

        public void Reset() {
            lower = 1; upper = 0; key = "";
        }

        public bool Enabled {
            get
            {
                return convert.IsString(key);
            }
        }
    }

    public class tsFeature : tsObject
    {
        public string guid { get; set; }
        public string code { get; set; }
        public string attrs { get; set; }

        List<Geometry> fGeometry;
        public List<Geometry> Geometry { get { return fGeometry; } }

        List<Relation> fRelations;

        SortedList<string, int> fcodes;
        List<int> fcounts;

        public tsFeature(tsContentTo Adoc) : base(Adoc) 
        {
            Capt = "Feature";
            guid = null;
            code = null;
            attrs = null;

            fGeometry = new List<Geometry>();

            fRelations = new List<Relation>();

            fcodes = new SortedList<string, int>();
            fcounts = new List<int>();
        }

        public override void reset(string key)
        {
            base.reset(key);
            guid = null;
            code = null;
            attrs = null;
            fGeometry.Clear();
            fRelations.Clear();
        }

        public override bool Enabled()
        {
            return convert.IsString(Key) &&
                   convert.IsString(code) && 
                   (fGeometry.Count > 0);
        }

        public void geometry(Geometry g) 
        {
            fGeometry.Add(new Geometry(g.lower,g.upper,g.key));
        }

        public void relation(Relation r) 
        {
            fRelations.Add( new Relation(r.code,r.dest) ); 
        }

        public void close()
        {
            if (fdoc != null) fdoc.featureTo(this);

            if (code != null) {
                int i = fcodes.IndexOfKey(code);

                if (i < 0) {
                    fcodes.Add(code, fcounts.Count);
                    fcounts.Add(1);
                }
                else {
                    i = fcodes.Values[i];
                    fcounts[i]++;
                } 
            }
        }

        public override void stat()
        {
            base.stat();

            if (fdoc != null)
             foreach (var c in fcodes) {
                 fdoc.writeKey("\t" + c.Key, fcounts[c.Value].ToString());
             }
        }

    }
}
