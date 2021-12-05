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
    public partial class blank_raschet : Form
    {
       private rasch_comp rasch_comp1 = null;
        public blank_raschet( rasch_comp rasch_comp_)
        {
            InitializeComponent();
            rasch_comp1 = rasch_comp_;
            this.panel_rasch.Controls.Add(rasch_comp1);
            
        }

    private void Close_Click(object sender, EventArgs e)
        {
           this.panel_rasch.Controls.Remove(rasch_comp1);
            Close();
        }
        private void blankNew_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
           
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Capture = false;
            Message m = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }
        private void zagolovok_editzaivit_MouseDown(object sender, MouseEventArgs e)
        {
            zagolovok_editzaivit.Capture = false;
            Message m = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }
       
    }
}
