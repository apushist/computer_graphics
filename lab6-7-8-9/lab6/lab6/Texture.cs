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
        public Bitmap Bitmap { get; private set; }
        public int Width => Bitmap?.Width ?? 0;
        public int Height => Bitmap?.Height ?? 0;

        public Texture(Bitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public static Texture CreateTestTexture()
        {
            int size = 256;
            var bitmap = new Bitmap(size, size);

            using (var g = Graphics.FromImage(bitmap))
            using (var brush1 = new SolidBrush(Color.Red))
            using (var brush2 = new SolidBrush(Color.Blue))
            using (var brush3 = new SolidBrush(Color.Green))
            using (var brush4 = new SolidBrush(Color.Yellow))
            {
                g.FillRectangle(brush1, 0, 0, size / 2, size / 2);
                g.FillRectangle(brush2, size / 2, 0, size / 2, size / 2);
                g.FillRectangle(brush3, 0, size / 2, size / 2, size / 2);
                g.FillRectangle(brush4, size / 2, size / 2, size / 2, size / 2);
            }

            return new Texture(bitmap);
        }

        public static Texture FromFile(string filePath)
        {
            var bitmap = new Bitmap(filePath);
            return new Texture(bitmap);
        }

        public void Dispose()
        {
            Bitmap?.Dispose();
        }
    }
}
