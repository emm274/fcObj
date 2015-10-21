/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 09.06.2014
 * Time: 11:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization; 

using Ini;
using fcData;
using objData;

namespace fcObj
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{

		dfmReport report;
		
		string DBpath;	
		fcDB DB;
		objDB OBJ;
		
		string OBJpath;
	
		List<List<string>> topoList;
		
		void DBChanged() {
			string s="Каталог";
			
			if (File.Exists(DBpath)) 
			s+=" - "+Path.GetFileName(DBpath);
			
			this.Text=s;
		}
					
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			DB = new fcDB();
			OBJ = new objDB();
			
			topoList = new List<List<string>>();
		}
	
			
		void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
	
			IniFile ini = new IniFile("");
			ini.WriteForm(Name,this);
			
			ini.WriteFile("DB",DBpath);
			ini.WriteFile("OBJ",OBJpath);
			
			ini.WriteSplitContainer("splitter1",splitContainer1);
			ini.WriteSplitContainer("splitter2",splitContainer2);
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			IniFile ini = new IniFile("");
			ini.ReadForm(Name,this);
			
			ini.ReadSplitContainer("splitter1",splitContainer1);
			ini.ReadSplitContainer("splitter2",splitContainer2);
			
			DBpath=ini.ReadFile("DB");
			DBChanged();
			
			OBJpath=ini.ReadFile("OBJ");
			
			report = new dfmReport();
		}

		void BtQueryClick(object sender, EventArgs e)
		{
			this.btQuery.Enabled=false;
			this.lbObj.BeginUpdate();
			this.lbObj.Items.Clear();
			
			if (File.Exists(DBpath)) 
				if (DB.open(DBpath)) 
				DB.getFeatureTypeList(this.lbObj.Items);
			
			if (this.lbObj.Items.Count > 0)
			this.lbObj.Items.Insert(0,"-");
			
			this.lbObj.EndUpdate();
			
			this.lbAttrs.Items.Clear();
			this.lbValues.Items.Clear();
			this.lbRoles.Items.Clear();
			
			this.btQuery.Enabled=true;
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
		    	
		      	DB.open(DBpath);
		      	this.btQuery.PerformClick();
		    }			
		}
		
		void LbObjClick(object sender, EventArgs e)
		{
			
			this.lbAttrs.BeginUpdate();
			this.lbRoles.BeginUpdate();
			
			this.lbAttrs.Items.Clear();
			this.lbRoles.Items.Clear();
			
			topoList.Clear();
			
			int i = this.lbObj.SelectedIndex-1;
			
			if (i < 0) {
				this.lbObj.Enabled=false;
				Cursor.Current=Cursors.WaitCursor;
				DB.getAllAttrs(this.lbAttrs.Items);
				Cursor.Current=Cursors.Default;
				this.lbObj.Enabled=true;
			}
			else {
				DB.getFeatureTypeAttrs(i,this.lbAttrs.Items);
				DB.getFeatureTypeRoles(i,this.lbRoles.Items,topoList);
			}
			
			this.lbAttrs.EndUpdate();
			this.lbRoles.EndUpdate();
			
			this.lbValues.Items.Clear();
			this.lbTopo.Items.Clear();
		}
		
		void PaTopResize(object sender, EventArgs e)
		{
			
		}
		
		void LbAttrsClick(object sender, EventArgs e)
		{
			
			this.lbValues.BeginUpdate();
			this.lbValues.Items.Clear();

			int i=this.lbObj.SelectedIndex;
			int j=this.lbAttrs.SelectedIndex;
			DB.getAttributeListedValues(i-1,j,this.lbValues.Items);
			
			this.lbValues.EndUpdate();
		}
		
		bool openOBJ() {
			
			OpenFileDialog dlg = new OpenFileDialog();
			
			if (File.Exists(OBJpath))  
			dlg.InitialDirectory=Path.GetDirectoryName(OBJpath);
			
			dlg.Filter = "Objects (*.obj)|*.obj";
		    dlg.RestoreDirectory = true ;

		    if (dlg.ShowDialog() == DialogResult.OK) {
		    	
		    	string s=dlg.FileName;
			    if (File.Exists(s)) {
		    		OBJpath=s; OBJ.Open(s);
		    		return true;
		    	}
		    }
			return false;
		}
		
		void BtObjClick(object sender, EventArgs e)
		{
			if (openOBJ()) {
		    	this.btObj.Enabled=false;
		    	
		    	this.lbObj.BeginUpdate();
	    		OBJ.getFeatureTypes(this.lbObj.Items);
		    	this.lbObj.EndUpdate();
		    	
		    	this.lbAttrs.Items.Clear();
		    	this.lbValues.Items.Clear();
		    	this.lbRoles.Items.Clear();
		    	
		    	this.btObj.Enabled=true;
		    }
		}
		
		void __progress(int pos, int max) {
			
			if (max > 0) progressBar.Maximum=max;
			if (pos < 0) pos=progressBar.Value+1;
			progressBar.Value=pos;
		}

		void BtSyncClick(object sender, EventArgs e)
		{
			
			if (File.Exists(DBpath)) 
     		if (DB.open(DBpath)) 
			
			if (openOBJ()) {
			
				this.btSync.Enabled=false;
				Cursor.Current=Cursors.WaitCursor;
				
				report.ClearAll();
				
				OBJ.log=report.log;
				OBJ.progress=__progress;
				
				progressBar.Visible=true;
				
				OBJ.syncFC(DB.catalog,0);

				OBJ.Close();
				
    			progressBar.Visible=false;

				Cursor.Current=Cursors.Default;
				this.btSync.Enabled=true;
			}
		}
		
		void PaTopSizeChanged(object sender, EventArgs e)
		{
			progressBar.Width=this.paTop.Width-16-progressBar.Left;
		}
		
		void LbRolesClick(object sender, EventArgs e)
		{
			
			lbTopo.BeginUpdate();
			lbTopo.Items.Clear();
			
			var i=lbRoles.SelectedIndex;
			
			if ((i >= 0) && (i < topoList.Count)) {
				List<string> tmp = topoList[i];
				if (tmp != null) 
				foreach(var s in tmp)
				lbTopo.Items.Add(s);
			}
			
			lbTopo.EndUpdate();
	
		}
	}
}
