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
            CH4Button.panel1 = panel1;
            panel1.Controls.Add(CH4Button);
            CH4.Add(CH4Button);
        }

        private void CreateCH4_Click(object sender, EventArgs e)
        {
            //Создание остальных элементов при нажатии на кнопку
            CH4Button CH4Button = new CH4Button();
            CH4Button.Location = new Point(0, 0);
            CH4Button.CreatedCH4 = CH4;
            CH4Button.panel1 = panel1;
            panel1.Controls.Add(CH4Button);
            CH4.Add(CH4Button);

            //Вывод дебаг информации
            foreach(CH4Button v in CH4)
            {
                Debug.Write("Connections: " + v.Connections + "; ");
                Debug.Write("Floor: " + v.floor + "; ");
                Debug.WriteLine(v.Parrent);
            }
            Debug.WriteLine("-------------------");
        }
    }
}
