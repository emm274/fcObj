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

using Convert;
using ofiles;

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
            if (convert.IsString(s))
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

        public void WriteInt(string Key, int v)
        {
            WriteValue("Ints", Key, v.ToString());
        }

        public int ReadInt(string Key)
        {
            string s = ReadValue("Ints", Key);
            int v;
            if (int.TryParse(s, out v)) return v;
            return 0;
        }

        public void WriteFile(string Key, string path)
        {
            WriteValue("Files", Key, path);
        }

        public string ReadFile(string Key)
        {
            string s = ReadValue("Files", Key);
            if (File.Exists(s)) return s;
            return "";
        }

        public void WriteDir(string Key, string path)
        {
            WriteValue("Dirs", Key, path);
        }

        public string ReadDir(string Key)
        {
            string s = ReadValue("Dirs", Key);
            if (Directory.Exists(s)) return s;
            return "";
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

            if (convert.IsString(s))
                return s;

            return def;
        }

        public void WriteSplitContainer(string Key, SplitContainer s)
        {
            WriteInt(Key, s.SplitterDistance);
        }

        public void ReadSplitContainer(string Key, SplitContainer s)
        {
            int v = ReadInt(Key);
            if ((v > s.Panel1MinSize) &&
                (v < s.Width - s.Panel1MinSize))
                s.SplitterDistance = v;
        }

    }

    class TSection
    {
        string fName;
        public string Name { get { return fName; } }
        bool fIsQuote = false;

        Dictionary<string, string> data;

        public TSection(string aName, bool aIsQuote)
        {
            fName = aName; fIsQuote = aIsQuote;
            data = new Dictionary<string, string>();
        }

        public void Add(string key, string value) {

            string v;

            if (data.TryGetValue(key, out v)) 
                data[key] = value;
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

        public void dump(StreamWriter sw)
        {
            sw.WriteLine("[" + fName + "]");

            foreach (var v in data)
            {
                string s = v.Value.ToString();
                if (fIsQuote) s = "\"" + s + "\"";
                sw.WriteLine(v.Key + "=" + s);
            }

            sw.WriteLine("[end_" + fName + "]\n");
        }

    }

    public class IniData : IniCustom
    {
        List<TSection> fSections;

        public IniData()
        {
            fSections = new List<TSection>();
        }

        bool getTag(string str, out string tag)
        {
            tag = null;
            string s = str.Trim();
            if (s.StartsWith("["))
            if (s.EndsWith("]"))
            if (s.Length > 2)
            {
                tag = s.Substring(1, s.Length - 2);
                return true;
            }

            return false;
        }

        public void loadFrom(string FName)
        {
            if (fSections.Count == 0)
            {
                fSections.Add(new TSection("Forms", true));
                fSections.Add(new TSection("Files", true));
                fSections.Add(new TSection("Dirs", true));
                fSections.Add(new TSection("Strs", true));

                fSections.Add(new TSection("Ints", false));
                fSections.Add(new TSection("Reals", false));
            }

  	  		string s=FName;
  	  		if (!convert.IsString(s)) {
  	  			Module mod = typeof(IniFile).Assembly.GetModules() [0];	
  	  			s=Path.ChangeExtension(mod.Name,".ini");
            }

            if (convert.IsString(s))
            try 
            {
                s = OFiles.GetIniPath(s);
                using (StreamReader sr = new StreamReader(s)) 
                {
                    string sect = null;

                    while (sr.Peek() >= 0) {
                        s=sr.ReadLine();
                        if (convert.IsString(s))
                        {
                            string t;
                            if (getTag(s, out t))
                            {
                                if (sect == null)
                                    sect = t;
                                else
                                    sect = null;
                            } else
                            if (sect != null)
                            {
                                int i = s.IndexOf("=");
                                int l = s.Length;
                                if ((i > 0) && (i < l - 1))
                                {
                                    t = s.Substring(i + 1);
                                    if (t.StartsWith("\""))
                                    if (t.EndsWith("\""))
                                    t = t.Substring(1, t.Length - 2);

                                    s = s.Substring(0, i);
                                    WriteValue(sect, s, t);
                                } 
                            }
                        }
                    }
                }
            }
            catch (Exception) {
            }
        }

        public void saveAs(string FName)
        {
            string s = FName;
            if (!convert.IsString(s))
            {
                Module mod = typeof(IniFile).Assembly.GetModules()[0];
                s = Path.ChangeExtension(mod.Name, ".ini");
            }

            if (convert.IsString(s))
            try
            {
                s = OFiles.GetIniPath(s);
                using (StreamWriter sw = new StreamWriter(s))
                {
                    foreach (var sect in fSections)
                    sect.dump(sw);
                }
            }
            catch (Exception)
            {
            }
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
            if (s != null) s.Add(Key, Value);
        }

        public override string ReadValue(string Section, string Key)
        {
            string v = null;
            TSection s = sectionOf(Section);
            if (s != null) v = s.Get(Key);
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

	}
}
