using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public class RoundButton : Button
    {
        public int Radius { get; set; } = 30;
        private bool MouseEntered = false;
        private bool MousePressed = false;
        public Color BackgroundColor { get; set; } = Color.FromKnownColor(KnownColor.ButtonFace);
        private StringFormat SF = StringFormat.GenericTypographic;
        
        
        public RoundButton() //Настройка отображения
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true);

            DoubleBuffered = true;
            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Подготовка
            Graphics g = e.Graphics;
            g.Clear(BackgroundColor);
            g.SmoothingMode = SmoothingMode.HighQuality;

            //Прямоугольник
            Rectangle rect = new Rectangle(0, 0, Width - 1, Width - 1);

            //Закругленный прямоугольник
            GraphicsPath rectGp = RoundedRectangle(rect);

            //Заливка цветом
            g.FillPath(new SolidBrush(BackColor), rectGp);
            g.DrawPath(new Pen(ForeColor), rectGp);

            //Эффекты при наведениии и нажатии
            if (MouseEntered)
            {
                g.FillPath(new SolidBrush(Color.FromArgb(30, Color.Black)), rectGp);
            }
            if (MousePressed)
            {
                g.FillPath(new SolidBrush(Color.FromArgb(60, Color.Black)), rectGp);
            }
            g.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }

        GraphicsPath RoundedRectangle(Rectangle rect) //Функция для закруглённого прямоугольника
        {
            GraphicsPath gp = new GraphicsPath();
            Radius = rect.Width; //Радиус закругления = ширине

            gp.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90);
            gp.AddArc(rect.X + rect.Width - Radius, rect.Y, Radius, Radius, 270, 90);
            gp.AddArc(rect.X + rect.Width - Radius, rect.Y + rect.Height - Radius, Radius, Radius, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - Radius, Radius, Radius, 90, 90);

            gp.CloseFigure();
            return gp;
        }

        protected override void OnMouseEnter(EventArgs e) //Изменения переменных при наведении мыши
        {
            base.OnMouseEnter(e);
            MouseEntered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) //Изменения переменных когда мышь убрали
        {
            base.OnMouseLeave(e);
            MouseEntered = false;
            MousePressed = false;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e) //Изменения переменных при клике
        {
            base.OnMouseClick(e);
            MousePressed = true;
            Invalidate();
        }
    }
}
