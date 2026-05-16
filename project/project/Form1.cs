namespace project
{
    public partial class Form1 : Form
    {
        Bitmap off;
        Image bg;
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            Load += Form1_Load;
            WindowState = FormWindowState.Maximized;
            ///////////// for multiple screen (for my home secound screen  )
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(1920, 0);

        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            off=new Bitmap(ClientSize.Width, ClientSize.Height);
        bg = Image.FromFile("bg.jpg");
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

        }
    }
}
