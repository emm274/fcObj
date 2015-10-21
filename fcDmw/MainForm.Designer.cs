/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.06.2015
 * Time: 15:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace fcDmw
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
            this.status = new System.Windows.Forms.TextBox();
            this.paClient = new System.Windows.Forms.Panel();
            this.paRight = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbClasses = new System.Windows.Forms.GroupBox();
            this.dgClasses = new System.Windows.Forms.DataGridView();
            this.used = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acronym = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbMsg = new System.Windows.Forms.GroupBox();
            this.lbMsg = new System.Windows.Forms.TextBox();
            this.paLeft = new System.Windows.Forms.Panel();
            this.btUpdate = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.gbFrag = new System.Windows.Forms.GroupBox();
            this.btListSave = new System.Windows.Forms.Button();
            this.btListLoad = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.y2_ = new System.Windows.Forms.TextBox();
            this.y1_ = new System.Windows.Forms.TextBox();
            this.btGet = new System.Windows.Forms.Button();
            this.x2_ = new System.Windows.Forms.TextBox();
            this.x1_ = new System.Windows.Forms.TextBox();
            this.btFrag = new System.Windows.Forms.Button();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.btConnect = new System.Windows.Forms.Button();
            this.password_ = new System.Windows.Forms.TextBox();
            this.login_ = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btCls = new System.Windows.Forms.Button();
            this.paClient.SuspendLayout();
            this.paRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgClasses)).BeginInit();
            this.gbMsg.SuspendLayout();
            this.paLeft.SuspendLayout();
            this.gbFrag.SuspendLayout();
            this.gbConn.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.status.Location = new System.Drawing.Point(0, 495);
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Size = new System.Drawing.Size(729, 20);
            this.status.TabIndex = 8;
            // 
            // paClient
            // 
            this.paClient.Controls.Add(this.paRight);
            this.paClient.Controls.Add(this.paLeft);
            this.paClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paClient.Location = new System.Drawing.Point(0, 0);
            this.paClient.Name = "paClient";
            this.paClient.Size = new System.Drawing.Size(729, 495);
            this.paClient.TabIndex = 9;
            // 
            // paRight
            // 
            this.paRight.Controls.Add(this.splitContainer1);
            this.paRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paRight.Location = new System.Drawing.Point(213, 0);
            this.paRight.Name = "paRight";
            this.paRight.Size = new System.Drawing.Size(516, 495);
            this.paRight.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbClasses);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbMsg);
            this.splitContainer1.Size = new System.Drawing.Size(516, 495);
            this.splitContainer1.SplitterDistance = 294;
            this.splitContainer1.TabIndex = 0;
            // 
            // gbClasses
            // 
            this.gbClasses.Controls.Add(this.dgClasses);
            this.gbClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbClasses.Location = new System.Drawing.Point(0, 0);
            this.gbClasses.Margin = new System.Windows.Forms.Padding(8);
            this.gbClasses.Name = "gbClasses";
            this.gbClasses.Size = new System.Drawing.Size(516, 294);
            this.gbClasses.TabIndex = 3;
            this.gbClasses.TabStop = false;
            this.gbClasses.Text = "Классы объектов";
            // 
            // dgClasses
            // 
            this.dgClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgClasses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.used,
            this.acronym,
            this.alias});
            this.dgClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgClasses.Location = new System.Drawing.Point(3, 16);
            this.dgClasses.Name = "dgClasses";
            this.dgClasses.ReadOnly = true;
            this.dgClasses.Size = new System.Drawing.Size(510, 275);
            this.dgClasses.TabIndex = 0;
            this.dgClasses.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgClasses_CellClick);
            // 
            // used
            // 
            this.used.HeaderText = "*";
            this.used.Name = "used";
            this.used.ReadOnly = true;
            this.used.Width = 32;
            // 
            // acronym
            // 
            this.acronym.HeaderText = "acronym";
            this.acronym.Name = "acronym";
            this.acronym.ReadOnly = true;
            this.acronym.Width = 80;
            // 
            // alias
            // 
            this.alias.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.alias.HeaderText = "alias";
            this.alias.Name = "alias";
            this.alias.ReadOnly = true;
            // 
            // gbMsg
            // 
            this.gbMsg.Controls.Add(this.btCls);
            this.gbMsg.Controls.Add(this.lbMsg);
            this.gbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMsg.Location = new System.Drawing.Point(0, 0);
            this.gbMsg.Name = "gbMsg";
            this.gbMsg.Size = new System.Drawing.Size(516, 197);
            this.gbMsg.TabIndex = 2;
            this.gbMsg.TabStop = false;
            this.gbMsg.Text = "Messages";
            this.gbMsg.SizeChanged += new System.EventHandler(this.gbMsg_SizeChanged);
            // 
            // lbMsg
            // 
            this.lbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMsg.Location = new System.Drawing.Point(3, 16);
            this.lbMsg.Multiline = true;
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.ReadOnly = true;
            this.lbMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lbMsg.Size = new System.Drawing.Size(510, 178);
            this.lbMsg.TabIndex = 0;
            // 
            // paLeft
            // 
            this.paLeft.Controls.Add(this.btUpdate);
            this.paLeft.Controls.Add(this.btLoad);
            this.paLeft.Controls.Add(this.gbFrag);
            this.paLeft.Controls.Add(this.gbConn);
            this.paLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.paLeft.Location = new System.Drawing.Point(0, 0);
            this.paLeft.Name = "paLeft";
            this.paLeft.Size = new System.Drawing.Size(213, 495);
            this.paLeft.TabIndex = 0;
            // 
            // btUpdate
            // 
            this.btUpdate.Location = new System.Drawing.Point(112, 307);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(89, 23);
            this.btUpdate.TabIndex = 12;
            this.btUpdate.Text = "Update";
            this.btUpdate.UseVisualStyleBackColor = true;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // btLoad
            // 
            this.btLoad.Location = new System.Drawing.Point(16, 308);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(89, 23);
            this.btLoad.TabIndex = 11;
            this.btLoad.Text = "Load map";
            this.btLoad.UseVisualStyleBackColor = true;
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // gbFrag
            // 
            this.gbFrag.Controls.Add(this.btListSave);
            this.gbFrag.Controls.Add(this.btListLoad);
            this.gbFrag.Controls.Add(this.label6);
            this.gbFrag.Controls.Add(this.label5);
            this.gbFrag.Controls.Add(this.label4);
            this.gbFrag.Controls.Add(this.label3);
            this.gbFrag.Controls.Add(this.y2_);
            this.gbFrag.Controls.Add(this.y1_);
            this.gbFrag.Controls.Add(this.btGet);
            this.gbFrag.Controls.Add(this.x2_);
            this.gbFrag.Controls.Add(this.x1_);
            this.gbFrag.Controls.Add(this.btFrag);
            this.gbFrag.Location = new System.Drawing.Point(8, 120);
            this.gbFrag.Name = "gbFrag";
            this.gbFrag.Size = new System.Drawing.Size(200, 181);
            this.gbFrag.TabIndex = 10;
            this.gbFrag.TabStop = false;
            this.gbFrag.Text = "Get fragment";
            // 
            // btListSave
            // 
            this.btListSave.Location = new System.Drawing.Point(107, 146);
            this.btListSave.Name = "btListSave";
            this.btListSave.Size = new System.Drawing.Size(75, 23);
            this.btListSave.TabIndex = 13;
            this.btListSave.Text = "[..] save";
            this.btListSave.UseVisualStyleBackColor = true;
            this.btListSave.Click += new System.EventHandler(this.btListSave_Click);
            // 
            // btListLoad
            // 
            this.btListLoad.Location = new System.Drawing.Point(15, 146);
            this.btListLoad.Name = "btListLoad";
            this.btListLoad.Size = new System.Drawing.Size(75, 23);
            this.btListLoad.TabIndex = 12;
            this.btListLoad.Text = "[..] load";
            this.btListLoad.UseVisualStyleBackColor = true;
            this.btListLoad.Click += new System.EventHandler(this.btListLoad_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "east";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "west";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "north";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "south";
            // 
            // y2_
            // 
            this.y2_.Location = new System.Drawing.Point(96, 120);
            this.y2_.Name = "y2_";
            this.y2_.Size = new System.Drawing.Size(88, 20);
            this.y2_.TabIndex = 6;
            this.y2_.TextChanged += new System.EventHandler(this.y2__TextChanged);
            // 
            // y1_
            // 
            this.y1_.Location = new System.Drawing.Point(96, 96);
            this.y1_.Name = "y1_";
            this.y1_.Size = new System.Drawing.Size(88, 20);
            this.y1_.TabIndex = 5;
            this.y1_.TextChanged += new System.EventHandler(this.y1__TextChanged);
            // 
            // btGet
            // 
            this.btGet.Location = new System.Drawing.Point(136, 16);
            this.btGet.Name = "btGet";
            this.btGet.Size = new System.Drawing.Size(45, 23);
            this.btGet.TabIndex = 4;
            this.btGet.Text = ">>";
            this.btGet.UseVisualStyleBackColor = true;
            this.btGet.Click += new System.EventHandler(this.BtGetClick);
            // 
            // x2_
            // 
            this.x2_.Location = new System.Drawing.Point(96, 72);
            this.x2_.Name = "x2_";
            this.x2_.Size = new System.Drawing.Size(88, 20);
            this.x2_.TabIndex = 3;
            this.x2_.TextChanged += new System.EventHandler(this.x2__TextChanged);
            // 
            // x1_
            // 
            this.x1_.Location = new System.Drawing.Point(96, 48);
            this.x1_.Name = "x1_";
            this.x1_.Size = new System.Drawing.Size(88, 20);
            this.x1_.TabIndex = 2;
            this.x1_.TextChanged += new System.EventHandler(this.x1__TextChanged);
            // 
            // btFrag
            // 
            this.btFrag.Location = new System.Drawing.Point(8, 16);
            this.btFrag.Name = "btFrag";
            this.btFrag.Size = new System.Drawing.Size(120, 23);
            this.btFrag.TabIndex = 1;
            this.btFrag.Text = "Show fragment";
            this.btFrag.UseVisualStyleBackColor = true;
            this.btFrag.Click += new System.EventHandler(this.BtFragClick);
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.btDisconnect);
            this.gbConn.Controls.Add(this.btConnect);
            this.gbConn.Controls.Add(this.password_);
            this.gbConn.Controls.Add(this.login_);
            this.gbConn.Controls.Add(this.label2);
            this.gbConn.Controls.Add(this.label1);
            this.gbConn.Location = new System.Drawing.Point(8, 8);
            this.gbConn.Name = "gbConn";
            this.gbConn.Size = new System.Drawing.Size(200, 106);
            this.gbConn.TabIndex = 9;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Database";
            // 
            // btDisconnect
            // 
            this.btDisconnect.Location = new System.Drawing.Point(103, 72);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(89, 23);
            this.btDisconnect.TabIndex = 5;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.BtDisconnectClick);
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(8, 72);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(89, 23);
            this.btConnect.TabIndex = 4;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.BtConnectClick);
            // 
            // password_
            // 
            this.password_.Location = new System.Drawing.Point(96, 42);
            this.password_.Name = "password_";
            this.password_.Size = new System.Drawing.Size(88, 20);
            this.password_.TabIndex = 3;
            // 
            // login_
            // 
            this.login_.Location = new System.Drawing.Point(96, 16);
            this.login_.Name = "login_";
            this.login_.Size = new System.Drawing.Size(88, 20);
            this.login_.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "login";
            // 
            // btCls
            // 
            this.btCls.Location = new System.Drawing.Point(424, 0);
            this.btCls.Name = "btCls";
            this.btCls.Size = new System.Drawing.Size(44, 23);
            this.btCls.TabIndex = 1;
            this.btCls.Text = ".cls";
            this.btCls.UseVisualStyleBackColor = true;
            this.btCls.Click += new System.EventHandler(this.btCls_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 515);
            this.Controls.Add(this.paClient);
            this.Controls.Add(this.status);
            this.Name = "MainForm";
            this.Text = "(dm) SODB Client";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormClosed);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.paClient.ResumeLayout(false);
            this.paRight.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbClasses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgClasses)).EndInit();
            this.gbMsg.ResumeLayout(false);
            this.gbMsg.PerformLayout();
            this.paLeft.ResumeLayout(false);
            this.gbFrag.ResumeLayout(false);
            this.gbFrag.PerformLayout();
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.Panel paClient;
        private System.Windows.Forms.Panel paRight;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbClasses;
        private System.Windows.Forms.DataGridView dgClasses;
        private System.Windows.Forms.DataGridViewTextBoxColumn used;
        private System.Windows.Forms.DataGridViewTextBoxColumn acronym;
        private System.Windows.Forms.DataGridViewTextBoxColumn alias;
        private System.Windows.Forms.GroupBox gbMsg;
        private System.Windows.Forms.Panel paLeft;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.GroupBox gbFrag;
        private System.Windows.Forms.Button btListSave;
        private System.Windows.Forms.Button btListLoad;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox y2_;
        private System.Windows.Forms.TextBox y1_;
        private System.Windows.Forms.Button btGet;
        private System.Windows.Forms.TextBox x2_;
        private System.Windows.Forms.TextBox x1_;
        private System.Windows.Forms.Button btFrag;
        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.TextBox password_;
        private System.Windows.Forms.TextBox login_;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox lbMsg;
        private System.Windows.Forms.Button btUpdate;
        private System.Windows.Forms.Button btCls;
	}
}
