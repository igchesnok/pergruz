using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PERGRUZ
{
    public partial class obrachen : UserControl
    {
       // [DllImport("user32.dll")]
     //   static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public rasch_comp rasch_comp = null;
        public obrachen()
        {
            InitializeComponent();
            toolTip_r1.SetToolTip(B_edit_zaivit, "Добавить нового заявителя");
        }

        private void red_raschet_Click(object sender, EventArgs e)
        {
            blank_raschet testDialog = new blank_raschet(rasch_comp);
            //  ShowWindow(testDialog.Handle, 3);
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
        private void CH_razr_CheckedChanged(object sender, EventArgs e)
        {
            L_dok_otkaz.Text = "№ постановления(отказа)";//"№ отказа выдачи согласования"
            L_Sigl_Razresh.Text = "№ разреш-ия";//"№ отказа выдачи согласования"

            L_gosposhlina.Visible = true;
            TB_gosposhlina     .Visible = true;
            L_gosposhlina_R    .Visible = true;
            CB_gosposhlina_F   .Visible = true;
            B_add_gosposh      .Visible = true;
            B_print_razresh    .Visible = true;
            B_print_postRazr   .Visible = true;
            L_viz_post         .Visible = true;
            CB_viz_post        .Visible = true;
                              
            B_print_Sogl.Visible = false;
            GP_plateji.Refresh();
        }

        private void CH_sogl_CheckedChanged(object sender, EventArgs e)
        {
            L_dok_otkaz.Text = "№ отказа выдачи согласования";//""
            L_Sigl_Razresh.Text = "№ согл-ия";//"№ отказа выдачи согласования"

            L_gosposhlina.Visible = false;
            TB_gosposhlina.Visible = false;
            L_gosposhlina_R.Visible = false;
            CB_gosposhlina_F.Visible = false;
            B_add_gosposh.Visible = false;
            B_print_razresh.Visible = false;
            B_print_postRazr.Visible = false;
            L_viz_post.Visible = false;
            CB_viz_post.Visible = false;

            B_print_Sogl.Visible = true;
            GP_plateji.Refresh();
        }

        private void B_edit_zaivit_Click(object sender, EventArgs e)
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
    }
}
