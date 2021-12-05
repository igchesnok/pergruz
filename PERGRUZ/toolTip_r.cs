using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PERGRUZ
{
    public partial class toolTip_r : ToolTip
    {
        public toolTip_r()
        {
            InitializeComponent();
            this.OwnerDraw = true;
            Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            Draw += new DrawToolTipEventHandler(this.toolTip_Draw);

        }

        public toolTip_r(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            OwnerDraw = true;
            Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            Draw += new DrawToolTipEventHandler(this.toolTip_Draw);

        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            int hfont = 10;
          
            using (Font f = new Font("Tahoma", hfont))
            {
                e.ToolTipSize = TextRenderer.MeasureText(
                    GetToolTip(e.AssociatedControl), f);
            }
        }
        //функция меняет шрифт и цвет всплывающей подсказки
        private void toolTip_Draw(System.Object sender, System.Windows.Forms.DrawToolTipEventArgs e)
        {
            int hfont = 12;
            Brush brush = Brushes.Black;
          
            e.Graphics.FillRectangle(Brushes.AliceBlue, e.Bounds);
            e.DrawBorder();
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                using (Font f = new Font("Tahoma", hfont))
                {
                    e.Graphics.DrawString(e.ToolTipText, f, brush, e.Bounds, sf);
                }
            }
        }
    }
}
