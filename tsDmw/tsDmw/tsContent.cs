#define test_enabled

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using otypes;
using tsObjects;
using xjson;

namespace tsContent
{

    interface tsObjectIntf : jsonListener
    {
        void reset(string key);
    }

    public class jsonObject : tsObjectIntf
    {
        jsonListener fparent = null;

        string fkey = "";

        public jsonObject(jsonListener Aparent)
        {
            fparent = Aparent;
        }

        public void __open(XJson doc, string key)
        {
            doc.setListener(this);
            fkey = key; reset(key);
        }

        public virtual void beginObject(XJson doc, string k)
        {
        }

        public virtual void endObject(XJson doc, string k)
        {
            if (fkey == k) {
                doc.setListener(fparent);
                close();
            }
        }

        public virtual void endArray(string k) {}

        public virtual void value(string k, int index, JsonToken typ, Object v) {}

        public virtual void reset(string key) {}

        public virtual void close() {}
    }

    class jsonContent : jsonObject
    {
        jsonData fData;
        XJson fjson;
        tsContentTo fdoc;

        public jsonContent(jsonListener Aparent,
                           tsContentTo Adoc) : base(Aparent)
        {
            fData = new jsonData(this,Adoc);
            fdoc = Adoc;
        }

        public override void beginObject(XJson doc, string k)
        {
            if (k == "data") fData.__open(fjson,k);
        }

        bool ParseStage(StreamReader stream, int value)
        {
            fdoc.SetStage(value);
            stream.BaseStream.Seek(0, SeekOrigin.Begin);
            fjson.setListener(this);
            return fjson.Parse(stream);
        }

        public bool ParseStream(StreamReader stream, tmessage message)
        {
            fjson = new XJson();
            fjson.message = message;
            fjson.setListener(this);

            bool rc = true;
            if (fdoc != null) {
                tsContentBound bound = new tsContentBound();

                jsonData temp = fData;
                fData = new jsonData(this, bound);

                fjson.setListener(this);
                fjson.Parse(stream);

                fData = temp;

                if (bound.count > 0)
                rc = fdoc.Bound(bound.minLat, bound.minLon, bound.maxLat, bound.maxLon);
            }

            if (rc) 
            if (fdoc == null)
                rc=ParseStage(stream,0);
            else
                for(int i=0; i < fdoc.GetStageCount(); i++ )
                rc=ParseStage(stream,i);

            if (rc) fData.log(message);

            fjson.Close();

            return rc; 
        }

        public bool ParseFile(string path, tmessage message)
        {
            bool rc = false;
            using (StreamReader stream = new StreamReader(path)) {
                rc = ParseStream(stream, message);
            }
            return rc;
        }
    }

    class jsonData : jsonObject
    {
        jsonPoint fPoint;
        jsonPolyline fPolyline;
        jsonPolygon fPolygon;
        jsonFeature fFeature;

        public jsonData(jsonListener Aparent,
                        tsContentTo doc) : base(Aparent)
        {
            fPoint = new jsonPoint(this, doc);
            fPolyline = new jsonPolyline(this, doc);
            fPolygon = new jsonPolygon(this, doc);
            fFeature = new jsonFeature(this, doc);
        }

        public override void reset(string key) 
        {
            fPoint.reset(null);
            fPolyline.reset(null);
            fPolygon.reset(null);
            fFeature.reset(null);
        }

        public override void beginObject(XJson doc, string k)
        {
            string id = null;
            tsObjectRef tag = tsConvert.StrToRef(k,ref id);

            switch (tag)
            {
                case tsObjectRef.refPoint:
                    fPoint.__open(doc,k);
                    break;

                case tsObjectRef.refPolyline:
                    fPolyline.__open(doc, k);
                    break;

                case tsObjectRef.refPolygon:
                    fPolygon.__open(doc, k);
                    break;

                case tsObjectRef.refFeature:
                    fFeature.__open(doc, k);
                    break;

            }
        }

        public override void close() {
            fPoint.stat();
            fPolyline.stat();
            fPolygon.stat();
            fFeature.stat();
        }

        public void log(tmessage message)
        {
            fPoint.log(message);
            fPolyline.log(message);
            fPolygon.log(message);
            fFeature.log(message);
        }

    }

    class jsonPoint : jsonObject
    {
        tsPoint p; 

        public jsonPoint(jsonListener Aparent,
                         tsContentTo doc) : base(Aparent) 
        {
            p = new tsPoint(doc);
        }

        public override void reset(string key) { p.reset(key); }

        public override void close() { p.close(); }

        public void stat() { p.stat(); }

        public override void value(string k, int index, JsonToken typ, Object v)
        {
            if (k == "lat") {
                if (typ == JsonToken.Float)
                p.Lat = (double)v;
            } else
            if (k == "lon") {
                if (typ == JsonToken.Float)
                p.Lon = (double)v;
            }
        }

        public void log(tmessage message)
        {
            p.log(message);
        }
    }

    class jsonPolyline : jsonObject
    {
        tsPolyline c;
        double flat=0, flon=0;
        bool finternalPoints=false;

        public jsonPolyline(jsonListener Aparent,
                            tsContentTo doc) : base(Aparent) 
        {
            c = new tsPolyline(doc);
        }

        public override void reset(string key) 
        { 
            c.reset(key);
            finternalPoints = false;
        }

        public override void close() { c.close(); }

        public void stat() { c.stat(); }

        public override void value(string k, int index, JsonToken typ, Object v)
        {
            if (finternalPoints) {

                if (k == "lat") {
                    if (typ == JsonToken.Float)
                        flat = (double)v;
                } else
                if (k == "lon") {
                    if (typ == JsonToken.Float)
                        flon = (double)v;
                }
            } else
            if (k == "startPoint") {
                if (typ == JsonToken.String)
                    c.startPoint = v.ToString();
            }
            else
            if (k == "endPoint") {
                if (typ == JsonToken.String)
                    c.endPoint = v.ToString();
            }
        }

        public override void beginObject(XJson doc, string k)
        {
            finternalPoints = false;
            if (k != null) finternalPoints = (k == "internalPoints");
        }

        public override void endObject(XJson doc, string k)
        {
            if (k == "internalPoints")
                c.internalPoint(flat, flon);
            else
                base.endObject(doc, k);
        }

        public override void endArray(string k) 
        { 
            if (k == "internalPoints")
            finternalPoints = false;
        }

        public void log(tmessage message)
        {
            c.log(message);
        }
    }

    class jsonPolygon : jsonObject
    {
        tsPolygon s;
        bool frings = false;

        public jsonPolygon(jsonListener Aparent,
                            tsContentTo doc)
            : base(Aparent)
        {
            s = new tsPolygon(doc);
        }

        public override void reset(string key)
        {
            s.reset(key);
            frings = false;
        }

        public override void close() { s.close(); }

        public void stat() { s.stat(); }

        public override void value(string k, int index, JsonToken typ, Object v)
        {
            if (frings && (k == "ref")) {
                if (typ == JsonToken.String)
                s.rings.Add(v.ToString());
            }
        }

        public override void beginObject(XJson doc, string k)
        {
            frings = false;
            if (k != null) frings = (k == "rings");
        }

        public void log(tmessage message)
        {
            s.log(message);
        }
    }

    class jsonFeature : jsonObject
    {
        tsFeature fe;

        bool fIsGeometry;
        bool fIsRelations;

        Geometry fgeometry;
        Relation frelation;

        public jsonFeature(jsonListener Aparent,
                            tsContentTo doc) : base(Aparent) 
        {
            fe = new tsFeature(doc);
            fIsGeometry = false;
            fIsRelations = false;
            fgeometry = new Geometry(1, 0, "");
            frelation = new Relation("", "","");
        }

        public override void reset(string key) 
        { 
            fe.reset(key);
            fIsGeometry = false;
            fIsRelations = false;
            fgeometry.Reset();
            frelation.Reset();
        }

        public override void close() { fe.close(); }

        public void stat() { fe.stat(); }

        public override void beginObject(XJson doc, string k)
        {
            switch(k) 
            {
                case "attributes":
                    if (!fIsRelations)
                    doc.startDumpBlock();
                    break;
                case "geometry":
                    fIsGeometry = true;
                    break;
                case "featureAssociations":
                    fIsRelations = true;
                    break;
            }
        }

        public override void endObject(XJson doc, string k)
        {
            if (k == "attributes") {

                if (!fIsRelations)
                {
                    fe.attrs = doc.stopDumpBlock();

#if (test_enabled)
                    bool rc = new XJson().ParseFromString(fe.attrs);
                    if (!rc)
                    {
                        doc.__message(fe.Name(), "attributes parse error");
                        doc.__message(null, fe.attrs);
                    }
#endif
                }

            } else

            if (k == "geometry") {
                if (fgeometry.Enabled) fe.geometry(fgeometry);
            } else

            if (k == "featureAssociations") {
                if (frelation.Enabled) fe.relation(frelation);
            } 
            
            else base.endObject(doc, k);
        }

        public override void value(string k, int index, JsonToken typ, Object v)
        {
            if (k == "globalId") {
                if (typ == JsonToken.String)
                fe.guid = v.ToString();
            } else
            if (k == "code") {
                if (typ == JsonToken.String)
                fe.code = v.ToString();
            } else
            if (fIsGeometry) {
                if (k == "lower") {
                    if (typ == JsonToken.Float)
                    fgeometry.lower = (double)v;
                } else

                if (k == "upper") {
                    if (typ == JsonToken.Float)
                    fgeometry.upper = (double)v;
                } else

                if (k == "ref") {
                    if (typ == JsonToken.String)
                    fgeometry.key = v.ToString();
                }
            } else

            if (fIsRelations) {
                if (k == "associationCode") {
                    if (typ == JsonToken.String)
                    frelation.code = v.ToString();
                } else

                    if (k == "roleCode")
                    {
                        if (typ == JsonToken.String)
                            frelation.role = v.ToString();
                    }
                    else

                        if (k == "ref")
                        {
                    if (typ == JsonToken.String)
                    frelation.dest = v.ToString();
                } 

            }
        }

        public override void endArray(string k)
        {
            if (k == "geometry")
                fIsGeometry = false;
            else
            if (k == "featureAssociations")
                fIsRelations = false;
        }

        public void log(tmessage message)
        {
            fe.log(message);
        }

    }
}
