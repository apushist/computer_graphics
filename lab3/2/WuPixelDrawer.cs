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

        public static void DrawWuPixelPattern(Graphics g, bool steep, int x, int y, float intensity, Rectangle bounds, Color color)
        {
            int drawX = steep ? y : x;
            int drawY = steep ? x : y;

            float[,] pattern = new float[,]
            {
                { 0.7f, 0.1f, 0.0f },
                { 0.3f, 0.9f, 0.6f },
                { 0.0f, 0.0f, 0.4f }
            };

            for (int ky = 0; ky < 3; ky++)
            {
                for (int kx = 0; kx < 3; kx++)
                {
                    float factor = pattern[ky, kx];
                    if (factor <= 0) continue;

                    int px = drawX + (kx - 1);
                    int py = drawY + (ky - 1);

                    if (px >= 0 && px < bounds.Width && py >= 0 && py < bounds.Height)
                    {
                        int alpha = (int)(intensity * factor * 255);
                        if (alpha > 0)
                        {
                            Color c = Color.FromArgb(alpha, color);
                            using Brush b = new SolidBrush(c);
                            g.FillRectangle(b, px + bounds.X, py + bounds.Y, 1, 1);
                        }
                    }
                }
            }
        }
    }
}
