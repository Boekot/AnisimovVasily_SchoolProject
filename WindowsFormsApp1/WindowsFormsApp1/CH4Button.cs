using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp1
{
    class CH4Button : RoundButton
    {
        Point DownPoint;
        public bool IsDragMode { get; set; } = false;
        public bool removed { get; set; } = false;
        public int count { get; set; } = 0;
        public List<CH4Button> CreatedCH4 { get; set; } = null;
        public CH4Button Parrent { get; set; } = null;
        public int Connections = 0;
        public CH4Button MyConnections { get; set; } = null;
        public Panel panel1 { get; set; } = null;
        public string floor = "";
        private List<int> len = new List<int>();
        private List<Point> NearPar = new List<Point>();
        private Panel ConnectionLine = null;

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Size = new Size(40, 40); //Стандартный размер
            this.Text = GetText(Connections);
            this.Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs mevent) //Режим передвижение обьекта с зажатой кнопкой мыши
        {
            DownPoint = mevent.Location;
            if (count == 0) IsDragMode = true;
            if ((count == 1) & (Connections == 1))
            {
                removed = true;
                Parrent.Connections = Parrent.Connections - 1;
                Parrent.Text = GetText(Parrent.Connections);
                Parrent.Invalidate();
                panel1.Controls.Remove(ConnectionLine);
            }
            base.OnMouseDown(mevent);
            count++;
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            //Часть отвечающая за передвижение
            DownPoint = mevent.Location;
            IsDragMode = false;
            base.OnMouseUp(mevent);


            if (count == 1)
            {
                //Определяем ближайший элемент перебором
                foreach (Button obj in CreatedCH4)
                {
                    int Ox = this.Location.X;
                    int Oy = this.Location.Y;
                    int Px = obj.Location.X;
                    int Py = obj.Location.Y;
                    if (obj!=this) len.Add((Ox-Px)*(Ox-Px)+(Oy-Py)*(Oy-Py));
                }
                Parrent = CreatedCH4[len.IndexOf(len.Min())];

                //Определяем ближающую точку рядом с найденным элементом
                len = new List<int>();
                NearPar.Add(new Point(Parrent.Location.X + 50, Parrent.Location.Y));
                NearPar.Add(new Point(Parrent.Location.X - 50, Parrent.Location.Y));
                NearPar.Add(new Point(Parrent.Location.X, Parrent.Location.Y + 50));
                NearPar.Add(new Point(Parrent.Location.X, Parrent.Location.Y - 50));

                foreach (Point obj in NearPar)
                {
                    int Ox = this.Location.X;
                    int Oy = this.Location.Y;
                    int Px = obj.X;
                    int Py = obj.Y;
                    len.Add((Ox-Px)*(Ox-Px)+(Oy-Py)*(Oy-Py));
                }

                Location = NearPar[len.IndexOf(len.Min())];
                DrawLine(len.IndexOf(len.Min()),this); //Отрисовка соединения

                //Запоминание еоличества связей и элемента, к которому прикреплён
                Connections = Connections + 1;
                this.Text = GetText(Connections);
                Parrent.Connections = Parrent.Connections + 1;
                Parrent.Text = GetText(Parrent.Connections);
                Parrent.MyConnections = this;
                Parrent.Invalidate();
                this.Invalidate();
            }
        }

        public void DrawLine(int angle, CH4Button It) //Создание соединений из Панелей с черным фоном (4 направления)
        {
            Pen pen = new Pen(Color.Black, 3);
            if (angle == 0)
            {
                Panel Line = new Panel();
                Line.Size = new Size(10, 4);
                Line.Location = new Point(-10 + It.Location.X, 18 + It.Location.Y);
                Line.BackColor=Color.Black;
                panel1.Controls.Add(Line);
                ConnectionLine = Line;
            }
            if (angle == 1)
            {
                Panel Line = new Panel();
                Line.Size = new Size(10, 4);
                Line.Location = new Point(40 + It.Location.X, 18 + It.Location.Y);
                Line.BackColor=Color.Black;
                panel1.Controls.Add(Line);
                ConnectionLine = Line;
            }
            if (angle == 2)
            {
                Panel Line = new Panel();
                Line.Size = new Size(4, 10);
                Line.Location = new Point(18 + It.Location.X, -10 + It.Location.Y);
                Line.BackColor=Color.Black;
                panel1.Controls.Add(Line);
                ConnectionLine = Line;
            }
            if (angle == 3) 
            { 
                Panel Line = new Panel(); 
                Line.Size = new Size(4, 10); 
                Line.Location = new Point(18 + It.Location.X, 40 + It.Location.Y); 
                Line.BackColor=Color.Black; 
                panel1.Controls.Add(Line);
                ConnectionLine = Line;
            }
        }

        private string GetText(int Connections) //Получаем название из кол-ва соединений
        {
            string ans = "C";
            if (Connections <= 3) ans = ans + "H";
            if (Connections <= 2) ans = ans + (4-Connections);
            return ans;
        }
        protected override void OnInvalidated(InvalidateEventArgs e) //Заготовка по будущую систему индексации
        {
            base.OnInvalidated(e);
            if (MyConnections != null)
            {
                MyConnections.Parrent = this;
                ///if (floor=="") MyConnections.floor = floor + "1";
                if (Connections<=2) MyConnections.floor = floor + "1";
                if (Connections==3) MyConnections.floor = floor + "a1";
                if (Connections==4) MyConnections.floor = floor + "b1";
            }
        }

        protected override void OnMouseMove(MouseEventArgs mevent)//Свободное передвижение обьекта
        {
            if (IsDragMode)
            {
                Point p = mevent.Location;
                Point dp = new Point(p.X - DownPoint.X, p.Y - DownPoint.Y);
                Location = new Point(Location.X + dp.X, Location.Y + dp.Y);
            }
            base.OnMouseMove(mevent);
        }
    }
}
