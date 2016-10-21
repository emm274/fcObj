/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 26.02.2015
 * Time: 14:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Ini;
using FC;
using FC.Extensions;
using objData;

namespace fcSync
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		string DBpath;	
		string OBJpath;

		void DBChanged() {
			string s="Каталог";

			lbLog.Items.Clear();
			
			if (File.Exists(DBpath)) {
				s+=" - "+Path.GetFileName(DBpath);
				lbLog.Items.Add(DBpath);
			}
			
			this.Text=s;
		}
		
		void OBJChanged() {
			DBChanged();
			
			bool en = false;
			
			if (File.Exists(OBJpath)) {
				lbLog.Items.Add(OBJpath);
				en=true;
			}
			
			btSync.Enabled=en;
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			IniFile ini = new IniFile("");
			ini.ReadForm(Name,this);
			
			DBpath=ini.ReadFile("DB");
			OBJpath=ini.ReadFile("OBJ");
			OBJChanged();
			
		}
		
		void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			IniFile ini = new IniFile("");
			ini.WriteForm(Name,this);
			
			ini.WriteFile("DB",DBpath);
			ini.WriteFile("OBJ",OBJpath);
		}
		
		void BtDBClick(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			
			if (File.Exists(DBpath))  
			dlg.InitialDirectory=Path.GetDirectoryName(DBpath);
			
			dlg.Filter = "dataBases (*.mdb)|*.mdb";
		    dlg.RestoreDirectory = true ;

		    if (dlg.ShowDialog() == DialogResult.OK) {
	    	
		    	DBpath=dlg.FileName;
				DBChanged();
		    }			
	
		}
		
		void paTopResize(object sender, EventArgs e)
		{
			progressBar.Width=this.paTop.Width-16-progressBar.Left;
		}
		
		void __progress(int pos, int max) {
			
			if (max > 0) progressBar.Maximum=max;
			if (pos < 0) pos=progressBar.Value+1;
			progressBar.Value=pos;
		}
		
		public int log(string str) {
			if (!this.Visible) this.Show();
			
			string s=str;
     		if ((s.Length > 0) && (s[0] == '^')) {
				s=s.Remove(0,1);
				int i=this.lbLog.Items.Count-1;
	  			this.lbLog.Items.RemoveAt(i);
	  			if (s.Length == 0) return 0;
			}
			
			this.lbLog.Items.Add(s);
			return 1;
		}
		
		void BtObjClick(object sender, EventArgs e)
		{

			OpenFileDialog dlg = new OpenFileDialog();
			
			if (File.Exists(OBJpath))  
			dlg.InitialDirectory=Path.GetDirectoryName(OBJpath);
			
			dlg.Filter = "Objects (*.obj)|*.obj";
		    dlg.RestoreDirectory = true ;

		    if (dlg.ShowDialog() == DialogResult.OK) {
		    	
		    	string s=dlg.FileName;
			    if (File.Exists(s)) {
		    		OBJpath=s; OBJChanged();
		    	}
		    }
		}
		
		void btSyncClick(object sender, EventArgs e)
		{
			
			if (File.Exists(DBpath)) 
			if (File.Exists(OBJpath)) {
			
				this.btSync.Enabled=false;
				Cursor.Current=Cursors.WaitCursor;
				
				var ds = new FC.Data.DataSource();
				ds.Open(DBpath);
				
				var cat = ds.GetCatalogues().FirstOrDefault();
				if (cat != null)
                {
					var OBJ = new objDB();
					OBJ.Open(OBJpath);
					
					OBJChanged();
				
					OBJ.log=log;
					OBJ.progress=__progress;
			
					progressBar.Visible=true;
			
					OBJ.syncFC(cat,0);
					OBJ.Close();
			
					progressBar.Visible=false;
				}
				
				Cursor.Current=Cursors.Default;
				this.btSync.Enabled=true;
			}
		}
		
	}
}
