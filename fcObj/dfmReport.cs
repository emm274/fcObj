/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.02.2015
 * Time: 15:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Ini;

namespace fcObj
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class dfmReport : Form
	{
		public dfmReport()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void DfmReportLoad(object sender, EventArgs e)
		{
			IniFile ini = new IniFile("");
			ini.ReadForm(Name,this);
		}
		
		void DfmReportFormClosing(object sender, FormClosingEventArgs e)
		{
			IniFile ini = new IniFile("");
			ini.WriteForm(Name,this);
			e.Cancel=true; this.Hide();
		}
		
		public void ClearAll() {
			lbLog.Items.Clear();
		}

		void __msg(ListBox.ObjectCollection list,string str) {
			if (list.Count >= 256) 
				list.RemoveAt(0);
			list.Add(str);
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
	}
}
