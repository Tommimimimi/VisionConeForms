using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisionConeForms
{
    public partial class Form1 : Form
    {
        //player variables
        Rectangle player;
        bool left, right, up, down;
        int speed = 20;
        Color semiTransparent = Color.FromArgb(100, 255, 100, 100);
        double playerX;
        double playerY;

        public Form1()
        {
            InitializeComponent();

            player = new Rectangle(40, 40, 40, 40);
            playerX = player.X;
            playerY = player.Y;

            this.DoubleBuffered = true;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            g.FillEllipse(Brushes.Black, player);
            g.DrawEllipse(Pens.Gray, player);

            Point center = new Point(player.X + player.Width / 2, player.Y + player.Height / 2);
            Point mouse = PointToClient(MousePosition);

            float angle = GetAngleDegrees(center, mouse);

            int coneRadius = 150;
            int coneWidth = 60;
            Rectangle visionArea = new Rectangle(center.X - coneRadius, center.Y - coneRadius, coneRadius * 2, coneRadius * 2);

            using (SolidBrush semiTransparentBrush = new SolidBrush(semiTransparent))
            {
                g.FillPie(semiTransparentBrush, visionArea, angle - coneWidth / 2, coneWidth);
            }
        }

        float GetAngleDegrees(Point center, Point check)
        {
            float dx = check.X - center.X;
            float dy = check.Y - center.Y;

            return (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) up = true;
            if (e.KeyCode == Keys.S) down = true;
            if (e.KeyCode == Keys.D) right = true;
            if (e.KeyCode == Keys.A) left = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) up = false;
            if (e.KeyCode == Keys.S) down = false;
            if (e.KeyCode == Keys.D) right = false;
            if (e.KeyCode == Keys.A) left = false;
        }

        private void movePlayer()
        {
            Point center = new Point((int)playerX + player.Width / 2, (int)playerY + player.Height / 2);
            Point mouse = PointToClient(MousePosition);
            float angle = GetAngleDegrees(center, mouse);
            double angleRad = angle * Math.PI / 180.0;

            if (up)
            {
                playerX += Math.Cos(angleRad) * speed;
                playerY += Math.Sin(angleRad) * speed;
            }

            if (down)
            {
                playerX -= Math.Cos(angleRad) * speed;
                playerY -= Math.Sin(angleRad) * speed;
            }

            if (right)
            {
                playerX -= Math.Sin(angleRad) * speed;
                playerY += Math.Cos(angleRad) * speed;
            }

            if (left)
            {
                playerX += Math.Sin(angleRad) * speed;
                playerY -= Math.Cos(angleRad) * speed;
            }

            player.X = (int)playerX;
            player.Y = (int)playerY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            movePlayer();
            Invalidate();
        }
    }
}
