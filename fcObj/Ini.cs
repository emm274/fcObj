/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 09.06.2014
 * Time: 21:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Ini
{
	/// <summary>
	/// Description of Ini.
	/// </summary>
	public class IniFile {
			
  	  	public string fpath;

  	  	[DllImport("kernel32")]
  	  	private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
  	  
  	  	[DllImport("kernel32")]
  	  	private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

  	  	public IniFile(string FName)
  	  	{
  	  		string s=FName;
  	  		if (s.Length == 0) {
  	  			Module mod = typeof(IniFile).Assembly.GetModules () [0];	
  	  			s=Path.ChangeExtension(mod.Name,".ini");
  	  		}
  	  		
			fpath = Environment.ExpandEnvironmentVariables("%APPDATA%");
			fpath += "\\dmw\\ini\\"; fpath += s;
  	  	}

  	  	public void WriteValue(string Section, string Key, string Value)
  	  	{
  	    	if (!Directory.Exists(Path.GetDirectoryName(fpath)))
  	        Directory.CreateDirectory(Path.GetDirectoryName(fpath));
  	         
  	        if (!File.Exists(fpath))
  	        using (File.Create(fpath)) { };
	
           	WritePrivateProfileString(Section, Key, Value, this.fpath);
  	  	}

      	public string ReadValue(string Section, string Key)
 	  	{
    	     StringBuilder temp = new StringBuilder(255);
    	     int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.fpath);
    	     return temp.ToString();
   	  	}

      	public void WriteForm(string Key, Form form)
      	{
      		WriteValue( "Forms",Key, String.Format("{0} {1} {2} {3}", 
      			form.Location.X,
      			form.Location.Y,
      			form.Size.Width,
      			form.Size.Height) );
      	}
      	
      	public bool ReadForm(string Key, Form form)
      	{
      		string s = ReadValue( "Forms",Key );
      		if (s.Length > 0) {
      			
		        string[] ts = s.Split(' ');
		        if (ts.Length == 4) {

		        	int[] v = new int[4];
		        	
		        	int k=0;
					foreach (string t in ts)
						
					if (Int32.TryParse(t,out v[k]))
						k++;
					else
						break;
						
					if (k == 4) {
						form.SetBounds(v[0],v[1],v[2],v[3]);
		        		return true;
					}
		        }
      		}
      		
      		return false;
      	}
      	
      	public void WriteFile(string Key, string path) {
      		WriteValue( "Files",Key,path );
      	}
      	
      	public string ReadFile(string Key) {
      		string s = ReadValue( "Files",Key );
      		if (File.Exists(s)) return s;
      		return "";
      	}
      	
      	public void WriteDir(string Key, string path) {
      		WriteValue( "Dirs",Key,path );
      	}
      	
      	public string ReadDir(string Key) {
      		string s = ReadValue( "Files",Key );
      		if (Directory.Exists(s)) return s;
      		return "";
      	}
      	
      	public void WriteInt(string Key, int v) {
      		WriteValue("Ints",Key,v.ToString());
      	}
      	
      	
      	public int ReadInt(string Key) {
      		string s = ReadValue( "Ints",Key );
      		int v;
      		if (int.TryParse(s,out v)) return v;
      		return 0;
      	}
      	
      	
      	public void WriteSplitContainer(string Key, SplitContainer s) {
      		WriteInt(Key,s.SplitterDistance);
		}
      	
      	public void ReadSplitContainer(string Key, SplitContainer s) {
      		int v=ReadInt(Key);
      		if ((v > s.Panel1MinSize) &&
      		    (v < s.Width-s.Panel1MinSize))
      			s.SplitterDistance=v;
      	}
      	
	}
}
