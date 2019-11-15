using System;
using System.Collections.Generic;
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
        private List<Point> zanyato = new List<Point>();
        private int x = 5, y = 1;
        private Point boxLocation;

        public void MoveBlock(Keys direction)
        {
            switch (direction)
            {
                case Keys.Left:
                    x -= 1;
                    break;
                case Keys.Right:
                    x += 1;
                    break;

            }
        }

        public void Update()
        {
            y += 1;
            if (zanyato.Contains(boxLocation) || y>19)
            {
                zanyato.Add(new Point(boxLocation.X, boxLocation.Y));
                x = 5;
                y = 1;
            }
            boxLocation = new Point(x, y);
        }

        public void Draw(Graphics graphics)
        {
            for (int x = 0; x <= 10; x++)
                graphics.DrawLine(Pens.Black, x * squareSize, 0, x * squareSize, 400);

            for (int y = 0; y <= 20; y++)
                graphics.DrawLine(Pens.Black, 0, y * squareSize, 200, y * squareSize);

            for (int z = 0; z < zanyato.Count; z++)
                graphics.FillRectangle(Brushes.Blue, zanyato[z].X * squareSize, zanyato[z].Y * squareSize, squareSize, squareSize);

            graphics.FillRectangle(Brushes.Blue, boxLocation.X * squareSize, boxLocation.Y * squareSize, squareSize, squareSize);
        }
    }
}
