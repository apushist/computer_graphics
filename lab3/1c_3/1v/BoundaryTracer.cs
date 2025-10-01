using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1v
{
    public static class BoundaryTracer
    {
        private static readonly Point[] directions =
        {
            new Point(1, 0), 
            new Point(1, 1),  
            new Point(0, 1),  
            new Point(-1, 1),  
            new Point(-1, 0), 
            new Point(-1, -1), 
            new Point(0, -1),   
            new Point(1, -1)  
        };

        private static bool IsColorClose(Color c1, Color c2, int tolerance = 10)
        {
            return Math.Abs(c1.R - c2.R) <= tolerance &&
                   Math.Abs(c1.G - c2.G) <= tolerance &&
                   Math.Abs(c1.B - c2.B) <= tolerance;
        }

        public static Point? FindStartPoint(Bitmap bmp, Color activeColor, int tolerance = 10)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (IsColorClose(bmp.GetPixel(x, y), activeColor, tolerance))
                        return new Point(x, y);
                }
            }
            return null;
        }

        public static List<Point> TraceBoundary(Bitmap bmp, Point start, Color activeColor, int tolerance = 10)
        {
            List<Point> boundary = new();
            Point current = start;
            int dirIndex = 0;

            boundary.Add(current);
            Point first = start;

            do
            {
                bool found = false;
                for (int i = 0; i < 8; i++)
                {
                    int checkIndex = (dirIndex + i) % 8;
                    Point next = new Point(current.X + directions[checkIndex].X,
                                           current.Y + directions[checkIndex].Y);

                    if (next.X >= 0 && next.Y >= 0 &&
                        next.X < bmp.Width && next.Y < bmp.Height)
                    {
                        if (IsColorClose(bmp.GetPixel(next.X, next.Y), activeColor, tolerance))
                        {
                            boundary.Add(next);
                            current = next;
                            dirIndex = (checkIndex + 6) % 8;
                            found = true;
                            break;
                        }
                    }
                }
                if (!found) break;
            }
            while (current != first);

            return boundary;
        }
    }
}
