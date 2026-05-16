using System.Security.Cryptography;

namespace project
{
    public class DDA
    {
        public float Xst, Yst;
        public float Xend, Yend;
        float dy, dx, m;
        public float cx, cy;
        int speed = 10;
        public int direction = 1;
        public void calc()
        {
            dy = Yend - this.Yst;
            dx = Xend - Xst;
            m = dy / dx;
            cx = Xst;
            cy = Yst;
        }
        public bool CalcNextPoint()
        {
            if (direction == 0)
            {
                float tempX = Xst;
                float tempY = Yst;
                Xst = Xend;
                Yst = Yend;
                Xend = tempX;
                Yend = tempY;
                calc();
                direction = 1;
                return true;
            }

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (Xst < Xend)
                {
                    cx += speed;
                    cy += m * speed;

                    if (cx >= Xend)
                    {
                        cx = Xend;
                        cy = Yend;
                        direction = 0;
                    }
                }
                else
                {
                    cx -= speed;
                    cy -= m * speed;

                    if (cx <= Xend)
                    {
                        cx = Xend;
                        cy = Yend;
                        direction = 0;
                    }
                }
            }
            else
            {
                if (Yst < Yend)
                {
                    cy += speed;

                    if (m != 0)
                        cx += (1 / m) * speed;

                    if (cy >= Yend)
                    {
                        cx = Xend;
                        cy = Yend;
                        direction = 0;
                    }
                }
                else
                {
                    cy -= speed;

                    if (m != 0)
                        cx -= (1 / m) * speed;

                    if (cy <= Yend)
                    {
                        cx = Xend;
                        cy = Yend;
                        direction = 0;
                    }
                }
            }

            return true;
        }
    }
    public class Circle
    {
        public int rad;
        public int xc;
        public int yc;
        public float thradian;
        public float st, end;
        public bool canmove = false;
        public PointF linepont = new PointF();
        public int movex = 0;
        public int movey = 0;
        public bool flag = false;

        public void Drawcircle(Graphics g)
        {
            for (float i = st; i <= end; i += 1.0f)
            {
                thradian = (float)((i * Math.PI) / 180);
                float x = (float)(rad * Math.Cos(thradian));
                float y = (float)(rad * Math.Sin(thradian));
                x += xc + movex;
                y += yc + movey;

                g.FillEllipse(Brushes.Green, x, y, 5, 5);
            }
            PointF tempst = Getnextpoint((int)st);
            PointF tempend = Getnextpoint((int)end);
            g.DrawLine(Pens.Green, xc + movex, yc + movey,
    tempst.X + movex, tempst.Y + movey);

            g.DrawLine(Pens.Green,
                xc + movex, yc + movey,
                tempend.X + movex, tempend.Y + movey);
        }
        public PointF Getnextpoint(int theta)
        {

            PointF p = new PointF();

            thradian = (float)(theta * Math.PI / 180);

            p.X = (float)(rad * Math.Cos(thradian)) + xc;
            p.Y = (float)(rad * Math.Sin(thradian)) + yc;
            return p;
        }
    }
    public partial class Form1 : Form
    {
        Bitmap off;
        Bitmap bg, car;
        List<DDA> lines = new List<DDA>();
        char state = 'x';
        float current_xend, current_yend;
        int ct = 0;
        Pen p = new Pen(Color.Black, 5);
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            Load += Form1_Load;
            WindowState = FormWindowState.Maximized;
            KeyDown += Form1_KeyDown;
            MouseDown += Form1_MouseDown;
            System.Windows.Forms.Timer tt = new System.Windows.Forms.Timer();
            tt.Tick += Tt_Tick;
            tt.Start();
            ///////////// for multiple screen (for my home secound screen  )
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(1920, 0);

        }

        private void Tt_Tick(object? sender, EventArgs e)
        {

            drawdb(CreateGraphics());
        }

        private void Form1_MouseDown(object? sender, MouseEventArgs e)
        {

        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
          rasm_el5t();

        }
        void rasm_el5t()
        {
            if (e.KeyCode == Keys.L)
            {
                state = 'l';
                if (ct == 0)
                {

                    DDA temp = new DDA();
                    temp.Xst = 0;
                    temp.Yst = (ClientSize.Height / 2) + 60;
                    temp.Yend = (ClientSize.Height / 2) + 60;
                    lines.Add(temp);
                    ct++;
                }
                else
                {
                    DDA temp = new DDA();
                    temp.Xst = current_xend;
                    temp.Yst = current_yend;
                    temp.Xend = current_xend;
                    temp.Yend = current_yend;
                    lines.Add(temp);
                }

            }


            if (state == 'l')
            {
                if (e.KeyCode == Keys.Right)
                {
                    lines[lines.Count - 1].Xend += 20;
                    current_xend = lines[lines.Count - 1].Xend;
                    current_yend = lines[lines.Count - 1].Yend;
                }
                if (e.KeyCode == Keys.Left)
                {
                    lines[lines.Count - 1].Xend -= 20;
                    current_xend = lines[lines.Count - 1].Xend;
                    current_yend = lines[lines.Count - 1].Yend;
                }
            }
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            bg = new Bitmap("bg.jpg");
            car = new Bitmap("car.png");
            car.MakeTransparent(car.GetPixel(0, 0));
          

        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            drawdb(e.Graphics);

        }
        void drawdb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawscene(g2);
            g.DrawImage(off, 0, 0);

        }
        void drawscene(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawImage(bg, 0, 0, ClientSize.Width, ClientSize.Height);
            g.DrawImage(car, 0, ClientSize.Height / 2, 80, 60);
            foreach (var l in lines)
            {
                g.DrawLine(p, l.Xst, l.Yst, l.Xend, l.Yend);
            }

        }
    }
}
