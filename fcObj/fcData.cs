/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 10.06.2014
 * Time: 19:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Convert;
using FC;
using FC.Extensions;

namespace fcData
{
	/// <summary>
	/// Description of fcData.
	/// </summary>
	public class fcDB
	{
		
		FC.Data.DataSource fds;
		FC.FeatureCatalogue fcat;
		bool fEnabled;
		
		public fcDB()
		{
			fds = new FC.Data.DataSource();
		}
		
		public bool open(string path) {
			
  			fEnabled=false;
  			fds.Open(path);

            fcat = fds.GetCatalogues().FirstOrDefault();
            fEnabled = (fcat != null);
	
			return fEnabled;
		}
		
		public FC.FeatureCatalogue catalog {
			get { return fcat; }
		}
		
		public int getFeatureTypeList(ListBox.ObjectCollection list)
		{
			list.Clear();
			
			if (fcat != null) {
				foreach(var ft in fcat.FeatureTypes) {
	   				string s=ft.Code;
	   				list.Add(s+": "+ft.TypeName);
				}
	   		}
		
			return list.Count;
		}

		public int getAllAttrs(ListBox.ObjectCollection list) {
			list.Clear();
			
			if (fcat != null) {
				var attrs = fcat.GetAttributes();
				if (attrs != null) {
					int i=0;
					foreach(var attr in attrs) {
						
						string s=attr.Alias;
						if (convert.IsString(s)) {
							s+="<"+attr.ValueType+">";
						
							string t=attr.ToString();
							if (convert.IsString(t)) s+=" // "+t;
							
							i++;
							list.Add(String.Format("{0}. {1}",i,s));
						}
					}
				}
			}
			
			return list.Count;
		}
		
		public int getFeatureTypeAttrs(int index, ListBox.ObjectCollection list)
		{
			list.Clear();
			
			if (fcat != null) {
				FC.FeatureType ft = fcat.FeatureTypes.ElementAt(index);
			
				if (ft != null) {
					
					foreach (var binding in ft.Bindings) {
						var attr = binding.PropertyType as FC.FeatureAttribute;
						
						if (attr != null) {
							
							string s=attr.Alias;
							if (convert.IsString(s)) {
								
								string t=attr.ValueType;
								if (t == "Класс") {
									var dt = binding.GetDataType();
									if (dt != null) t+="-"+dt.Code;
								}
								
								s+="<"+t+">";
								
								t=binding.Cardinality;
								s+=" ["+t+"]";
								
								t=attr.ToString();
								if (convert.IsString(t)) s+=" // "+t;
							
								list.Add(s);
							}
							
						}
					}
					
					
					list.Add("");
					
					var es = ft.ConstrainedBy.GetConstraint( typeof(AttributeValueConstraint) ) as FC.AttributeValueConstraint;

					if (es != null)
					foreach(var e in es.ValidationRules)
					list.Add(String.Format("<{0}>",e.ОграничениеПоСемантике));
					
				}
				
			}
			
			return list.Count;
		}
		
		public int getAttributeListedValues(int index1, int index2, ListBox.ObjectCollection list)
		{
			list.Clear();
			
			IList<FC.FeatureAttribute> attrs=null;
			
			if (fcat != null) 
			
			if (index1 < 0) 
				attrs = fcat.GetAttributes();
			else {
				FC.FeatureType ft = fcat.FeatureTypes.ElementAt(index1);
				if (ft != null) attrs = ft.GetAttibutes();
			}

			if (attrs != null) 
			if ((index2 >= 0) && (index2 < attrs.Count)) {
				FC.FeatureAttribute attr = attrs.ElementAt(index2);
					
				if (attr != null) {
					IList<FC.ListedValue> values = attr.GetListedValuesList();
		
					if (values != null) 
					foreach(var v in values)
					list.Add(v.Code+": "+v.Label);
				}
			}
					
 			return list.Count;
		}
		

		public int getFeatureTypeRoles(int index, 
		                               ListBox.ObjectCollection list,
		                               List<List<string>> topoList)
		{
			list.Clear();
			
			if (fcat != null) 
			if ((index >= 0) && (index < fcat.FeatureTypes.Count)) {
				FC.FeatureType ft = fcat.FeatureTypes.ElementAt(index);
				if (ft != null) {
					
					foreach (var binding in ft.Bindings) {
						var role = binding.PropertyType as FC.AssociationRole;
						
						if (role != null) 
						if (role.IsNavigable) {
						
							var dest = role.ValueType;
							if (dest != null) {
								
								string key=role.Alias;
         						string name=role.MemberName;

								
								string s="";
		  						if (key != null) s=key;
		  						if (name.Length > 0) {
		  							if (s.Length > 0) s+="/";
		  							s+=name;
		  						}
																
								FC.AssociationRole.RoleType typ=role.Type;
								s+="["+typ.ToString()+"]";
								
         						s+=" ["+dest.Code+"]";
         						s+=" ["+binding.Cardinality+"]";
         				
         						var es = ft.ConstrainedBy.GetConstraint( typeof(AssociationRoleConstraint) ) as FC.AssociationRoleConstraint;

         						string t="";
         						
								if (es != null)
         						foreach(var e in es.ValidationRules) 
         						if (e.AssociationRoleName == name) {
         							t=e.ОграничениеПоСемантике; break;
         						}
         						
								if (t.Length > 0) s+=" \""+t+"\"";
								
								list.Add(s);

								if (topoList != null) {

									var tmp = new List<string>();
									
									if (es != null)
	         						foreach(var e in es.TopologicRules) 
	         						if (e.AssociationRoleName == name) {
										t=e.SourceGeom+","+e.TargetGeom+": "+e.ОграничениеПоТопологии;
										tmp.Add(t);
									}
									
									if (tmp.Count > 0)	topoList.Add(tmp);
									else				topoList.Add(null);
								}
							}
						}
					}
				}
			}
			
			return list.Count;
		}
		
	}
}
