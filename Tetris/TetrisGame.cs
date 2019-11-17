using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    class TetrisGame
    {
        readonly Point[] shapeBoxesOffsets = new Point[3];
        private readonly Random random = new Random();
        public event Action Defeat = delegate { };
        private int squareSize = 20;
        private int tetrisWight = 10;
        private int tetrisHeight = 20;
        private int score = 0;
        private int shapes = 0;
        private int[,] zanjato = new int[10, 20];
        private int newBoxPositionX = 5;
        private int newBoxPositionY = 1;
        private Point boxLocation;

        public void MoveBlock(Keys direction)
        {
            switch (direction)
            {
                case Keys.Left:
                    if (newBoxPositionX > 0 &
                        shapeBoxesOffsets.All(t => t.X > 0) &&
                        zanjato[shapeBoxesOffsets[0].X - 1, shapeBoxesOffsets[0].Y] == 0 &&
                        zanjato[shapeBoxesOffsets[1].X - 1, shapeBoxesOffsets[1].Y] == 0 &&
                        zanjato[shapeBoxesOffsets[2].X - 1, shapeBoxesOffsets[2].Y] == 0 &&
                        zanjato[newBoxPositionX - 1, newBoxPositionY] == 0)
                    {
                        shapeBoxesOffsets[0].X -= 1;
                        shapeBoxesOffsets[1].X -= 1;
                        shapeBoxesOffsets[2].X -= 1;
                        newBoxPositionX -= 1;
                    }

                    break;
                case Keys.Right:
                    if (newBoxPositionX < tetrisWight - 1 &&
                        shapeBoxesOffsets.All(t => t.X < tetrisWight - 1) &&
                        zanjato[shapeBoxesOffsets[0].X + 1, shapeBoxesOffsets[0].Y] == 0 &&
                        zanjato[shapeBoxesOffsets[1].X + 1, shapeBoxesOffsets[1].Y] == 0 &&
                        zanjato[shapeBoxesOffsets[2].X + 1, shapeBoxesOffsets[2].Y] == 0 &&
                        zanjato[newBoxPositionX + 1, newBoxPositionY] == 0)
                    {
                        shapeBoxesOffsets[0].X += 1;
                        shapeBoxesOffsets[1].X += 1;
                        shapeBoxesOffsets[2].X += 1;
                        newBoxPositionX += 1;
                    }

                    break;
                case Keys.Up:
                    TurnShape();
                    break;
                case Keys.Space:
                    while (newBoxPositionY < tetrisHeight - 2||
                           shapeBoxesOffsets.All(t => t.Y < tetrisHeight - 2) ||
                           zanjato[newBoxPositionX, newBoxPositionY + 1] != 1 ||
                           zanjato[shapeBoxesOffsets[0].X, shapeBoxesOffsets[0].Y + 1] != 1 ||
                           zanjato[shapeBoxesOffsets[1].X, shapeBoxesOffsets[1].Y + 1] != 1 ||
                           zanjato[shapeBoxesOffsets[2].X, shapeBoxesOffsets[2].Y + 1] != 1)
                    {
                        newBoxPositionY += 1;
                        shapeBoxesOffsets[0].Y += 1;
                        shapeBoxesOffsets[1].Y += 1;
                        shapeBoxesOffsets[2].Y += 1;
                  //      boxLocation = new Point(newBoxPositionX, newBoxPositionY);

                    }
                    zanjato[newBoxPositionX, newBoxPositionY] = 1;
                    zanjato[shapeBoxesOffsets[0].X, shapeBoxesOffsets[0].Y] = 1;
                    zanjato[shapeBoxesOffsets[1].X, shapeBoxesOffsets[1].Y] = 1;
                    zanjato[shapeBoxesOffsets[2].X, shapeBoxesOffsets[2].Y] = 1;
                    newBoxPositionX = 5;
                    newBoxPositionY = 1;
                  //  boxLocation = new Point(newBoxPositionX, newBoxPositionY);
                    GetShape();
                    CheckLine();
                    break;
            }
        }

        public void Update()
        {
            if (newBoxPositionY > tetrisHeight - 2 ||
                shapeBoxesOffsets[0].Y > tetrisHeight - 2 ||
                shapeBoxesOffsets[1].Y > tetrisHeight - 2 ||
                shapeBoxesOffsets[2].Y > tetrisHeight - 2)
            {
                zanjato[newBoxPositionX, newBoxPositionY] = 1;
                zanjato[shapeBoxesOffsets[0].X, shapeBoxesOffsets[0].Y] = 1;
                zanjato[shapeBoxesOffsets[1].X, shapeBoxesOffsets[1].Y] = 1;
                zanjato[shapeBoxesOffsets[2].X, shapeBoxesOffsets[2].Y] = 1;
                newBoxPositionX = 5;
                newBoxPositionY = 1;
                // boxLocation = new Point(newBoxPositionX, newBoxPositionY);
                GetShape();
                CheckLine();
            }

            if (zanjato[newBoxPositionX, newBoxPositionY + 1] == 1 ||
                zanjato[shapeBoxesOffsets[0].X, shapeBoxesOffsets[0].Y + 1] == 1 ||
                zanjato[shapeBoxesOffsets[1].X, shapeBoxesOffsets[1].Y + 1] == 1 ||
                zanjato[shapeBoxesOffsets[2].X, shapeBoxesOffsets[2].Y + 1] == 1)
            {
                if (shapeBoxesOffsets.Any(t => t.Y == 1)) Defeat();
                zanjato[newBoxPositionX, newBoxPositionY] = 1;
                zanjato[shapeBoxesOffsets[0].X, shapeBoxesOffsets[0].Y] = 1;
                zanjato[shapeBoxesOffsets[1].X, shapeBoxesOffsets[1].Y] = 1;
                zanjato[shapeBoxesOffsets[2].X, shapeBoxesOffsets[2].Y] = 1;
                newBoxPositionX = 5;
                newBoxPositionY = 1;
              //  boxLocation = new Point(newBoxPositionX, newBoxPositionY);
                GetShape();
                CheckLine();
            }

            newBoxPositionY += 1;
            shapeBoxesOffsets[0].Y += 1;
            shapeBoxesOffsets[1].Y += 1;
            shapeBoxesOffsets[2].Y += 1;
          //  boxLocation = new Point(newBoxPositionX, newBoxPositionY);
        }

        private void CheckLine()
        {
            int countLine = 0;

            for (int i = 0; i < tetrisHeight; i++)
                for (int x = 0; x < tetrisWight; x++)
                {
                    if (zanjato[x, i] != 1)
                        break;

                    if (x >= tetrisWight - 1)
                    {
                        countLine++;
                        for (int l = 0; l < tetrisWight; l++)
                            zanjato[l, i] = 0;

                        for (int p = i - 1; p > 0; p--)
                            for (int z = 0; z < tetrisWight; z++)
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
            for (int x = 0; x <= tetrisWight; x++)
                graphics.DrawLine(Pens.Black, x * squareSize, 0, x * squareSize, 400);

            for (int y = 0; y <= tetrisHeight; y++)
                graphics.DrawLine(Pens.Black, 0, y * squareSize, 200, y * squareSize);

            for (int x = 0; x < tetrisWight; x++)
                for (int y = 0; y < tetrisHeight; y++)
                    graphics.FillRectangle(zanjato[x, y] == 1 ? Brushes.Blue : Brushes.Green, x * squareSize,
                    y * squareSize, squareSize - 1, squareSize - 1);

            graphics.FillRectangle(Brushes.Blue, newBoxPositionX * squareSize, newBoxPositionY * squareSize, squareSize - 1, squareSize - 1);
            graphics.FillRectangle(Brushes.Blue, shapeBoxesOffsets[0].X * squareSize, shapeBoxesOffsets[0].Y * squareSize, squareSize - 1, squareSize - 1);
            graphics.FillRectangle(Brushes.Blue, shapeBoxesOffsets[1].X * squareSize, shapeBoxesOffsets[1].Y * squareSize, squareSize - 1, squareSize - 1);
            graphics.FillRectangle(Brushes.Blue, shapeBoxesOffsets[2].X * squareSize, shapeBoxesOffsets[2].Y * squareSize, squareSize - 1, squareSize - 1);
        }

        public int getScore()
        {
            return score;
        }

        public void TurnShape()
        {
            for (int i = 0; i < 3; i++)
            {
                if (shapeBoxesOffsets[i].X == newBoxPositionX + 1 && shapeBoxesOffsets[i].Y == newBoxPositionY)
                {
                    shapeBoxesOffsets[i].X = newBoxPositionX;
                    shapeBoxesOffsets[i].Y = newBoxPositionY + 1;
                }

                else if (shapeBoxesOffsets[i].X == newBoxPositionX && shapeBoxesOffsets[i].Y == newBoxPositionY + 1)
                {
                    shapeBoxesOffsets[i].X = newBoxPositionX - 1;
                    shapeBoxesOffsets[i].Y = newBoxPositionY;
                }

                else if (shapeBoxesOffsets[i].X == newBoxPositionX + 1 && shapeBoxesOffsets[i].Y == newBoxPositionY + 1)
                {
                    shapeBoxesOffsets[i].X = newBoxPositionX - 1;
                    shapeBoxesOffsets[i].Y = newBoxPositionY + 1;
                }

                else if (shapeBoxesOffsets[i].X == newBoxPositionX && shapeBoxesOffsets[i].Y == newBoxPositionY - 1)
                {
                    shapeBoxesOffsets[i].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[i].Y = newBoxPositionY;
                }

                else if (shapeBoxesOffsets[i].X == newBoxPositionX - 1 && shapeBoxesOffsets[i].Y == newBoxPositionY + 1)
                {
                    shapeBoxesOffsets[i].X = newBoxPositionX - 1;
                    shapeBoxesOffsets[i].Y = newBoxPositionY - 1;
                }

                else if (shapeBoxesOffsets[i].X == newBoxPositionX - 1 && shapeBoxesOffsets[0].Y == newBoxPositionY)
                {
                    shapeBoxesOffsets[0].X = newBoxPositionX;
                    shapeBoxesOffsets[0].Y = newBoxPositionY - 1;
                }

                else if (shapeBoxesOffsets[0].X == newBoxPositionX + 1 && shapeBoxesOffsets[0].Y == newBoxPositionY - 1)
                {
                    shapeBoxesOffsets[0].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[0].Y = newBoxPositionY + 1;
                }

                else if (shapeBoxesOffsets[0].X == newBoxPositionX - 1 && shapeBoxesOffsets[0].Y == newBoxPositionY - 1)
                {
                    shapeBoxesOffsets[0].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[0].Y = newBoxPositionY - 1;
                }

            }
        }

        public void GetShape()
        {
          ///  boxLocation = new Point(5, 1);
            shapes = random.Next(5);

            switch (shapes)
            {
                case 0:

                    shapeBoxesOffsets[0].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[0].Y = newBoxPositionY;
                    shapeBoxesOffsets[1].X = newBoxPositionX;
                    shapeBoxesOffsets[1].Y = newBoxPositionY + 1;
                    shapeBoxesOffsets[2].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[2].Y = newBoxPositionY + 1; ;
                    break;
                case 1:
                    shapeBoxesOffsets[0].X = newBoxPositionX;
                    shapeBoxesOffsets[0].Y = newBoxPositionY - 1;
                    shapeBoxesOffsets[1].X = newBoxPositionX;
                    shapeBoxesOffsets[1].Y = newBoxPositionY + 1; ;
                    shapeBoxesOffsets[2].X = newBoxPositionX - 1;
                    shapeBoxesOffsets[2].Y = newBoxPositionY + 1; ;
                    break;
                case 2:
                    shapeBoxesOffsets[0].X = newBoxPositionX;
                    shapeBoxesOffsets[0].Y = newBoxPositionY - 1;
                    shapeBoxesOffsets[1].X = newBoxPositionX;
                    shapeBoxesOffsets[1].Y = newBoxPositionY + 1; ;
                    shapeBoxesOffsets[2].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[2].Y = newBoxPositionY + 1; ;
                    break;
                case 3:
                    shapeBoxesOffsets[0].X = newBoxPositionX - 1;
                    shapeBoxesOffsets[0].Y = newBoxPositionY;
                    shapeBoxesOffsets[1].X = newBoxPositionX;
                    shapeBoxesOffsets[1].Y = newBoxPositionY - 1;
                    shapeBoxesOffsets[2].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[2].Y = newBoxPositionY - 1;
                    break;
                case 4:
                    shapeBoxesOffsets[0].X = newBoxPositionX - 1;
                    shapeBoxesOffsets[0].Y = newBoxPositionY - 1;
                    shapeBoxesOffsets[1].X = newBoxPositionX;
                    shapeBoxesOffsets[1].Y = newBoxPositionY - 1;
                    shapeBoxesOffsets[2].X = newBoxPositionX + 1;
                    shapeBoxesOffsets[2].Y = newBoxPositionY;
                    break;
            }
        }

        private void GameOver()
        {

        }
    }
}
