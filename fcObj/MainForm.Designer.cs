/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 09.06.2014
 * Time: 11:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace fcObj
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.paTop = new System.Windows.Forms.Panel();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btSync = new System.Windows.Forms.Button();
			this.btObj = new System.Windows.Forms.Button();
			this.btQuery = new System.Windows.Forms.Button();
			this.btDB = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.lbObj = new System.Windows.Forms.ListBox();
			this.lbAttrs = new System.Windows.Forms.ListBox();
			this.lbValues = new System.Windows.Forms.ListBox();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.lbRoles = new System.Windows.Forms.ListBox();
			this.lbTopo = new System.Windows.Forms.ListBox();
			this.paTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.SuspendLayout();
			// 
			// paTop
			// 
			this.paTop.Controls.Add(this.progressBar);
			this.paTop.Controls.Add(this.btSync);
			this.paTop.Controls.Add(this.btObj);
			this.paTop.Controls.Add(this.btQuery);
			this.paTop.Controls.Add(this.btDB);
			this.paTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.paTop.Location = new System.Drawing.Point(0, 0);
			this.paTop.Name = "paTop";
			this.paTop.Size = new System.Drawing.Size(429, 45);
			this.paTop.TabIndex = 2;
			this.paTop.SizeChanged += new System.EventHandler(this.PaTopSizeChanged);
			this.paTop.Resize += new System.EventHandler(this.PaTopResize);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(173, 10);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(244, 23);
			this.progressBar.TabIndex = 7;
			this.progressBar.Visible = false;
			// 
			// btSync
			// 
			this.btSync.Location = new System.Drawing.Point(127, 8);
			this.btSync.Name = "btSync";
			this.btSync.Size = new System.Drawing.Size(31, 25);
			this.btSync.TabIndex = 6;
			this.btSync.Text = ".S";
			this.btSync.UseVisualStyleBackColor = true;
			this.btSync.Click += new System.EventHandler(this.BtSyncClick);
			// 
			// btObj
			// 
			this.btObj.Location = new System.Drawing.Point(88, 8);
			this.btObj.Name = "btObj";
			this.btObj.Size = new System.Drawing.Size(31, 25);
			this.btObj.TabIndex = 5;
			this.btObj.Text = ".O";
			this.btObj.UseVisualStyleBackColor = true;
			this.btObj.Click += new System.EventHandler(this.BtObjClick);
			// 
			// btQuery
			// 
			this.btQuery.Location = new System.Drawing.Point(52, 8);
			this.btQuery.Name = "btQuery";
			this.btQuery.Size = new System.Drawing.Size(31, 25);
			this.btQuery.TabIndex = 4;
			this.btQuery.Text = "?";
			this.btQuery.UseVisualStyleBackColor = true;
			this.btQuery.Click += new System.EventHandler(this.BtQueryClick);
			// 
			// btDB
			// 
			this.btDB.Location = new System.Drawing.Point(12, 8);
			this.btDB.Name = "btDB";
			this.btDB.Size = new System.Drawing.Size(31, 25);
			this.btDB.TabIndex = 2;
			this.btDB.Text = "DB";
			this.btDB.UseVisualStyleBackColor = true;
			this.btDB.Click += new System.EventHandler(this.BtDBClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 45);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
			this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer1.Size = new System.Drawing.Size(429, 339);
			this.splitContainer1.SplitterDistance = 224;
			this.splitContainer1.TabIndex = 3;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.lbObj);
			this.splitContainer2.Panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.lbAttrs);
			this.splitContainer2.Panel2.Controls.Add(this.lbValues);
			this.splitContainer2.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.splitContainer2.Size = new System.Drawing.Size(429, 224);
			this.splitContainer2.SplitterDistance = 291;
			this.splitContainer2.TabIndex = 0;
			// 
			// lbObj
			// 
			this.lbObj.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbObj.FormattingEnabled = true;
			this.lbObj.Location = new System.Drawing.Point(0, 0);
			this.lbObj.Name = "lbObj";
			this.lbObj.Size = new System.Drawing.Size(291, 224);
			this.lbObj.TabIndex = 0;
			this.lbObj.Click += new System.EventHandler(this.LbObjClick);
			// 
			// lbAttrs
			// 
			this.lbAttrs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbAttrs.FormattingEnabled = true;
			this.lbAttrs.Location = new System.Drawing.Point(0, 0);
			this.lbAttrs.Name = "lbAttrs";
			this.lbAttrs.Size = new System.Drawing.Size(134, 142);
			this.lbAttrs.TabIndex = 1;
			this.lbAttrs.Click += new System.EventHandler(this.LbAttrsClick);
			// 
			// lbValues
			// 
			this.lbValues.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lbValues.FormattingEnabled = true;
			this.lbValues.Location = new System.Drawing.Point(0, 142);
			this.lbValues.Name = "lbValues";
			this.lbValues.Size = new System.Drawing.Size(134, 82);
			this.lbValues.Sorted = true;
			this.lbValues.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.lbRoles);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.lbTopo);
			this.splitContainer3.Size = new System.Drawing.Size(429, 111);
			this.splitContainer3.SplitterDistance = 280;
			this.splitContainer3.TabIndex = 0;
			// 
			// lbRoles
			// 
			this.lbRoles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbRoles.FormattingEnabled = true;
			this.lbRoles.Location = new System.Drawing.Point(0, 0);
			this.lbRoles.Name = "lbRoles";
			this.lbRoles.Size = new System.Drawing.Size(280, 111);
			this.lbRoles.Sorted = true;
			this.lbRoles.TabIndex = 1;
			this.lbRoles.Click += new System.EventHandler(this.LbRolesClick);
			// 
			// lbTopo
			// 
			this.lbTopo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTopo.FormattingEnabled = true;
			this.lbTopo.Location = new System.Drawing.Point(0, 0);
			this.lbTopo.Name = "lbTopo";
			this.lbTopo.Size = new System.Drawing.Size(145, 111);
			this.lbTopo.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(429, 384);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.paTop);
			this.Name = "MainForm";
			this.Text = "fcObj";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.paTop.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.ListBox lbAttrs;
		private System.Windows.Forms.Button btQuery;
		private System.Windows.Forms.Panel paTop;
		private System.Windows.Forms.Button btDB;
		private System.Windows.Forms.ListBox lbRoles;
		private System.Windows.Forms.ListBox lbObj;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ListBox lbValues;
		private System.Windows.Forms.Button btObj;
		private System.Windows.Forms.Button btSync;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.ListBox lbTopo;
	}
}
