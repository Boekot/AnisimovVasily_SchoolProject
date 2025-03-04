using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<CH4Button> CH4 = new List<CH4Button>();
        public Form1()
        {
            InitializeComponent();

            //Создание начального элемента
            CH4Button CH4Button = new CH4Button();
            CH4Button.Location = new Point(panel1.Width/2, panel1.Height/2);
            CH4Button.count = 2;
            CH4Button.floor = 1;
            CH4Button.panel1 = panel1;
            CH4Button.canbedel = false;
            panel1.Controls.Add(CH4Button);
            CH4.Add(CH4Button);
        }

        private void CreateCH4_Click(object sender, EventArgs e)
        {
            var tbool = true;
            foreach (CH4Button v in CH4.ToArray())
            {
                if (v.floor == 0) { tbool = false; }
            }
            if (tbool)
            {
                //Создание остальных элементов при нажатии на кнопку
                CH4Button CH4Button = new CH4Button();
                CH4Button.Location = new Point(0, 0);
                CH4Button.CreatedCH4 = CH4;
                CH4Button.panel1 = panel1;
                panel1.Controls.Add(CH4Button);
                CH4.Add(CH4Button);

                //Вывод дебаг информации
                foreach (CH4Button v in CH4)
                {
                    Debug.Write("Connections: " + v.Connections + "; ");
                    Debug.Write("Floor: " + v.floor + "; ");
                    Debug.WriteLine(v.Parrent);
                }
                Debug.WriteLine("-------------------");
            }
        }
        int countCH4 = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int nopar = 0;
            foreach (CH4Button v in CH4.ToArray())
            {
                if (v.removed == true)
                {
                    CH4.Remove(v);
                    panel1.Controls.Remove(v.ConnectionLine);
                    panel1.Controls.Remove(v);
                }
                if (v.Parrent == null) { nopar++; }
            }
            if (nopar == 1)
            {
                if (CH4.ToArray().Length!=countCH4)
                {
                    countCH4 = CH4.ToArray().Length;
                    namechain(CH4);
                }
            }
        }


        private static List<CH4Button> findmaxchain(List<CH4Button> TempCH4)
        {
            //Находим все возможные концы цепи
            List<CH4Button> PosEnds = new List<CH4Button>();
            foreach (CH4Button v in TempCH4)
            {
                if (v.Connections == 1)
                {
                    PosEnds.Add(v);
                }
            }

            CH4Button prevfloor1 = new CH4Button();
            foreach (CH4Button v in TempCH4)
            {
                if (v.floor == 1) { prevfloor1 = v; }
            }

            ///Считаем самую длинную цепь
            int maxlen = 0;
            CH4Button maxlenel1 = new CH4Button();
            CH4Button maxlenel2 = new CH4Button();
            CH4Button maxlenelconto = new CH4Button();
            foreach (CH4Button v in PosEnds)
            {
                List<CH4Button> TempCH4_2 = new List<CH4Button>();
                foreach (CH4Button v1 in TempCH4)
                {
                    TempCH4_2.Add(v1);
                }

                CH4Button TempButtton2 = v;
                while (TempButtton2.floor != 1)
                {
                    TempCH4_2.Remove(TempButtton2);
                    TempButtton2 = TempButtton2.Parrent;
                }
                TempCH4_2.Remove(TempButtton2);

                //Для каждой пары точек концов ищем с максимальным кол-вом элементов между ними
                foreach (CH4Button v2 in PosEnds)
                {
                    if (v2 != v)
                    {
                        int ConnectedToFloor = 0;
                        CH4Button ConnectedToElement = new CH4Button();
                        TempButtton2 = v2;
                        while (TempButtton2.floor != 1)
                        {
                            Debug.WriteLine(TempCH4_2.Contains(TempButtton2).ToString());
                            if (!TempCH4_2.Contains(TempButtton2))
                            {
                                ConnectedToFloor = TempButtton2.floor;
                                ConnectedToElement = TempButtton2;
                                break;
                            }
                            else
                            {
                                Debug.WriteLine("Tryed to remove TempButton from TempCH4_2");
                                TempCH4_2.Remove(TempButtton2);
                                TempButtton2 = TempButtton2.Parrent;
                            }
                        }
                        if (ConnectedToFloor == 0) { ConnectedToFloor++; ConnectedToElement = TempButtton2; }
                        Debug.WriteLine("------");
                        Debug.WriteLine("Max len: " + maxlen);
                        Debug.WriteLine("MB max len: " + (v.floor-ConnectedToFloor+v2.floor-ConnectedToFloor+1));
                        if ((v == prevfloor1) ^ (v2 == prevfloor1)) { Debug.WriteLine("Конец равен предыдущему!!"); }
                        Debug.WriteLine("------");
                        if (maxlen < (v.floor-ConnectedToFloor+v2.floor-ConnectedToFloor+1))
                        {
                            maxlen = v.floor-ConnectedToFloor+v2.floor-ConnectedToFloor+1;
                            maxlenel1 = v;
                            maxlenel2 = v2;
                            maxlenelconto = ConnectedToElement;
                            Debug.WriteLine("Изменение концов!!!!!");
                            Debug.WriteLine("Connected to floor: " + ConnectedToFloor);
                        }
                    }
                }
            }

            //Записываем ответ в список
            List<CH4Button> ans = new List<CH4Button>();
            ans.Add(maxlenel1);
            ans.Add(maxlenel2);
            ans.Add(maxlenelconto);
            return (ans);
        }

        private static List<CH4Button> fingendbeg(List<CH4Button> FromFunction)
        {
            CH4Button maxlenel1 = FromFunction[0];
            CH4Button maxlenel2 = FromFunction[0];
            CH4Button maxlenelconto = FromFunction[0];
            Debug.WriteLine("Концы найдены, начинаем пересчет");
            maxlenel1.Invalidate();
            maxlenel2.Invalidate();
            maxlenelconto.Invalidate();
            int torad1 = 0;
            int torad2 = 0;
            CH4Button TempButtton6 = maxlenel1;
            if (maxlenel1.Parrent!=null)
            {
                TempButtton6 = maxlenel1;
                while (TempButtton6.Connections < 3)
                {
                    torad1++;
                    TempButtton6 = TempButtton6.Parrent;
                    if (TempButtton6==null) { break; }
                }
                torad1++;
            }
            else
            {
                TempButtton6 = maxlenel2;
                torad1 = maxlenel2.floor;
                while (TempButtton6.Connections < 3)
                {
                    torad1--;
                    TempButtton6 = TempButtton6.Parrent;
                    if (TempButtton6==null) { break; }
                }
            }
            if (maxlenel2.Parrent!=null)
            {
                TempButtton6 = maxlenel2;
                while (TempButtton6.Connections < 3)
                {
                    torad2++;
                    TempButtton6 = TempButtton6.Parrent;
                    if (TempButtton6==null) { break; }
                }
                torad2++;
            }
            else
            {
                TempButtton6 = maxlenel1;
                torad2 = maxlenel1.floor;
                while (TempButtton6.Connections < 3)
                {
                    torad2--;
                    TempButtton6 = TempButtton6.Parrent;
                    if (TempButtton6==null) { break; }
                }
            }
            if (torad1 >= torad2)
            {
                TempButtton6 = maxlenel1;
                maxlenel1 = maxlenel2;
                maxlenel2 = TempButtton6;
            }
            ///Перенаправляем цепь с предыдущего надала до точки соединения
            CH4Button TempButtton3 = maxlenelconto;
            CH4Button TempButtton4 = maxlenelconto.Parrent;
            TempButtton3.Parrent = null;
            if (TempButtton4 != null)
            {
                while (TempButtton4.floor != 1)
                {
                    CH4Button TempButtton5 = TempButtton4.Parrent;
                    TempButtton4.Parrent = TempButtton3;
                    TempButtton3 = TempButtton4;
                    TempButtton4 = TempButtton5;
                    Debug.WriteLine("TempB4: " + TempButtton4.floor);
                    Debug.WriteLine("TempB3.par: " + TempButtton3.Parrent);
                }
                TempButtton4.Parrent = TempButtton3;
            }
            Debug.WriteLine("Пересчет цепи начало->точка соединения законен.");
            ///Перенаправляем цепь с точки соединения до начала
            while (maxlenel1.Parrent != null)
            {
                TempButtton3 = maxlenel1;
                TempButtton4 = null;

                while (TempButtton3.Parrent != null)
                {
                    TempButtton4 = TempButtton3;
                    TempButtton3 = TempButtton3.Parrent;
                }
                TempButtton3.Parrent = TempButtton4;
                TempButtton4.Parrent = null;
            }
            //Записываем ответ в список
            List<CH4Button> ans = new List<CH4Button>();
            ans.Add(maxlenel1);
            ans.Add(maxlenel2);
            ans.Add(maxlenelconto);
            return (ans);
        }

        private void namechain(List<CH4Button> CH4)
        {
            for (int ii = 0; ii < 10; ii++)
            {
                string Text = string.Empty;
                int maxfloor = 0;
                CH4Button maxfloorEl = new CH4Button();
                List<CH4Button> TempCH4 = new List<CH4Button>();

                foreach (CH4Button v in CH4)
                {
                    TempCH4.Add(v);
                    v.Invalidate();
                }

                List<CH4Button> FromFunction = new List<CH4Button>();
                FromFunction = findmaxchain(TempCH4);
                FromFunction = fingendbeg(FromFunction);
                CH4Button maxlenel1 = FromFunction[0];
                CH4Button maxlenel2 = FromFunction[0];
                CH4Button maxlenelconto = FromFunction[0];


                maxlenel1.floor = 1;
                for (int i = 0; i < CH4.Count(); i++)
                {
                    foreach (CH4Button v in CH4)
                    {
                        if (v.Parrent != null) { v.floor = v.Parrent.floor + 1; }
                    }
                }

                foreach (CH4Button v in CH4)
                {
                    Debug.Write("Connections: " + v.Connections + "; ");
                    Debug.Write("Floor: " + v.floor + "; ");
                    if (v == maxlenel1) { Debug.Write("Является началом цепи!!!"); }
                    Debug.WriteLine(v.Parrent);
                }
                Debug.WriteLine("-------------------");

                Debug.Write("Before 1st step: ");
                Debug.WriteLine(string.Join("; ", TempCH4));


                foreach (CH4Button v in TempCH4)
                {
                    if (maxfloor < v.floor) { maxfloor = v.floor; maxfloorEl = v; }
                }

                string TextBase = string.Empty;
                if (maxfloor == 1) { TextBase = "метан"; }
                else if (maxfloor == 2) { TextBase = "этан"; }
                else if (maxfloor == 3) { TextBase = "пропан"; }
                else if (maxfloor == 4) { TextBase = "бутан"; }
                else if (maxfloor == 5) { TextBase = "пентан"; }
                else if (maxfloor == 6) { TextBase = "гексан"; }
                else if (maxfloor == 7) { TextBase = "гептан"; }
                else if (maxfloor == 8) { TextBase = "октан"; }
                else if (maxfloor == 9) { TextBase = "нонан"; }
                else if (maxfloor == 10) { TextBase = "декан"; }
                CH4Button TempButtton = maxfloorEl;
                while (TempButtton.floor != 1)
                {
                    TempCH4.Remove(TempButtton);
                    TempButtton = TempButtton.Parrent;
                }
                TempCH4.Remove(TempButtton);

                Debug.WriteLine("");
                Debug.Write("After 1st step: ");
                Debug.WriteLine(string.Join("; ", TempCH4));
                Debug.WriteLine("");
                Debug.Write("TempCH4.Count: ");
                Debug.WriteLine("");
                Debug.WriteLine(TempCH4.Count.ToString());
                List<int> Radicals = new List<int>();


                while (TempCH4.Count > 0)
                {
                    foreach (CH4Button v in TempCH4)
                    {
                        maxfloor = 0;
                        if (maxfloor < v.floor) { maxfloor = v.floor; maxfloorEl = v; }
                    }
                    TempButtton = maxfloorEl;
                    int TempInt = 0;
                    int ConnectedToFloor = 0;
                    while (TempButtton.floor != 1)
                    {
                        Debug.WriteLine(TempCH4.Contains(TempButtton).ToString());
                        if (!TempCH4.Contains(TempButtton))
                        {
                            ConnectedToFloor = TempButtton.floor;
                            break;
                        }
                        else
                        {
                            Debug.WriteLine("Tryed to remove TempButton from TempCH4");
                            TempCH4.Remove(TempButtton);
                            TempButtton = TempButtton.Parrent;
                            TempInt++;
                        }
                    }
                    Debug.Write("TempCH4.Count Before add to radicals: ");
                    Debug.WriteLine(TempCH4.Count.ToString());
                    Radicals.Add(TempInt);
                    Radicals.Add(ConnectedToFloor);
                }

                Debug.WriteLine(string.Join("; ", Radicals));
                List<int> Metil = new List<int>();
                List<int> Ethil = new List<int>();
                List<int> Propil = new List<int>();
                List<int> Butil = new List<int>();
                List<int> Pentil = new List<int>();
                List<string> Rnum = new List<string>();

                Rnum.Add("");
                Rnum.Add("ди");
                Rnum.Add("три");
                Rnum.Add("тетра");
                Rnum.Add("пента");
                Rnum.Add("гекса");



                for (int i = 0; i < (Radicals.Count)/2; i++)
                {
                    if (Radicals[i*2] == 1) Metil.Add(Radicals[i*2+1]);
                    if (Radicals[i*2] == 2) Ethil.Add(Radicals[i*2+1]);
                    if (Radicals[i*2] == 3) Propil.Add(Radicals[i*2+1]);
                    if (Radicals[i*2] == 4) Butil.Add(Radicals[i*2+1]);
                    if (Radicals[i*2] == 5) Pentil.Add(Radicals[i*2+1]);
                }

                if (Butil.Count > 0) Text += string.Join(",", Butil) + "-" + Rnum[Butil.Count-1] + "бутил-";
                if (Metil.Count > 0) Text += string.Join(",", Metil) + "-" + Rnum[Metil.Count-1] + "метил-";
                if (Pentil.Count > 0) Text += string.Join(",", Pentil) + "-" + Rnum[Pentil.Count-1] + "пентил-";
                if (Propil.Count > 0) Text += string.Join(",", Propil) + "-" + Rnum[Propil.Count-1] + "пропил-";
                if (Ethil.Count > 0) Text += string.Join(",", Ethil) + "-" + Rnum[Ethil.Count-1] + "этил-";
                Text += TextBase;

                textBox1.Text = Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(CH4Button v in CH4)
            {
                v.removed = true;
            }
            //Создание начального элемента
            CH4Button CH4Button = new CH4Button();
            CH4Button.Location = new Point(panel1.Width/2, panel1.Height/2);
            CH4Button.count = 2;
            CH4Button.floor = 1;
            CH4Button.panel1 = panel1;
            panel1.Controls.Add(CH4Button);
            CH4.Add(CH4Button);
            textBox1.Text = "метан";
        }
    }
}
