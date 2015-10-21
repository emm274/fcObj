/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 18.06.2015
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;

namespace fcDmw
{
	public class ClassList: Dictionary<string,string> 
	{
		
	}
	
	/// <summary>
	/// Description of sodb.
	/// </summary>
	public class sodb
	{
        public delegate void tmsg(string message);

        private tmsg fstatus;
        public tmsg status { set { fstatus = value; } }

        private tmsg fmessage;
        public tmsg message { set { fmessage = value; } }

        private tmsg fendTask;
        public tmsg endTask { set { fendTask = value; } }

        void __status(string message)
        {
            if (fstatus != null) fstatus(message);
        }

        void __message(string capt, string message)
        {
            if (fmessage != null)
            {
                string s = message;
                if (capt != null) s = String.Format("{0}: {1}.", capt, message);
                fmessage(s);
            }
        }

        void __messages(string capt, List<SOAPService.Message> Messages)
        {
            foreach (var msg in Messages)
            __message(capt,msg.Value);
        }

        void __messages1(string capt, List<String> list)
        {
            foreach (var s in list)
            __message(capt,s);
        }

        void __error(string capt, string message)
        {
            __message(capt,message);
        }

        void __endTask(string message)
        {
            if (fendTask != null) fendTask(message);
        }

        SOAPService.SODBServiceClient service = null;
        internal string token = null;
		
        sodbFeatureToIntf fdoc;
        
        int fcount;
        string fkey = "";

        double fx1=0, fy1=0, fx2=0, fy2=0;
        
		public sodb()
		{
			BasicHttpBinding binding = new BasicHttpBinding();
			binding.MaxReceivedMessageSize = Int32.MaxValue;
		}

        string GetPwdHash(string pwd)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pwd);

            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash.ToUpper();
        }


        public bool IsAuthorized
        {
            get { return (token != null); }
        }

        public bool Connected()
        {
            if (IsAuthorized) 
            if (service != null) { 
                CommunicationState st = service.State;
                return (st == CommunicationState.Opened);
            }
            return false;
        }

        public bool Connect(string login, string password)
        {
            if (! IsAuthorized)
            try
            {
                service = new SOAPService.SODBServiceClient();

                try
                {
                    int minutes = 60 * 12;
                    string pwdHash = GetPwdHash(password);

                    service.ClientCredentials.UserName.UserName = login;
                    service.ClientCredentials.UserName.Password = pwdHash;

                    token = service.Authorization(login, pwdHash, minutes);
                    if (token != null) return true;
                }
                catch (Exception ex)
                {
                    __error("Ошибка авторизации", ex.Message);
                }


            }
            catch (Exception ex)
            {
                __error("Ошибка сервиса", ex.Message);
            }

            return IsAuthorized;
        }

        public void Disconnect()
        {
            if (IsAuthorized) 
            try
            {
                service.CloseConnection(token);
            }
            catch (Exception ex)
            { 
                __error("Disconnect",ex.Message); 
            }

            token = null;
            service = null;
        }

        public SOAPService.FeatureClassSchema GetFeatureClassSchema(string Cl_Id) {
            return service.GetFeatureClassSchema(token,Cl_Id);
        }

        public int GetFeatureClasses(ClassList list)
        {
        	list.Clear();
        	
            if (IsAuthorized) {
        		
        		try {
	        		List<SOAPService.FeatureClass> featureClasses = service.GetFeatureClasses(token);
        		
	        		if (featureClasses != null) 
	        		foreach(var ft in featureClasses) {

	        			string typ = ft.ClassType.ToString();
                        if (typ == "FeatureClass")
                        list.Add(ft.Cl_Id, ft.Name);
	        		}
        		}
	            catch(Exception ex)
	            { __error("GetFeatureClasses",ex.Message); }
        		
        	}
        	
        	return list.Count;
        }

        public void SetExtent(double x1, double y1, double x2, double y2)
        {
            fx1 = x1; fy1 = y1; fx2 = x2; fy2 = y2;
        }
        
        public void beginGetFrame() { fcount = 0; }

        public bool GetFrame(string cl_id, double x1, double y1, double x2, double y2, sodbFeatureToIntf doc)
        {
            if (IsAuthorized)
            {
                fdoc = doc; fkey = cl_id; SetExtent(x1, y1, x2, y2); 

                try
                {
                    SOAPService.Consist consist = GetConsist();
                    List<SOAPService.Condition> conditions = new List<SOAPService.Condition>();

                    SOAPService.FeatureRequest request = new SOAPService.FeatureRequest();

                    request.Cl_Id = cl_id;
                    request.Consist = consist;

                    SOAPService.Extent extent = new SOAPService.Extent()
                    {
                        Left = y1, Top = x2, Right = y2, Bottom = x1
                    };

                    SOAPService.RequestResponse requestResponse = service.RequestNSAsync(token, request, extent);
                    var st = service.GetRequestStatus(token, requestResponse.RequestId);

                    Task.Run(() => RequestTask(requestResponse, st));
                    return true;
                }
                catch (Exception ex)
                { __error("GetFrame", ex.Message); }

            }

            return false;
        }

        private void RequestTask(SOAPService.RequestResponse requestResponse, SOAPService.RequestInfo st)
        {
        	try {
        		
	            while ((st = service.GetRequestStatus(token, requestResponse.RequestId)).Status == SOAPService.RequestStatus.Performed)
	            {
	               Thread.Sleep(2000);
	            }

                if (st.Status == SOAPService.RequestStatus.Ready)
                {
                	while (true) {
	                    SOAPService.FeatureResult rc = service.GetRequestResult(token, st.RequestId, 1);

	                    if (rc.Messages.Count > 0) 
	                    __messages(fkey,rc.Messages);

                        int k = rc.Features.Count;

                        if (k > 0)
                        if (fdoc != null) 
                        fdoc.Features(this, rc.Features);
	                    		
	                    if (k == 0) break; fcount+=k;
                	}

                }
                else if (st.Status == SOAPService.RequestStatus.Error)
                    __messages("RequestTask",st.Messages);

        	}
            catch(Exception ex)
            { __error("RequestTask",ex.Message); }

            __endTask("");

        }
        
        SOAPService.Consist GetConsist()
        {
            List<SOAPService.GeometryType> geomTypes = new List<SOAPService.GeometryType>();
            geomTypes.Add((SOAPService.GeometryType)Enum.Parse(typeof(SOAPService.GeometryType), "Point"));
            geomTypes.Add((SOAPService.GeometryType)Enum.Parse(typeof(SOAPService.GeometryType), "Polyline"));
            geomTypes.Add((SOAPService.GeometryType)Enum.Parse(typeof(SOAPService.GeometryType), "Polygon"));

            List<decimal> scales = new List<decimal>();

            SOAPService.Consist consist = new SOAPService.Consist();
            consist.ObjMetaData = false;
            consist.Geometries = true;
            consist.GeomMetaData = false;
            consist.GeoNames = true;
            consist.NameMetaData = false;
            consist.Attributes = true;
            consist.AttrMetaData = false;
            consist.Fields = "*";
            consist.GeomTypes = geomTypes;
            consist.GeomScales = scales;
             
            return consist;
        }

        public SOAPService.Feature GetFeature(string key)
        {
            List<string> keys = new List<string>();
            keys.Add(key);

            SOAPService.Consist consist = GetConsist();

            SOAPService.Extent extent = new SOAPService.Extent()
            {
                Left = fy1,
                Top = fx2,
                Right = fy2,
                Bottom = fx1
            };

            try
            {
                SOAPService.FeatureResult rc = service.GetFeatures(token, keys, consist, extent);

                if (rc.Features.Count > 0)
                    return rc.Features[0];

            }
            catch (Exception ex)
            { __error("GetDatatype", ex.Message); }

            return null;
        }

        public string beginUpdate()
        {
            try
            {
                return service.StartEditing(token);
            }
            catch (Exception ex)
            { __error("StartEditing", ex.Message); }

            return null;
        }

        public void endUpdate(string session)
        {
            try
            {
                service.StopEditing(token, session);
            }
            catch (Exception ex)
            { __error("StopEditing", ex.Message); }

        }

        public void applyUpdate(string session, string capt) {
            try
            {
                var er = service.SaveEditingAsync(token, session, capt);

                var st = service.GetRequestStatus(token, er.RequestId);
                while ((st = service.GetRequestStatus(token, er.RequestId)).Status == SOAPService.RequestStatus.Performed)
                {
                    Thread.Sleep(2000);
                }

                if (st.Status == SOAPService.RequestStatus.Error)
                    __messages("SaveEditingAsync", st.Messages);

            }
            catch (Exception ex)
            { __error("SaveEditingAsync", ex.Message); }

        }

        public List<string> addFeatures(string session, List<SOAPService.Feature> list)
        {
            try
            {
                SOAPService.RequestResponse rc = service.AddFeatures(token, session, list);

                List<string> keys = new List<string>();

                foreach (var v in rc.Values)
                {
                    string key = v.ToString();
                    keys.Add(key);
                    __message("AddFeatures", key);
                }

                return keys;
            }
            catch (Exception ex)
            { __error("AddFeatures", ex.Message); }

            return null;
        }

        public string addFeature(string session, SOAPService.Feature fe) 
        {
            List<SOAPService.Feature> list = new List<SOAPService.Feature>();
            list.Add(fe);

            try
            {
                SOAPService.RequestResponse rc = service.AddFeatures(token, session, list);

                if (rc.Values.Count == 1)
                {
                    string key = rc.Values[0].ToString();
                    if (key != null)
                    if (key.Length > 0)
                    {
                        __message("addFeature", key);
                        return key;
                    }
                }
            }
            catch (Exception ex)
            { __error("addFeature", ex.Message); }

            return null;
        }

        public bool editFeature(string session, SOAPService.Feature fe)
        {
            List<SOAPService.Feature> list = new List<SOAPService.Feature>();
            list.Add(fe);

            try
            {
                SOAPService.RequestResponse rc = service.EditFeatures(token, session, list);
                __message("editFeature", fe.Key);
                __messages1("editFeature", rc.Values);
                return true;
            }
            catch (Exception ex)
            { __error("editFeature", ex.Message); }

            return false;
        }

        public bool deleteFeature(string session, string key)
        {
            List<String> list = new List<String>();
            list.Add(key);

            try
            {
                SOAPService.RequestResponse rc = service.DeleteFeatures(token, session, list);
                __message("deleteFeature",key);
                __messages1("deleteFeature", rc.Values);
                return true;
            }
            catch (Exception ex)
            { __error("deleteFeature", ex.Message); }

            return false;
        }

        public string NumberToStr(SOAPService.NumberValue v)
        {
            string s = "";
            if (v.Upper == null)
            {
                if (v.Lower != null)  {
                    s = ">";
                    if (v.IncludeLower) s += "=";
                    s += v.Lower.ToString();
                }
            }
            else
            if (v.Lower == null)
            {
                if (v.Upper != null) {
                    s = "<";
                    if (v.IncludeUpper) s += "=";
                     s += v.Upper.ToString();
                }
            }
            else
            if ((v.Lower == v.Upper) &&
                 v.IncludeLower &&
                 v.IncludeUpper)
                s = v.Lower.ToString();
            else  {
                s = v.Lower.ToString() + ".." + v.Upper.ToString();
                if (v.IncludeLower) s = "[" + s; else s = "(" + s;
                if (v.IncludeUpper) s += "]"; else s += ")";
            }

            return s;
        }

        public bool compAttrValue(SOAPService.AttrValue v1, SOAPService.AttrValue v2)
        {
            SOAPService.NumberStringAttrValue vv1 = v1.Value;
            SOAPService.NumberStringAttrValue vv2 = v2.Value;

            if ((vv1 is SOAPService.StringValue) &&
                (vv2 is SOAPService.StringValue)) {
                string s1=(vv1 as SOAPService.StringValue).Text;
                string s2=(vv2 as SOAPService.StringValue).Text;
                return (s1 == s2);
            }

            return false;
        }

	}
}
