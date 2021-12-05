using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;
using unvell.ReoGrid.DataFormat;
using unvell.ReoGrid.IO;


namespace PERGRUZ
{
    public partial class rasch_comp_ : UserControl
    {
        
        public Worksheet worksheet;
        public karta karta_ = null;
        public blank_Rascheta_vreda Rasch_Vred_ = null;
        private WorksheetRangeStyle MyStyle_itog;
        private WorksheetRangeStyle MyStyle_itogC;
        private WorksheetRangeStyle MyStyle_GRUP;
        private RangeBorderStyle MyStyleBorder;
        private int tekMarshryt = 1;//текущее колличество введенных маршрутов
        private List<marsh_obr> DanL_ = new List<marsh_obr>();

        //СТРУКТУРА ХРАНЕНИЯ ДАННЫХ ПО ГРУППЕ ТЕЛЕЖКИ
        struct Gdan
        {
            public List<int> nosi;//ПОРЯДКОВЫ НОМЕР ОСИ
            public double sumLosiG;//СУММА МЕЖОСЕВЫХ РАСТОЯНИЙ ПО ГРУППЕ ТЕЛЕЖКИ
            public double sumMosiG;//СУММА ОСЕВЫХ НАГРУЗОК ПО ГРУППЕ ТЕЛЕЖКИ
            public bool skatn;//СКАТНОСТЬ КОЛЕС ДЛЯ ТЕЛЕЖКИ
            public bool naOsi_Kol;//КОЛЛИЧЕСТВО КОЛЕС НА ОСИ
        }
        //структура хранения:   1. строки всех маршрутов, 
        //                      2. общая протяженность
        struct marshdon
        {
            public string str_marsh;
            public double prot_marsh;
        }
        //структура хранения маршрута
        struct marsh_obr
        {
            public string[,] marsh;//2-х мерный масив 1. участк улицы, 2. протяженность участка 
                                   //в послнднй записи масива хранится строка маршрута "автостроителей-свердлова" и общая протяженность
            public bool obrat;//признак перевозки груза в обратном направлении
        }

        public rasch_comp_()
        {
            InitializeComponent();
            reoGridControl1.SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);//ОТКЛЮЧАЕМ ИНСТРУМЕНТ ДОБАВЛЕНИЯ И ПЕРЕКЛЮЧЕНИЯ ЛИСТОВ
            reoGridControl1.ShowScrollEndSpacing = false;//НЕдобавлять в конец листа немного свободного пространства
            MyStyle_itog = new WorksheetRangeStyle();
            MyStyle_GRUP = new WorksheetRangeStyle();
            MyStyleBorder = new RangeBorderStyle();
            MyStyleBorder.Color = Color.FromArgb(127, 13, 40, 198); //Произвольный цвет с прозрачностью
            MyStyleBorder.Style = BorderLineStyle.Solid; //Сплошная

            MyStyle_itog.Flag = PlainStyleFlag.FontSize |//| PlainStyleFlag.FontStyleItalic | PlainStyleFlag.FontName | PlainStyleFlag.FillPattern | PlainStyleFlag.TextWrap 
           PlainStyleFlag.TextColor | PlainStyleFlag.FontStyleBold |
           PlainStyleFlag.HorizontalAlign | PlainStyleFlag.VerticalAlign | PlainStyleFlag.BackColor;
            MyStyle_itog.Bold = false; //Начертание шрифта Полужирный
            MyStyle_itog.HAlign = ReoGridHorAlign.Center; //По центру
            MyStyle_itog.VAlign = ReoGridVerAlign.Middle; //Посередине
            MyStyle_itog.FontSize = 12;
            MyStyle_itog.TextColor = Color.FromArgb(255, 40, 16, 99); //Произвольный цвет с прозрачностью  Изменяем цвет шрифта 
            MyStyle_itogC = new WorksheetRangeStyle();
            MyStyle_itogC.CopyFrom(MyStyle_itog);
            MyStyle_itogC.BackColor = Color.Empty; //Очищаем ячейки
            MyStyle_itogC.BackColor = Color.FromArgb(67, 177, 188, 250); //Произвольный цвет с прозрачностью  Изменяем цвет шрифта 

            MyStyle_GRUP.CopyFrom(MyStyle_itog);
            MyStyle_GRUP.BackColor = Color.Empty; //Очищаем ячейки
            MyStyle_GRUP.BackColor = Color.FromArgb(67, 177, 188, 150); //Произвольный цвет с прозрачностью  Изменяем цвет шрифта 


            worksheet = reoGridControl1.CurrentWorksheet;
            obnov(); //прорисовывает таблицу ввода осевых нагрузок
        }
        private void Form1_Load(object sender, EventArgs e)
        {
          //  karta_ = new karta();
         //  karta_.Show();
         //   karta_.Hide();
            Rasch_Vred_ = new blank_Rascheta_vreda();
            int god = DateTime.Now.Year;
            if (god == 2020)
            {
                comboBoxGod.SelectedIndex = 0;
            }
            else
            {
                if (god == 2021)
                {
                    comboBoxGod.SelectedIndex = 1;
                }
                else
                {
                    if (god == 2022)
                    {
                        comboBoxGod.SelectedIndex = 2;
                    }
                    else
                    {
                        comboBoxGod.SelectedIndex = 3;
                    }
                }
            }
            comboBox_maxdor.SelectedIndex = 1;
            karta_.Marshryt += Print_Marshryt;   // Добавляем обработчик для события Marshryt
        }

        private void buton_obnov_Click(object sender, EventArgs e)
        {
            obnov(); //прорисовывает таблицу ввода осевых нагрузок
        }
        //прорисовывает таблицу ввода осевых нагрузок
        private void obnov()
        {
            worksheet.Reset();
            worksheet.SetSettings(WorksheetSettings.View_ShowHeaders, false);//ОТКЛЮЧАЕМ ЗАГОЛОВКИ
            worksheet.SetSettings(WorksheetSettings.View_ShowGridLine, false); //ОТКЛЮЧАЕМ ЛИНИИ СЕТКИ
            int kol_osi = Convert.ToInt32(tBox_kolOsi.Text) * 2 + 1;

            worksheet.Resize(6, kol_osi);
            worksheet[0, 0] = "Заполните поля";
            worksheet[1, 0] = "№ оси";
            worksheet[2, 0] = "растояния межосевое, м";
            worksheet[3, 0] = "нагрузки осевые, т";
            worksheet[4, 0] = "кол-во колес на оси";
            worksheet[5, 0] = "скатность колес";
            //Получаем позицию заморозки/закрепления
            CellPosition pos = worksheet.FreezePos;
            //Проверяем - зафиксирован ли данный лист:
            bool isFrozen = worksheet.IsFrozen;
            //Фиксируем до указанной позиции (вводим третий аргумент, указывающий точку фиксации)
            worksheet.FreezeToCell(0, 1);//Обычная фиксация (указана только позиция)
            worksheet.SetColumnsWidth(0, 1, 200);
            //Числовой формат данных (Number)
            NumberDataFormatter.NumberFormatArgs FFNum = new NumberDataFormatter.NumberFormatArgs();
            FFNum.DecimalPlaces = 3; //Количество знаков после запятой/точки: 0,1234
            FFNum.UseSeparator = true; //Использовать разделитель: 123,456
            worksheet.SetRangeDataFormat(new RangePosition(2, 1, 2, kol_osi), CellDataFormatFlag.Number, FFNum);

            DropdownListCell mydropdown;
            int j = 1;
            for (int i = 1; i < kol_osi; i = i + 2)
            {
                mydropdown = new DropdownListCell("1", "2");
                worksheet[5, i] = "1";
                worksheet[5, i] = new object[] { mydropdown };

                mydropdown = new DropdownListCell("<4", ">=4");
                worksheet[4, i] = "<4";
                worksheet[4, i] = new object[] { mydropdown };

                worksheet[1, i] = j;
                //Обьединяем ячейки
                reoGridControl1.CurrentWorksheet.MergeRange(new RangePosition(1, i, 1, 2));
                reoGridControl1.CurrentWorksheet.MergeRange(new RangePosition(3, i, 1, 2));
                reoGridControl1.CurrentWorksheet.MergeRange(new RangePosition(4, i, 1, 2));
                reoGridControl1.CurrentWorksheet.MergeRange(new RangePosition(5, i, 1, 2));
                reoGridControl1.CurrentWorksheet.MergeRange(new RangePosition(2, i + 1, 1, 2));
                j++;
            }
            //ширина ячеек
            worksheet.SetColumnsWidth(1, kol_osi - 1, 40);
            //ФОРМАТИРУЕМ СТОРОКУ ИТОГО ПО ДНЯМ ПО ГОРОДУ
            reoGridControl1.CurrentWorksheet.SetRangeStyles(new RangePosition(1, 0, 5, kol_osi), MyStyle_itog);

            reoGridControl1.CurrentWorksheet.SetRangeStyles(new RangePosition(0, 0, 2, kol_osi), MyStyle_itogC);
            reoGridControl1.CurrentWorksheet.SetRangeStyles(new RangePosition(2, 0, 6, 1), MyStyle_itogC);
            reoGridControl1.CurrentWorksheet.SetRangeStyles(new RangePosition(2, 1, 1, 1), MyStyle_itogC);
            reoGridControl1.CurrentWorksheet.SetRangeStyles(new RangePosition(2, kol_osi, 1, 1), MyStyle_itogC);

            //Параметры границ диапазона или ячейки
            reoGridControl1.CurrentWorksheet.SetRangeBorders(new RangePosition(1, 0, 5, kol_osi), BorderPositions.All, MyStyleBorder); //Все границы

            ButtonCell button = new ButtonCell("РАСЧЕТ");
            reoGridControl1.CurrentWorksheet.MergeRange(new RangePosition(0, 1, 1, 3));
            reoGridControl1.CurrentWorksheet[0, 1] = button;
            button.Click += (s, e) => RaschetNosi_Click();


        }
        //обработка закрытия приложения освобожденеи ресурсов
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose(true);
            if (karta_ != null)
            {
                karta_.Dispose();
            }
        }
        //показываем карту
        private void button1_Click(object sender, EventArgs e)
        {
            karta_.Show();
        }
        //кнопка очистки маршрута
        private void button2_Click(object sender, EventArgs e)
        {
            richText_Marshryt.Clear();
            textBoxMarch.Text = "1";
            tekMarshryt = 1;
            DanL_.Clear();
            comboBox_Lmarshryt.Items.Clear();
        }

        //обработка события выбора маршрута в браузере
        private void Print_Marshryt()
        {
            marsh_obr Dan_;//ДВУХ МЕРНЫЙ МАСИВ ХРАНЕНИЯ МАРШРУТА 1. УЧАСТОК УЛИЦИ, 2. ЕГО ПРОТЯЖЕННОСТЬ И ПРИЗНАК ОБРАТНОГО ХОДА ПО МАРШРУТУ
            Dan_.marsh = karta_.Dan;
            if (checkBox_obratno.Checked == false)
            {
                Dan_.obrat = false;// rez.str_marsh = rez.str_marsh + i + ". " + marsh[marsh.GetUpperBound(0), 0] + " ";
            }
            else
            {
                Dan_.obrat = true;// rez.str_marsh = rez.str_marsh + i + ". " + marsh[marsh.GetUpperBound(0), 0] + " — и обратно ";
            }


            DanL_.Add(Dan_);//СПИСОК ХРАНЕИЯ МАРШРУТОВ

            marshdon rez = str_Marshryt();//заполняем стркутуру с данными по маршрутуам
            textBoxMarch.Text = rez.prot_marsh.ToString();//отображаем общую протяженность
            richText_Marshryt.Text = rez.str_marsh;//отображаем все маршруту

            tekMarshryt++;
            comboBox_Lmarshryt.Items.Add(tekMarshryt - 1);
            comboBox_Lmarshryt.SelectedIndex = comboBox_Lmarshryt.Items.Count - 1;


        }
        //обработка нажатия кнопки удаления маршрута
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox_Lmarshryt.Text != "")
            {
                int tek_marsh = Convert.ToInt32(comboBox_Lmarshryt.Text);
                DanL_.RemoveAt(tek_marsh - 1);

                marshdon rez = str_Marshryt();//заполняем стркутуру с данными по маршрутуам
                textBoxMarch.Text = rez.prot_marsh.ToString();//отображаем общую протяженность
                richText_Marshryt.Text = rez.str_marsh;//отображаем все маршруту

                tekMarshryt--;
                comboBox_Lmarshryt.Items.Remove(tekMarshryt);
                comboBox_Lmarshryt.SelectedIndex = comboBox_Lmarshryt.Items.Count - 1;
                if (tek_marsh == tekMarshryt - 1) { SelectRichText(richText_Marshryt, Convert.ToInt32(comboBox_Lmarshryt.Text)); }//принудительная подсветка если удоляет предпоследний маршрут
            }
        }
        //обработка подсветки  маршрута при выборе № маршруту
        private void comboBox_Lmarshryt_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRichText(richText_Marshryt, Convert.ToInt32(comboBox_Lmarshryt.Text));
        }
        //функция обработки выбора и подсветки маршрута в RichTextBox
        private void SelectRichText(RichTextBox rch, int nom)
        {
            string target = "";

            if (nom == 1) { target = "1. "; }
            else
            {
                target = " " + nom.ToString() + ". ";
            }
            int pos = rch.Text.IndexOf(target);
            if (pos < 0)
            { // Не найдено. Выберите ничего.
                rch.Select(0, 0);
            }
            else
            {
                // Найден текст. Выберите его.
                rch.Select(0, rch.Text.Length);
                richText_Marshryt.SelectionColor = Color.Black;
                nom++;
                int pos_;
                int len = 0;
                if (nom < tekMarshryt)
                {
                    target = " " + nom.ToString() + ". ";
                    pos_ = rch.Text.IndexOf(target);
                    len = pos_ - pos;
                }
                else { len = rch.Text.Length - pos; }

                rch.Select(pos, len);
                richText_Marshryt.SelectionColor = Color.Green;
            }

        }
        //ФУНЦИЯ ПЕРЕВОДА СТРУКТУРЫ СПИСКА МАРШРУТОВ В СТРОКОВОЕ ПРЕДСТАВЛЕНИЕ
        private marshdon str_Marshryt()
        {
            marshdon rez = new marshdon();
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            int i = 1;
            foreach (var marsh in DanL_)
            {
                if (marsh.obrat == false)
                {
                    rez.str_marsh = rez.str_marsh + i + ". " + marsh.marsh[marsh.marsh.GetUpperBound(0), 0] + " ";
                    rez.prot_marsh = rez.prot_marsh + Convert.ToDouble(marsh.marsh[marsh.marsh.GetUpperBound(0), 1], provider);
                }
                else
                {
                    rez.str_marsh = rez.str_marsh + i + ". " + marsh.marsh[marsh.marsh.GetUpperBound(0), 0] + " — и обратно ";
                    rez.prot_marsh = rez.prot_marsh + (Convert.ToDouble(marsh.marsh[marsh.marsh.GetUpperBound(0), 1], provider) * 2);
                }
                i = i + 1;
            }
            return rez;
        }
        //функция расчета нагрузок на оси
        //и транспортное средство
        private void RaschetNosi_Click()
        {
            Gdan Lda_ = new Gdan();
            List<int> N_osi = new List<int>();
            List<Gdan> DanGR = new List<Gdan>();
            Rasch_Vred_.naimfail = comboBoxTagach.Text + ((combox_pricep.Text == "") ? "" : ("_" + combox_pricep.Text));
            int kol_osi = Convert.ToInt32(tBox_kolOsi.Text) * 2 + 1;
            object[,] tabl = new object[kol_osi + 1, 9];
            int j = 1;
            double sumL;
            double sumM;
            bool skatn;
            bool naOsi_Kol;
            //В ЦИКЛЕ ЗАПОЛНЯЕМ СТРУКТУРУ ГРУП  СБЛИЖЕННЫМ ОСЕЙ
            for (int i = 2; i < kol_osi - 1; i = i + 2)
            {
                try
                {
                    skatn = true;//по умолчанию тележка двух скатная
                    naOsi_Kol = true;//по умолчанию на оси меньше 4 колес
                    if ((double)worksheet[2, i] <= 2.5)
                    {
                        sumL = 0;
                        sumM = 0;

                        try
                        {
                            while ((double)worksheet[2, i] <= 2.5)
                            {
                                N_osi.Add(j);
                                sumL = sumL + (double)worksheet[2, i];
                                sumM = sumM + Convert.ToDouble(worksheet[3, i - 1]);
                                if (Convert.ToString(worksheet[5, i - 1]) == "1") { skatn = false; }

                                if (Convert.ToString(worksheet[4, i - 1]) == ">=4") { naOsi_Kol = false; }
                                i = i + 2;
                                j++;
                            }
                        }
                        catch (Exception) { }
                        if (Convert.ToString(worksheet[5, i - 1]) == "1") { skatn = false; }
                        if (Convert.ToString(worksheet[4, i - 1]) == ">=4") { naOsi_Kol = false; }
                        N_osi.Add(j);
                        Lda_.nosi = new List<int>(N_osi); ;
                        Lda_.sumLosiG = sumL;
                        Lda_.sumMosiG = sumM + Convert.ToDouble(worksheet[3, i - 1]);
                        Lda_.skatn = skatn;
                        Lda_.naOsi_Kol = naOsi_Kol;
                        DanGR.Add(Lda_);
                        N_osi.Clear();
                        j++;
                        if (kol_osi - 3 == i)
                        {
                            N_osi.Add(j);
                            Lda_.nosi = new List<int>(N_osi);
                            Lda_.sumLosiG = 0;
                            Lda_.sumMosiG = Convert.ToDouble(worksheet[3, i + 1]);

                            if (Convert.ToString(worksheet[4, i - 1]) == ">=4") { naOsi_Kol = false; }
                            if (Convert.ToString(worksheet[5, i - 1]) == "1") { skatn = false; }
                            Lda_.skatn = skatn;
                            Lda_.naOsi_Kol = naOsi_Kol;
                            DanGR.Add(Lda_);
                        }
                    }
                    else
                    {
                        N_osi.Add(j);
                        Lda_.nosi = new List<int>(N_osi);
                        Lda_.sumLosiG = 0;
                        Lda_.sumMosiG = Convert.ToDouble(worksheet[3, i - 1]);

                        if (Convert.ToString(worksheet[4, i - 1]) == ">=4") { naOsi_Kol = false; }
                        if (kol_osi - 3 == i)
                        {
                            if (Convert.ToString(worksheet[5, i - 1]) == "1") { skatn = false; }
                            Lda_.skatn = skatn;
                            Lda_.naOsi_Kol = naOsi_Kol;
                            DanGR.Add(Lda_);
                            N_osi.Clear();
                            j++;
                            N_osi.Add(j);
                            Lda_.nosi = new List<int>(N_osi);
                            skatn = true;
                            if (Convert.ToString(worksheet[5, i + 1]) == "1") { skatn = false; }
                            Lda_.sumLosiG = 0;
                            Lda_.sumMosiG = Convert.ToDouble(worksheet[3, i + 1]);
                            Lda_.skatn = skatn;
                            Lda_.naOsi_Kol = naOsi_Kol;
                            DanGR.Add(Lda_);
                            N_osi.Clear();
                            j++;
                        }
                        else
                        {
                            if (Convert.ToString(worksheet[5, i - 1]) == "1") { skatn = false; }
                            Lda_.skatn = skatn;
                            Lda_.naOsi_Kol = naOsi_Kol;
                            DanGR.Add(Lda_);
                            N_osi.Clear();
                            j++;
                        }
                    }
                }
                catch (Exception) { }
            }
            int Nosi = 1;
            int ch = 0;
            var workbook = ReoGridControl.CreateMemoryWorkbook();
          if ( File.Exists(Application.StartupPath + "\\dan\\tabosi.xlsx")== true)
                 workbook.Load(Application.StartupPath + "\\dan\\tabosi.xlsx", FileFormat.Excel2007);


            var tabosi = workbook.Worksheets[0];

            var workbook_tabSosi = ReoGridControl.CreateMemoryWorkbook();
            if (File.Exists(Application.StartupPath + "\\dan\\tabSosi.xlsx")== true)
                      workbook_tabSosi.Load(Application.StartupPath + "\\dan\\tabSosi.xlsx", FileFormat.Excel2007);


                var tabSosi = workbook_tabSosi.Worksheets[0];


            string maxdor = comboBox_maxdor.Text;
            int maxdor_ = 0;
            int skat_;
            int kol_osiG_;
            if (maxdor == "6,5")
            {
                maxdor_ = 2;
            }
            else
            {
                if (maxdor == "10")
                {
                    maxdor_ = 4;
                }
                else
                {
                    if (maxdor == "11,5")
                    {
                        maxdor_ = 6;
                    }
                }
            }
            double Ksez = 0.35;
            if (checkBox_PogodYsl.Checked == true) { Ksez = 1; }
            double dan = Convert.ToDouble(tabosi[0, 0]);
            foreach (Gdan gryp in DanGR)
            {
                int kol_osiG = gryp.nosi.Count;//КОЛЛИЧЕСТВО ОСЕЙ В ГРУППЕ
                skat_ = 0;
                kol_osiG_ = 0;//СМЕЩЕНИЕ В ТАБЛИЦЕ МАХ ДОПУСТИМЫХ НАГРУЗОК ДЛЯ ОСЕЙ ТЕЛЕЖЕК

                if (kol_osiG > 1)
                {
                    #region ЕСЛИ ИМЕЕМ ГРУППУ СБЛИЖЕННЫХ  ОСЕЙ
                    if (kol_osiG == 2)
                    {
                        kol_osiG_ = 1;//ДЛЯ 2-Х ОСНЫХ
                    }
                    else
                    {
                        if (kol_osiG == 3)
                        {
                            kol_osiG_ = 5;//ДЛЯ 3-Х ОСНЫХ
                        }
                        else
                        {
                            kol_osiG_ = 9;//ДЛЯ ОСТАЛЬНЫХ
                        }
                    }
                    double srL = gryp.sumLosiG / (kol_osiG - 1);//СРЕНЕЕ МЕЖОСЕВОЕ РАСТОЯНИЕ
                    double srM = gryp.sumMosiG / kol_osiG;//СРЕДНЯЯ НАГРУЗКА НА ОСЬ
                    bool skat = gryp.skatn;//СКАТНОСТЬ ДЛЯ ОДНОСКАТНЫХ = false
                    bool naOsi_Kol_ = gryp.naOsi_Kol;//КОЛИЧЕСТВО КОЛЕС НА ОСИ БОЛЬШЕ 3 ИЛИ МЕНЬШЕ 4
                    foreach (int item in gryp.nosi)
                    {
                        skat_ = 0;//СМЕЩЕНИЕ В ТАБЛИЦЕ МАХ ДОПУСТИМЫХ НАГРУЗОК ДЛЯ ОСЕЙ ТЕЛЕЖЕК С ДВУХ СКАТНЫМИ КОЛЕСАМИ
                        tabl[ch, 4] = "ось № " + Nosi;
                        tabl[ch, 2] = kol_osiG;
                        tabl[ch, 5] = srM;
                        //ЗАПОЛНЯЕМ СКАТНОСТЬ ДЛЯ ОСЕЙ
                        if (skat == false) { tabl[ch, 1] = "1"; } else { tabl[ch, 1] = "2"; skat_ = 1; }
                        //ЗАПОЛНЯЕМ КОЛЛИЧЕСТВО КОЛЕС НА ОСИ ДЛЯ ОСЕЙ С КОЛИЧЕСТВОМ КОЛЕС <4 = TRUE
                        if (naOsi_Kol_ == false) { tabl[ch, 0] = "2"; kol_osiG_ = 13; } else { tabl[ch, 0] = "1"; }
                        int stolb = maxdor_ + skat_;
                        if (srL <= 1)
                        {
                            tabl[ch, 3] = Convert.ToDouble(tabosi[kol_osiG_, stolb]);
                        }
                        else
                        {
                            if (srL <= 1.3 && srL > 1)
                            {
                                tabl[ch, 3] = Convert.ToDouble(tabosi[kol_osiG_ + 1, stolb]); ;
                            }
                            else
                            {
                                if (srL <= 1.8 && srL > 1.3)
                                {
                                    if (kol_osiG_ == 5 && checkBox3.Checked == true) { stolb = maxdor_ + 1; }
                                    tabl[ch, 3] = Convert.ToDouble(tabosi[kol_osiG_ + 2, stolb]); ;
                                }
                                else
                                {
                                    if (srL <= 2.5 && srL > 1.8)
                                    {
                                        tabl[ch, 3] = Convert.ToDouble(tabosi[kol_osiG_ + 3, stolb]); ;
                                    }
                                }
                            }
                        }

                        tabl[ch, 6] = Convert.ToDouble(tabl[ch, 5]) - Convert.ToDouble(tabl[ch, 3]);
                        if (Convert.ToDouble(tabl[ch, 6]) < 0) { tabl[ch, 6] = 0; }
                        tabl[ch, 7] = Convert.ToDouble(tabl[ch, 6]) / Convert.ToDouble(tabl[ch, 3]);
                        tabl[ch + 1, 4] = srL;
                        reoGridControl1.CurrentWorksheet.SetRangeStyles(new RangePosition(1, ch + 1, 1, 1), MyStyle_GRUP);
                        int i = 0;
                        double a = Convert.ToDouble(tabl[ch, 7]) * 100;
                        if (a > 2)
                        {
                            double ma = Convert.ToDouble(tabSosi[i, 0]);
                            double mi = Convert.ToDouble(tabSosi[i, 1]);
                            while (!(a >= ma && a < mi) && i < 58)
                            {
                                i++;
                                ma = Convert.ToDouble(tabSosi[i, 0]);
                                mi = Convert.ToDouble(tabSosi[i, 1]);
                            }
                            if (i < 58 && checkBox_TablFD.Checked == true)
                            {
                                tabl[ch, 8] = tabSosi[i, maxdor_];
                            }
                            else
                            { //расчет 
                                double H = Convert.ToDouble(maxdor);
                                string okrug = "Приволжский";

                                double Posi = Convert.ToDouble(tabl[ch, 6]);
                                tabl[ch, 8] = Raschet_Osi(H, okrug, Posi, Ksez);
                            }
                        }
                        else
                        {
                            tabl[ch, 8] = 0;
                        }
                        ch = ch + 2;
                        Nosi++;
                    }
                    tabl[ch - 1, 4] = Convert.ToDouble(worksheet[2, ch]);
                    #endregion
                }
                else
                {
                    #region для одиночных осей
                    tabl[ch, 4] = "ось № " + Nosi;
                    if (gryp.skatn == false) { tabl[ch, 1] = "1"; } else { tabl[ch, 1] = "2"; skat_ = 1; }
                    if (gryp.naOsi_Kol == false) { tabl[ch, 0] = "2"; } else { tabl[ch, 0] = "1"; }
                    tabl[ch, 2] = kol_osiG;
                    tabl[ch, 5] = gryp.sumMosiG;
                    tabl[ch + 1, 4] = Convert.ToDouble(worksheet[2, ch + 2]);
                    int stolb = maxdor_ + skat_;
                    tabl[ch, 3] = Convert.ToDouble(Convert.ToString(tabosi[0, stolb]));
                    tabl[ch, 6] = Convert.ToDouble(tabl[ch, 5]) - Convert.ToDouble(tabl[ch, 3]);

                    if (Convert.ToDouble(tabl[ch, 6]) < 0) { tabl[ch, 6] = 0; }
                    tabl[ch, 7] = Convert.ToDouble(tabl[ch, 6]) / Convert.ToDouble(tabl[ch, 3]);
                    int i = 0;
                    // string d= Convert.ToString(tabl[ch, 7]);
                    double a = Convert.ToDouble(tabl[ch, 7]) * 100;

                    if (a > 2)
                    {
                        double ma = Convert.ToDouble(tabSosi[i, 0]);
                        double mi = Convert.ToDouble(tabSosi[i, 1]);
                        while (!(a >= ma && a < mi) && i < 58)
                        {
                            i++;
                            ma = Convert.ToDouble(tabSosi[i, 0]);
                            mi = Convert.ToDouble(tabSosi[i, 1]);
                        }
                        if (i < 58 && checkBox_TablFD.Checked == true)
                        {
                            tabl[ch, 8] = tabSosi[i, maxdor_];
                        }
                        else
                        { //расчет 
                            double H = Convert.ToDouble(maxdor);
                            string okrug = "Приволжский";
                            double Posi = Convert.ToDouble(tabl[ch, 6]);
                            tabl[ch, 8] = Raschet_Osi(H, okrug, Posi, Ksez);
                        }
                    }
                    else
                    {
                        tabl[ch, 8] = 0;
                    }
                    ch = ch + 2;
                    Nosi++;
                    #endregion
                }
            }
            //______________________________________________________________________________________________________
            ReoGridControl reoGrid_ = Rasch_Vred_.reoGrid;
            Worksheet worksheet_ = reoGrid_.CurrentWorksheet;
            worksheet_[6, 0] = tabl;
            object Pos_ = worksheet_[8, 7];
            int kol = Convert.ToInt32(tBox_kolOsi.Text);
            double maxobchM = 0;

            if (combox_pricep.Text == "")
            {
                worksheet_[207, 4] = 1;
                if (kol == 2)
                {
                    maxobchM = 18;
                }
                else
                {
                    if (kol == 3)
                    {
                        maxobchM = 25;
                    }
                    else
                    {
                        if (kol == 4)
                        {
                            maxobchM = 32;
                        }
                        else
                        {
                            maxobchM = 38;
                        }
                    }
                }
            }
            else
            {
                worksheet_[207, 0] = 2;
                if (kol == 3)
                {
                    maxobchM = 28;
                }
                else
                {
                    if (kol == 4)
                    {
                        maxobchM = 36;
                    }
                    else
                    {
                        if (kol == 5)
                        {
                            maxobchM = 40;
                        }
                        else
                        {
                            maxobchM = 44;
                        }
                    }
                }
            }
            worksheet_[212, 9] = Ksez;
            worksheet_[4, 0] = "Данные для расчета по нагрузке на осях" + " тягача гос. номер  " + comboBoxTagach.Text + ((combox_pricep.Text == "") ? "" : (", прицепа гос. номер  " + combox_pricep.Text));
            worksheet_[207, 4] = kol;
            worksheet_[207, 3] = maxobchM;
            worksheet_[207, 6] = Convert.ToDouble(worksheet_[207, 5]) - Convert.ToDouble(worksheet_[207, 3]);
            if (Convert.ToDouble(worksheet_[207, 6]) < 0) { worksheet_[207, 6] = 0; }
            worksheet_[207, 7] = Convert.ToDouble(worksheet_[207, 6]) / Convert.ToDouble(worksheet_[207, 5]);

            int i_ = 0;
            // string d= Convert.ToString(tabl[ch, 7]);
            double a_ = Convert.ToDouble(worksheet_[207, 7]) * 100;
            if (a_ > 2)
            {
                double ma_ = Convert.ToDouble(tabSosi[i_, 0]);
                double mi_ = Convert.ToDouble(tabSosi[i_, 1]);
                while (!(a_ >= ma_ && a_ < mi_) && i_ < 58)
                {
                    i_++;
                    ma_ = Convert.ToDouble(tabSosi[i_, 0]);
                    mi_ = Convert.ToDouble(tabSosi[i_, 1]);
                }
                if (i_ < 58)
                {
                    double kof = 1;
                    if (a_ < 16)
                    {
                        if (comboBoxGod.Text == "2020")
                        {
                            kof = 0.4;
                        }
                        else
                        {
                            if (comboBoxGod.Text == "2021")
                            {
                                kof = 0.6;
                            }
                            else
                            {
                                if (comboBoxGod.Text == "2022")
                                {
                                    kof = 0.8;
                                }
                            }
                        }
                    }
                    worksheet_[207, 8] = Convert.ToDouble(tabSosi[i_, 7]) * kof;
                }
                else
                { //расчет  
                    worksheet_[207, 8] = Raschet_M("Приволжский", Convert.ToDouble(worksheet_[207, 7]) * 100);
                }
            }
            else { worksheet_[207, 8] = 0; }

            worksheet_[209, 9] = textBoxMarch.Text;
            worksheet_[210, 9] = comboBox_maxdor.Text;
            worksheet_[211, 9] = textBoxKolPoezd.Text;
            double qw = Convert.ToDouble(textBoxKolPoezd.Text);
            double ee = Convert.ToDouble(textBoxMarch.Text);
            double er = Convert.ToDouble(worksheet_[213, 9]);
            double eew = Convert.ToDouble(worksheet_[6, 9]);
            double ewew = Convert.ToDouble(worksheet_[207, 9]);
            worksheet_[214, 9] = qw * ee * er * (eew + ewew);
            worksheet_.HideRows(ch + 6, 205 - (ch + 6));
            Rasch_Vred_.Show();
        }
        //расчет возмещенеия вреда для оси
        //      H -  коэффициент, учитывающий природно-климатические условия
        //      okrug - Федеральный округ
        //      Posi- величина превышения фактической нагрузки на ось транспортного средства над допустимой 
        //      Ksez - Федеральный округ
        private double Raschet_Osi(double H, string okrug, double Posi, double Ksez)
        {
            double Pisxosi = 0;
            double a = 0;
            double b = 0;
            double Kkdz = 0;
            double Kraprem = 0;
            double Ppomi = 0;

            if (H == 6)
            {
                Pisxosi = 8500;
                a = 7.3;
                b = 0.27;
            }
            else
            {
                if (H == 10)
                {
                    Pisxosi = 1840;
                    a = 37.7;
                    b = 2.4;
                }
                else
                {
                    if (H == 11.5)
                    {
                        Pisxosi = 840;
                        a = 39.5;
                        b = 2.7;
                    }
                }
            }
            if (okrug == "Приволжский")
            {
                Kkdz = 1.67;
                Kraprem = 0.94;
            }
            Posi = Math.Pow(Posi, 1.92);
            Ppomi = Kkdz * Kraprem * Ksez * Pisxosi * (1 + 0.2 * Posi * (a / H - b));
            return Ppomi;
        }
        //расчет возмещенеия вреда для общей массы
        //      okrug - Федеральный округ
        //      Ppm- превышения фактической массы транспортного средства над допустимой, процентов
        private double Raschet_M(string okrug, double Ppm)
        {
            double Kraprem = 0;
            double Kpm = 0;
            double PM = 0;

            if (okrug == "Приволжский")
            {
                Kraprem = 0.94;
                Kpm = 0.498;
            }
            PM = Kraprem * Kpm * 7365 * (1 + 0.01675 * Ppm);
            return PM;
        }
        //кнопка показа папки с файлами расчета вреда
        //гос №тяг_гос №приц
        private void papka_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", Application.StartupPath + "\\расчет\\");
        }
        struct tab_osi
        {
            public double mejosiR;
            public double nagrosi;
            public string skat;
            public string kolkoles;
        }
        struct danZaivlen
        {
            public List<tab_osi> osev;
            public string gosN_tag;
            public string gosN_pric;
            public List<marsh_obr> marshryt;
            public bool PogodYsl;
            public bool TablFD;
            public bool pnevmat13_18;
            public int KolPoezd;
            public double koefDif;
            public int godPervozki;
            public double sum_vred;
        }
        private void save_raschet_Click(object sender, EventArgs e)
        {
            int kol_osi = Convert.ToInt32(tBox_kolOsi.Text) * 2 + 1;
            tab_osi osid = new tab_osi();
            danZaivlen tab_zaivl = new danZaivlen();
            tab_zaivl.osev = new List<tab_osi>();
            for (int i = 2; i <= kol_osi; i = i + 2)
            {
                osid.mejosiR = Convert.ToDouble(worksheet[2, i]);
                osid.nagrosi = Convert.ToDouble(worksheet[3, i - 1]);
                osid.skat = Convert.ToString(worksheet[4, i - 1]);
                osid.kolkoles = Convert.ToString(worksheet[5, i - 1]);
                tab_zaivl.osev.Add(osid);

            }
        }

    }
}
