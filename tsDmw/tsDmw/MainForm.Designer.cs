namespace tsDmw
{
    partial class DfmTsDmw
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveContentAsText = new System.Windows.Forms.ToolStripMenuItem();
            this.saveContentAsMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusApiVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTask = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbBranches = new System.Windows.Forms.ListBox();
            this.lbMsg = new System.Windows.Forms.TextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.HelpMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(541, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginItem,
            this.toolStripSeparator1,
            this.dumpItem,
            this.saveContentAsText,
            this.saveContentAsMap,
            this.toolStripMenuItem1,
            this.ExitItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // LoginItem
            // 
            this.LoginItem.Name = "LoginItem";
            this.LoginItem.Size = new System.Drawing.Size(196, 22);
            this.LoginItem.Text = "Авторизация...";
            this.LoginItem.Click += new System.EventHandler(this.LoginItem_Click);
            // 
            // dumpItem
            // 
            this.dumpItem.Name = "dumpItem";
            this.dumpItem.Size = new System.Drawing.Size(196, 22);
            this.dumpItem.Text = "get content...";
            this.dumpItem.Click += new System.EventHandler(this.dumpItem_Click);
            // 
            // saveContentAsText
            // 
            this.saveContentAsText.Name = "saveContentAsText";
            this.saveContentAsText.Size = new System.Drawing.Size(196, 22);
            this.saveContentAsText.Text = "save content as text...";
            this.saveContentAsText.Click += new System.EventHandler(this.saveContentAsText_Click);
            // 
            // saveContentAsMap
            // 
            this.saveContentAsMap.Name = "saveContentAsMap";
            this.saveContentAsMap.Size = new System.Drawing.Size(196, 22);
            this.saveContentAsMap.Text = "save content as <dm>...";
            this.saveContentAsMap.Click += new System.EventHandler(this.saveContentAsMap_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(193, 6);
            // 
            // ExitItem
            // 
            this.ExitItem.Name = "ExitItem";
            this.ExitItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.ExitItem.Size = new System.Drawing.Size(196, 22);
            this.ExitItem.Text = "Выход";
            this.ExitItem.Click += new System.EventHandler(this.ExitItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutItem});
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(24, 20);
            this.HelpMenu.Text = "?";
            // 
            // AboutItem
            // 
            this.AboutItem.Name = "AboutItem";
            this.AboutItem.Size = new System.Drawing.Size(150, 22);
            this.AboutItem.Text = "О программе...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusApiVersion,
            this.statusTask,
            this.statusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 415);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(541, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusApiVersion
            // 
            this.statusApiVersion.Name = "statusApiVersion";
            this.statusApiVersion.Size = new System.Drawing.Size(0, 17);
            // 
            // statusTask
            // 
            this.statusTask.Name = "statusTask";
            this.statusTask.Size = new System.Drawing.Size(0, 17);
            // 
            // statusProgress
            // 
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbBranches);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbMsg);
            this.splitContainer1.Size = new System.Drawing.Size(541, 391);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 2;
            // 
            // lbBranches
            // 
            this.lbBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBranches.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbBranches.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbBranches.FormattingEnabled = true;
            this.lbBranches.ItemHeight = 16;
            this.lbBranches.Location = new System.Drawing.Point(0, 0);
            this.lbBranches.Name = "lbBranches";
            this.lbBranches.Size = new System.Drawing.Size(180, 391);
            this.lbBranches.TabIndex = 0;
            this.lbBranches.Click += new System.EventHandler(this.lbBranches_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMsg.Location = new System.Drawing.Point(0, 0);
            this.lbMsg.Multiline = true;
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.ReadOnly = true;
            this.lbMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lbMsg.Size = new System.Drawing.Size(357, 391);
            this.lbMsg.TabIndex = 0;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // DfmTsDmw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 437);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "DfmTsDmw";
            this.Text = "(dm) Клиент ООБГД";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DfmTsDmw_FormClosed);
            this.Load += new System.EventHandler(this.DfmTsDmw_Load);
            this.Shown += new System.EventHandler(this.DfmTsDmw_Shown);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExitItem;
        private System.Windows.Forms.ToolStripMenuItem LoginItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem AboutItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusApiVersion;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbBranches;
        private System.Windows.Forms.TextBox lbMsg;
        private System.Windows.Forms.ToolStripMenuItem dumpItem;
        private System.Windows.Forms.ToolStripStatusLabel statusTask;
        private System.Windows.Forms.ToolStripMenuItem saveContentAsText;
        private System.Windows.Forms.ToolStripMenuItem saveContentAsMap;
        private System.Windows.Forms.ToolStripStatusLabel statusProgress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

