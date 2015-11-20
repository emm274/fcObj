/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 07.07.2015
 * Time: 16:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Convert;
using xmap;

namespace fcDmw
{
	/// <summary>
	/// Description of sodbFeatureToMap.
	/// </summary>
	public class sodbFeatureToMap: sodbFeatureToIntf
	{
		const double DegToRad = Math.PI/180;
		

		xmap.Ixmap_auto fmap;
        xmap.IFeature obj;

		string fPath;
		int fCount;
		bool fEnabled;

        struct tnode
        {
            public tnode(int Aowner, string Aacronym, string Akey)
            {
                owner = Aowner; acronym = Aacronym; key = Akey;
            }

            public int owner;
            public string acronym;
            public string key;
        }

        List<tnode> nodes = new List<tnode>();

		public sodbFeatureToMap(string Path)
		{
			fmap = new xmap_autoClass();
            obj = fmap.Feature;

			fPath=Path; fCount=0;
		}

     	public void Close()
		{
            fmap.end_roles();
            fmap.Close();
		}
     	
     	public void Extent(double x1,double y1,double x2, double y2) {

            fCount = 0;

            double b1 = Math.Truncate(x1) * DegToRad;
            x1 *= DegToRad; y1 *= DegToRad; x2 *= DegToRad; y2 *= DegToRad;
            
            fmap.NewMap(fPath, "oom", x1, y1, x2, y2, 9, 3, b1, 0, 0, 50000);
     		fEnabled=fmap.Enabled == 1;
     	}

        public void workDir(string s)
        {
            if (s != null)
                fmap.WorkDir = s;
        }

        SOAPService.RelationRule __RelationRule()
        {
            return null;
        }

     	void __contour(xmap.IPoly mf, List<SOAPService.Point> pp, int loc) {
			foreach(var p in pp)
			mf.AddPoint(p.Y*DegToRad,p.X*DegToRad);
			mf.endContour(loc);
     	}
     	
     	bool __obj(ref int count) 
        {
   			int down=0;
   			if (count == 1) down=1;
     			
   			obj.New(down);
            if (obj.Offset > 0)
            {
                count++;
                return true;
            }

     		return false;
     	}

        void __metadata(sodb db, List<SOAPService.Metadata> list, int typ, string acronym, ref int count)
        {
            if (db.MetadataExists(list))
            {
                if (typ >= 0) 
                    obj.begin_meta(typ,acronym);
                else 
                {
                    obj.Reset(); obj.Loc = 30;
                    obj.acronym = acronym;
                }

                xmap.IAttrs hf = obj.hf;
                
                foreach (var m in list) 
                if (!m.IsCompositeAttribute) 
                if (m.Values != null)
                if (m.Values.Count > 0)
                {
                    SOAPService.MetaValue v = m.Values[0];
                    if (convert.IsString(v.Value)) 
                    hf.Add(m.Akronim,v.Value);
                }

                if (__obj(ref count))
                {
                    ulong pos = fmap.Position;
                    int count1 = 1;

                    foreach (var m in list)
                    if (m.IsCompositeAttribute)
                    if (m.CompositeMetadata != null)
                    __metadata(db,m.CompositeMetadata, -1, m.Akronim,ref count1);

                    fmap.Position = pos;
                }

                if (typ >= 0) obj.end_meta();
            }
        }

        string attrValue(sodb db, SOAPService.AttrValue v)
        {
            SOAPService.NumberStringAttrValue vv = v.Value;

            if (vv is SOAPService.NumberValue)
                return db.NumberToStr(vv as SOAPService.NumberValue);
            else if (vv is SOAPService.StringValue)
                return (vv as SOAPService.StringValue).Text;

            return "";
        }

        void fill_hf(sodb db, SOAPService.Feature fe)
        {
            xmap.IAttrs hf = obj.hf;
            hf.Add("Key", fe.Key);

            List<SOAPService.Attribute> Attributes = fe.Attributes;
            if (Attributes != null)
            foreach (var a in Attributes)
            if (!a.IsCompositeAttribute)
            foreach (var v in a.Values)
            if (v.Value != null)
            {
                string s = attrValue(db, v);

                if (s != null)
                if (s.Length > 0)
                {
                    SOAPService.Geometry shp = v.Shape;
                    if (shp == null) hf.Add(a.Attr_Id, s);
                }
            }
        }

        int attr_shape(SOAPService.Geometry shp)
        {
            if (shp != null)
            {
                xmap.IPoly mf = obj.mf;

                if (shp.Type == SOAPService.GeometryType.Point)
                {
                    obj.Loc = 31;
                    SOAPService.Point pt = (SOAPService.Point)shp;
                    mf.AddPoint(pt.Y * DegToRad, pt.X * DegToRad);
                    mf.endContour(1);
                }
                else
                if (shp.Type == SOAPService.GeometryType.Polyline)
                {
                    obj.Loc = 32;
                    SOAPService.Polyline ln = (SOAPService.Polyline)shp;
                    foreach (var path in ln.Paths)
                    __contour(mf, path.Points, 2);
                }
                else
                if (shp.Type == SOAPService.GeometryType.Polygon)
                {
                    obj.Loc = 33;
                    SOAPService.Polygon rgn = (SOAPService.Polygon)shp;
                    foreach (var ring in rgn.Rings)
                    __contour(mf, ring.Points, 3);
                }

                return mf.PartCount;
            }    

            return 0;

        }

        bool BasePoint(SOAPService.Feature fe)
        {
            SOAPService.Point pt = fe.BaseShapePoint;
            if (pt != null)
            {
                xmap.IPoly mf = obj.mf;
                mf.AddPoint(pt.Y * DegToRad, pt.X * DegToRad);
                mf.endContour(1);

                return true;
            }

            return false;
        }

        bool BasePolyline(SOAPService.Feature fe)
        {
            SOAPService.Polyline ln = fe.BaseShapePolyline;
            if (ln != null)
            if (ln.Paths.Count > 0) {
                xmap.IPoly mf = obj.mf;
                foreach (var path in ln.Paths)
                __contour(mf, path.Points, 2);

                return true;
            }

            return false;
        }

        bool BasePolygon(SOAPService.Feature fe)
        {
            SOAPService.Polygon rgn = fe.BaseShapePolygon;

            if (rgn != null)
            if (rgn.Rings.Count > 0) {
                xmap.IPoly mf = obj.mf;
                foreach (var ring in rgn.Rings)
                __contour(mf, ring.Points, 3);

                return true;
            }

            return false;
        }

        void Datatypes(sodb db,
                      List<SOAPService.Feature> list,
                      SOAPService.Feature dt,  
                      int ptr) 
        {
            ulong pos = fmap.Position;
            int count1 = 1;

            List<SOAPService.Attribute> Attributes = dt.Attributes;

            if (Attributes != null)
            foreach (var a in Attributes)
            if (a.IsCompositeAttribute)
            foreach (var vv in a.Values)
            if (vv.Value != null)
            addDatatype(db,list, ptr, a.Attr_Id, vv, ref count1);

            __metadata(db, dt.Metadata, 2, "", ref count1);

            fmap.Position = pos;
        }

        void Datatype(sodb db,
                      List<SOAPService.Feature> list,   
                      SOAPService.Feature dt,  
                      string Attr_id, 
                      ref int count)
        {
            obj.Reset();

            int loc = 0;
            if (BasePoint(dt))    loc = 1; else
            if (BasePolyline(dt)) loc = 2; else
            if (BasePolygon(dt))  loc = 3;

            obj.Loc = 30+loc;
            obj.acronym = Attr_id;

            if (loc > 0) {
                __obj(ref count);

                obj.Reset();
                obj.Loc = 30;
                obj.acronym = Attr_id;
                fill_hf(db, dt);

                ulong pos = fmap.Position;
                int count1 = 1;

                if (__obj(ref count1))
                Datatypes(db, list, dt, obj.Offset);

                fmap.Position = pos;
            }
            else {
                fill_hf(db, dt);
                if (__obj(ref count)) 
                Datatypes(db, list, dt, obj.Offset);
            }
        }

        void addDatatype(sodb db, 
                         List<SOAPService.Feature> list, 

                         int owner,

                         string Attr_id, 
                         SOAPService.AttrValue v,
            
                         ref int count)
        {

            string key = attrValue(db, v);
            if (convert.IsString(key)) {

                SOAPService.Feature dt = null;
                foreach (var fe in list)
                if (fe.Key == key)
                { dt = fe; break; }

                if (dt != null)
                    Datatype(db, list, dt, Attr_id, ref count);
                else
                {
                    tnode node = new tnode(owner, Attr_id, key);
                    nodes.Add(node);
                }

            }
        }

        bool obj_shape(sodb db, List<SOAPService.Metadata> meta, ref int count, ref int offset)
        {
            if (__obj(ref count))
            {
                if (count == 1)
                {
                    offset = obj.Offset;
                    __metadata(db,meta, 1, "", ref count);
                }
                else
                {
                    ulong pos = fmap.Position;
                    int count1 = 1;
                    __metadata(db,meta, 1, "", ref count1);
                    fmap.Position = pos;
                }

                return true;
            }

            return false;
        }

     	public void Features(sodb db, List<SOAPService.Feature> list)
        {
     		foreach(var fe in list)
            if (fe.IsRealFeature) {

                obj.Loc = 0;
     			obj.acronym=fe.Cl_Id;
     			xmap.IPoly mf=obj.mf;
     			obj.Reset();

				SOAPService.Point pt = fe.BaseShapePoint;
				SOAPService.Polyline ln = fe.BaseShapePolyline;
                SOAPService.Polygon rgn = fe.BaseShapePolygon;

                fill_hf(db,fe); 

     			fmap.SeekLayer(obj.Code);
				int __count = 0;
                int __offs = 0;

                if (rgn != null)
                if (rgn.Rings.Count > 0) {
                    obj.Loc = 3;

                    foreach (var ring in rgn.Rings)
                        __contour(mf, ring.Points, 3);

                    obj_shape(db,rgn.Metadata, ref __count, ref __offs);
                }

				if (ln != null) 
				if (ln.Paths.Count > 0) {

                    obj.Loc = 2;
                    if (__count > 0) { 
                        obj.Reset(); obj.Loc = 102;
                    }
					
					foreach(var path in ln.Paths) 
					__contour(mf,path.Points,2);

                    obj_shape(db,ln.Metadata, ref __count, ref __offs);
                }
				
				if (pt != null) {
                    obj.Loc = 1;
                    if (__count > 0) {
                        obj.Reset(); obj.Loc = 101;
                    }

					mf.AddPoint(pt.Y*DegToRad,pt.X*DegToRad);
					mf.endContour(1);

                    obj_shape(db,pt.Metadata, ref __count, ref __offs);
                }

                if (__offs > 0) {

                    __metadata(db,fe.Metadata, 0, "", ref __count);     

                    // datatypes + attrv
                    List<SOAPService.Attribute> Attributes = fe.Attributes;

                    if (Attributes != null)
                    foreach (var a in Attributes)
                    foreach (var v in a.Values)
                    if (v.Value != null)

                    if (a.IsCompositeAttribute)
                        addDatatype(db, list, __offs, a.Attr_Id, v, ref __count);
                    else {
                        SOAPService.Geometry shp = v.Shape;
                        if (shp != null)
                        {
                            obj.Reset();
                            obj.acronym = a.Attr_Id;
                            attr_shape(shp);

                            string s = attrValue(db, v);

                            if (s != null)
                                if (s.Length > 0)
                                {
                                    xmap.IAttrs hf = obj.hf;
                                    hf.Add("Key", shp.Key);
                                    hf.Add(a.Attr_Id, s);

                                    if (hf.Count > 1)
                                        __obj(ref __count);
                                }
                        }
                        else  {
                            if (v.Metadata != null)
                            __metadata(db, v.Metadata, 2, a.Attr_Id, ref __count);
                        }
                    }

                    // roles
                    List<SOAPService.Relation> relations = fe.Relations;
                    if (relations != null)
                    foreach (var relation in relations) 
                    if (relation.Role == 0) {

                        SOAPService.RelationRule rule = relation.RelationRule;
                        if (rule != null) { 
                            string s = rule.Cl_Id + "/" + rule.Name;
                            int typ = (int)rule.Type;
                            fmap.Add_Role(__offs, relation.Key, s, typ);
                        }
                    }

                    fCount++;

                    if (__count > 1) {
                        int ptr = fmap.Goto_upper;
                    }
                }
     		}
            else {

                string key = fe.Key;
                foreach(var node in nodes)
                if (node.key == key) {

                    int ptr = fmap.Seek_Node[node.owner,""];
                    if (ptr == node.owner)
                    {
                        int count = 1;
                        Datatype(db, list, fe, node.acronym, ref count);
                    }

                    nodes.Remove(node);
                    break;
                }
            }
		}
     	
     	public int GetCount() { return fCount; }
     		
		public string GetPath() { return fPath; }
	}
}
