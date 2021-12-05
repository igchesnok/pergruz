namespace PERGRUZ
{
    partial class blank_raschet
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
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.zagolovok_editzaivit = new System.Windows.Forms.Label();
            this.panel_rasch = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX1.Location = new System.Drawing.Point(947, 887);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "ОТМЕНА";
            this.buttonX1.Click += new System.EventHandler(this.Close_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SlateGray;
            this.panel1.Controls.Add(this.buttonX2);
            this.panel1.Controls.Add(this.zagolovok_editzaivit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1023, 27);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonX2.Location = new System.Drawing.Point(998, 4);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(22, 20);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 0;
            this.buttonX2.Text = "Х";
            this.buttonX2.TextColor = System.Drawing.Color.Red;
            this.buttonX2.Click += new System.EventHandler(this.Close_Click);
            // 
            // zagolovok_editzaivit
            // 
            this.zagolovok_editzaivit.AutoSize = true;
            this.zagolovok_editzaivit.BackColor = System.Drawing.Color.SlateGray;
            this.zagolovok_editzaivit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.zagolovok_editzaivit.ForeColor = System.Drawing.Color.GhostWhite;
            this.zagolovok_editzaivit.Location = new System.Drawing.Point(171, 4);
            this.zagolovok_editzaivit.Name = "zagolovok_editzaivit";
            this.zagolovok_editzaivit.Size = new System.Drawing.Size(556, 20);
            this.zagolovok_editzaivit.TabIndex = 0;
            this.zagolovok_editzaivit.Text = "РЕДАКТОР  ПАРАМЕТРОВ УЧИТЫВАЕМЫХ ПРИ РАСЧЕТЕ ПЕРЕГРУЗА";
            this.zagolovok_editzaivit.UseMnemonic = false;
            this.zagolovok_editzaivit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zagolovok_editzaivit_MouseDown);
            // 
            // panel_rasch
            // 
            this.panel_rasch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_rasch.Location = new System.Drawing.Point(0, 27);
            this.panel_rasch.Name = "panel_rasch";
            this.panel_rasch.Size = new System.Drawing.Size(1023, 856);
            this.panel_rasch.TabIndex = 3;
            // 
            // blank_raschet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1023, 912);
            this.ControlBox = false;
            this.Controls.Add(this.panel_rasch);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "blank_raschet";
            this.Opacity = 0.96D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blankNew_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label zagolovok_editzaivit;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        public System.Windows.Forms.Panel panel_rasch;
    }
}