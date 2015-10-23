namespace enterText
{
    partial class EnterText
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
            this.paClient = new System.Windows.Forms.Panel();
            this.paBottom = new System.Windows.Forms.Panel();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.lbText = new System.Windows.Forms.Label();
            this.paClient.SuspendLayout();
            this.paBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // paClient
            // 
            this.paClient.Controls.Add(this.lbText);
            this.paClient.Controls.Add(this.textBox);
            this.paClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paClient.Location = new System.Drawing.Point(0, 0);
            this.paClient.Name = "paClient";
            this.paClient.Size = new System.Drawing.Size(362, 113);
            this.paClient.TabIndex = 0;
            // 
            // paBottom
            // 
            this.paBottom.Controls.Add(this.btCancel);
            this.paBottom.Controls.Add(this.btOK);
            this.paBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paBottom.Location = new System.Drawing.Point(0, 72);
            this.paBottom.Name = "paBottom";
            this.paBottom.Size = new System.Drawing.Size(362, 41);
            this.paBottom.TabIndex = 1;
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(86, 8);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(181, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(12, 35);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(338, 20);
            this.textBox.TabIndex = 0;
            // 
            // lbText
            // 
            this.lbText.AutoSize = true;
            this.lbText.Location = new System.Drawing.Point(29, 19);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(28, 13);
            this.lbText.TabIndex = 1;
            this.lbText.Text = "Text";
            // 
            // EnterText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 113);
            this.Controls.Add(this.paBottom);
            this.Controls.Add(this.paClient);
            this.Name = "EnterText";
            this.Text = "EnterText";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EnterText_FormClosed);
            this.Load += new System.EventHandler(this.EnterText_Load);
            this.paClient.ResumeLayout(false);
            this.paClient.PerformLayout();
            this.paBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel paClient;
        private System.Windows.Forms.Label lbText;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Panel paBottom;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
    }
}