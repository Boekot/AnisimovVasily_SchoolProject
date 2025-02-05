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
            CH4Button.Location = new Point(panel1.Width/2,panel1.Height/2);
            CH4Button.count = 2;
            CH4Button.floor = "1";
            CH4Button.panel1 = panel1;
            panel1.Controls.Add(CH4Button);
            CH4.Add(CH4Button);
        }

        private void CreateCH4_Click(object sender, EventArgs e)
        {
            var tbool = true;
            foreach (CH4Button v in CH4.ToArray())
            { 
                if (v.floor == "") { tbool = false; }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (CH4Button v in CH4.ToArray())
            {
                if (v.removed == true)
                {
                    CH4.Remove(v);
                    panel1.Controls.Remove(v);
                }

            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string Text = string.Empty;
            int maxfloor = 0;
            CH4Button maxfloorEl = new CH4Button();
            List<CH4Button> TempCH4 = new List<CH4Button>();

            foreach (CH4Button v in CH4)
            {
                TempCH4.Add(v);
            }
            Debug.Write("Before 1st step: ");
            Debug.WriteLine(string.Join("; ", TempCH4));

            
            foreach (CH4Button v in TempCH4)
            {
                int summ = 0;
                for (int i = 0; i < v.floor.Length; i++)
                {
                    if (char.IsNumber(v.floor[i]))
                    {

                        summ += Convert.ToInt32(v.floor[i].ToString());
                    }
                }
                if (maxfloor < summ) { maxfloor = summ; maxfloorEl = v; }
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
            while (TempButtton.floor != "1")
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
                    int summ = 0;
                    for (int i = 0; i < v.floor.Length; i++)
                    {
                        if (char.IsNumber(v.floor[i]))
                        {

                            summ += Convert.ToInt32(v.floor[i].ToString());
                        }
                    }
                    if (maxfloor < summ) { maxfloor = summ; maxfloorEl = v; }
                }
                TempButtton = maxfloorEl;
                int TempInt = 0;
                int ConnectedToFloor = 0;
                while (TempButtton.floor != "1")
                {
                    Debug.WriteLine(TempCH4.Contains(TempButtton).ToString());
                    if (!TempCH4.Contains(TempButtton))
                    {
                        int summ = 0;
                        for (int i = 0; i < TempButtton.floor.Length; i++)
                        {
                            if (char.IsNumber(TempButtton.floor[i]))
                            {
                                summ += Convert.ToInt32(TempButtton.floor[i].ToString());
                            }
                        }
                        ConnectedToFloor = summ;
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
}
