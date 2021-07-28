
namespace RothsAutoTull
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.clientBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.spKeyBox = new System.Windows.Forms.ComboBox();
            this.hpKeyBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spBox = new System.Windows.Forms.ComboBox();
            this.hpBox = new System.Windows.Forms.ComboBox();
            this.spLabel = new System.Windows.Forms.Label();
            this.hpLabel = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panaceaBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clientBox
            // 
            this.clientBox.FormattingEnabled = true;
            this.clientBox.Location = new System.Drawing.Point(62, 6);
            this.clientBox.Name = "clientBox";
            this.clientBox.Size = new System.Drawing.Size(219, 23);
            this.clientBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Client :";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.panaceaBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.spKeyBox);
            this.groupBox1.Controls.Add(this.hpKeyBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.spBox);
            this.groupBox1.Controls.Add(this.hpBox);
            this.groupBox1.Controls.Add(this.spLabel);
            this.groupBox1.Controls.Add(this.hpLabel);
            this.groupBox1.Location = new System.Drawing.Point(14, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 174);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Pots";
            // 
            // spKeyBox
            // 
            this.spKeyBox.FormattingEnabled = true;
            this.spKeyBox.Location = new System.Drawing.Point(123, 51);
            this.spKeyBox.Name = "spKeyBox";
            this.spKeyBox.Size = new System.Drawing.Size(121, 23);
            this.spKeyBox.TabIndex = 7;
            // 
            // hpKeyBox
            // 
            this.hpKeyBox.FormattingEnabled = true;
            this.hpKeyBox.Location = new System.Drawing.Point(123, 20);
            this.hpKeyBox.Name = "hpKeyBox";
            this.hpKeyBox.Size = new System.Drawing.Size(121, 23);
            this.hpKeyBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "%";
            // 
            // spBox
            // 
            this.spBox.FormattingEnabled = true;
            this.spBox.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99",
            "100"});
            this.spBox.Location = new System.Drawing.Point(36, 50);
            this.spBox.Name = "spBox";
            this.spBox.Size = new System.Drawing.Size(62, 23);
            this.spBox.TabIndex = 3;
            this.spBox.Text = "50";
            // 
            // hpBox
            // 
            this.hpBox.FormattingEnabled = true;
            this.hpBox.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99",
            "100"});
            this.hpBox.Location = new System.Drawing.Point(36, 20);
            this.hpBox.Name = "hpBox";
            this.hpBox.Size = new System.Drawing.Size(62, 23);
            this.hpBox.TabIndex = 2;
            this.hpBox.Text = "100";
            // 
            // spLabel
            // 
            this.spLabel.AutoSize = true;
            this.spLabel.Location = new System.Drawing.Point(7, 54);
            this.spLabel.Name = "spLabel";
            this.spLabel.Size = new System.Drawing.Size(20, 15);
            this.spLabel.TabIndex = 1;
            this.spLabel.Text = "SP";
            // 
            // hpLabel
            // 
            this.hpLabel.AutoSize = true;
            this.hpLabel.Location = new System.Drawing.Point(7, 23);
            this.hpLabel.Name = "hpLabel";
            this.hpLabel.Size = new System.Drawing.Size(23, 15);
            this.hpLabel.TabIndex = 0;
            this.hpLabel.Text = "HP";
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(37, 362);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(94, 29);
            this.startBtn.TabIndex = 3;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(137, 362);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(94, 29);
            this.stopBtn.TabIndex = 4;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Panacea";
            // 
            // panaceaBox
            // 
            this.panaceaBox.FormattingEnabled = true;
            this.panaceaBox.Location = new System.Drawing.Point(64, 86);
            this.panaceaBox.Name = "panaceaBox";
            this.panaceaBox.Size = new System.Drawing.Size(121, 23);
            this.panaceaBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 403);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clientBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox clientBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox spBox;
        private System.Windows.Forms.ComboBox hpBox;
        private System.Windows.Forms.Label spLabel;
        private System.Windows.Forms.Label hpLabel;
        private System.Windows.Forms.ComboBox spKeyBox;
        private System.Windows.Forms.ComboBox hpKeyBox;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.ComboBox panaceaBox;
        private System.Windows.Forms.Label label4;
    }
}

