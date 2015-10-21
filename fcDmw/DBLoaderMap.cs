using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmap;
using fcDmw.SOAPService;
using ofiles;
using Convert;

namespace fcDmw
{
    public class DBLoaderMap
    {

        const double RadToDeg = 180 / Math.PI;
        const double eps = 1e-6;

        public delegate void tmsg(string message);

        private tmsg fmessage;
        public tmsg message { set { fmessage = value; } }

        void __message(string message)
        {
            if (fmessage != null) fmessage(message);
        }

        xmap.Ixmap_auto map = null;
        xmap.IFeature obj = null;
        string session = null;
        int fcount = 0;

        sodbFeatureToText flog = null;
        TTextWrite ferr = new TTextWrite();

        sodb fdb = null;
        List<FeatureClassSchema> ffc = null;

        bool fIsUpdate;

        public DBLoaderMap()
        {
            map = new xmap_autoClass();
            obj = map.Feature;
            ffc = new List<FeatureClassSchema>();
        }

        void Error(string msg)
        {
            ferr.writeLine(msg);
            if (flog != null)
            flog.Error(msg);
        }

        void Error1(int offset, string msg)
        {
            string s = String.Format("offset={0} {1}", offset, msg);
            ferr.writeLine(s); Error(msg);
        }

        void log_fe(SOAPService.Feature fe, string msg)
        {
            if (flog != null)
            {
                if (msg != null) flog.Error(msg);

                var features = new List<Feature>();
                features.Add(fe);
                flog.Features(fdb, features);
            }
        }

        List<Path> __paths(xmap.IPoly mf)
        {
            var pp = new List<Path>();

            int parts = mf.PartCount;
            for (int ic = 0; ic < parts; ic++)
            {
                var p = new SOAPService.Path();
                p.Points = new List< SOAPService.Point >();

                int np; mf.GetContourCount(ic,out np);
                for (int ip = 0; ip < np; ip++)
                {
                    double x, y;
                    mf.GetPointd(ic, ip, out y, out x);
                    x *= RadToDeg; y *= RadToDeg;
                    p.Points.Add(new Point() { X = x, Y = y });
                }
                 
                if (p.Points.Count > 0) pp.Add(p);
            }


            return pp;
        }

        List<Ring> __rings(xmap.IPoly mf)
        {
            var pp = new List<Ring>();

            int parts = mf.PartCount;
            for (int ic = 0; ic < parts; ic++)
            {
                var p = new SOAPService.Ring();
                p.Points = new List<SOAPService.Point>();

                int np; mf.GetContourCount(ic, out np);
                for (int ip = 0; ip < np; ip++)
                {
                    double x, y;
                    mf.GetPointd(ic, ip, out y, out x);
                    x *= RadToDeg; y *= RadToDeg;
                    p.Points.Add(new Point() { X = x, Y = y });

                }

                if (p.Points.Count > 0) pp.Add(p);
            }


            return pp;
        }

        Point __point(xmap.IPoly mf)
        {
            var pt = new Point();
            pt.Type = GeometryType.Point;

            if (mf.PartCount > 0)
            {
                double x, y;
                mf.GetPointd(0, 0, out y, out x);
                pt.X = x*RadToDeg; pt.Y = y*RadToDeg;
            }

            return pt;
        }

        Polyline __polyline(xmap.IPoly mf)
        {
            var pp = new Polyline();
            pp.Type = GeometryType.Polyline;
            pp.Paths = __paths(mf);
            return pp;
        }

        Polygon __polygon(xmap.IPoly mf)
        {
            var pp = new Polygon();
            pp.Type = GeometryType.Polygon;
            pp.Rings = __rings(mf);
            return pp;
        }

        SOAPService.FeatureClassSchema __Schema(string Cl_Id)
        {
            SOAPService.FeatureClassSchema fc = ffc.Where(x => x.Cl_Id == Cl_Id).FirstOrDefault();
            if (fc == null)
            {
                fc = fdb.GetFeatureClassSchema(Cl_Id);
                if (fc != null) ffc.Add(fc);
            }

            return fc;
        }

        SOAPService.RelationRule __Rule(string Cl_Id,string Rule_Id)
        {
            SOAPService.FeatureClassSchema fc = __Schema(Cl_Id);
            if (fc != null) 
            if (fc.Relations != null)
            foreach(var rule in fc.Relations) 
            if (rule.Cl_Id == Rule_Id) 
            return rule;

            return null;
        }

        SOAPService.Attribute __attr(List<SOAPService.Attribute> list, 
                                     string Cl_Id, string Attr_Id)
        {
            if (Attr_Id == null) return null;
            if (Attr_Id.Length == 0) return null;

            SOAPService.Field f = null;

            SOAPService.FeatureClassSchema fc = __Schema(Cl_Id);

            if (fc != null) 
            f = fc.Fields.Where(x => x.Attr_Id == Attr_Id).FirstOrDefault();

            if (f == null)
                Error(String.Format("class[{0}] attribute[{1}] not found.", Cl_Id, Attr_Id));
            else
            {
                SOAPService.Attribute a = list.Where(x => x.Attr_Id == Attr_Id).FirstOrDefault();
                if (a == null)
                {
                    a = new SOAPService.Attribute();
                    a.Attr_Id = Attr_Id;
                    list.Add(a);
                }

                a.Type = f.Type;
                a.IsCompositeAttribute = f.IsCompositeAttribute;
                a.CompositAttrCl_Id = f.CompositAttrCl_Id;

                return a;
            }

            return null;
        }

        SOAPService.AttrValue __attrValue(SOAPService.Attribute a, string str, double v1, double v2, int op)
        {

            if (a != null)

            if (str != null)
            if (str.Length > 0)
            {

                var v = new SOAPService.AttrValue();

                if ((a.Type == DataType.String) ||
                    (a.Type == DataType.Text) ||
                    (a.Type == DataType.Domain)) { 
                    var vv = new SOAPService.StringValue();
                    vv.Text = str; v.Value = vv;
                } else
                if ((a.Type == DataType.Float) ||
                    (a.Type == DataType.Integer)) {
                    if (op > 0) {
                        var vv = new SOAPService.NumberValue();

                        if ((op == 1) || (op == 3)) {   // > >=
                            vv.Lower = (decimal)v1;
                            vv.IncludeLower = (op == 3);
                        } else

                        if ((op == 2) || (op == 4)) {   // < <=
                            vv.Upper = (decimal)v1;
                            vv.IncludeUpper = (op == 4);
                        } 
                        else {
                            vv.Lower = (decimal)v1;
                            vv.Upper = (decimal)v2;
                            vv.IncludeLower = (op == 5) || (op == 8);
                            vv.IncludeUpper = (op == 5) || (op == 9);
                            vv.Inverval = (v1 < v2);
                        }

                        vv.Text = str;
                        v.Value = vv;
                    } 
                    else {
                        decimal f;
                        if (decimal.TryParse(str, out f)) {
                            var vv = new SOAPService.NumberValue();
                            vv.Lower = f;
                            vv.Upper = f;
                            vv.IncludeLower = true;
                            vv.IncludeUpper = true;
                            vv.Inverval = false;

                            vv.Text = str;
                            v.Value = vv;
                        }
                    }
                }

                if (v.Value != null) {

                    if (a.Values == null)
                    a.Values = new List<SOAPService.AttrValue>();

                    a.Values.Add(v);
                    return v;
                }
            }

            return null;
        }

        void addAttrs(string Cl_Id,
                      SOAPService.Feature main_ft,
                      int main_code, int main_loc,  
                      List<SOAPService.Attribute> attrs)
        {

            xmap.IAttrs hf = obj.hf;

            int n = hf.Count;
            for (int i = 0; i < n; i++)
            {
                string key, str;
                double v1, v2; int op;
                hf.GetValue(i, out key, out str, out v1, out v2, out op);
                SOAPService.Attribute a = __attr(attrs, Cl_Id, key);
                if (a != null) __attrValue(a, str, v1, v2, op);
            }

            childs(Cl_Id, main_ft,main_code,main_loc, attrs);

        }

        bool frst_Datatype(int code)
        {
            code = code % 10000;

            if (map.Goto_down > 0)
                do
                {
                    obj.Get();
                    if (obj.Loc == 30)
                        if ((obj.Code % 10000) == code)
                            return true;
                } while (map.Goto_right > 0);

            return false;
        }

        bool __syncPoint(Point src, Point dst)
        {
            double d = src.X - dst.X;
            if (Math.Abs(d) < eps)
            {
                d = src.Y - dst.Y;
                if (Math.Abs(d) < eps)
                {
                    src.X = dst.X;
                    src.Y = dst.Y;
                    return true;
                }
            }

            return false;
        }

        void __syncPoints(List<Point> src, List<Point> dst)
        {
            foreach(var p1 in src)
            foreach (var p2 in dst)
            {
                double d=p2.X-p1.X;
                if (Math.Abs(d) < eps) {
                    d = p2.Y - p1.Y;
                    if (Math.Abs(d) < eps)
                    {
                        p1.X = p2.X;
                        p1.Y = p2.Y;
                        break;
                    }
                }
            }
        }

        bool __compPoint(Point p1, Point p2)
        {
            double d=p2.X-p1.X;
            if (Math.Abs(d) < eps) {
                d = p2.Y - p1.Y;
                return (Math.Abs(d) < eps);
            }

            return false;
        }

        bool __compPoints(List<Point> src, List<Point> dst)
        {
            int n = src.Count;
            if (n != dst.Count)
                return false;
            else
            {
                for (int i = 0; i < n; i++)
                {
                    Point p1 = src[i], p2 = dst[i];
                    double d;
                    
                    d = Math.Abs(p2.X - p1.X);
                    if (d > eps) return false;

                    d = Math.Abs(p2.Y - p1.Y);
                    if (d > eps) return false;
                }

                return true;
            }
        }

        bool _compPolyline(Polyline src, Polyline dst)
        {
            if (src.Paths != null)
            if (src.Paths.Count > 0)
            if (dst.Paths != null)
            if (dst.Paths.Count > 0)
            {
                int n = src.Paths.Count;
                if (n != dst.Paths.Count) return false;

                for (int i = 0; i < n; i++)
                {
                    List<Point> p1 = src.Paths[i].Points;
                    List<Point> p2 = dst.Paths[i].Points;

                    bool rc = __compPoints(p1, p2);
                    if (!rc) return false;
                }

                return true;
            };

            return false;
        }

        bool _compPolygon(Polygon src, Polygon dst)
        {
            if (src.Rings != null)
            if (src.Rings.Count > 0)
            if (dst.Rings != null)
            if (dst.Rings.Count > 0)
            {
                int n = src.Rings.Count;
                if (n != dst.Rings.Count) return false;

                for (int i = 0; i < n; i++)
                {
                    List<Point> p1 = src.Rings[i].Points;
                    List<Point> p2 = dst.Rings[i].Points;

                    bool rc = __compPoints(p1, p2);
                    if (!rc) return false;
                }

                return true;
            }

            return false;
        }

        void _PointToPoints(Point pt, List<Point> pp)
        {
            foreach (var p in pp)
            {
                double d = pt.X - p.X;
                if (Math.Abs(d) < eps)
                {
                    d = pt.Y - p.Y;
                    if (Math.Abs(d) < eps)
                    {
                        pt.X = p.X;
                        pt.Y = p.Y;
                        break;
                    }
                }
            }
        }

        void _PointsToPoint(List<Point> pp, Point pt)
        {
            double x = pt.X, y = pt.Y;

            foreach (var p in pp)
            {
                double d = x - p.X;
                if (Math.Abs(d) < eps)
                {
                    d = y - p.Y;
                    if (Math.Abs(d) < eps)
                    {
                        p.X = x;
                        p.Y = y;
                        break;
                    }
                }
            }
        }

        void _PointToPolyLine(Point pt, Polyline c)
        {
            if (pt != null)
            if (c != null)
            if (c.Paths != null)
            foreach (var pp in c.Paths)
            _PointToPoints(pt, pp.Points);
        }

        void _PointToPolygon(Point pt, Polygon s)
        {
            if (pt != null)
            if (s != null)
            if (s.Rings != null)
            foreach (var pp in s.Rings)
            _PointToPoints(pt, pp.Points);
        }

        void _PolylineToPoint(Polyline c, Point p)
        {
            if (c != null)
            if (c.Paths != null)
            foreach (var pp in c.Paths)
            _PointsToPoint(pp.Points, p);
        }

        bool _PolylineToPolyline(Polyline src, Polyline dst)
        {
            if (src != null)
            if (src.Paths != null)
            if (src.Paths.Count > 0)

            if (dst != null)
            if (dst.Paths != null)
            if (dst.Paths.Count > 0) {
                __syncPoints(src.Paths[0].Points,
                                dst.Paths[0].Points);

                return _compPolyline(src, dst);
            };

            return false;
        }

        void _PolyLineToPolygon(Polyline c, Polygon s)
        {
            if (c != null)
            if (c.Paths != null)

            if (s != null)
            if (s.Rings != null)
            __syncPoints(c.Paths[0].Points,
                         s.Rings[0].Points);
        }

        void _PolygonToPoint(Polygon s, Point p)
        {
            if (s != null)
            if (s.Rings != null)
            foreach (var pp in s.Rings)
            _PointsToPoint(pp.Points, p);
        }

        void _PolygonToPolyLine(Polygon s, Polyline c)
        {
            if (c != null)
            if (c.Paths != null)

            if (s != null)
            if (s.Rings != null)
            __syncPoints(s.Rings[0].Points,
                         c.Paths[0].Points);
        }

        bool _PolygonToPolygon(Polygon src, Polygon dst)
        {
            if (src.Rings != null)
            if (src.Rings.Count > 0)

            if (dst.Rings != null)
            if (dst.Rings.Count > 0)
            {
                __syncPoints(src.Rings[0].Points,
                                dst.Rings[0].Points);

                return _compPolygon(src, dst);
            }

            return false;
        }

        void _syncGeometry(Geometry shp, Feature fe, Feature _fe)
        {
            Point base_p = fe.BaseShapePoint;
            Polyline base_c = fe.BaseShapePolyline;
            Polygon base_s = fe.BaseShapePolygon;

            if (base_p != null)
            if (base_p.Status == EditableStatus.Unchanged)
            if (_fe.BaseShapePoint != null)
            base_p = _fe.BaseShapePoint;

            if (base_c != null)
            if (base_c.Status == EditableStatus.Unchanged)
            if (_fe.BaseShapePolyline != null)
            base_c = _fe.BaseShapePolyline;

            if (base_s != null)
            if (base_s.Status == EditableStatus.Unchanged)
            if (_fe.BaseShapePolygon != null)
            base_s = _fe.BaseShapePolygon;

            if (shp != null)

            if (shp.Type == GeometryType.Point)
            {
                Point p = (Point)shp;
                if (base_p != null)
                    __syncPoint(p, base_p);
                else
                if (base_c != null)
                    _PointToPolyLine(p, base_c);
                else
                if (base_s != null)
                    _PointToPolygon(p, base_s);
            }
            else
            if (shp.Type == GeometryType.Polyline) {
                Polyline c = (Polyline)shp;

                if (base_c != null)
                    _PolylineToPolyline(c, base_c);
                else
                if (base_s != null)
                    _PolyLineToPolygon(c, base_s);
            }
        }

        bool _compShape(Geometry g1, Geometry g2)
        {
            if (g1 != null)
            if (g2 != null)
            if (g1.Type == g2.Type)

            if (g1.Type == GeometryType.Point)
                return __compPoint((Point)g1, (Point)g2);
            else
            if (g1.Type == GeometryType.Polyline)
                return _compPolyline((Polyline)g1, (Polyline)g2);
            else
            if (g1.Type == GeometryType.Polygon) 
                return _compPolygon((Polygon)g1, (Polygon)g2);

            return false;
        }

        void __editBaseShapes(SOAPService.Feature fe, SOAPService.Feature _fe)
        {
            if (fe.BaseShapePoint == null) {
                if (_fe.BaseShapePoint != null) {
                    fe.BaseShapePoint = _fe.BaseShapePoint;
                    fe.BaseShapePoint.Status = SOAPService.EditableStatus.Deleted;
                }
            }
            else
            if (_fe.BaseShapePoint == null)
                fe.BaseShapePoint.Status = SOAPService.EditableStatus.Added;
            else  {
                fe.BaseShapePoint.Key = _fe.BaseShapePoint.Key;
                fe.BaseShapePoint.Status = SOAPService.EditableStatus.Modified;
                bool rc = __syncPoint(fe.BaseShapePoint,_fe.BaseShapePoint);
                if (rc) fe.BaseShapePoint.Status = SOAPService.EditableStatus.Unchanged;
            }

            if (fe.BaseShapePolyline == null)
            {
                if (_fe.BaseShapePolyline != null)
                {
                    fe.BaseShapePolyline = _fe.BaseShapePolyline;
                    fe.BaseShapePolyline.Status = SOAPService.EditableStatus.Deleted;
                }
            }
            else
                if (_fe.BaseShapePolyline == null)
                    fe.BaseShapePolyline.Status = SOAPService.EditableStatus.Added;
                else
                {
                    fe.BaseShapePolyline.Key = _fe.BaseShapePolyline.Key;
                    fe.BaseShapePolyline.Status = SOAPService.EditableStatus.Modified;
                    bool rc = _PolylineToPolyline(fe.BaseShapePolyline, _fe.BaseShapePolyline);
                    if (rc) fe.BaseShapePolyline.Status = SOAPService.EditableStatus.Unchanged;
                }

            if (fe.BaseShapePolygon == null)
            {
                if (_fe.BaseShapePolygon != null)
                {
                    fe.BaseShapePolygon = _fe.BaseShapePolygon;
                    fe.BaseShapePolygon.Status = SOAPService.EditableStatus.Deleted;
                }
            }
            else
                if (_fe.BaseShapePolygon == null)
                    fe.BaseShapePolygon.Status = SOAPService.EditableStatus.Added;
                else
                {
                    fe.BaseShapePolygon.Key = _fe.BaseShapePolygon.Key;
                    fe.BaseShapePolygon.Status = SOAPService.EditableStatus.Modified;
                    bool rc = _PolygonToPolygon(fe.BaseShapePolygon, _fe.BaseShapePolygon);
                    if (rc) fe.BaseShapePolygon.Status = SOAPService.EditableStatus.Unchanged;
                }
        }

        void __editAttrValues(Feature fe, Feature _fe)
        {
            if (fe.Attributes != null)
            foreach (var a1 in fe.Attributes) 
            {
                SOAPService.Attribute a2 = null;

                if (_fe.Attributes != null)
                a2 = _fe.Attributes.Where(x => x.Attr_Id == a1.Attr_Id).FirstOrDefault();

                if (a2 != null) a1.Id = a2.Id;

                if (a1.Values != null)                
                foreach (var v1 in a1.Values)
                {
                    v1.Status = EditableStatus.Added;

                    if (a2 != null) 
                    foreach (var v2 in a2.Values) {
                        v1.Id = v2.Id;

                        v1.Status = EditableStatus.Modified;
                        if (fdb.compAttrValue(v1, v2))
                        v1.Status = EditableStatus.Unchanged;

                        if (v1.Shape != null) {
                            _syncGeometry(v1.Shape, fe,_fe);
                            v1.Shape.Status = EditableStatus.Modified;
                            if (_compShape(v1.Shape,v2.Shape)) 
                            v1.Shape.Status = EditableStatus.Unchanged;
                        }

                        a2.Values.Remove(v2);
                        break;
                    }

                    if (v1.Status == EditableStatus.Added) 
                    if (v1.Shape != null) 
                    _syncGeometry(v1.Shape, fe,_fe);
                }

                if (a2 != null)
                if (a2.Values != null)
                foreach (var v2 in a2.Values) {
                    v2.Status = SOAPService.EditableStatus.Deleted;
                    if (a1.Values == null)
                    a1.Values = new List<SOAPService.AttrValue>();
                    a1.Values.Add(v2);
                }

                if (a2 != null)
                _fe.Attributes.Remove(a2);
            }

            if (_fe.Attributes != null)
            foreach (var a in _fe.Attributes) 
            if (a.Values != null)
            if (a.Values.Count > 0)
            {
                foreach (var v in a.Values)
                v.Status = SOAPService.EditableStatus.Deleted;

                if (fe.Attributes == null)
                fe.Attributes = new List<SOAPService.Attribute>();

                fe.Attributes.Add(a);
            }
        }

        void __editRelations(SOAPService.Feature fe, SOAPService.Feature _fe)
        {
            if (fe.Relations != null)
            foreach (var r1 in fe.Relations)
            if (r1.RelationRule != null)
            {
                r1.Status=SOAPService.EditableStatus.Added;

                if (_fe.Relations != null) 
                foreach(var r2 in _fe.Relations) 
                if (r2.RelationRule != null)
                if (r2.RelationRule.Cl_Id == r1.RelationRule.Cl_Id)
                if (r2.Role == r1.Role) {

                    r1.Status = SOAPService.EditableStatus.Modified;
                    if (r1.Shape == null)
                    r1.Status = SOAPService.EditableStatus.Unchanged;

                    _fe.Relations.Remove(r2); break;
                }
            }

            if (_fe.Relations != null)  
            foreach(var r2 in _fe.Relations) {

                r2.Status = SOAPService.EditableStatus.Deleted;

                if (fe.Relations == null)
                fe.Relations = new List<SOAPService.Relation>();

                fe.Relations.Add(r2);
            }
        }

        void FeatureToFeature(Feature fe, Feature _fe)
        {
            Point p2 = _fe.BaseShapePoint;
            Polyline c2 = _fe.BaseShapePolyline;
            Polygon s2 = _fe.BaseShapePolygon;

            Point p = fe.BaseShapePoint;
            if (p != null) {
                if (p2 != null) __syncPoint(p,p2);
                if (c2 != null) _PointToPolyLine(p,c2);
                if (s2 != null) _PointToPolygon(p,s2);
            }

            Polyline c = fe.BaseShapePolyline;
            if (c != null) {
                if (p2 != null) _PolylineToPoint(c,p2);
                if (c2 != null) _PolylineToPolyline(c, c2);
                if (s2 != null) _PolyLineToPolygon(c, s2);
            }

            Polygon s = fe.BaseShapePolygon;
            if (s != null)
            {
                if (p2 != null) _PolygonToPoint(s,p2);
                if (c2 != null) _PolygonToPolyLine(s,c2);
                if (s2 != null) _PolygonToPolygon(s, s2);
            }
        }

        void sync_fe_relations(Feature fe)
        {
            if (fe.Relations != null)
            foreach (var r in fe.Relations) {

                Feature _fe = fdb.GetFeature(r.Key);
                if (_fe != null)
                {
                    FeatureToFeature(fe, _fe);

                    if (fe.Attributes != null)
                    foreach (var a in fe.Attributes)

                    if (a.Values != null)
                    foreach (var v in a.Values)

                    if (v.Shape != null)
                    _syncGeometry(v.Shape, fe, _fe);
                }
            }    
        }

        string db_fe(Feature fe, int offset, int uc)
        {
            string key = fe.Key;
            string msg = null;
            string err = null;

            uc &= 0xffff;   // off tx_uc_new for feature childs 

            if (uc == 0)
            {
                sync_fe_relations(fe);
                key = fdb.addFeature(session, fe);
                if (key != null) fe.Key = key;
            }
            else
            if (!convert.IsString(key))
                err = "has not guid."; 
            else
            if ((uc & 1) == 0) {
                err = "flag[db] off";
            } else

            if ((uc & 2) != 0) {
                fdb.deleteFeature(session, fe.Key);
                msg = "delete"; key = null;
            }
            else 
            if (uc != 1) {
                Feature _fe = fdb.GetFeature(key);
                if (_fe == null) {
                    string s = String.Format("object <{0}> not found in the DB.", key);
                    __message(s); return null;
                }

                __editBaseShapes(fe, _fe);
                __editAttrValues(fe, _fe);
                __editRelations(fe, _fe);

                fdb.editFeature(session, fe);
                msg = "edit";
            }

            if (uc != 1) log_fe(fe, msg);

            if (err != null)
            {
                Error1(offset, err);
                return null;
            }

            return key;
        }

        void __baseShape(SOAPService.Feature fe, int loc)
        {
            if ((loc >= 1) && (loc <= 3))
            {
                xmap.IPoly mf = obj.mf;
                mf.IsWGS = 1;

                if (mf.PartCount > 0)
                    switch (loc)
                    {
                        case 1:
                            if (fe.BaseShapePoint == null)
                            fe.BaseShapePoint = __point(mf);
                            break;
                        case 2:
                            if (fe.BaseShapePolyline == null)
                            fe.BaseShapePolyline = __polyline(mf);
                            break;
                        case 3:
                            if (fe.BaseShapePolygon == null)
                            fe.BaseShapePolygon = __polygon(mf);
                            break;
                    }
            }
        }

        SOAPService.AttrValue __datatype(int code, int loc, SOAPService.Attribute a ) 
        {
            SOAPService.AttrValue v = null;

            string acronym = a.CompositAttrCl_Id;
            var fe = new SOAPService.Feature() { Cl_Id = acronym, IsRealFeature = false };

            int offset = obj.Offset;

            if ((loc >= 1) && (loc <= 3))
            {
                __baseShape(fe, loc);
                if (!frst_Datatype(code)) return null;
            }

            int uc = 0;    // add feature
            if (fIsUpdate) uc = obj.UpdateCode;

            fe.Key = obj.GUID;

            var attrs = new List<SOAPService.Attribute>();
            addAttrs(acronym, null, 0, 0, attrs);

            if (attrs.Count > 0) {
                fe.Attributes = attrs;

                string key = db_fe(fe, offset, uc);

                if (key != null)
                if (key.Length > 0) 
                {
                    v = new SOAPService.AttrValue();
                    var vv = new SOAPService.StringValue();
                    vv.Text = key; v.Value = vv;

                    if (a.Values == null)
                    a.Values = new List<SOAPService.AttrValue>();

                    a.Values.Add(v);
                }
            }

            return v;
        }

        void childs(string Cl_Id, 
                    SOAPService.Feature main_fe,
                    int main_code, int main_loc,
                    List<SOAPService.Attribute> attrs)  
        {
            ulong pos = map.Position;

            if (map.Goto_down > 0) 
            do  {
                ulong pos1 = map.Position;

                obj.Get();
                int code = obj.Code;
                int loc = obj.Loc;

                if ((loc >= 1) && (loc <= 3)) {

                    if (main_fe != null)
                    if (code == main_code)
                    __baseShape(main_fe, loc);

                } else

                if ((loc >= 30) && (loc <= 33)) {

                    code = code % 100000;

                    if ((code < 995) || (code > 1000))
                    {

                        string attr_cl = map.AcronymA[code];
                        SOAPService.Attribute a = __attr(attrs, Cl_Id, attr_cl);

                        if (a == null)
                            Error(String.Format("class[{0}] datatype[{1}] acronym not found.", Cl_Id, code));
                        else
                        {
                            SOAPService.AttrValue v = null;

                            if (a.IsCompositeAttribute)
                            {
                                v = __datatype(code, loc % 10, a);
                                map.Position = pos1;
                                obj.Get();
                            }
                            else
                            {
                                xmap.IAttrs hf = obj.hf;

                                int n = hf.Count;
                                for (int i = 0; i < n; i++)
                                {
                                    string key, str;
                                    double v1, v2; int op;
                                    hf.GetValue(i, out key, out str, out v1, out v2, out op);
                                    if (key == attr_cl)
                                    {
                                        v = __attrValue(a, str, v1, v2, op);

                                        if (v != null) {
                                            xmap.IPoly mf = obj.mf;
                                            if (mf.PartCount > 0)
                                            {
                                                mf.IsWGS = 1;

                                                switch (loc % 10)
                                                {
                                                    case 1:
                                                        v.Shape = __point(mf);
                                                        v.Shape.Type = SOAPService.GeometryType.Point;
                                                        break;
                                                    case 2:
                                                        v.Shape = __polyline(mf);
                                                        break;
                                                    case 3:
                                                        v.Shape = __polygon(mf);
                                                        break;
                                                }
                                            }
                                        }

                                        break;
                                    }
                                }

                                if (v == null) {
                                    string s = String.Format("attrv[{0}] without value.", attr_cl);
                                    Error1(obj.Offset, s);
                                }
                            }

                            if (a.Values == null) attrs.Remove(a);
                        }
                    }

                } else

                if ((loc >= 40) && (loc <= 43)) {

                    if (main_fe != null)
                    {
                        int locid2, dir, typ;
                        string guid2;
                        string rule_Id;
                        string name;

                        obj.GetRole(out locid2, out guid2, out dir, out typ, out rule_Id, out name);

                        if (dir == 1)   // source
                        if (guid2 != null)
                        if (guid2.Length > 0)
                        {
                            SOAPService.RelationRule rule = __Rule(main_fe.Cl_Id, rule_Id);
                            if (rule != null)
                            {
                                SOAPService.Relation relation = new SOAPService.Relation();
                                relation.Key = guid2;
                                relation.Role = 0;
                                relation.RelationRule = rule;

                                if (main_fe.Relations == null)
                                main_fe.Relations = new List<Relation>();

                                main_fe.Relations.Add(relation);
                            }
                        }
                    }
                }

                map.Position = pos1;
            } while (map.Goto_right > 0);

            map.Position = pos;
        }

        void objects()
        {
            ulong pos = map.Position;

            // objects
            if (map.Goto_down > 0) do
            {
                ulong pos1 = map.Position;

                obj.Get();
                int offset = obj.Offset;
                string acronym = obj.Acronym;
                int code = obj.Code, loc = obj.Loc;

                int uc = 0;    // add feature
                if (fIsUpdate) uc = obj.UpdateCode;

                if (acronym != null)
                if (acronym.Length > 0)
                if ((loc >= 1) && (loc <= 3)) 
                {
                    var fe = new SOAPService.Feature() { Cl_Id=acronym, IsRealFeature = true };
                    fe.Key = obj.GUID;

                    __baseShape(fe, loc);

                    map.Position = pos1;

                    var attrs = new List<SOAPService.Attribute>();
                    addAttrs(acronym,fe,code,loc, attrs);

                    if (attrs.Count > 0)
                    fe.Attributes = attrs;

                    db_fe(fe, offset, uc); fcount++;
                }    

                map.Position = pos1;
            } while (map.Goto_right > 0);


            map.Position = pos;

        }

        void layers()
        {
            if (map.Goto_down > 0) 
            do { objects();
            } while (map.Goto_right > 0);
        }

        public void workDir(string s)
        {
            if (s != null) 
            map.WorkDir = s;
        }

        public void exec(string path, sodb db, sodbFeatureToText Alog, bool AIsUpdate)
        {

            map.OpenMap(path); fdb = db; flog = Alog; fIsUpdate = AIsUpdate; 

            if (map.Enabled != 1)
                __message(String.Format("open map '{0}' false.",path));
            {

                map.IsWGS84=1;
                double x1,y1,x2,y2;
                int pps = map.get_Bound(out x1, out y1, out x2, out y2);
                if (pps != 1)
                    __message("expected world coordinate system.");
                else
                {
                    db.SetExtent(x1 * RadToDeg, y1 * RadToDeg, x2 * RadToDeg, y2 * RadToDeg);

                    session = db.beginUpdate();
                    if (session == null)
                        __message("session == null");
                    {
                        __message(String.Format("session = {0}", session));

                        string err = System.IO.Path.ChangeExtension(path, ".err");
                        ferr.open(err);

                        if (map.Goto_root == 0)
                            __message("Goto_root == 0");
                        layers();

                        ferr.close();

                        if (fcount > 0)

                            if (fIsUpdate)
                            {
                                __message(String.Format("Update {0} objects.", fcount));
                                __message("apply Update");
                                db.applyUpdate(session, "UpdateObjects");
                            }
                            else
                            {
                                __message(String.Format("Add {0} objects.", fcount));
                                __message("apply Update");
                                db.applyUpdate(session, "LoaderMap");
                            }

                        db.endUpdate(session);
                        __message("end session");
                    }
                }
            }

            map.Close(); fdb = null; flog = null;
        }
    }
}
