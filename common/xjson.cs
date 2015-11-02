using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using classMsg;
using Convert;

namespace xjson
{
    public interface jsonListener
    {
        void beginObject(XJson doc, string k);
        void endObject(XJson doc, string k);
        void endArray(string k);
        void value(string k, int index, JsonToken typ, Object v);
    }

    public class XJson: ClassMsg {

        JsonTextReader freader;
        string fstatus = "";
        public string status { get { return fstatus; } }

        Stack<string> fkeys = new Stack<string>();

        jsonListener fListener = null;

        bool fIsDumpBlock = false;
        StringBuilder fdumpBlock = new StringBuilder();
        JsonTextWriter fwriter;

        public bool status_success()
        {
            if (convert.IsString(fstatus))
            return fstatus == "success";
            return false;
        }

        public void startDumpBlock()
        {
            if (fIsDumpBlock) fwriter.Close();

            fdumpBlock.Clear();
            StringWriter sw = new StringWriter(fdumpBlock);
            fwriter = new JsonTextWriter(sw);
            fwriter.WriteStartObject();

            fIsDumpBlock = true;
        }

        public string stopDumpBlock()
        {
            fIsDumpBlock = false;
            if (fwriter != null) fwriter.Close();
            return fdumpBlock.ToString();
        }

        public void setListener(jsonListener value)
        {
            fListener = value; 
        }

        void __parse_error(string capt)
        {
            __message(capt, "parse error");
        }

        protected virtual void beginObject(string k)
        {
            if (fListener != null) fListener.beginObject(this,k);
        }

        protected virtual void endObject(string k)
        {
            if (fListener != null) fListener.endObject(this,k);
        }

        protected virtual void endArray(string k)
        {
            if (fListener != null) fListener.endArray(k);
        }

        protected virtual void value(string k, int index, JsonToken typ, Object v)
        {
            if (fListener != null) fListener.value(k, index, typ, v);
        }

        bool __token(int index)
        {
            bool rc = freader.Read();

            if (rc && fIsDumpBlock) {
                JsonToken k = freader.TokenType;
                Object v = freader.Value;

                switch (k)
                {
                    case JsonToken.StartObject:
                        fwriter.WriteStartObject();
                        break;
                    case JsonToken.EndObject:
                        fwriter.WriteEndObject();
                        break;
                    case JsonToken.StartArray:
                        fwriter.WriteStartArray();
                        break;
                    case JsonToken.EndArray:
                        fwriter.WriteEndArray();
                        break;

                    case JsonToken.PropertyName:
                        if (v != null)
                        fwriter.WritePropertyName(v.ToString());
                        break;

                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.String:
                    case JsonToken.Boolean:
                        if (v != null)
                        fwriter.WriteValue(v);
                        break;
                }
            }

            return rc;
        }

        bool __object(string name)
        {
            if (name != null) {
                fkeys.Push(name);
                beginObject(name);
            }

            bool rc = true; int index = 0;

            while (__token(index))
            {
                JsonToken token = freader.TokenType;
                rc = false;

                if (token == JsonToken.EndObject)
                {
                    rc = true; break;
                }
                else
                if (token == JsonToken.PropertyName) {

                    Object s = freader.Value;
                    if (s != null)
                    rc = __value(s.ToString());
                }

                if (!rc) {
                    __parse_error("__object");
                    break;
                }

                index++;
            }

            if (name != null) {
                endObject(name);
                fkeys.Pop();
            }
           
            return rc;
        }

        bool __array(string name)
        {
            int index = 0;
            bool rc = true;

            while (__token(index)) {

                JsonToken token = freader.TokenType;
                rc = false;

                if (token == JsonToken.EndArray)
                {
                    rc = true; break;
                }
                else
                if (token == JsonToken.StartObject)
                    rc = __object(name);
                else
                if ((token == JsonToken.Integer) ||
                    (token == JsonToken.Float) ||
                    (token == JsonToken.String) ||
                    (token == JsonToken.Boolean)) {
                    Object v = freader.Value;
                    if (v != null) {
                        value(name, index, token, v);
                        rc = true;
                    }
                }
                else
                if (token == JsonToken.Null)
                {
                    rc = true;
                }

                if (!rc) {
                    __parse_error("__array");
                    break;
                }

                index++;
            }

            endArray(name);

            return rc;
        }

        bool __value(string name)
        {
            if (__token(0)) {
                JsonToken token = freader.TokenType;

                if (token == JsonToken.Null)
                    return true;
                else
                if (token == JsonToken.StartObject)
                    return (__object(name));
                else
                if (token == JsonToken.StartArray)
                    return (__array(name));
                else
                if ((token == JsonToken.Integer) ||
                    (token == JsonToken.Float) ||
                    (token == JsonToken.String) ||
                    (token == JsonToken.Boolean))
                {
                    Object v = freader.Value;
                    if (v != null)
                        if (name == "status")
                        {
                            fstatus = v.ToString();
                            return true;
                        }
                        else
                        {
                            value(name, 0, token, v);
                            return true;
                        }
                }
            }

            return false;
        }

        public void Close()
        {
            freader.Close();
        } 

        public bool Parse(StreamReader stream)
        {
            fkeys.Clear();
            fstatus = "";
            fIsDumpBlock = false;

            freader = new JsonTextReader(stream);

            try
            {
                while (__token(0))
                {
                    JsonToken token = freader.TokenType;

                    if (token == JsonToken.StartObject)
                        __object(null);
                    else
                    {
                        if (token != JsonToken.None)
                        {
                            __parse_error("__json");
                            return false;
                        }

                        break;
                    }
                }

            }
            catch (Exception e)
            {
                __message("json", e.Message);
                return false;
            }

            return true;
        }

        public bool ParseFromString(string s)
        {
            return Parse( convert.StringToStream(s) );
        }

    }

    public class XJsonStringWriter
    {
        StringBuilder sb;
        StringWriter sw;
        JsonWriter fwriter;
        public JsonWriter writer { get { return fwriter; } }

        public XJsonStringWriter()
        {
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            fwriter = new JsonTextWriter(sw);
        }

        public byte[] Data()
        {
            return UTF8Encoding.UTF8.GetBytes(sb.ToString());
        }
    }

}
