using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using Convert;
using classMsg;
using ofiles;
using xmap;
using tsObjects;

namespace tsDmw
{
    class tsLoaderMap : ClassMsg
    {
        const double RadToDeg = 180 / Math.PI;
        const int dec = 7;

        const string vcTag = "O/S/P/";
        const string veTag = "O/S/C/";
        const string ssTag = "O/S/S/";
        const string feTag = "O/F/";

        xmap.Ixmap_auto fmap;
        xmap.IFeature fobj, fobj1;

        xmap.Ixmap_auto fbase_map;
        xmap.IFeature fbase_obj, fbase_obj1;
        bool fIsUpdate;

        JsonTextWriter fwriter;

        int fadded, fupdated, fdeleted;

        List<Geometry> fGeometry = new List<Geometry>();

        TTextWrite ferr = new TTextWrite();

        struct point2d
        {
            public double x;
            public double y;

            public point2d(double Ax, double Ay) { x = Ax; y = Ay; }
        }

        public tsLoaderMap()
        {
            fmap = new xmap_auto();
            fobj = fmap.Feature;
            fobj1 = fmap.Feature;

            fbase_map = new xmap_auto();
            fbase_obj = fbase_map.Feature;
            fbase_obj1 = fbase_map.Feature;
        }

        void _message(string msg) {
            __message("LoaderMap",msg);
        }

        void Error(string msg)
        {
            ferr.writeLine(msg);
        }

        void Error1(int offset, string msg)
        {
            string s = String.Format("offset={0}: {1}", offset, msg);
            ferr.writeLine(s); 
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
            return Math.Round(v, dec);
        }


        bool PointEquals(point2d p1, point2d p2)
        {
            return (mRound(p1.x) == mRound(p2.x)) &&
                   (mRound(p1.y) == mRound(p2.y)); 
        }

        bool PartEquals(xmap.IPoly mf1, xmap.IPoly mf2, int part)
        {
            int n;
            mf1.GetContourCount(part, out n);

            for (int i = 1; i < n - 1; i++)
            {
                bool rc = PointEquals( Point2d(mf1, part, i),
                                       Point2d(mf2, part, i) );
                if (!rc) return false;

            }

            return true;
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

        bool move_node(point2d p1, point2d p2, string key)
        {
            if (!PointEquals(p1, p2))
            {
                fwriter.WritePropertyName(vcTag + key);
                fwriter.WriteStartArray();

                writePoint(p1);
                writePoint(p2);
                fwriter.WriteEndArray();

                return true;
            }

            return false;
        }

        bool edgeLocked(xmap.IPoly mf, int part)
        {
            int n;
            mf.GetContourCount(part, out n);

            if (n >= 2)
            {
                point2d a = Point2d(mf, part, 0);
                point2d b = Point2d(mf, part, n - 1);

                return ((a.x == b.x) && (a.y == b.y));
            }

            return false;
        }

        void writeEdge(xmap.IPoly mf, int part, bool locked, string id)
        {
            string n1 = vcTag + id;
            string n2 = n1;
            if (!locked) {
                n1 += "_1";
                n2 += "_2";
            }

            fwriter.WriteStartObject();

            writeValues("interpolationType", "loxodromic");
            writeValues("startPoint", n1);
            writeValues("endPoint", n2);

            int n;
            mf.GetContourCount(part, out n);

            if (n > 2)
            {
                fwriter.WritePropertyName("internalPoints");
                fwriter.WriteStartArray();

                for (int i = 1; i < n - 1; i++)
                    writePoint(Point2d(mf, part, i));

                fwriter.WriteEndArray();
            }

            fwriter.WriteEndObject();
        }

        string edge(xmap.IPoly mf, int part, string id)
        {
            int n;
            mf.GetContourCount(part, out n);

            if (n > 0) {

                bool locked = edgeLocked(mf,part);

                point2d a = Point2d(mf, part, 0);
                point2d b = Point2d(mf, part, n - 1);

                if (locked)
                    node(a, id); 
                else {
                    node(a, id + "_1");
                    node(b, id + "_2");
                }

                string key = veTag + id;
                fwriter.WritePropertyName(key);
                fwriter.WriteStartArray();

                writeEdge(mf, part, locked, id);

                fwriter.WriteEndArray();

                return key;
            }

            return "";
        }

        bool move_edge(xmap.IPoly mf1,
                       xmap.IPoly mf2,
                       int part, string id)
        {
            bool rc = false, rc1 = false;

            int n1; 
            mf1.GetContourCount(part, out n1);

            int n2;
            mf2.GetContourCount(part, out n2);

            if (n1 * n2 > 0)
            {
                bool locked1 = edgeLocked(mf1, part);
                bool locked2 = edgeLocked(mf2, part);

                point2d a1 = Point2d(mf1, part, 0);
                point2d b1 = Point2d(mf1, part, n1 - 1);

                point2d a2 = Point2d(mf2, part, 0);
                point2d b2 = Point2d(mf2, part, n2 - 1);

                string id1 = id, id2 = id;
                if (!locked2) {
                    id1 += "_1";
                    id2 += "_2";
                }

                if (locked1 == locked2)
                {
                    if (!PointEquals(a1, a2)) rc = move_node(a1, a2, id1);
                    if (!PointEquals(b1, b2)) rc = move_node(b1, b2, id2);
                }
                else
                {
                    if (locked2)
                        node(a2, id);
                    else
                    {
                        node(a2, id + "_1");
                        node(b2, id + "_2");
                    }

                    rc = true; rc1 = true;
                }

                if (!rc1 && (n1 == n2))
                rc1 = !PartEquals(mf1, mf2, part);

                if (rc1)
                {
                    fwriter.WritePropertyName(veTag + id);
                    fwriter.WriteStartArray();

                    writeEdge(mf1, part, locked1, id);
                    writeEdge(mf2, part, locked2, id);

                    fwriter.WriteEndArray();
                    rc = true;
                }
            }

            return rc;
        }

        string part_key(string key, int i)
        {
            int i1 = i + 1;
            return key + "_" + i1.ToString();
        }

        void write_surface(xmap.IPoly mf, string key)
        {
            fwriter.WriteStartObject();

            fwriter.WritePropertyName("rings");
            fwriter.WriteStartArray();

            for (int i = 0; i < mf.PartCount; i++)
            {
                fwriter.WriteStartObject();

                string s = "forward";
                writeValues("orientation", s);

                if (i == 0) s = "exterior";
                else s = "interior";

                writeValues("exteriorInterior", s);

                writeValues("ref", veTag + part_key(key, i));

                fwriter.WriteEndObject();
            }

            fwriter.WriteEndArray();

            fwriter.WriteEndObject();
        }

        string surface(xmap.IPoly mf, string id)
        {
            int Nparts = mf.PartCount;
            for (int i = 0; i < Nparts; i++)
            edge(mf,i, part_key(id,i));

            string key = ssTag + id;
            fwriter.WritePropertyName(key);
            fwriter.WriteStartArray();

            write_surface(mf, id);

            fwriter.WriteEndArray();

            return key;
        }

        bool move_surface(xmap.IPoly mf1, xmap.IPoly mf2, string id)
        {
            bool rc = false;

            int n1 = mf1.PartCount;
            int n2 = mf2.PartCount;
            int n = Math.Max(n1, n2);

            for (int i = 0; i < n; i++)
            {
                string s = part_key(id,i);

                if (i < n2)
                    rc |= move_edge(mf1, mf2, i, s);
                else 
                    edge(mf2,i,s);
            }

            if (n1 != n2)
            {
                string key = ssTag + id;
                fwriter.WritePropertyName(key);
                fwriter.WriteStartArray();

                write_surface(mf1, id);
                write_surface(mf2, id);

                fwriter.WriteEndArray();

                rc = true;
            }

            return rc;
        }

        string shape(xmap.IFeature obj)
        {
            int loc = obj.Loc % 100;

            string key = obj.guid;
            if (convert.IsString(key))
            {
                xmap.IPoly mf = obj.mf; mf.IsWGS = 1;

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
            }

            return null;
        }

        bool move_shape(xmap.IFeature obj1, xmap.IFeature obj2)
        {
            int loc = obj1.Loc;
            if (loc == obj2.Loc) {

                xmap.IPoly mf1 = obj1.mf; mf1.IsWGS = 1;
                xmap.IPoly mf2 = obj2.mf; mf2.IsWGS = 1;

                string key = obj1.guid;
                if (convert.IsString(key))

                if (mf1.PartCount > 0)
                if (mf2.PartCount > 0)

                switch (loc % 100)
                {
                case 1:
                    return move_node(Point2d(mf1, 0, 0),
                                     Point2d(mf2, 0, 0),
                                     key);
                case 2:
                    return move_edge(mf1,mf2,0,key);
                case 3:
                    return move_surface(mf1, mf2, key);
                }
            }

            return false;
        }

        List<int> get_geoms(xmap.Ixmap_auto map, int offset)
        {
            int top;
            int ptr = map.get_frst_geometry(offset, out top);

            if (ptr > 0) {
                List<int> list = new List<int>();

                while (ptr > 0)
                {
                    list.Add(ptr);
                    ptr = map.get_next_geometry(ptr, top);
                }

                return list;
            }

            return null;
        }

        int seek_guid(xmap.IFeature obj, List<int> list, string guid)
        {
            if (list != null)
            for (int i = 0; i < list.Count; i++)
            {
                obj.Get(list[i]);
                if (obj.guid == guid)
                return i;
            }

            return -1;
        } 

        int move_geoms(int offset1,
                       xmap.Ixmap_auto map1,
                       xmap.IFeature obj1,

                       int offset2,
                       xmap.Ixmap_auto map2,
                       xmap.IFeature obj2)
        {
            int rc = 0;

            List<int> list1 = get_geoms(map1,offset1);
            List<int> list2 = get_geoms(map2,offset2);

            if (list2 != null)
            {
                int i = 0;
                while (i < list2.Count)
                {
                    obj2.Get(list2[i]);
                    string guid = obj2.guid;

                    int j = -1;
                    if (convert.IsString(guid))
                    j = seek_guid(obj1, list1, guid);

                    if (j < 0)
                    {
                        shape(obj2); rc |= 2;
                    }
                    else
                    {
                        if (move_shape(obj1, obj2)) rc |= 1;
                        list1.RemoveAt(i); i--;
                        list2.RemoveAt(j);
                    }

                    i++;
                }
            }

            if (list1 != null)
            if (list1.Count > 0) rc |= 2;

            if (list2 != null)
            if (list2.Count > 0) rc |= 2;

            return rc;
        }

        void attributes(xmap.IFeature obj)
        {
            string info = obj.Info;
            if (convert.IsString(info))
            {
                fwriter.WritePropertyName("attributes");
                fwriter.WriteRawValue(info);
            }
            else writeNullObject("attributes");
        }

        void relations(xmap.Ixmap_auto map, int offset)
        {
            int top, rc = 0;
            int ptr = map.get_frst_relation(offset,out top);
            while (ptr > 0) {
                int locid, dir, typ;
                string guid, code, name;
                map.get_relation(ptr, out locid, out dir, out typ, out guid, out code, out name);

                if ((dir == 1) &&
                    (typ >= 1) && (typ <= 3) &&
                    convert.IsString(guid)) {

                    if (rc == 0)
                    {
                        fwriter.WritePropertyName("featureAssociations");
                        fwriter.WriteStartArray();
                    }

                    fwriter.WriteStartObject();
                    writeValues("ref", feTag + guid);
                    writeValues("associationCode", code);

                    string role = "Association";
                    if (typ == 2) role = "Aggregation";
                    else
                    if (typ == 3) role = "Composition";

                    writeValues("roleCode", role);
                    writeNullObject("attributes");

                    fwriter.WriteEndObject();
                    rc++;
                }

                ptr = map.get_next_relation(ptr, top);
            }

            if (rc > 0) fwriter.WriteEndArray();
        }

        void write_geometry()
        {
            if (fGeometry.Count > 0)
            {
                fwriter.WritePropertyName("geometry");
                fwriter.WriteStartArray();

                foreach (var g in fGeometry)
                {
                    fwriter.WriteStartObject();

                    if (g.lower == g.upper)
                        writeNullObject("scaleRange");
                    else
                    {
                        fwriter.WritePropertyName("scaleRange");
                        fwriter.WriteStartObject();
                        writeValuef("lower", g.lower);
                        writeValuef("upper", g.upper);
                        fwriter.WriteEndObject();
                    }

                    writeValues("ref", g.key);
                    fwriter.WriteEndObject();
                }

                fwriter.WriteEndArray();
            }
        }

        void feature(xmap.Ixmap_auto map, xmap.IFeature obj)
        {
            fwriter.WriteStartObject();

            writeValues("code", obj.acronym);
            writeValues("globalId", obj.guid);
            attributes(obj);
            write_geometry();
            relations(map,obj.Offset);

            fwriter.WriteEndObject();
        }

        void __nextGeometry(string key, xmap.IFeature obj)
        {
            int lower, upper;
            obj.GetScaleRange(out lower, out upper);
            Geometry g = new Geometry(lower, upper, key);
            fGeometry.Add(g);
        }

        void add_feature(xmap.Ixmap_auto map, xmap.IFeature obj)
        {
            fGeometry.Clear();
            string s = shape(obj);
            if (convert.IsString(s)) {
                __nextGeometry(s, obj);

                int top;
                int ptr = fmap.get_frst_geometry(obj.Offset, out top);
                while (ptr > 0)
                {
                    fobj1.Get(ptr);
                    s = shape(fobj1);
                    if (convert.IsString(s))
                    __nextGeometry(s, fobj1);

                    ptr = fmap.get_next_geometry(ptr, top);
                }
            }

            fwriter.WritePropertyName(feTag + obj.guid);
            fwriter.WriteStartArray();

            feature(map,obj);

            fwriter.WriteEndArray();

            fadded++;
        }

        void geometry_ref(xmap.IFeature obj)
        {
            string key = obj.guid;
            
            if (convert.IsString(key))
            switch (obj.Loc % 100)
            {
                case 1:
                    __nextGeometry(vcTag + key,obj);
                    break;
                case 2:
                    __nextGeometry(veTag + key,obj);
                    break;
                case 3:
                    __nextGeometry(ssTag + key,obj);
                    break;
            }
        }

        void ref_feature(xmap.Ixmap_auto map, xmap.IFeature obj, xmap.IFeature obj1) 
        {
            fGeometry.Clear();
            geometry_ref(obj);

            int top;
            int ptr = map.get_frst_geometry(obj.Offset, out top);
            while (ptr > 0)
            {
                obj1.Get(ptr);
                geometry_ref(obj1);
                ptr = map.get_next_geometry(ptr, top);
            }

            feature(map, obj);
        }

        void objects()
        {
            ulong pos = fmap.Position;

            // objects
            if (fmap.Goto_down > 0) do
                {
                    ulong pos1 = fmap.Position;

                    fobj.Get(0);
                    int offset = fobj.Offset;
                    string code = fobj.acronym;
                    string guid = fobj.guid;
                    int loc = fobj.Loc;

                    int uc = 0;    // add feature
                    if (fIsUpdate) uc = fobj.UpdateCode;
                    uc &= 0xffff;   // off tx_uc_new for feature childs 

                    fGeometry.Clear();

                    if (convert.IsString(code))
                    if (convert.IsString(guid))
                    if ((loc >= 1) && (loc <= 3))

                    if (uc == 0)
                        add_feature(fmap,fobj);
                    else {
                        int rc = fbase_map.get_Seek_Node(0, guid);
                        if (rc == 0)
                            Error(String.Format("guid {0} not found.", guid));
                        else
                        {
                            fbase_obj.Get(0);

                            if ((uc & 2) != 0)
                            {
                                fwriter.WritePropertyName(feTag + guid);
                                fwriter.WriteStartArray();

                                ref_feature(fbase_map, fbase_obj,fbase_obj1);

                                fwriter.WriteValue(0);
                                fwriter.WriteValue(0);

                                fwriter.WriteEndArray();
                                fdeleted++;
                            }
                            else
                            {
                                bool upd = false;

                                if ((uc & 0x10) != 0)       // mf
                                    if (move_shape(fbase_obj, fobj)) upd = true;

                                if ((uc & 0x100) != 0)      // geoms 
                                {
                                    int rc1 = move_geoms(fbase_obj.Offset,
                                                         fbase_map,
                                                         fbase_obj1, 
                                                         
                                                         offset,
                                                         fmap,
                                                         fobj1);

                                    if (rc1 != 0) upd = true;
                                    if ((rc1 & 2) == 0)
                                    uc &= 0xfeff; 
                                }

                                if ((uc & 0x1a4) != 0)   // code, hf, roles, geoms
                                {
                                    fwriter.WritePropertyName(feTag + guid);
                                    fwriter.WriteStartArray();

                                    ref_feature(fbase_map, fbase_obj,fbase_obj1);
                                    ref_feature(fmap, fobj,fobj1);

                                    fwriter.WriteEndArray();
                                    upd = true;
                                }

                                if (upd) fupdated++;
                            }
                        }
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

        bool open_map(xmap.Ixmap_auto map, string path)
        {
            map.OpenMap(path);
            map.IsWGS84 = 1;

            if (map.Enabled == 1) return true;
            _message(String.Format("open map '{0}' false.", path));
            return false;
        }

        bool open_base(string path)
        {
            string fn = Path.ChangeExtension(path, ".dm");
            return open_map(fbase_map,fn);
        }

        public bool exec(string path, string main, string dest, string branch, string comment)
        {
            fIsUpdate = convert.IsString(main);

            bool rc = false;

            if (open_map(fmap,path))

            if (!fIsUpdate || open_base(main) ) 
            {
                double x1, y1, x2, y2;
                int pps = fmap.get_Bound(out x1, out y1, out x2, out y2);
                if (pps != 1)
                    _message("expected world coordinate system.");
                else 
                if (fmap.Goto_root == 0)
                    _message("Goto_root == 0");
                else {
                    try
                    {
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

                            fadded = 0;
                            fupdated = 0;
                            fdeleted = 0;

                            ferr.open(Path.ChangeExtension(path, ".err"));

                            layers();

                            ferr.close();

                            fwriter.WriteEndObject();

                            fwriter.WriteEndObject();
                            fwriter.Close();
                            sw.Close();

                            int rc1 = fadded + fupdated + fdeleted;

                            if (rc1 > 0) { 

                                if (fadded > 0)
                                __message(null, String.Format("Add {0} objects.", fadded));

                                if (fupdated > 0)
                                __message(null,String.Format("Update {0} objects.", fupdated));

                                if (fdeleted > 0)
                                    __message(null, String.Format("Delete {0} objects.", fdeleted));

                                rc = true;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        _message(e.Message);
                    }
                }
            }

            fbase_map.Close();
            fmap.Close();

            return rc;
        }

        public string GetBaseMap(string tdm)
        {
            string fname = null;

            fmap.OpenMap(tdm);
            if (fmap.Enabled == 1)
            fname = fmap.BaseMap;
            fmap.Close();

            return fname;
        }
    }
}
