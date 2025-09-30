using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public static class WuPixelDrawer
    {
        public static void DrawWuPixel(Graphics g, bool steep, int x, int y, float intensity, Rectangle bounds, Color color)
        {
            int drawX = steep ? y : x;
            int drawY = steep ? x : y;

            if (drawX >= 0 && drawX < bounds.Width && drawY >= 0 && drawY < bounds.Height)
            {
                Color c = Color.FromArgb((int)(intensity * 255), color);
                using Brush b = new SolidBrush(c);
                g.FillRectangle(b, drawX + bounds.X, drawY + bounds.Y, 1, 1);
            }
        }
    }
}
