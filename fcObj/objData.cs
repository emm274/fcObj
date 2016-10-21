/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.02.2015
 * Time: 11:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;

using xobj;
using FC;
using FC.Extensions;
using Convert;

namespace objData
{
	
	public class objDB
	{
		xobj.Ixobj_auto fobj;
		xobj.Iattr fidx;
		xobj.Ienum fenum;
		
		List<string> fdtList;
		List<string> fdtList0;
		int fdtUpdCount;
		int fdtNewCount;
		int fdtCount;
		
		List<string> fattrList;
		int fattrUpdCount;
		int fattrNewCount;

		int fenumCount;
		int fenumUpdCount;
		int fenumNewCount;
		
		int fobjCount;
		int fobjUpdCount;
		int fobjNewCount;
		
		int froleCount;
		int froleUpdCount;
		int froleNewCount;
		
		int ferrCount;
		
		string fobjPath;
		
	  	public delegate int tlog(string message);
	  	public delegate void tprogress(int pos, int max);
	
		private tlog flog = null;
		private tprogress fprogress;
		
		public tlog log {
			set { flog=value; }
		}

		public tprogress progress {
			set { fprogress=value; }
		}

		void __err() {
			ferrCount++;
		}
		
		void __log(string msg) {
			if (flog != null) flog(msg);
		}
		
		void log_int(string capt, int v) {
			__log(String.Format("{0}={1}.",capt,v));
		}
		
		void __progress(int pos, int max) {
			if (fprogress != null) fprogress(pos,max);
		}
		
		public objDB()
		{
			fobj = new xobj_autoClass();
			
			fdtList = new List<string>();
			fdtList0 = new List<string>();
			fattrList = new List<string>();
		}
		
		public int Open(string path)
        {
            if (fobj == null)
                return 0;

			fobj.Open(path);
			fidx=fobj.GetAttributes;
			fenum=fobj.GetEnum;
			
			fobjPath=path;
			return fobj.FeatureCount;
		}
		
		public void Close()
        {
            if (fobj != null)
                fobj.Close();
		}
		
		public int getFeatureTypes(ListBox.ObjectCollection list)
		{
			list.Clear();
			
			int n=fobj.FeatureCount;
			int i;
			
			for (i=0; i<n; i++) {
				
				xobj.Ifeature ft = fobj.Feature[i];
				
				if (ft != null) 
					list.Add(ft.acronym+": "+
							 ft.Caption);
				
			}
			
			return list.Count;
		}
		
		// цикл по всем  атрибутам -- не используется
		void syncAllAttr(FC.FeatureCatalogue catalog) {
			
			var attrs = catalog.GetAttributes();
			if (attrs == null) 
				__log("FC.FeatureCatalogue.GetAttributes == null");
			else {
				
				__log("атрибуты...");
				Application.DoEvents();
				
				foreach(var attr in attrs) {
					string key=attr.Code;
					if (convert.IsString(key)) 
					if (!fattrList.Contains(key)) {
					
						fattrList.Add(key);
						
						string typ=attr.ValueType;
						string capt=attr.ToString();
							
						int rc;
						fidx.sync(key,typ,capt,out rc);
						
						if (rc == 1) fattrUpdCount++; else
						if (rc == 2) fattrNewCount++; else
						if (rc != 0) __err();
					}
							
					Application.DoEvents();
				}
				
				__log("^");
			}
		}
		
		// синхронизация домена
		int syncEnum(FC.FeatureAttribute attr) {
			
			int enumw=0;
			
			if (fenum != null) {
				
				// получить домен
				IList<FC.ListedValue> values = attr.GetListedValuesList();
				if (values != null) {
					
					fenum.beginUpdate(); 
					
				    // цикл по записям домена
				    foreach(var v in values) {
				    	string s=v.Code; int i;	
				    	if (!Int32.TryParse(s,out i)) enumw=1;
						fenum.addItem(s,v.Label,v.Definition);
				    }
					
					int rc;
					fenum.endUpdate(attr.Code,out rc);

					fenumCount++;
					if (rc == 1) fenumUpdCount++; else
					if (rc == 2) fenumNewCount++; else
					if (rc < 0) __err();
				}
			}
			
			return enumw;
		}
		
		// синхронизация атрибута
		bool syncAttr(xobj.Iblank blank, FC.Binding binding) 
		{
			var attr = binding.PropertyType as FC.FeatureAttribute;
			
			if (attr != null) {
			
				string key=attr.Code;	// акроним
				if (convert.IsString(key)) {
					
					int rc;
					
					string typ=attr.ValueType;
					
					FC.FeatureType dt=null;
					string dt1="";
					
					if (typ == "Класс") {
						dt=binding.GetDataType();	
						if (dt != null) dt1=dt.Code;
					}

   					string Nset=binding.Cardinality;

					int flags=0;
					
					if (!binding.Voidable) flags+=1;

					if (typ == "Домен") {
						var vt = binding.ConstrainedBy.GetConstraint( typeof(ValueTypeConstraint) ) as 
							FC.ValueTypeConstraint.DomainValueType;
						if (vt != null) if (vt.IsMultiValue) flags+=8;
					} else
						
					if (typ == "Дробное интервальное") flags+=8;
					
					if (!fattrList.Contains(key)) {
				
						fattrList.Add(key); 

	  					if (dt != null) syncDT(dt);
	  					if (typ == "Домен") {
	  						rc=syncEnum(attr);
	  						if (rc == 0) typ="dbase";
	  					}
	  					
						string capt=attr.ToString();
						fidx.sync(key,typ,capt,out rc);
						
						if (rc == 1) fattrUpdCount++; else
						if (rc == 2) fattrNewCount++; else
						if (rc != 0) __err();
					}

					blank.attr(key,Nset,dt1,flags,out rc);
					
					if (rc < 0) __err();
					
					return true;
				}
			}
			
			return false;
		}
		
		// синхронизация родительских бланков
		int syncParentDT(xobj.Iblank blank, FC.FeatureType dt) {
			
			var rc =0;
			var parents = dt.InheritsFrom;
			if (parents != null) 
			foreach (var parent in parents) {
				FC.FeatureType up=parent.Supertype;
				if (up != null) {
					string s=up.Code;
					if (convert.IsString(s)) 
					if (syncDT(up) > 0) {
						int rc1;
						if (!up.IsDataType()) s="$"+s;
						blank.attr("$inherited","",s,0,out rc1);
						if (rc1 < 0) __err(); else rc++;
					}
				}
			}
			
			return rc;
		}
		
		// синхронизация бланка
		int syncDT(FC.FeatureType dt) {
			
			int rc=0;
			
			string key=dt.Code;
			if (convert.IsString(key)) 
				
			if (fdtList.Contains(key)) {
				rc=1;
				if (fdtList0.Contains(key)) rc=0;
			}
			else	
			if (!fdtList.Contains(key)) {
				fdtList.Add(key); 
				
				var blank = fobj.GetBlank;
				if (blank == null)
					__err();
				else {
					string t=key;
					if (dt.IsDataType()) 
					t+="/"+dt.TypeName;
					blank.beginUpdate(t);
					
					rc=syncParentDT(blank,dt);
					
					foreach (var binding in dt.Bindings) 
					if (syncAttr(blank,binding)) rc++;
					
					if (rc > 0) {
						var es = dt.ConstrainedBy.GetConstraint( typeof(AttributeValueConstraint) ) as AttributeValueConstraint;

						int rc1;
						
						if (es != null)
						foreach(var e in es.ValidationRules) {
							blank.logic(e.ОграничениеПоСемантике,
									    e.Комментарий,out rc1);
							if (rc1 < 0) __err();
						}
					
						if (dt.IsDataType())
							blank.endUpdate(key,"",out rc1);
						else	
							blank.endUpdate("",key,out rc1);

						if (rc1 == 1) fdtUpdCount++; else
						if (rc1 == 2) fdtNewCount++; else
						if (rc1 != 0) __err();
						
						if (rc1 >= 0) fdtCount++;
					}
				}
				
				if (rc == 0) fdtList0.Add(key);
				
			}
			
			return rc;
		}
		
		// синхронизация всех datatype's -- не используется 
		void syncAllDT(FC.FeatureCatalogue catalog) {
			
			__log("бланки...");
				
			foreach(var dt in catalog.FeatureTypes) {
				if (dt.IsDataType()) syncDT(dt);
				Application.DoEvents();
			}
			
			__log("^");
		}
		
		int GeomToInt(string s) {
			if (s == "Точка") return 1; else
			if (s == "Линия") return 2; else
			if (s == "Полигон") return 3; 
			return 0;
		}
		
		bool addRole(xobj.Iroles roles,
					 FC.FeatureType owner,
					 FC.AssociationRole role,
		             FC.FeatureType dest, 
		             string Nset) {
			
			FC.AssociationRole.RoleType typ=role.Type;
         				
			string key=role.Code;
			string name=role.MemberName;
			string hf="";
			string tp="";
         						
			var es = owner.ConstrainedBy.GetConstraint( typeof(AssociationRoleConstraint) ) as FC.AssociationRoleConstraint;

			if (es != null) {
				foreach(var e in es.ValidationRules) 
				if (e.AssociationRoleName == name) {
					hf=e.ОграничениеПоСемантике; break;
				}
         						
				foreach(var e in es.TopologicRules) 
				if (e.AssociationRoleName == name) {
       							
					string t=GeomToInt(e.SourceGeom)+","+
							 GeomToInt(e.TargetGeom)+","+
							 e.ОграничениеПоТопологии;
       							
					if (tp.Length > 0) tp+="/";
					tp+=t;
				}
			}
   						
  			string s="";
  			if (key != null) s=key;
  			if (name.Length > 0) {
  				if (s.Length > 0) s+="/";
  				s+=name;
  			}
  			
  			int rc;
  			roles.role(s,((int) typ)-1,dest.Code,Nset,hf,tp,out rc);
  			if (rc < 0) { __err(); return false; }

			return true;
		}

         // добавить связи от связи
        int addRolesFromRole(
            FC.FeatureType owner,
            FC.AssociationRole role,
            FC.FeatureType dest,
            string Nset,
            xobj.Iroles roles)
        {
            if (dest == null)
                return 0;

            int count = 0;

            var childs = dest.InheritsTo;
            if (childs != null)
                if (childs.Count == 0)
                    childs = null;

            if (childs == null)
            {
                bool rc = addRole(roles, owner, role, dest, Nset);
                if (rc) count++;
            }
            else
            {
                foreach (var child in childs)
                {
                    FC.FeatureType dest1 = child.Subtype;
                    count += addRolesFromRole(owner, role, dest1, Nset, roles);                 
                }
            }

            return count;
        }

		// добавить связи объекта  
		int addRoles(FC.FeatureType owner,
					 FC.FeatureType obj,
		             xobj.Iroles roles)
        {
			int count=0;
				
			foreach (var binding in obj.Bindings) {
				var role = binding.PropertyType as FC.AssociationRole;
						
				if (role != null) 
				if (role.IsNavigable) {
                        count += addRolesFromRole(owner, role, role.ValueType, binding.Cardinality, roles);
  				}
			}
			
			return count;
		}
		

		// синхронизация родительских связей
		int syncParentRoles(FC.FeatureType fe, xobj.Iroles roles)
        {
			var count=0;

			var parents = fe.InheritsFrom;
			if (parents != null) 
			foreach (var parent in parents) {
				FC.FeatureType up=parent.Supertype;
				if (up != null) {
                    count += syncParentRoles(up, roles);
					count += addRoles(fe, up, roles);
				}
			}
			
			return count;
		}
		
		// синхронизация связей объекта 
		void syncRoles(FC.FeatureType fe, xobj.Iroles roles)
        {
		
			roles.beginUpdate(); 
			
			int count=syncParentRoles(fe,roles);
			count+= addRoles(fe,fe,roles);

			int rc;
			roles.endUpdate(fe.Code,out rc);

			if (rc < 0) __err(); else {
				if (count > 0) froleCount++;
				if (rc == 1) froleUpdCount++; else
				if (rc == 2) froleNewCount++;
			}
		}
			
		// синхронизация объектов 
		void syncFE(FC.FeatureCatalogue catalog, int mode)
        {
            if ((mode & 1) != 0)
                __log("объекты...");
            else
                __log("связи...");

			__progress(0,catalog.FeatureTypes.Count);
			
			var roles = fobj.GetRoles;
			
			IList<FC.FeatureType> ftlist=catalog.GetFeatureTypesInHierarchicalOrder();
			
			if (ftlist != null) 
			foreach(var ft in ftlist) 
            {

				if (!ft.IsDataType()) 
                {
					string key=ft.Code;
					if (convert.IsString(key)) 
                    {
						if ((mode & 1) != 0) 
                        {
							int loc=0;
							var c = ft.ConstrainedBy.GetConstraint(typeof(GeometryConstraint)) as GeometryConstraint;
							if (c != null) 
                            {
								if (c.Pt) loc+=1;
								if (c.Ln) loc+=2;
								if (c.Pl) loc+=4;
							}
 
							string capt=ft.TypeName;
							int rc;	fobj.sync(key,loc,capt,out rc);
					
							if (rc == 1) fobjUpdCount++; else
							if (rc == 2) fobjNewCount++; else
							if (rc != 0) __err();
					
							fobjCount++; syncDT(ft);
						}
						
						if ((mode & 2) != 0) 
                        {
							if (roles != null) syncRoles(ft,roles);
						}
					}
				}
				
				Application.DoEvents();
				__progress(-1,0);
			}
			
			__log("^");
		}
		
		public void syncFC(FC.FeatureCatalogue catalog, int mode) {
			
			if (catalog !=null) 
			
			if (fidx == null)
				__log("fidx == null");
			else {

				__log("Синхронизация: "+Path.GetFileName(fobjPath));
				__log("");
				
			    fobjCount=0;
			    fobjUpdCount=0;
			    fobjNewCount=0;
				
				fdtList.Clear();
				fdtList0.Clear();
				fdtUpdCount=0;
				fdtNewCount=0;
				fdtCount=0;
			    
				fattrList.Clear();
				fattrUpdCount=0;
				fattrNewCount=0;

				fenumCount=0;
				fenumUpdCount=0;
		        fenumNewCount=0;
				
				froleCount=0;
				froleUpdCount=0;
				froleNewCount=0;
				
				ferrCount=0;
				
				if ((mode & 1) != 0)
				syncAllAttr(catalog);
				
				if ((mode & 2) != 0)
				syncAllDT(catalog);
				
  				syncFE(catalog,1);
  				syncFE(catalog,2);
  				
  				if (fobj.EditFlag > 0) fobj.SaveAs("");
				
				if (fattrList.Count > 0) {
					log_int("атрибуты",fattrList.Count);
					if (fattrUpdCount > 0) log_int("\tизменено",fattrUpdCount);
					if (fattrNewCount > 0) log_int("\tдобавлено",fattrNewCount);
				}

				if (fenumCount > 0) {
					log_int("Домены",fenumCount);
					if (fenumUpdCount > 0) log_int("\tизменено",fenumUpdCount);
					if (fenumNewCount > 0) log_int("\tдобавлено",fenumNewCount);
				}

				if (fdtCount > 0) {
					log_int("бланки",fdtCount);
					if (fdtUpdCount > 0) log_int("\tизменено",fdtUpdCount);
					if (fdtNewCount > 0) log_int("\tдобавлено",fdtNewCount);
				}
				
				if (fobjCount > 0) {
					log_int("объекты",fobjCount);
					if (fobjUpdCount > 0) log_int("\tизменено",fobjUpdCount);
					if (fobjNewCount > 0) log_int("\tдобавлено",fobjNewCount);
				}
				
				if (froleCount > 0) {
					log_int("связи",froleCount);
					if (froleUpdCount > 0) log_int("\tизменено",froleUpdCount);
					if (froleNewCount > 0) log_int("\tдобавлено",froleNewCount);
				}
  				
				if (ferrCount > 0) {
					__log(""); log_int("\tошибки",ferrCount);
				}
			}
		}
	}
}
