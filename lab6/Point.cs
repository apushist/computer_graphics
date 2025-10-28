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
		public double W { get; set; }  // Однородная координата

		public Point3D(double x, double y, double z, double w = 1.0)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		// Преобразование точки с помощью матрицы
		public Point3D Transform(Matrix4x4 matrix)
		{
			return matrix * this;
		}

		// Векторные операции

		public static Point3D operator -(Point3D a, Point3D b)
		{
			return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z, 0); // W=0 для векторов
		}

		public static Point3D operator +(Point3D a, Point3D b)
		{
			return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z, 1);
		}

		// Векторное произведение
		public static Point3D CrossProduct(Point3D a, Point3D b)
		{
			return new Point3D(
				a.Y * b.Z - a.Z * b.Y,
				a.Z * b.X - a.X * b.Z,
				a.X * b.Y - a.Y * b.X,
				0  // W=0 для векторов
			);
		}

		// Скалярное произведение
		public static double DotProduct(Point3D a, Point3D b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}

		// Длина вектора
		public double Length()
		{
			return Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		// Нормализация вектора
		public Point3D Normalize()
		{
			double length = Length();
			if (length == 0) return this;

			return new Point3D(
				X / length,
				Y / length,
				Z / length,
				W
			);
		}

		// Расстояние между точками
		public double DistanceTo(Point3D other)
		{
			double dx = X - other.X;
			double dy = Y - other.Y;
			double dz = Z - other.Z;
			return Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		public override string ToString()
		{
			return $"({X:F2}, {Y:F2}, {Z:F2}, {W:F2})";
		}
	}
}
