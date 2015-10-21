/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 15.06.2014
 * Time: 9:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FC;


namespace fcObjc
{
	class Program
	{
		
		string QuotedStr(string s) {
			return "\""+s+"\"";
		}
		
		public static void Main(string[] args)
		{
			Console.WriteLine("fcObjc Demo:");
		
			if (args.Length == 0)
				Console.WriteLine("usage: fcObjc.exe FeatureCatalogue.mdb");	
			else {
				
				string path = args[0];
				if (File.Exists(path) == false) 
					Console.WriteLine(path + "not found");
				{
					Console.WriteLine(path + "\n");
					
					FC.Data.DataSource ds = new FC.Data.DataSource();
					FC.FeatureCatalogue cat;
					
					ds.Open(path);
					cat = ds.GetCatalogues().FirstOrDefault();
					
				    if (cat != null) {
				      
						string fn = Path.ChangeExtension(path,".txt");
						StreamWriter txt = new System.IO.StreamWriter(fn);
        
						txt.WriteLine("[attributes]");
						
						// пример 
						// IList<FeatureAttribute> attrs = ds.GetAttributes(cat);
						// foreach(FC.FeatureAttribute a in attrs)
						
	  			    	//Атрибуты полный список
	  			    	foreach(FC.FeatureAttribute a in ds.GetAttributes(cat)) {
	  			    		
	  			    		txt.WriteLine("\"" + a.MemberName + "\" \"" + a.ValueType +'"');
		    			    
		    			    if (a.ListedValues.Count > 0) {
		    			    		
		    			    	txt.WriteLine("\t{");
		    			    	
			    			    //домены значений атрибутов
		    				   	foreach (var lv in a.ListedValues)
 						   		txt.WriteLine("\t" + lv.Code + " \"" + lv.Label + '"');
		    				    	
				    			txt.WriteLine("\t}");
		    			    }
  			    		
	  			    	}
	  			    	
		    			txt.WriteLine("[end_attributes]");
		    			txt.WriteLine("");
		    			
						txt.WriteLine("[objects]");
						
						foreach(FeatureType ft in cat.FeatureTypes) {
							
		    				Console.WriteLine(ft.Code  + " " + ft.TypeName);
		    				txt.WriteLine(ft.Code + " \"" + ft.TypeName + "\"");
		    			    
		    			    if (ft.GetAttibutes().Count > 0) {
		    			    
		    					txt.WriteLine("\t[attributes]");
		    			    	
		    			    	//Атрибуты объекта
		    			    	foreach(FC.FeatureAttribute a in ft.GetAttibutes())
		    			    	{
		    			    		txt.WriteLine("\t\"" + a.MemberName + "\" \"" + a.ValueType +'"');
		    			    	}
		    			    	
		    			    	txt.WriteLine("\t[end_attributes]");
		    			    }
		    				
		    				IList <AssociationRole> rlist = ft.GetAssociationRoles();
		    				if (rlist != null) 
		    				if (rlist.Count > 0) {
		    					txt.WriteLine("\t[roles]");
		    			    	
		    			    	foreach (AssociationRole r in rlist) {
		    			    		
		    			    		int i = (int)r.Type;
		    			    		FeatureType ft2 = r.ValueType;
		    			    		
		    			    		txt.WriteLine("\t" + i.ToString() +" \""+ft2.Code + "\" \"" + ft2.TypeName + "\"");
		    			    	}
		    					
		    			    	txt.WriteLine("\t[end_roles]");
		    				}
		    			    	
				    	}
						
		    			txt.WriteLine("[end_objects]");
		    			txt.Close();
					}

				}
			}
				
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}