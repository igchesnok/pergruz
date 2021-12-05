using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PERGRUZ
{
    public partial class karta : Form
    {
        public delegate void AccountHandler();
        public event AccountHandler Marshryt;
        public string[,] Dan;
        public karta()
        {
            var path = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            WebCore.Initialize(new WebConfig { PluginsPath = path + "\\plugins" });

            InitializeComponent();
            this.webControl1.Source = new System.Uri(path + "\\swfobject\\router.html", System.UriKind.Absolute);
            webControl1.LoadingFrameComplete += LoadingFramecompleted;
        }
        public void LoadingFramecompleted(object sender, FrameEventArgs e)
        {
            //после завершения загрузки создать глобальные объекты вызова функций:
            //  2) Чтение метаданных выводимых на карту  объектов
            JSObject obj1 = webControl1.CreateGlobalJavascriptObject("jso_geo_Objects");
            obj1.Bind(geo_Objects);
        }
        private JSValue geo_Objects(object sender, JavascriptMethodEventArgs e)//  2) Чтение метаданных выводимых на карту  объектов
        {
            object[] amas = (object[])e.Arguments[0];
           if (amas.Count() > 0)
            {
                string[,] dan = new string[amas.Count(), 2];//МАСИВ С УЛИЦ ИЗ МАРШРУТА
                int i = 0;
                foreach (var item in amas)
                {
                    string ylica = Convert.ToString((object)e.Arguments[0][i][0]);
                    string L_ylica = Convert.ToString((object)e.Arguments[0][i][1]);
                    dan[i, 0] = ylica;
                    dan[i, 1] = L_ylica;
                    i++;
                }
                Dan = dan;
                Marshryt?.Invoke();   // 2.Вызов события 

            }
            return "My geo_Objects";
        }
        private int x = 0; private int y = 0;
        private void karta_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X; y = e.Y;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            karta_MouseDown(this, e);
            {
                if (e.Button == MouseButtons.Left)
                    this.OnMouseDown(e);
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void karta_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));

            }
        }
    }
}