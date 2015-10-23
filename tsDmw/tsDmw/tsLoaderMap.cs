using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using Convert;
using classMsg;
using xmap;

namespace tsDmw
{
    class tsLoaderMap : ClassMsg
    {
        const double RadToDeg = 180 / Math.PI;
        const double eps = 1e8;

        const string vcTag = "O/S/P/";
        const string veTag = "O/S/C/";
        const string ssTag = "O/S/S/";
        const string feTag = "O/F/";

        xmap.Ixmap_auto fmap;
        xmap.IFeature fobj;

        JsonTextWriter fwriter;

        bool fIsUpdate;

        int fcount;

        List<string> fParts = new List<string>();
        List<string> fGeometry = new List<string>();

        public tsLoaderMap()
        {
            fmap = new xmap_auto();
            fobj = fmap.Feature;
        }

        void _message(string msg) {
            __message("LoaderMap",msg);
        }

        struct point2d {
            public double x;
            public double y;

            public point2d(double Ax, double Ay) { x = Ax; y = Ay; }
        }

        point2d Point2d(xmap.IPoly mf, int part, int index)
        {
            double x, y; mf.GetPointd(part, index, out x, out y);
            return new point2d(x*RadToDeg, y*RadToDeg);
        }

        void writeValuef(string key, double v)
        {
            fwriter.WritePropertyName(key);
            fwriter.WriteValue(v);
        }

        void writeValues(string key, string v)
        {
            fwriter.WritePropertyName(key);
            fwriter.WriteValue(v);
        }

        void writeNullObject(string key)
        {
            fwriter.WritePropertyName(key);
            fwriter.WriteStartObject();
            fwriter.WriteEndObject();
        }

        double mRound(double v)
        {
            v=Math.Round(v*eps);
            return v / eps;
        }

        void writePoint(point2d p)
        {
            fwriter.WriteStartObject();
            writeValuef("lat", mRound(p.x));
            writeValuef("lon", mRound(p.y));
            fwriter.WriteEndObject();
        }

        string node(point2d p, string id)
        {
            string key = vcTag + id;
            fwriter.WritePropertyName(key);
            fwriter.WriteStartArray();
            writePoint(p);
            fwriter.WriteEndArray();
            return key;
        }

        string edge(xmap.IPoly mf, int part, string id)
        {
            int count;
            mf.GetContourCount(part, out count);

            point2d a = Point2d(mf, part, 0);
            point2d b = Point2d(mf, part, count-1);

            string n1,n2;

            if ((a.x == b.x) && (a.y == b.y))
            {
                n1 = node(a, id); n2 = n1;
            }
            else {
                n1 = node(a, id + "_1");
                n2 = node(b, id + "_2"); 
            }

            string key = veTag + id;
            fwriter.WritePropertyName(key);
            fwriter.WriteStartArray();
            fwriter.WriteStartObject();

            writeValues("interpolationType", "loxodromic");
            writeValues("startPoint", n1);
            writeValues("endPoint", n2);

            if (count > 2) {
                fwriter.WritePropertyName("internalPoints");
                fwriter.WriteStartArray();

                for (int i = 1; i < count - 1; i++)
                writePoint( Point2d(mf,part,i) );

                fwriter.WriteEndArray();
            }

            fwriter.WriteEndObject();
            fwriter.WriteEndArray();
            return key;
        }

        string surface(xmap.IPoly mf, string id)
        {
            fParts.Clear();
            int Nparts = mf.PartCount;
            for (int i = 0; i < Nparts; i++) {
                string s = id;
                if (Nparts > 1) s+="_" + i.ToString();
                fParts.Add( edge(mf,i,s) );
            }

            string key = ssTag + id;
            fwriter.WritePropertyName(key);
            fwriter.WriteStartArray();
            fwriter.WriteStartObject();

            fwriter.WritePropertyName("rings");
            fwriter.WriteStartArray();

            for (int i = 0; i < mf.PartCount; i++)
            {
                fwriter.WriteStartObject();

                string s="forward";
                writeValues("orientation", s);

                if (i == 0) s = "exterior";
                else s = "interior";

                writeValues("exteriorInterior", s);

                writeValues("ref", fParts[i]);

                fwriter.WriteEndObject();
            }

            fwriter.WriteEndArray();

            fwriter.WriteEndObject();
            fwriter.WriteEndArray();

            return key;
        }

        string shape(int loc,string key)
        {
            xmap.IPoly mf = fobj.mf;
            mf.IsWGS = 1;

            if (mf.PartCount > 0)
            switch (loc)
            {
                case 1:
                    return node(Point2d(mf, 0, 0), key);
                case 2:
                    return edge(mf, 0, key);
                case 3:
                    return surface(mf, key);
            }

            return null;
        }

        void objects()
        {
            ulong pos = fmap.Position;

            // objects
            if (fmap.Goto_down > 0) do
                {
                    ulong pos1 = fmap.Position;

                    fobj.Get();
                    int offset = fobj.Offset;
                    string acronym = fobj.Acronym;
                    string guid = fobj.guid;
                    int code = fobj.Code, 
                    loc = fobj.Loc;

                    int uc = 0;    // add feature
                    if (fIsUpdate) uc = fobj.UpdateCode;

                    if (convert.IsString(acronym))
                    if (convert.IsString(guid))
                    if ((loc >= 1) && (loc <= 3)) {

                        fGeometry.Clear();
                        string s = shape(loc, guid);
                        if (convert.IsString(s))
                        fGeometry.Add(s);

                        fwriter.WritePropertyName(feTag+guid);
                        fwriter.WriteStartArray();
                        fwriter.WriteStartObject();

                        writeValues("code", acronym);
                        writeValues("globalId", guid);

                        string info = fobj.Info;
                        if (convert.IsString(info))
                        {
                            fwriter.WritePropertyName("attributes");
                            fwriter.WriteRawValue(info);
                        }
                        else writeNullObject("attributes");

                        if (fGeometry.Count > 0)
                        {
                            fwriter.WritePropertyName("geometry");
                            fwriter.WriteStartArray();

                            foreach (var g in fGeometry) {
                                fwriter.WriteStartObject();
                                writeValues("ref", g);
                                writeNullObject("scaleRange");
                                fwriter.WriteEndObject();
                            }

                            fwriter.WriteEndArray();
                        }

                        fwriter.WriteEndObject();
                        fwriter.WriteEndArray();

                        fcount++;
                    }

                    fmap.Position = pos1;
                } while (fmap.Goto_right > 0);

            fmap.Position = pos;
        }

        void layers()
        {
            if (fmap.Goto_down > 0)
            do { objects();
            } while (fmap.Goto_right > 0);
        }

        public void workDir(string s)
        {
            if (s != null)
                fmap.WorkDir = s;
        }

        public bool exec(string path, string dest, string branch, string comment, bool IsUpdate)
        {
            fmap.OpenMap(path); fIsUpdate = IsUpdate;

            if (fmap.Enabled != 1)
                _message(String.Format("open map '{0}' false.", path));
            else {
                fmap.IsWGS84 = 1;
                double x1, y1, x2, y2;
                int pps = fmap.get_Bound(out x1, out y1, out x2, out y2);
                if (pps != 1)
                    _message("expected world coordinate system.");
                else 
                if (fmap.Goto_root == 0)
                    _message("Goto_root == 0");
                else {
                    StreamWriter sw = File.CreateText(dest);
                    if (sw == null)
                        _message(String.Format("create file '{0}' false.", dest));
                    else
                    {
                        fwriter = new JsonTextWriter(sw);
                        fwriter.Formatting = Formatting.Indented;

                        fwriter.WriteStartObject();

                        writeValues("branch",branch);
                        writeValues("message",comment);

                        fwriter.WritePropertyName("patch");
                        fwriter.WriteStartObject();

                        fcount = 0; layers();

                        fwriter.WriteEndObject();

                        fwriter.WriteEndObject();
                        fwriter.Close();
                        sw.Close();

                        if (fcount > 0) { 

                            if (fIsUpdate)
                                __message(null,String.Format("Update {0} objects.", fcount));
                            else
                                __message(null,String.Format("Add {0} objects.", fcount));

                            fmap.Close(); return true;
                        }
                    }
                }

                fmap.Close();
            }

            return false;
        }
    }
}
