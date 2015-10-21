/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 09.06.2014
 * Time: 21:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Ini
{
    public class IniCustom
    {
        public virtual void WriteValue(string Section, string Key, string Value) {}

        public virtual string ReadValue(string Section, string Key) { return ""; }

        public void WriteForm(Form form)
        {
            WriteValue("Forms", form.Name, String.Format("{0} {1} {2} {3}",
                form.Location.X,
                form.Location.Y,
                form.Size.Width,
                form.Size.Height));
        }

        public bool ReadForm(Form form)
        {
            string s = ReadValue("Forms", form.Name);
            if (s.Length > 0)
            {

                string[] ts = s.Split(' ');
                if (ts.Length == 4)
                {

                    int[] v = new int[4];

                    int k = 0;
                    foreach (string t in ts)

                        if (Int32.TryParse(t, out v[k]))
                            k++;
                        else
                            break;

                    if (k == 4)
                    {
                        form.SetBounds(v[0], v[1], v[2], v[3]);
                        return true;
                    }
                }
            }

            return false;
        }

    }

    class TSection
    {
        string fName;
        public string Name { get { return fName; } }

        Dictionary<string, string> data;

        public TSection(string aName)
        {
            fName = aName;
            data = new Dictionary<string, string>();
        }

        public void Add(string key, string value) {

            string v;
            if (data.TryGetValue(key, out v)) 
                v = value;
            else   
                data.Add(key, value);
        }

        public string Get(string key)
        {
            string v;
            if (data.TryGetValue(key, out v))
            return v;
            return null;
        }

        public void CopyTo(IniCustom dest)
        {
            foreach (var v in data)
            dest.WriteValue(fName, v.Key, v.Value.ToString());
        }

    }

    public class IniData : IniCustom
    {
        List<TSection> fSections;

        public IniData()
        {
            fSections = new List<TSection>();
        }

        TSection sectionOf(string name)
        {
            foreach(var s in fSections)
            if (s.Name == name) return s;

            return null;
        }

        public override void WriteValue(string Section, string Key, string Value)
        {
            TSection s = sectionOf(Section);
            if (s == null)
            {
                s = new TSection(Section);
                fSections.Add(s);
            }

            if (s != null) s.Add(Key, Value);
        }

        public override string ReadValue(string Section, string Key)
        {
            string v = null;
            TSection s = sectionOf(Section);
            if (s != null) v = s.Get(Key);

            if (v == null) {
                IniFile ini = new IniFile("");
                v = ini.ReadValue(Section, Key);

                if ((v != null) && (v.Length > 0))
                    WriteValue(Section, Key, v);
            }

            return v;
        }
        
        public void CopyTo(IniCustom dest)
        {
            foreach(var s in fSections) 
            s.CopyTo(dest);
        }
        
    }

	public class IniFile : IniCustom {
			
  	  	public string fpath;

  	  	[DllImport("kernel32")]
  	  	private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
  	  
  	  	[DllImport("kernel32")]
  	  	private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

  	  	public IniFile(string FName)
  	  	{
  	  		string s=FName;
  	  		if (s.Length == 0) {
  	  			Module mod = typeof(IniFile).Assembly.GetModules() [0];	
  	  			s=Path.ChangeExtension(mod.Name,".ini");
  	  		}
  	  		
			fpath = Environment.ExpandEnvironmentVariables("%APPDATA%");
			fpath += "\\dmw\\ini\\"; fpath += s;
  	  	}

        public void Reset()
        {
            if (File.Exists(fpath))
            File.Delete(fpath);
        }

  	  	public override void WriteValue(string Section, string Key, string Value)
  	  	{
  	    	if (!Directory.Exists(Path.GetDirectoryName(fpath)))
  	        Directory.CreateDirectory(Path.GetDirectoryName(fpath));
  	         
  	        if (!File.Exists(fpath))
  	        using (File.Create(fpath)) { };
	
           	WritePrivateProfileString(Section, Key, Value, this.fpath);
  	  	}

      	public override string ReadValue(string Section, string Key)
 	  	{
    	     StringBuilder temp = new StringBuilder(255);
    	     int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.fpath);
    	     return temp.ToString();
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

        public void WriteReal(string Key, double v)
        {
            WriteValue("Reals", Key, v.ToString());
        }

        public double ReadReal(double def, string Key)
        {
            string s = ReadValue("Reals", Key);
            double v;
            if (double.TryParse(s, out v)) return v;
            return def;
        }

        public void WriteBool(string Key, bool v)
        {
            int i = 0; if (v) i = 1;
            WriteValue("Ints", Key, i.ToString());
        }

        public bool ReadBool(string Key)
        {
            string s = ReadValue("Ints", Key);
            int v;
            if (int.TryParse(s, out v)) return (v == 1);
            return false;
        }

        public void WriteStr(string Key, string v)
        {
            WriteValue("Strs", Key, v);
        }

        public string ReadStr(string def, string Key)
        {
            string s = ReadValue("Strs", Key);
            if (s.Length > 0) return s; 
            return def;
        }

        public void WriteSplitContainer(string Key, SplitContainer s)
        {
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
