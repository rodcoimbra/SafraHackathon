namespace Report_Generator_V1
{
    partial class Form1
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
            this.buttonGenReport = new System.Windows.Forms.Button();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonGenReport
            // 
            this.buttonGenReport.Location = new System.Drawing.Point(72, 164);
            this.buttonGenReport.Name = "buttonGenReport";
            this.buttonGenReport.Size = new System.Drawing.Size(159, 34);
            this.buttonGenReport.TabIndex = 0;
            this.buttonGenReport.Text = "Generate Report";
            this.buttonGenReport.UseVisualStyleBackColor = true;
            this.buttonGenReport.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(64, 67);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(61, 13);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "User name:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(131, 64);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(131, 108);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 4;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(69, 111);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 347);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.buttonGenReport);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGenReport;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label labelPassword;
    }
}

