using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PERGRUZ
{
    public partial class Form_Glavn : Form
    {
        public karta karta_ = null;
        private rasch_comp rasch_comp_ = null;
        public Form_Glavn()
        {
            InitializeComponent();
            rasch_comp_ = new rasch_comp();
            obrachen1.rasch_comp= rasch_comp_;

            karta_ = rasch_comp_.karta_;
            SetDoubleBuffered(obrachen1);
        }
        private void SetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, control, new object[] { true });
        }
        //обработка закрытия приложения освобожденеи ресурсов

        private void Form_Glavn_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            this.Dispose(true);
            if (karta_ != null)
            {
                karta_.Dispose();
            }
        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            superTabControl1.CreateTab("sdfd");
        }

        
      

        private void new_zaivl_Click_1(object sender, EventArgs e)
        {
            blank_zaivl testDialog = new blank_zaivl();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                //  this.txtResult.Text = testDialog.TextBox1.Text;
            }
            else
            {
                // this.txtResult.Text = "Cancelled";
            }
            testDialog.Dispose();
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            blankNew testDialog = new blankNew();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                //  this.txtResult.Text = testDialog.TextBox1.Text;
            }
            else
            {
                // this.txtResult.Text = "Cancelled";
            }
            testDialog.Dispose();
        }

        private void superTabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
        {

        }
    }
}
