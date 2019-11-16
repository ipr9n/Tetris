using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tetris
{
    public partial class Form1 : Form
    {
        private TetrisGame tetrisGame = new TetrisGame();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 120;
            pictureBox1.Location = new Point(1,1);
            pictureBox1.Size = new Size(500,1000);
            label1.Location = new Point(210, 20);
            this.Size = pictureBox1.Size;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            tetrisGame.MoveBlock(e.KeyCode);

            if (!timer1.Enabled)
                timer1.Start();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            tetrisGame.Draw(e.Graphics);
            label1.Text = "Очки: \n" + tetrisGame.getScore();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tetrisGame.Update();
            pictureBox1.Refresh();
        }
    }
}
