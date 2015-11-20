/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 06/24/2015
 * Time: 10:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace fcDmw
{
	/// <summary>
	/// Description of sodbFeatureToText.
	/// </summary>
	public class sodbFeatureToText: sodbFeatureToIntf
	{
		StreamWriter file;
		string fPath;
		int fcount;

        sodb fdb;

		public sodbFeatureToText(string Path)
		{
			file=new System.IO.StreamWriter(Path);
			fPath=Path;
		}

        public void Error(string msg)
        {
            file.WriteLine("*** "+msg);
        }

		void writeKey(string Key,string Value)
		{
			file.WriteLine(Key+" "+Value);
		}
		
		void writeKeys(string Key,string Value)
		{
            if (Value != null)
                file.WriteLine(Key + " \"" + Value + "\"");
            else
                file.WriteLine(Key);
		}
		
		void writeKeyi(string Key,int Value)
		{
			file.WriteLine(Key+String.Format(" {0:0}",Value ));
		}

        void writeKeyss(string Key, string Value, SOAPService.EditableStatus status)
        {
            if (Value == null)
                file.WriteLine(Key);
            else
            {
                string s = Key + " \"" + Value + "\"";

                if (status != SOAPService.EditableStatus.Unchanged)
                    s += " " + fdb.EditableStatusStr(status);

                file.WriteLine(s);
            }
        }

        void writeKeyis(string Key, int Value, SOAPService.EditableStatus status)
        {
            string v = String.Format(" {0:0}", Value);

            if (status != SOAPService.EditableStatus.Unchanged)
            v += " " + fdb.EditableStatusStr(status);

            file.WriteLine(Key + v);
        }

        void writePoints(List<SOAPService.Point> pp)
		{
			file.WriteLine(pp.Count);
			foreach(var p in pp) 
			file.WriteLine(String.Format("{0:0.0000000000} {1:0.0000000000}",p.Y,p.X));
		}

		public void Close()
		{
			file.Close();
		}

		public string GetPath() { return fPath; }
		
		public void Extent(double x1,double y1,double x2, double y2)
		{
			fcount=0;
			file.WriteLine(String.Format("extent {0:0.000000} {1:0.000000} {2:0.000000} {3:0.000000}",x1,y1,x2,y2));
		}

        public void workDir(string s) { }

		public int GetCount() { return fcount; }

		void writePoint(SOAPService.Point pt) {
			string v = String.Format("{0:0.0000000000} {1:0.0000000000}",pt.Y,pt.X);
			writeKeyss("Point",v,pt.Status);
		}
		
		void writePolyline(SOAPService.Polyline pl) {
			if (pl.Paths.Count > 0) {
				writeKeyis("Polyline",pl.Paths.Count,pl.Status);
				foreach(var path in pl.Paths) 
				writePoints(path.Points);
			}
		}

		void writePolygon(SOAPService.Polygon rgn) {
			if (rgn.Rings.Count > 0) {
                writeKeyis("Polygon",rgn.Rings.Count,rgn.Status);
				foreach(var ring in rgn.Rings)
				writePoints(ring.Points);
			}
		}

        void writeMetaData(sodb db, SOAPService.Metadata m, string tab)
        {
            string s = tab + "\"" + m.Akronim + "\" \"" + m.Metadata_Cl_Id + "\"";

            if (m.Status != SOAPService.EditableStatus.Unchanged)
            s += " " + fdb.EditableStatusStr(m.Status);

            if (m.IsCompositeAttribute)
            {
                if (db.MetadataExists(m.CompositeMetadata))
                {
                    file.WriteLine(s);
                    file.WriteLine(tab + "{");

                    if (m.CompositeMetadata != null)
                    if (m.CompositeMetadata.Count > 0)
                    foreach (var m1 in m.CompositeMetadata)
                    writeMetaData(db,m1, tab + "\t");

                    file.WriteLine(tab + "}");
                }
            }
            else
            {
                string t = "";    
                if (m.Values != null)
                    foreach (var v in m.Values)
                    {
                        if (t.Length > 0) t += " ";

                        if (v.Status != SOAPService.EditableStatus.Unchanged)
                        t += "/" + fdb.EditableStatusStr(v.Status) + "/ ";

                        t+="\"" + v.Value + "\"";
                    }

                if (t.Length > 0)
                file.WriteLine(s+" "+t);
            }
        }

        void writeMetadatas(sodb db, List<SOAPService.Metadata> list, string capt)
        {
            if (db.MetadataExists(list)) 
            {
                file.WriteLine("metadata " + capt + " {");

                foreach (var m in list)
                    writeMetaData(db,m, "\t");

                file.WriteLine("}");
            }
        }

        void writeAttributes(sodb db, SOAPService.Feature fe, List<string> datatypes)
        {
            List<SOAPService.Attribute> Attributes = fe.Attributes;

            if (Attributes != null)
            foreach (var a in Attributes)

            if (a.Values != null)
            foreach (var v in a.Values)
            if (v.Value != null)
            {

                SOAPService.NumberStringAttrValue vv = v.Value;

                string s = "", t = "";
                if (vv is SOAPService.NumberValue)
                {
                    s = db.NumberToStr(vv as SOAPService.NumberValue);
                    t = "N";
                }
                else if (vv is SOAPService.StringValue)
                {
                    s = (vv as SOAPService.StringValue).Text;
                    t = "S";

                    if (a.IsCompositeAttribute) datatypes.Add(s);
                }

                string typ = a.Type.ToString();

                string k = String.Format("attribute {0}[{1}][{2}]", a.Attr_Id, typ, t);

                if (a.IsCompositeAttribute) k += "[C]";

                writeKeyss(k,s,v.Status);

                SOAPService.Geometry shp = v.Shape;
                if (shp != null)
                {
                    writeKey("Geometry", shp.Key);

                    if (shp.Type == SOAPService.GeometryType.Point)
                    {
                        SOAPService.Point pt = (SOAPService.Point)shp;
                        writePoint(pt);
                    }
                    else
                    if (shp.Type == SOAPService.GeometryType.Polyline)
                    {
                        SOAPService.Polyline ln = (SOAPService.Polyline)shp;
                        writePolyline(ln);
                    }
                    else
                    if (shp.Type == SOAPService.GeometryType.Polygon)
                    {
                        SOAPService.Polygon rgn = (SOAPService.Polygon)shp;
                        writePolygon(rgn);
                    }
                }

                writeMetadatas(db,v.Metadata,a.Attr_Id);
           }
        }

        void writeDatatypes(sodb db, List<SOAPService.Feature> list, List<string> keys)
        {
            foreach (var key in keys) {
                foreach (var fe in list)
                if (fe.Key == key) { 
                    writeFeature(db,list,fe); 
                    break; 
                }
            }
        }

        void writeFeature(sodb db,  List<SOAPService.Feature> list, SOAPService.Feature fe)
        {
            fdb = db;

            file.WriteLine("{");

            writeKey("Key", fe.Key);
            writeKey("Acronym", fe.Cl_Id);

            writeMetadatas(db,fe.Metadata, "object");

            SOAPService.Point pt = fe.BaseShapePoint;
            SOAPService.Polyline ln = fe.BaseShapePolyline;
            SOAPService.Polygon rgn = fe.BaseShapePolygon;

            if (pt != null)
            {
                writePoint(pt);
                writeMetadatas(db,pt.Metadata, "point");
            }

            if (ln != null)
            {
                writePolyline(ln);
                writeMetadatas(db,ln.Metadata, "polyline");
            }

            if (rgn != null)
            {
                writePolygon(rgn);
                writeMetadatas(db,rgn.Metadata, "polygon");
            }

            List<string> datatypes = new List<string>();
            writeAttributes(db, fe, datatypes);

            List<SOAPService.Relation> relations = fe.Relations;
 
            if (relations != null)
            foreach (var relation in relations)
            {
                string s;

                s = String.Format("relation {0} role={1}", relation.Key, relation.Role);
                if (relation.Status != SOAPService.EditableStatus.Unchanged)
                s += " " + fdb.EditableStatusStr(relation.Status);

                file.WriteLine(s);

                SOAPService.RelationRule rule = relation.RelationRule;
                if (rule != null)
                {
                    s = String.Format("rule {0} {1} {2} {3} // {4}",
                        rule.Cl_Id, rule.Source.Cl_Id, rule.Target.Cl_Id, 
                        rule.Type,rule.Name);
                    file.WriteLine(s);
                }
            }

            file.WriteLine("}");
            file.WriteLine("");

            writeDatatypes(db, list,datatypes);
        } 

		public void Features(sodb db,  List<SOAPService.Feature> list) 
        {
			fcount+=list.Count;
            foreach (var fe in list)
            writeFeature(db,list,fe);
		}
	}
}
