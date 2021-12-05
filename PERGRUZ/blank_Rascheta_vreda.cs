using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;
using unvell.ReoGrid.IO;

namespace PERGRUZ
{
    public partial class blank_Rascheta_vreda : Form
    {
        public ReoGridControl reoGrid;
        public string naimfail="";
        public blank_Rascheta_vreda()
        {
            InitializeComponent();
            try
            {
                reoGridControl1.Load(Application.StartupPath + "\\dan\\BLANK_RASCH_VREDA.xlsx", unvell.ReoGrid.IO.FileFormat.Excel2007, Encoding.UTF8);
                reoGridControl1.SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);//ОТКЛЮЧАЕМ ИНСТРУМЕНТ ДОБАВЛЕНИЯ И ПЕРЕКЛЮЧЕНИЯ ЛИСТОВ
                reoGridControl1.CurrentWorksheet.SetSettings(WorksheetSettings.View_ShowHeaders, false);//ОТКЛЮЧАЕМ ЗАГОЛОВКИ
                reoGridControl1.CurrentWorksheet.SetSettings(WorksheetSettings.View_ShowGridLine, false); //ОТКЛЮЧАЕМ ЛИНИИ СЕТКИ
                reoGridControl1.ShowScrollEndSpacing = false;//НЕдобавлять в конец листа немного свободного пространства
                reoGridControl1.CurrentWorksheet.Resize(221, 11);//Задаём размер таблицы используя указанные выше переменные
                                                                 // worksheet.SelectionMode = WorksheetSelectionMode.None;//ОТКЛЮЧАЕМ ВЫДЕЛЕНИЕ ЯЧЕЙКИ
                reoGridControl1.CurrentWorksheet.SetSettings(WorksheetSettings.Edit_Readonly, true);//ОТКЛЮЧАЕМ РЕДАКТИРОВАНИЕ ЯЧЕЕК


                reoGrid = reoGridControl1;

            }
            catch (Exception)
            {

               
            }
        }
        private int x = 0; private int y = 0;
        private void blank_Rascheta_vreda_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X; y = e.Y;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            reoGridControl1.Load(Application.StartupPath + "\\dan\\BLANK_RASCH_VREDA.xlsx", unvell.ReoGrid.IO.FileFormat.Excel2007, Encoding.UTF8);
            reoGridControl1.SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);//ОТКЛЮЧАЕМ ИНСТРУМЕНТ ДОБАВЛЕНИЯ И ПЕРЕКЛЮЧЕНИЯ ЛИСТОВ
            reoGridControl1.CurrentWorksheet.SetSettings(WorksheetSettings.View_ShowHeaders, false);//ОТКЛЮЧАЕМ ЗАГОЛОВКИ
            reoGridControl1.CurrentWorksheet.SetSettings(WorksheetSettings.View_ShowGridLine, false); //ОТКЛЮЧАЕМ ЛИНИИ СЕТКИ
            reoGridControl1.ShowScrollEndSpacing = false;//НЕдобавлять в конец листа немного свободного пространства
            reoGridControl1.CurrentWorksheet.Resize(221, 11);//Задаём размер таблицы используя указанные выше переменные
                                                             // worksheet.SelectionMode = WorksheetSelectionMode.None;//ОТКЛЮЧАЕМ ВЫДЕЛЕНИЕ ЯЧЕЙКИ
            reoGridControl1.CurrentWorksheet.SetSettings(WorksheetSettings.Edit_Readonly, true);//ОТКЛЮЧАЕМ РЕДАКТИРОВАНИЕ ЯЧЕЕК

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            blank_Rascheta_vreda_MouseDown(this, e);
            {
                if (e.Button == MouseButtons.Left)
                    this.OnMouseDown(e);
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void blank_Rascheta_vreda_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int filesCount = Directory.GetFiles(Application.StartupPath + "\\расчет" ,"*.*", SearchOption.TopDirectoryOnly).Length; //только в текущей папке
            string put = Application.StartupPath + "\\расчет\\" + naimfail +"_"+ filesCount + ".xlsx";
            reoGridControl1.Save(put, unvell.ReoGrid.IO.FileFormat.Excel2007);
        }
    }
}
