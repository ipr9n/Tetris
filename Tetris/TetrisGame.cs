using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    class TetrisGame
    {
        public int offsetBox1x, offsetBox1y, offsetBox2x, offsetBox2y, offsetBox3x, offsetBox3y;
        private new Random random = new Random();
        private int squareSize = 20;
        private int score = 0;
        private int shapes = 0;
        int[,] zanjato = new int[10, 20];
        private int newBoxPositionX = 5, newBoxPositionY = 1;
        private Point boxLocation;

        public void MoveBlock(Keys direction)
        {
            switch (direction)
            {
                case Keys.Left:
                    if (newBoxPositionX > 0 || newBoxPositionX + offsetBox1x > 0 || newBoxPositionX + offsetBox2x > 0 || newBoxPositionX + offsetBox3x > 0)
                        newBoxPositionX -= 1;
                    break;
                case Keys.Right:
                    if (newBoxPositionX < 9 || newBoxPositionX + offsetBox1x < 9 || newBoxPositionX + offsetBox2x < 9 || newBoxPositionX + offsetBox3x < 9)
                        newBoxPositionX += 1;
                    break;
            }
        }

        public void Update()
        {
            if (newBoxPositionY > 18 || newBoxPositionY + offsetBox1y > 18 || newBoxPositionY + offsetBox2y > 18 || newBoxPositionY + offsetBox3y > 18)
            {
                zanjato[newBoxPositionX, newBoxPositionY] = 1;
                zanjato[newBoxPositionX + offsetBox1x, newBoxPositionY + offsetBox1y] = 1;
                zanjato[newBoxPositionX + offsetBox2x, newBoxPositionY + offsetBox2y] = 1;
                zanjato[newBoxPositionX + offsetBox3x, newBoxPositionY + offsetBox3y] = 1;
                newBoxPositionX = 5;
                newBoxPositionY = 1;
                boxLocation = new Point(newBoxPositionX, newBoxPositionY);
                GetShape();
                checkLine();
            }

            if (zanjato[newBoxPositionX, newBoxPositionY + 1] == 1 ||
                zanjato[newBoxPositionX + offsetBox1x, newBoxPositionY + 1 + offsetBox1y] == 1 ||
                zanjato[newBoxPositionX + offsetBox2x, newBoxPositionY + 1 + offsetBox2y] == 1 ||
                zanjato[newBoxPositionX + offsetBox3x, newBoxPositionY + 1 + offsetBox3y] == 1)
            {
                zanjato[newBoxPositionX, newBoxPositionY] = 1;
                zanjato[newBoxPositionX + offsetBox1x, newBoxPositionY + offsetBox1y] = 1;
                zanjato[newBoxPositionX + offsetBox2x, newBoxPositionY + offsetBox2y] = 1;
                zanjato[newBoxPositionX + offsetBox3x, newBoxPositionY + offsetBox3y] = 1;
                newBoxPositionX = 5;
                newBoxPositionY = 1;
                boxLocation = new Point(newBoxPositionX, newBoxPositionY);
                GetShape();
                checkLine();
            }
            newBoxPositionY += 1;
            boxLocation = new Point(newBoxPositionX, newBoxPositionY);
        }

        private void checkLine()
        {
            int countLine = 0;
            for (int i = 0; i < 20; i++)
                for (int x = 0; x < 10; x++)
                {
                    if (zanjato[x, i] != 1)
                        break;

                    if (x >= 9)
                    {
                        countLine++;
                        for (int l = 0; l < 10; l++)
                            zanjato[l, i] = 0;

                        for (int p = i - 1; p > 0; p--)
                            for (int z = 0; z < 10; z++)
                                if (zanjato[z, p] == 1)
                                {
                                    zanjato[z, p] = 0;
                                    zanjato[z, p + 1] = 1;
                                }
                    }

                }

            switch (countLine)
            {
                case 1:
                    score += 100;
                    break;
                case 2:
                    score += 300;
                    break;
                case 3:
                    score += 700;
                    break;
                case 4:
                    score += 1500;
                    break;
            }
        }

        public void Draw(Graphics graphics)
        {
            for (int x = 0; x <= 10; x++)
                graphics.DrawLine(Pens.Black, x * squareSize, 0, x * squareSize, 400);

            for (int y = 0; y <= 20; y++)
                graphics.DrawLine(Pens.Black, 0, y * squareSize, 200, y * squareSize);

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 20; y++)
                    graphics.FillRectangle(zanjato[x, y] == 1 ? Brushes.Blue : Brushes.Green, x * squareSize,
                    y * squareSize, squareSize - 1, squareSize - 1);

            graphics.FillRectangle(Brushes.Blue, boxLocation.X * squareSize, boxLocation.Y * squareSize, squareSize - 1, squareSize - 1);
            graphics.FillRectangle(Brushes.Blue, (boxLocation.X + offsetBox1x) * squareSize, (boxLocation.Y + offsetBox1y) * squareSize, squareSize - 1, squareSize - 1);
            graphics.FillRectangle(Brushes.Blue, (boxLocation.X + offsetBox2x) * squareSize, (boxLocation.Y + offsetBox2y) * squareSize, squareSize - 1, squareSize - 1);
            graphics.FillRectangle(Brushes.Blue, (boxLocation.X + offsetBox3x) * squareSize, (boxLocation.Y + offsetBox3y) * squareSize, squareSize - 1, squareSize - 1);
        }

        public int getScore()
        {
            return score;
        }

        public void GetShape()
        {
            boxLocation = new Point(5, 0);
            shapes = random.Next(5);

            switch (shapes)
            {
                case 0:

                    offsetBox1x = 1;
                    offsetBox1y = 0;
                    offsetBox2x = 1;
                    offsetBox2y = 1;
                    offsetBox3x = 0;
                    offsetBox3y = 1;
                    break;
                case 1:
                    offsetBox1x = 0;
                    offsetBox1y = -1;
                    offsetBox2x = 0;
                    offsetBox2y = 1;
                    offsetBox3x = -1;
                    offsetBox3y = 1;
                    break;
                case 2:
                    offsetBox1x = 0;
                    offsetBox1y = -1;
                    offsetBox2x = 0;
                    offsetBox2y = 1;
                    offsetBox3x = 1;
                    offsetBox3y = 1;
                    break;
                case 3:
                    offsetBox1x = -1;
                    offsetBox1y = 0;
                    offsetBox2x = 0;
                    offsetBox2y = -1;
                    offsetBox3x = 1;
                    offsetBox3y = -1;
                    break;
                case 4:
                    offsetBox1x = -1;
                    offsetBox1y = -1;
                    offsetBox2x = 0;
                    offsetBox2y = -1;
                    offsetBox3x = 1;
                    offsetBox3y = 0;
                    break;
            }
        }

    }
}
