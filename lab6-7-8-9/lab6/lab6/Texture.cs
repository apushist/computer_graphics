using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lab6
{
    public class Texture
    {
        public Bitmap Bitmap { get; set; }
        public string Name { get; set; }

        public Texture()
        {
        }

        public Texture(Bitmap bitmap, string name = "")
        {
            Bitmap = bitmap;
            Name = name;
        }

        public static Texture CreateTestTexture()
        {
            int size = 256;
            Bitmap bmp = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                int tileSize = 32;
                for (int y = 0; y < size; y += tileSize)
                {
                    for (int x = 0; x < size; x += tileSize)
                    {
                        bool isBlack = ((x / tileSize + y / tileSize) % 2) == 0;
                        Color color = isBlack ? Color.Black : Color.White;

                        using (var brush = new SolidBrush(color))
                        {
                            g.FillRectangle(brush, x, y, tileSize, tileSize);
                        }
                    }
                }

                using (var pen = new Pen(Color.Red, 4))
                {
                    g.DrawLine(pen, 0, 0, size, size);
                    g.DrawLine(pen, size, 0, 0, size);
                }
            }

            return new Texture { Bitmap = bmp };
        }
    }
}
