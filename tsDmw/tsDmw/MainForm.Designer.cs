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
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteCommit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.dumpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveJsonAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.LoadMap = new System.Windows.Forms.ToolStripMenuItem();
            this.loadUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.dialWorkDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusWorkDir = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusApiVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTask = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbCommit = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lbBranches = new System.Windows.Forms.ListBox();
            this.btClear = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.TextBox();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MapToJsonItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.fileMenu,
            this.HelpMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(541, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginItem,
            this.toolStripSeparator3,
            this.deleteCommit,
            this.toolStripSeparator1,
            this.dumpItem,
            this.saveJsonAs,
            this.toolStripSeparator2,
            this.LoadMap,
            this.loadUpdates,
            this.dialWorkDir,
            this.toolStripMenuItem1,
            this.MapToJsonItem,
            this.toolStripMenuItem2,
            this.ExitItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(48, 20);
            this.fileMenu.Text = "Файл";
            this.fileMenu.Click += new System.EventHandler(this.fileMenu_Click);
            // 
            // LoginItem
            // 
            this.LoginItem.Name = "LoginItem";
            this.LoginItem.Size = new System.Drawing.Size(225, 22);
            this.LoginItem.Text = "Авторизация...";
            this.LoginItem.Click += new System.EventHandler(this.LoginItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(222, 6);
            // 
            // deleteCommit
            // 
            this.deleteCommit.Name = "deleteCommit";
            this.deleteCommit.Size = new System.Drawing.Size(225, 22);
            this.deleteCommit.Text = "Удалить последний патч";
            this.deleteCommit.Click += new System.EventHandler(this.deleteCommit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(222, 6);
            // 
            // dumpItem
            // 
            this.dumpItem.Name = "dumpItem";
            this.dumpItem.Size = new System.Drawing.Size(225, 22);
            this.dumpItem.Text = "Сохранить как...";
            this.dumpItem.Click += new System.EventHandler(this.dumpItem_Click);
            // 
            // saveJsonAs
            // 
            this.saveJsonAs.Name = "saveJsonAs";
            this.saveJsonAs.Size = new System.Drawing.Size(225, 22);
            this.saveJsonAs.Text = "save json as...";
            this.saveJsonAs.Click += new System.EventHandler(this.saveJsonAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(222, 6);
            // 
            // LoadMap
            // 
            this.LoadMap.Name = "LoadMap";
            this.LoadMap.Size = new System.Drawing.Size(225, 22);
            this.LoadMap.Text = "Загрузить карту...";
            this.LoadMap.Click += new System.EventHandler(this.LoadMap_Click);
            // 
            // loadUpdates
            // 
            this.loadUpdates.Name = "loadUpdates";
            this.loadUpdates.Size = new System.Drawing.Size(225, 22);
            this.loadUpdates.Text = "Загрузить изменения...";
            this.loadUpdates.Click += new System.EventHandler(this.loadUpdates_Click);
            // 
            // dialWorkDir
            // 
            this.dialWorkDir.Name = "dialWorkDir";
            this.dialWorkDir.Size = new System.Drawing.Size(225, 22);
            this.dialWorkDir.Text = "Рабочая папка...";
            this.dialWorkDir.Click += new System.EventHandler(this.dialWorkDir_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(222, 6);
            // 
            // ExitItem
            // 
            this.ExitItem.Name = "ExitItem";
            this.ExitItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.ExitItem.Size = new System.Drawing.Size(225, 22);
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
            this.AboutItem.Size = new System.Drawing.Size(158, 22);
            this.AboutItem.Text = "О программе...";
            this.AboutItem.Click += new System.EventHandler(this.AboutItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusWorkDir,
            this.statusApiVersion,
            this.statusTask,
            this.statusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 415);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(541, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusWorkDir
            // 
            this.statusWorkDir.Name = "statusWorkDir";
            this.statusWorkDir.Size = new System.Drawing.Size(0, 17);
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
            this.splitContainer1.Panel1.Controls.Add(this.lbCommit);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.lbBranches);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.lbMsg);
            this.splitContainer1.Size = new System.Drawing.Size(541, 391);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 2;
            // 
            // lbCommit
            // 
            this.lbCommit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbCommit.FormattingEnabled = true;
            this.lbCommit.Location = new System.Drawing.Point(0, 167);
            this.lbCommit.Name = "lbCommit";
            this.lbCommit.Size = new System.Drawing.Size(180, 224);
            this.lbCommit.TabIndex = 2;
            this.lbCommit.Click += new System.EventHandler(this.lbCommit_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 164);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(180, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // lbBranches
            // 
            this.lbBranches.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBranches.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbBranches.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbBranches.FormattingEnabled = true;
            this.lbBranches.ItemHeight = 16;
            this.lbBranches.Location = new System.Drawing.Point(0, 0);
            this.lbBranches.Name = "lbBranches";
            this.lbBranches.Size = new System.Drawing.Size(180, 164);
            this.lbBranches.TabIndex = 0;
            this.lbBranches.Click += new System.EventHandler(this.lbBranches_Click);
            // 
            // btClear
            // 
            this.btClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClear.Location = new System.Drawing.Point(297, 8);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(27, 23);
            this.btClear.TabIndex = 1;
            this.btClear.Text = "x";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
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
            this.lbMsg.SizeChanged += new System.EventHandler(this.lbMsg_SizeChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(222, 6);
            // 
            // MapToJsonItem
            // 
            this.MapToJsonItem.Name = "MapToJsonItem";
            this.MapToJsonItem.Size = new System.Drawing.Size(209, 22);
            this.MapToJsonItem.Text = "Экспорт карты в json...";
            this.MapToJsonItem.Click += new System.EventHandler(this.MapToJsonItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
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
        private System.Windows.Forms.ToolStripMenuItem saveJsonAs;
        private System.Windows.Forms.ToolStripStatusLabel statusProgress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem LoadMap;
        private System.Windows.Forms.ToolStripMenuItem dialWorkDir;
        private System.Windows.Forms.ToolStripStatusLabel statusWorkDir;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.ListBox lbCommit;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem deleteCommit;
        private System.Windows.Forms.ToolStripMenuItem loadUpdates;
        private System.Windows.Forms.ToolStripMenuItem MapToJsonItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}

