using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.Events;

namespace PERGRUZ
{
    public partial class shet : ReoGridControl
    {
        private Worksheet worksheet;// УКАЗАТЕЛТ ЛИСТА

        //местоположение курсора
        private string[] zagolovok_=new string[2];
        public string[] zagolovok
        {
            get { return zagolovok_; }
            set
            {
               string[] oldValue = zagolovok_;
                zagolovok_ = value;
                if (oldValue != zagolovok_)
                {
                    int i = 0;
                    foreach (var naim in zagolovok_)
                    {
                        // получаем экземпляры заголовка
                        var colHeader = worksheet.ColumnHeaders[i];
                        colHeader.Text = naim;
                        i++;
                        Invalidate();
                        Update();
                    }
                }
            }
        }
        public shet()
        {
            InitializeComponent();
            worksheet = CurrentWorksheet;
            worksheet.SelectionRangeChanged += (s, e) =>
            {
                RangePosition rect = worksheet.SelectionRange;//отслеживаем сообщение выделения ячеек и получаем регион
            };
            SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);//ОТКЛЮЧАЕМ ИНСТРУМЕНТ ДОБАВЛЕНИЯ И ПЕРЕКЛЮЧЕНИЯ ЛИСТОВ
         //   CurrentWorksheet.SetSettings(WorksheetSettings.View_ShowHeaders, false);//ОТКЛЮЧАЕМ ЗАГОЛОВКИ
          //  CurrentWorksheet.SetSettings(WorksheetSettings.View_ShowGridLine, false); //ОТКЛЮЧАЕМ ЛИНИИ СЕТКИ
            ShowScrollEndSpacing = false;//НЕдобавлять в конец листа немного свободного пространства
            CurrentWorksheet.Resize(1, 5);//Задаём размер таблицы используя указанные выше переменные
                                                             // worksheet.SelectionMode = WorksheetSelectionMode.None;//ОТКЛЮЧАЕМ ВЫДЕЛЕНИЕ ЯЧЕЙКИ
            worksheet.SetSettings(WorksheetSettings.Edit_Readonly, true);//ОТКЛЮЧАЕМ РЕДАКТИРОВАНИЕ ЯЧЕЕК
            //Перенос текста (по умолчанию - без переноса, - no-wrap)
            //Создаём экземпляр стиля
             WorksheetRangeStyle MyStyle = new WorksheetRangeStyle();
              MyStyle =CurrentWorksheet.GetRangeStyles(CurrentWorksheet.SelectionRange);
             MyStyle.TextWrapMode = TextWrapMode.WordBreak;
            worksheet.SelectionMode = WorksheetSelectionMode.Row;//выделение всей строки
            worksheet.CellEditCharInputed += worksheet_CellEditCharInputed;
            worksheet.CellEditTextChanging += worksheet_CellEditTextChanging;
            worksheet.CellDataChanged += worksheet_CellDataChanged;
            worksheet.CellMouseDown += worksheet_CellMouseDown;

        }

        private void worksheet_CellEditCharInputed(object sender, CellEditCharInputEventArgs e)
        {

        }
        private void worksheet_CellEditTextChanging(object sender, CellEditTextChangingEventArgs e)
        {

        }


        private void worksheet_CellDataChanged(object sender, CellEventArgs e)
        {
            //   reoGridControl1.CurrentWorksheet.AutoFitRowHeight(e.Cell.Row, true);
        }
        private int celCol = 0;
        private void worksheet_CellMouseDown(object sender, CellMouseEventArgs e)
        {
            var cell = e.Cell;
            if (cell != null) celCol = cell.Column;
            else celCol = 0;
            //  var sheet = reoGridControl1.CurrentWorksheet;
            //  cell = sheet.CreateAndGetCell(e.CellPosition);
        }
    }
}
