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
        public bool can_move = false;
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
            if (can_move)
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
        public bool can_move = false;
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

                g.FillEllipse(Brushes.Black, x, y, 5, 5);
            }
            PointF tempst = Getnextpoint((int)st);
            PointF tempend = Getnextpoint((int)end);
            //g.DrawLine(Pens.Green, xc + movex, yc + movey,tempst.X + movex, tempst.Y + movey);

            //g.DrawLine(Pens.Green,xc + movex, yc + movey,tempend.X + movex, tempend.Y + movey);
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
        List<Circle> c1 = new List<Circle>();
        bool is_start = false;
        List<char> road = new List<char>();
        int line_index = 0;
        int circle_index = 0;
        int road_index = 0;
        int car_x = 0;
        int car_y = 0;
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
            if (is_start)
                motion();

            drawdb(CreateGraphics());
        }

        private void Form1_MouseDown(object? sender, MouseEventArgs e)
        {

        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                is_start = true;
                foreach (var l in lines)
                {
                    l.calc();
                }
            }
            drawline(e.KeyCode);
            drawcircles(e.KeyCode);
        }
        void drawline(Keys keyCode)
        {
            if (keyCode == Keys.L)
            {
                state = 'l';
                if (ct == 0)
                {

                    DDA temp = new DDA();
                    temp.Xst = 0;
                    temp.Yst = (ClientSize.Height / 2) + car.Height;
                    temp.Yend = (ClientSize.Height / 2) + car.Height;
                    temp.cx = temp.Xst;
                    temp.cy = temp.Yst;

                    lines.Add(temp);
                    road.Add('l');
                    car_x = (int)lines[0].cx;
                    car_y = (int)lines[0].cy - car.Height;
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
                    road.Add('l');
                }

            }


            if (state == 'l')
            {
                if (keyCode == Keys.Right)
                {
                    if (lines[lines.Count - 1].Xend < car.Width)
                    {
                        lines[lines.Count - 1].Xend += 80;

                    }
                    lines[lines.Count - 1].Xend += 20;
                    current_xend = lines[lines.Count - 1].Xend;
                    current_yend = lines[lines.Count - 1].Yend;
                }
                if (keyCode == Keys.Left)
                {
                    lines[lines.Count - 1].Xend -= 20;
                    current_xend = lines[lines.Count - 1].Xend;
                    current_yend = lines[lines.Count - 1].Yend;
                }
            }
        }
        void drawcircles(Keys keyCode)
        {

            if (keyCode == Keys.C)

            {
                state = 'c';
                Circle temp = new Circle();
                temp.rad = 50;
                temp.xc = (int)current_xend;
                temp.yc = (int)current_yend - temp.rad;
                temp.st = 0;
                temp.end = 360;
                c1.Add(temp);
                road.Add('c');
            }
            if (state == 'c')
            {
                if (ct > 0)
                {

                    if (keyCode == Keys.Right)
                    {
                        c1[c1.Count - 1].rad += 20;
                        c1[c1.Count - 1].yc -= 20;
                    }
                    if (keyCode == Keys.Left)
                    {
                        c1[c1.Count - 1].rad -= 20;
                        c1[c1.Count - 1].yc += 20;
                    }
                }
            }
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            bg = new Bitmap("bg.jpg");
            car = new Bitmap("car2.png");
            car.MakeTransparent(car.GetPixel(0, 0));

        }

        void motion()
        {
            if (is_start)
            {
                if (road_index < road.Count)
                {
                    /////// for line motion

                    if (road[road_index] == 'l')
                    {
                        lines[line_index].CalcNextPoint();
                        lines[line_index].can_move = true;
                        car_x = (int)lines[line_index].cx;
                        car_y = (int)lines[line_index].cy - car.Height;
                        if (lines[line_index].direction == 0)
                        {
                            lines[line_index].can_move = false;
                            line_index++;
                            road_index++;
                        }
                    }

                    // for circle motion
                    else if (road[road_index] == 'c')
                    {
                        
                    }
                }
            }
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
            if (!is_start)
            {

                g.DrawImage(car, 0, ClientSize.Height / 2);
            }
            else
            {
                g.DrawImage(car, car_x, car_y);

            }
            foreach (var l in lines)
            {
                g.DrawLine(p, l.Xst, l.Yst, l.Xend, l.Yend);
                //g.FillEllipse(Brushes.Black,l.cx, l.cy,15,15);
            }
            foreach (var c in c1)
            {
                c.Drawcircle(g);

            }
        }
    }
}
