﻿/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.02.2015
 * Time: 15:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace fcObj
{
	partial class dfmReport
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ListBox lbLog;
		
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
			this.lbLog = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lbLog
			// 
			this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbLog.FormattingEnabled = true;
			this.lbLog.Location = new System.Drawing.Point(0, 0);
			this.lbLog.Name = "lbLog";
			this.lbLog.Size = new System.Drawing.Size(361, 409);
			this.lbLog.TabIndex = 1;
			// 
			// dfmReport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(361, 409);
			this.Controls.Add(this.lbLog);
			this.Name = "dfmReport";
			this.Text = "log";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DfmReportFormClosing);
			this.Load += new System.EventHandler(this.DfmReportLoad);
			this.ResumeLayout(false);

		}
	}
}
