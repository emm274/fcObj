namespace lx2video
{
    partial class MainForm
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
            this.lbLatitude = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbLongitude = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAltitude = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbRoll = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPitch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lbDirection = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbLatitude
            // 
            this.lbLatitude.Location = new System.Drawing.Point(195, 12);
            this.lbLatitude.Name = "lbLatitude";
            this.lbLatitude.ReadOnly = true;
            this.lbLatitude.Size = new System.Drawing.Size(132, 20);
            this.lbLatitude.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "latitude";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "longitude";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbLongitude
            // 
            this.lbLongitude.Location = new System.Drawing.Point(195, 38);
            this.lbLongitude.Name = "lbLongitude";
            this.lbLongitude.ReadOnly = true;
            this.lbLongitude.Size = new System.Drawing.Size(132, 20);
            this.lbLongitude.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "altitude";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbAltitude
            // 
            this.lbAltitude.Location = new System.Drawing.Point(195, 64);
            this.lbAltitude.Name = "lbAltitude";
            this.lbAltitude.ReadOnly = true;
            this.lbAltitude.Size = new System.Drawing.Size(132, 20);
            this.lbAltitude.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "roll";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRoll
            // 
            this.lbRoll.Location = new System.Drawing.Point(195, 90);
            this.lbRoll.Name = "lbRoll";
            this.lbRoll.ReadOnly = true;
            this.lbRoll.Size = new System.Drawing.Size(132, 20);
            this.lbRoll.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "pitch";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbPitch
            // 
            this.lbPitch.Location = new System.Drawing.Point(195, 116);
            this.lbPitch.Name = "lbPitch";
            this.lbPitch.ReadOnly = true;
            this.lbPitch.Size = new System.Drawing.Size(132, 20);
            this.lbPitch.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(145, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "direction";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbDirection
            // 
            this.lbDirection.Location = new System.Drawing.Point(195, 142);
            this.lbDirection.Name = "lbDirection";
            this.lbDirection.ReadOnly = true;
            this.lbDirection.Size = new System.Drawing.Size(132, 20);
            this.lbDirection.TabIndex = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 355);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbDirection);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbPitch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbRoll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbAltitude);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbLongitude);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbLatitude);
            this.Name = "MainForm";
            this.Text = "lx2 client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox lbLatitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox lbLongitude;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox lbAltitude;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox lbRoll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox lbPitch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox lbDirection;
    }
}

