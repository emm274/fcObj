/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.06.2015
 * Time: 15:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Ini;
using xedits;
using xfiles;
using DmwAuto;

namespace fcDmw
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		dmwAuto dmw;
		client fclient;
		
		sodb fsodb;
		
    	double fsouth,fnorth,fwest,feast;
		
    	const double RadToDeg = 180/Math.PI;
    	
    	ClassList classes = new ClassList();
    	
        List<string> classList = new List<string>();

    	string fmsg;
    	string fdataDir = "";
        string finiDir = "";
    		
  		sodbFeatureToIntf fdoc;
  		
		Stack<string> fgetList = new Stack<string>();
    	
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			fclient = new client();
			fclient.OnRect=__frag;
			
			fsodb = new sodb();
            fsodb.status = __status;
            fsodb.message = __message;
            fsodb.endTask = __endTask;

            login_.Text = "niitp";
            password_.Text = "niitp";

            Enabled_ctrls();
		}

		void MainFormLoad(object sender, EventArgs e)
		{
			IniFile ini = new IniFile("");
			ini.ReadForm(this);
            fdataDir = ini.ReadDir("dataDir");
            finiDir = ini.ReadDir("iniDir");

            fsouth = ini.ReadReal(59.3, "south");
            fnorth = ini.ReadReal(59.4, "north");
            fwest  = ini.ReadReal(24.0, "west");
            feast  = ini.ReadReal(24.3, "east");

            login_.Text = ini.ReadStr("niitp", "login");
            password_.Text = ini.ReadStr("niitp", "password");

            classList.Clear();
            for (int i = 0; i < 256; i++)
            {
                string k = String.Format("!key{0}", i+1);
                string v = ini.ReadStr("", k);
                if (v == null) break;
                if (v.Length == 0) break;

                if (classList.IndexOf(v) < 0) 
                classList.Add(v);
            }

            if (classList.Count == 0)
            {
                classList.Add("RivRiv");
                classList.Add("Lake");
            }

            XEdits.FloatTextBox(x1_);
            XEdits.FloatTextBox(y1_);
            XEdits.FloatTextBox(x2_);
            XEdits.FloatTextBox(y2_);

            dmw = new dmwAuto();
            dmw.Connect();

            dmw.draw_Vector(-20003,0,0,null,null);  // wgs_coord = true 
            dmw.draw_Vector(-20013,0,0,null,null);  // int_coord = true 

            fclient.Start();

            displayFrag(null, null);
		}

        private void MainFormClosed(object sender, FormClosedEventArgs e)
        {
            fsodb.Disconnect();

			IniFile ini = new IniFile("");
            ini.Reset();

			ini.WriteForm(this);
            ini.WriteDir("dataDir", fdataDir);
            ini.WriteDir("iniDir", finiDir);

            ini.WriteReal("south", fsouth);
            ini.WriteReal("north", fnorth);
            ini.WriteReal("west", fwest);
            ini.WriteReal("east", feast);

            ini.WriteStr("login", login_.Text);
            ini.WriteStr("password", password_.Text);

            int i = 0;
            foreach (string key in classList)
            {
                i++;
                ini.WriteStr(String.Format("!key{0}",i), key);
            }
			
			fclient.Stop();

            dmw.draw_Vector(0, 0, 0, null, null);
            dmw.Disconnect();
		}

        bool sodb_Connected()
        {
            if (fsodb.Connected()) return true;

            fsodb.Disconnect();
            BtConnectClick(null, null);

            return false;
        }

        void Enabled_ctrls()
        {
            bool conn = fsodb.Connected();

            btConnect.Enabled = !conn;
            btDisconnect.Enabled = conn;
            btGet.Enabled = conn;
            btLoad.Enabled = conn;
            btUpdate.Enabled = conn;
            btListLoad.Enabled = conn;
            btListSave.Enabled = conn;
        }

		void BtFragClick(object sender, EventArgs e)
		{
		  	dmw.pick_Caption("Fragment...");
			dmw.pick(dmw.pick_rect,0,0);
		}
		
		
		private void displayFrag(object o, EventArgs e)
        {
			x1_.Text=String.Format("{0:0.000000}", fsouth); 
			y1_.Text=String.Format("{0:0.000000}", fwest); 
			x2_.Text=String.Format("{0:0.000000}", fnorth); 
			y2_.Text=String.Format("{0:0.000000}", feast);

            double[] mf = new double[5 * 2];
            mf[0] = fsouth; mf[1] = fwest;
            mf[2] = fnorth; mf[3] = fwest;
            mf[4] = fnorth; mf[5] = feast;
            mf[6] = fsouth; mf[7] = feast;
            mf[8] = fsouth; mf[9] = fwest;

            dmw.draw_Vector(1, 2, 13 + (3 << 8), mf, null);

        }
		
		void __frag(int x1, int y1, int x2, int y2) {
			
			int[] x = new int[4];
			int[] y = new int[4];
			
			x[0]=x1; y[0]=y1;
			x[1]=x2; y[1]=y1;
			x[2]=x2; y[2]=y2;
			x[3]=x1; y[3]=y2;
			
			double b,l;
			int i;
			
			dmw.l_to_r(x[0],y[0], out b,out l); 
			
			double b1=b, b2=b,  l1=l, l2=l;
			
			for (i=1; i<4; i++) {
				dmw.l_to_r(x[i],y[i], out b,out l);
				b1=Math.Min(b1,b); b2=Math.Max(b2,b);				
				l1=Math.Min(l1,l); l2=Math.Max(l2,l);				
			}

            fsouth = b1 * RadToDeg;
            fwest = l1 * RadToDeg;
            fnorth = b2 * RadToDeg;
            feast = l2 * RadToDeg;

			this.Invoke(new EventHandler(displayFrag));

            dmw.pick_Caption("");
		}

        private void MainForm_Activated(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
        
        private void GetFeatureClasses()
        {
        	fsodb.GetFeatureClasses(classes);
        	
        	dgClasses.Rows.Clear();
        	foreach(KeyValuePair<string, string> obj in classes) {
        		var row = dgClasses.Rows[ dgClasses.Rows.Add() ];

                string used = "";
                if (classList.IndexOf(obj.Key) >= 0) used=" +";

                row.Cells[0].Value = used;
                row.Cells[1].Value = obj.Key;
        		row.Cells[2].Value = obj.Value;
        	}
        }
        
		void BtConnectClick(object sender, EventArgs e)
		{
            btConnect.Enabled = false;
            beginProcess("Connecting...");

            if (fsodb.Connect(login_.Text, password_.Text))
            GetFeatureClasses();

            Enabled_ctrls();

            endProcess();
		}
		
		void BtDisconnectClick(object sender, EventArgs e)
		{
            fsodb.Disconnect();
            Enabled_ctrls();
		}
		
		void beginProcess(string capt) {
            Cursor.Current = Cursors.WaitCursor;
            status.Text = capt; 
            
            if (lbMsg.Text.Length > 0)
            lbMsg.AppendText(Environment.NewLine);

            btGet.Enabled=false;
            btLoad.Enabled=false;
            btUpdate.Enabled=false;
  		}
		
		void endProcess() {
            Cursor.Current = Cursors.Default;
            status.Text=""; Enabled_ctrls();
		}

        private void message(string msg)
        {
            if (lbMsg.Text.Length > 0)
            lbMsg.AppendText(Environment.NewLine);
            lbMsg.AppendText(msg);

        }

		private void invokeMsg(object o, EventArgs e)
		{
            message(fmsg);
		}
        
		private void invokeStatus(object o, EventArgs e)
		{
			status.Text=fmsg;
		}
        
		void getNextClass() {

            bool rc = false;

            if (fgetList.Count > 0)  {
                string s = fgetList.Pop(); __status(s + "...");
                rc = fsodb.GetFrame(s, fsouth, fwest, fnorth, feast, fdoc);
            }

            if (!rc) {
                if (fdoc != null) {
                    int count = fdoc.GetCount();

                    if (count > 0)
                    {
                        string s = String.Format("Received {0:0} objects.", count);
                        message(s);

                        s = Path.GetFileName(fdoc.GetPath());
                        message("<" + s + "> created.");
                    }

                    fdoc.Close(); fdoc = null;
                }

                endProcess();
                status.Text = fmsg;
            }
		}
		
		private void invokeEndTask(object o, EventArgs e)
		{
			getNextClass();
		}
        
        void __status(string message)
        {
        	fmsg = message;
        	this.Invoke(new EventHandler(invokeStatus));
        }

        void __message(string message)
        {
        	fmsg = message;
        	this.Invoke(new EventHandler(invokeMsg));
        }

        void __endTask(string message)
        {
        	fmsg = message;
        	this.Invoke(new EventHandler(invokeEndTask));
        }

        int dialDest(ref string Dest)
        {
        	Dest="";
        	
        	SaveFileDialog dlg = new SaveFileDialog();

            if (Directory.Exists(fdataDir))
            dlg.InitialDirectory = fdataDir;
        	
			dlg.Title="Save fragment as";
            dlg.Filter = "Files (*.dm)|*.dm|Files (*.txt)|*.txt";
		    dlg.RestoreDirectory = true;
        	
		    if (dlg.ShowDialog() == DialogResult.OK) {
		    	Dest=dlg.FileName;
                fdataDir = Path.GetDirectoryName(Dest);
		    	
		    	string ext=Path.GetExtension(Dest).ToLower();
		    	if (ext == ".dm") return 0; else
		    	if (ext == ".txt") return 1;
		    }
		    
        	return -1;
        }

        int GetSelectedClasses()
        {
            fgetList.Clear();
            foreach (DataGridViewRow row in dgClasses.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string used = row.Cells[0].Value.ToString();
                    if (used.Length > 0)
                    {
                        string key = row.Cells[1].Value.ToString();
                        fgetList.Push(key);
                    }
                }
            }

            return fgetList.Count;
        }

        void BtGetClick(object sender, EventArgs e)
		{
            if (sodb_Connected()) 
            
            if (GetSelectedClasses() > 0) {

        		string dest="";
        		int rc=dialDest(ref dest);

        		if (rc >= 0) {
        			
        			fdoc=null;
        			
        			if (rc == 0)
		        		fdoc = new sodbFeatureToMap(dest);
        			else
		        		fdoc = new sodbFeatureToText(dest);

	        		if (fdoc != null) {

                        fdoc.workDir(dmw.WorkDir());

   						beginProcess("Get fragment...");
			
	    				fdoc.Extent(fsouth, fwest, fnorth, feast);

   						fsodb.beginGetFrame();
    					getNextClass();
	        		}
        		}
			}
		}

        private void btLoad_Click(object sender, EventArgs e)
        {
            string path="";

            if (sodb_Connected()) 
            if (XFiles.dialFile(ref fdataDir,
                                "Files (*.dm)|*.dm",
                                "Загрузить карту",
                                null,true,
                                out path)) {

                string s = Path.GetFileName(path)+"...";
                beginProcess(s);

                DBLoaderMap loader = new DBLoaderMap();

                string log = Path.ChangeExtension(path, ".log");
                var doc = new sodbFeatureToText(log);

                loader.message = __message;
                loader.workDir( dmw.WorkDir() );

                loader.exec(path,fsodb,doc,false);

                doc.Close(); doc = null;

                endProcess();
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            string path="";

            if (sodb_Connected())
                if (XFiles.dialFile(ref fdataDir,
                                    "Files (*.tdm)|*.tdm",
                                    "Загрузить изменения",
                                    null,true, out path))
            {
                string s = Path.GetFileName(path) + "...";
                beginProcess(s);

                DBLoaderMap loader = new DBLoaderMap();

                string log = Path.ChangeExtension(path, ".log");
                var doc = new sodbFeatureToText(log);

                loader.message = __message;
                loader.workDir(dmw.WorkDir());

                loader.exec(path, fsodb, doc,true);

                doc.Close(); doc = null;

                endProcess();
            }
        }

        private void x1__TextChanged(object sender, EventArgs e)
        {
            double v;
            if (Double.TryParse(x1_.Text, out v)) fsouth = v;
        }

        private void x2__TextChanged(object sender, EventArgs e)
        {
            double v;
            if (Double.TryParse(x2_.Text, out v)) fnorth = v;
        }

        private void y1__TextChanged(object sender, EventArgs e)
        {
            double v;
            if (Double.TryParse(y1_.Text, out v)) fwest = v;
        }

        private void y2__TextChanged(object sender, EventArgs e)
        {
            double v;
            if (Double.TryParse(y2_.Text, out v)) feast = v;
        }

        private void btListLoad_Click(object sender, EventArgs e)
        {
            string path = "";
            if (XFiles.dialFile(ref finiDir,
                                "Files (*.txt)|*.txt",
                                "Загрузить список классов",
                                null,true,out path)) {

                classList.Clear();

                try
                {
                    // Create an instance of StreamReader to read from a file. 
                    // The using statement also closes the StreamReader. 
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string key;
                        while ((key = sr.ReadLine()) != null)
                        classList.Add(key);
                    }
                }
                catch (Exception ex)
                {
                   string msg = "The file could not be read: " + ex.Message;
                   __message(msg);
                }

                foreach (DataGridViewRow row in dgClasses.Rows)
                {
                    if (row.Cells[1].Value != null)
                    {
                        string used = "";
                        string key = row.Cells[1].Value.ToString();

                        if (key != null)
                            if (classList.IndexOf(key) >= 0)
                                used = " +";

                        row.Cells[0].Value = used;
                    }
                }

            }
        }

        private void btListSave_Click(object sender, EventArgs e)
        {
            string path = "";
            int rc = XFiles.dialSaveFile(ref finiDir,
                                         "Files (*.txt)|*.txt",
                                         "Сохранить список классов как",
                                          0, out path);
 
            if (rc >= 0) {
                var file = new StreamWriter(path);
                if (file != null) {

                    foreach (DataGridViewRow row in dgClasses.Rows)
                    {
                        if (row.Cells[0].Value != null) {
                            string used = row.Cells[0].Value.ToString();
                            string key = row.Cells[1].Value.ToString();
                            if (used.Length > 0) file.WriteLine(key);
                        }
                    }

                    file.Close();
                }

            }

        }

        private void dgClasses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            int c = e.ColumnIndex;

            if (c == 0)
            if ((r >= 0) && (r < dgClasses.Rows.Count))
            {
                var row = dgClasses.Rows[r];
                string s = row.Cells[0].Value.ToString();
                if (s.Length > 0) s = ""; else s = " +";
                row.Cells[0].Value=s;

                string key= row.Cells[1].Value.ToString();

                if (s.Length > 0)
                {
                    if (classList.IndexOf(key) < 0)
                    classList.Add(key);
                }
                else
                {
                    classList.Remove(key);
                }
            }
        }

        private void gbMsg_SizeChanged(object sender, EventArgs e)
        {
            btCls.Left = gbMsg.Width-24-btCls.Width;
        }

        private void btCls_Click(object sender, EventArgs e)
        {
            lbMsg.Clear();
        }

	}
}
