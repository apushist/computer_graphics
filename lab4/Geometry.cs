using System;
using System.Drawing;
using System.Collections.Generic;

namespace lab4
{
    public static class Geometry
    {
        public static double Distance(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static bool IsNearLine(Point p, Point p1, Point p2, double threshold = 6.0)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            if (dx == 0 && dy == 0)
                return Distance(p, p1) < threshold;

            double t = ((p.X - p1.X) * dx + (p.Y - p1.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            double projX = p1.X + t * dx;
            double projY = p1.Y + t * dy;

            double dist = Distance(p, new Point((int)projX, (int)projY));
            return dist < threshold;
        }

        public static bool TryGetIntersection(Point p1, Point p2, Point q1, Point q2, out Point intersection)
        {
            intersection = new Point();

            float a1 = p2.Y - p1.Y;
            float b1 = p1.X - p2.X;
            float c1 = a1 * p1.X + b1 * p1.Y;

            float a2 = q2.Y - q1.Y;
            float b2 = q1.X - q2.X;
            float c2 = a2 * q1.X + b2 * q1.Y;

            float det = a1 * b2 - a2 * b1;
            if (det == 0)
                return false;

            float x = (b2 * c1 - b1 * c2) / det;
            float y = (a1 * c2 - a2 * c1) / det;

            if (x >= Math.Min(p1.X, p2.X) && x <= Math.Max(p1.X, p2.X) &&
                y >= Math.Min(p1.Y, p2.Y) && y <= Math.Max(p1.Y, p2.Y) &&
                x >= Math.Min(q1.X, q2.X) && x <= Math.Max(q1.X, q2.X) &&
                y >= Math.Min(q1.Y, q2.Y) && y <= Math.Max(q1.Y, q2.Y))
            {
                intersection = new Point((int)x, (int)y);
                return true;
            }

            return false;
        }

        public static Rectangle GetBounds(Shape s)
        {
            if (s?.Points == null || s.Points.Count == 0)
                return Rectangle.Empty;
            
            int minX = s.Points.Min(p => p.X);
            int minY = s.Points.Min(p => p.Y);
            int maxX = s.Points.Max(p => p.X);
            int maxY = s.Points.Max(p => p.Y);
            
            var rect = Rectangle.FromLTRB(minX, minY, maxX, maxY);
            rect.Inflate(6, 6);
            return rect;
        }

        public static bool IsPointInPolygon(Point point, List<Point> polygon)
        {
            if (polygon == null || polygon.Count < 3)
                return false;

            bool inside = false;
            int n = polygon.Count;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) /
                     (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public static bool IsConvexPolygon(List<Point> polygon)
        {
            if (polygon == null || polygon.Count < 3)
                return true;

            bool gotNegative = false;
            bool gotPositive = false;
            int n = polygon.Count;

            for (int i = 0; i < n; i++)
            {
                Point a = polygon[i];
                Point b = polygon[(i + 1) % n];
                Point c = polygon[(i + 2) % n];

                float crossProduct = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);

                if (crossProduct < 0) gotNegative = true;
                else if (crossProduct > 0) gotPositive = true;

                if (gotNegative && gotPositive) return false;
            }

            return true;
        }
    }
}
