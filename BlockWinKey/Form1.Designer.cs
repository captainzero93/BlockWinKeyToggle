namespace BlockWinKey
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
            this.components = new System.ComponentModel.Container();
            this.toggleCheckBox = new System.Windows.Forms.CheckBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // toggleCheckBox
            // 
            this.toggleCheckBox.AutoSize = true;
            this.toggleCheckBox.Location = new System.Drawing.Point(10, 10);
            this.toggleCheckBox.Name = "toggleCheckBox";
            this.toggleCheckBox.Size = new System.Drawing.Size(121, 17);
            this.toggleCheckBox.TabIndex = 0;
            this.toggleCheckBox.Text = "Block Windows Key";
            this.toggleCheckBox.UseVisualStyleBackColor = true;
            this.toggleCheckBox.CheckedChanged += new System.EventHandler(this.toggleCheckBox_CheckedChanged);
            // 
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(10, 57);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(223, 131);
            this.logTextBox.TabIndex = 2;
            // 
            // trayIcon
            // 
            this.trayIcon.Text = "Windows Key Blocker";
            this.trayIcon.Visible = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 198);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.toggleCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows Key Blocker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox toggleCheckBox;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.NotifyIcon trayIcon;
    }
}