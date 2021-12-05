namespace PERGRUZ
{
    partial class blank_zaivl
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
            this.zagolovok_editzaivit = new System.Windows.Forms.Label();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SlateGray;
            this.panel1.Controls.Add(this.zagolovok_editzaivit);
            this.panel1.Controls.Add(this.buttonX2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1165, 26);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // zagolovok_editzaivit
            // 
            this.zagolovok_editzaivit.AutoSize = true;
            this.zagolovok_editzaivit.BackColor = System.Drawing.Color.SlateGray;
            this.zagolovok_editzaivit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.zagolovok_editzaivit.ForeColor = System.Drawing.Color.GhostWhite;
            this.zagolovok_editzaivit.Location = new System.Drawing.Point(271, 5);
            this.zagolovok_editzaivit.Name = "zagolovok_editzaivit";
            this.zagolovok_editzaivit.Size = new System.Drawing.Size(276, 20);
            this.zagolovok_editzaivit.TabIndex = 0;
            this.zagolovok_editzaivit.Text = "РЕДАКТОР ДАННЫХ ОБРАЩЕНИЯ";
            this.zagolovok_editzaivit.UseMnemonic = false;
            this.zagolovok_editzaivit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zagolovok_editzaivit_MouseDown);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonX2.Location = new System.Drawing.Point(1142, 2);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(21, 21);
            this.buttonX2.TabIndex = 0;
            this.buttonX2.Text = "Х";
            this.buttonX2.TextColor = System.Drawing.Color.Red;
            this.buttonX2.Click += new System.EventHandler(this.Close_Click);
            // 
            // blank_zaivl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1165, 782);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "blank_zaivl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "РЕДАКТОР ДАННЫХ ЗАЯВЛЕНИЯ";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blank_zaivl_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label zagolovok_editzaivit;
        private DevComponents.DotNetBar.ButtonX buttonX2;
    }
}