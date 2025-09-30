using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public static class LineAlgorithms
    {
        public static void DrawLineBresenham(Graphics g, Point p0, Point p1, Rectangle bounds, Color color)
        {
            int x0 = p0.X < bounds.Width ? p0.X : p0.X - bounds.Width;
            int x1 = p1.X < bounds.Width ? p1.X : p1.X - bounds.Width;
            int y0 = p0.Y;
            int y1 = p1.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            using Brush b = new SolidBrush(color);
            while (true)
            {
                if (x0 >= 0 && x0 < bounds.Width && y0 >= 0 && y0 < bounds.Height)
                    g.FillRectangle(b, x0 + bounds.X, y0, 1, 1);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }

        public static void DrawLineWu(Graphics g, Point p0, Point p1, Rectangle bounds, Color color)
        {
            int x0 = p0.X;
            int y0 = p0.Y;
            int x1 = p1.X;
            int y1 = p1.Y;

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dx == 0 ? 1 : dy / dx;

            float y = y0 + gradient;

            WuPixelDrawer.DrawWuPixel(g, steep, x0, y0, 1f, bounds, color);

            for (int x = x0 + 1; x < x1; x++)
            {
                int yInt = (int)y;
                float frac = y - yInt;

                WuPixelDrawer.DrawWuPixel(g, steep, x, yInt, 1 - frac, bounds, color);
                WuPixelDrawer.DrawWuPixel(g, steep, x, yInt + 1, frac, bounds, color);

                y += gradient;
            }

            WuPixelDrawer.DrawWuPixel(g, steep, x1, y1, 1f, bounds, color);
        }

        private static void Swap(ref int a, ref int b)
        {
            (b, a) = (a, b);
        }
    }
}
