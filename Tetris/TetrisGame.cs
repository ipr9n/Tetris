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
        private int squareSize = 20;
        private int score = 0;
        int[,] zanjato = new int[10, 20];
        private int newBoxPositionX = 5, newBoxPositionY = 0;
        private Point boxLocation;

        public void MoveBlock(Keys direction)
        {
            switch (direction)
            {
                case Keys.Left:
                    if (newBoxPositionX > 0)
                        newBoxPositionX -= 1;
                    break;
                case Keys.Right:
                    if (newBoxPositionX < 9)
                        newBoxPositionX += 1;
                    break;
            }
        }

        public int getScore()
        {
            return score;
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

        public void Update()
        {
            if (newBoxPositionY > 18)
            {
                zanjato[newBoxPositionX, newBoxPositionY] = 1;
                newBoxPositionX = 5;
                newBoxPositionY = 1;
                checkLine();
            }

            if (zanjato[newBoxPositionX, newBoxPositionY + 1] == 1)
            {
                zanjato[newBoxPositionX, newBoxPositionY] = 1;
                checkLine();
            }

            newBoxPositionY += 1;
            boxLocation = new Point(newBoxPositionX, newBoxPositionY);
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
        }
    }
}
