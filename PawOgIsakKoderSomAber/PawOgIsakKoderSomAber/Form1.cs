using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PawOgIsakKoderSomAber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Redraw on resize.
        private void Form1_Load(object sender, EventArgs e)
        {
            ResizeRedraw = true;
            Image image = Image.FromFile("cow_PNGXXX.png");
            pictureBox1.Height = image.Height;
            pictureBox1.Width = image.Width;
            pictureBox1.Image = image;
        }

        // Draw an ellipse.
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(10, 10,
                this.ClientSize.Width - 20,
                this.ClientSize.Height - 20);
            e.Graphics.FillEllipse(Brushes.Yellow, rect);
            using (Pen thick_pen = new Pen(Color.Red, 5))
            {
                e.Graphics.DrawEllipse(thick_pen, rect);
            }
        }
    }
}
