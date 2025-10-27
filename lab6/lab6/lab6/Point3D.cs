using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Point3D(double x, double y, double z, double w = 1.0)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public void Transform(Matrix4x4 matrix)
        {
            Point3D result = matrix.Multiply(this);
            X = result.X;
            Y = result.Y;
            Z = result.Z;
            W = result.W;

            if (W != 0 && W != 1)
            {
                X /= W;
                Y /= W;
                Z /= W;
                W = 1.0;
            }
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z, 0);
        }

        public static Point3D CrossProduct(Point3D a, Point3D b)
        {
            return new Point3D(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X,
                0
            );
        }

        public static double DotProduct(Point3D a, Point3D b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public void Normalize()
        {
            double length = Math.Sqrt(X * X + Y * Y + Z * Z);
            if (length > 0)
            {
                X /= length;
                Y /= length;
                Z /= length;
            }
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public override string ToString()
        {
            return string.Format("({0:F2}, {1:F2}, {2:F2}, {3:F2})", X, Y, Z, W);
        }
    }
}
