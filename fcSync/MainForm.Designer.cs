/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 26.02.2015
 * Time: 14:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace fcSync
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Panel paTop;
		private System.Windows.Forms.Button btObj;
		private System.Windows.Forms.Button btDB;
		private System.Windows.Forms.ListBox lbLog;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btSync;
		
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
			this.btSync = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btObj = new System.Windows.Forms.Button();
			this.btDB = new System.Windows.Forms.Button();
			this.lbLog = new System.Windows.Forms.ListBox();
			this.paTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// paTop
			// 
			this.paTop.Controls.Add(this.btSync);
			this.paTop.Controls.Add(this.progressBar);
			this.paTop.Controls.Add(this.btObj);
			this.paTop.Controls.Add(this.btDB);
			this.paTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.paTop.Location = new System.Drawing.Point(0, 0);
			this.paTop.Name = "paTop";
			this.paTop.Size = new System.Drawing.Size(434, 48);
			this.paTop.TabIndex = 0;
			this.paTop.Resize += new System.EventHandler(this.paTopResize);
			// 
			// btSync
			// 
			this.btSync.Location = new System.Drawing.Point(208, 8);
			this.btSync.Name = "btSync";
			this.btSync.Size = new System.Drawing.Size(40, 32);
			this.btSync.TabIndex = 3;
			this.btSync.Text = ">>";
			this.btSync.UseVisualStyleBackColor = true;
			this.btSync.Click += new System.EventHandler(this.btSyncClick);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(256, 16);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(168, 15);
			this.progressBar.TabIndex = 2;
			this.progressBar.Visible = false;
			// 
			// btObj
			// 
			this.btObj.Location = new System.Drawing.Point(88, 8);
			this.btObj.Name = "btObj";
			this.btObj.Size = new System.Drawing.Size(112, 31);
			this.btObj.TabIndex = 1;
			this.btObj.Text = "Классификатор";
			this.btObj.UseVisualStyleBackColor = true;
			this.btObj.Click += new System.EventHandler(this.BtObjClick);
			// 
			// btDB
			// 
			this.btDB.Location = new System.Drawing.Point(8, 8);
			this.btDB.Name = "btDB";
			this.btDB.Size = new System.Drawing.Size(72, 31);
			this.btDB.TabIndex = 0;
			this.btDB.Text = "Каталог";
			this.btDB.UseVisualStyleBackColor = true;
			this.btDB.Click += new System.EventHandler(this.BtDBClick);
			// 
			// lbLog
			// 
			this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbLog.FormattingEnabled = true;
			this.lbLog.Location = new System.Drawing.Point(0, 48);
			this.lbLog.Name = "lbLog";
			this.lbLog.Size = new System.Drawing.Size(434, 210);
			this.lbLog.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 258);
			this.Controls.Add(this.lbLog);
			this.Controls.Add(this.paTop);
			this.Name = "MainForm";
			this.Text = "fcSync";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.paTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
