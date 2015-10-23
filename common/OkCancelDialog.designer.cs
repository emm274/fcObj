namespace OkCancelDialogs
{
    partial class OkCancelDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.paBottom = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.paClient = new System.Windows.Forms.Panel();
            this.paBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // paBottom
            // 
            this.paBottom.Controls.Add(this.btCancel);
            this.paBottom.Controls.Add(this.btOK);
            this.paBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paBottom.Location = new System.Drawing.Point(0, 234);
            this.paBottom.Name = "paBottom";
            this.paBottom.Size = new System.Drawing.Size(292, 39);
            this.paBottom.TabIndex = 0;
            this.paBottom.Resize += new System.EventHandler(this.paBottom_Resize);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(144, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(53, 8);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // paClient
            // 
            this.paClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paClient.Location = new System.Drawing.Point(0, 0);
            this.paClient.Name = "paClient";
            this.paClient.Size = new System.Drawing.Size(292, 234);
            this.paClient.TabIndex = 1;
            // 
            // OkCancelDialog
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.paClient);
            this.Controls.Add(this.paBottom);
            this.Name = "OkCancelDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OkCancelDialog_FormClosed);
            this.Load += new System.EventHandler(this.OkCancelDialog_Load);
            this.paBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel paBottom;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Panel paClient;
    }
}