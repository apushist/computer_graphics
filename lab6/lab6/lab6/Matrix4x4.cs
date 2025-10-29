using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class Matrix4x4
    {
        private double[,] data;

        public Matrix4x4()
        {
            data = new double[4, 4];
            MakeIdentity();
        }

        public Matrix4x4(double[,] initialData)
        {
            if (initialData.GetLength(0) != 4 || initialData.GetLength(1) != 4)
                throw new ArgumentException("Matrix must be 4x4");

            data = (double[,])initialData.Clone();
        }

        public void MakeIdentity()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    data[i, j] = (i == j) ? 1.0 : 0.0;
        }

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            Matrix4x4 result = new Matrix4x4();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result.data[i, j] = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        result.data[i, j] += a.data[i, k] * b.data[k, j];
                    }
                }
            }

            return result;
        }

        public Point3D Multiply(Point3D point)
        {
            double x = data[0, 0] * point.X + data[0, 1] * point.Y + data[0, 2] * point.Z + data[0, 3] * point.W;
            double y = data[1, 0] * point.X + data[1, 1] * point.Y + data[1, 2] * point.Z + data[1, 3] * point.W;
            double z = data[2, 0] * point.X + data[2, 1] * point.Y + data[2, 2] * point.Z + data[2, 3] * point.W;
            double w = data[3, 0] * point.X + data[3, 1] * point.Y + data[3, 2] * point.Z + data[3, 3] * point.W;

            return new Point3D(x, y, z, w);
        }

        public static Matrix4x4 CreateTranslation(double dx, double dy, double dz)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.data[0, 3] = dx;
            matrix.data[1, 3] = dy;
            matrix.data[2, 3] = dz;
            return matrix;
        }

        public static Matrix4x4 CreateScale(double sx, double sy, double sz)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.data[0, 0] = sx;
            matrix.data[1, 1] = sy;
            matrix.data[2, 2] = sz;
            return matrix;
        }

        public static Matrix4x4 CreateRotationX(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.data[1, 1] = cos;
            matrix.data[1, 2] = -sin;
            matrix.data[2, 1] = sin;
            matrix.data[2, 2] = cos;
            return matrix;
        }

        public static Matrix4x4 CreateRotationY(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.data[0, 0] = cos;
            matrix.data[0, 2] = sin;
            matrix.data[2, 0] = -sin;
            matrix.data[2, 2] = cos;
            return matrix;
        }

        public static Matrix4x4 CreateRotationZ(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.data[0, 0] = cos;
            matrix.data[0, 1] = -sin;
            matrix.data[1, 0] = sin;
            matrix.data[1, 1] = cos;
            return matrix;
        }

        public static Matrix4x4 CreateReflection(string plane)
        {
            if (string.IsNullOrWhiteSpace(plane))
                throw new ArgumentException("plane is null or empty");

            string p = plane.Trim().ToUpperInvariant();

            var matrix = new Matrix4x4();
            matrix.MakeIdentity();

            switch (p)
            {
                case "XY":
                    matrix.data[2, 2] = -1.0;
                    break;
                case "XZ":
                    matrix.data[1, 1] = -1.0;
                    break;
                case "YZ":
                    matrix.data[0, 0] = -1.0;
                    break;
                default:
                    throw new ArgumentException("Invalid plane. Use XY, XZ, or YZ");
            }

            return matrix;
        }

		public static Matrix4x4 CreateScaleAroundCenter(double sx, double sy, double sz, Point3D center)
		{
			// 1. Смещение в начало координат
			var toOrigin = CreateTranslation(-center.X, -center.Y, -center.Z);
			// 2. Масштабирование
			var scale = CreateScale(sx, sy, sz);
			// 3. Обратное смещение
			var fromOrigin = CreateTranslation(center.X, center.Y, center.Z);

			return fromOrigin * scale * toOrigin;
		}

		public static Matrix4x4 CreateRotationAroundAxis(Point3D pointA, Point3D pointB, double angle)
		{
			// Вектор оси
			Point3D axis = pointB - pointA;
			Point3D unitAxis = axis.Normalize();

			// Упрощенная реализация - поворот вокруг оси через начало координат
			double cosA = Math.Cos(angle);
			double sinA = Math.Sin(angle);
			double oneMinusCosA = 1 - cosA;

			double x = unitAxis.X;
			double y = unitAxis.Y;
			double z = unitAxis.Z;

			var rotation = new Matrix4x4();

			rotation.data[0, 0] = cosA + x * x * oneMinusCosA;
			rotation.data[0, 1] = x * y * oneMinusCosA - z * sinA;
			rotation.data[0, 2] = x * z * oneMinusCosA + y * sinA;

			rotation.data[1, 0] = y * x * oneMinusCosA + z * sinA;
			rotation.data[1, 1] = cosA + y * y * oneMinusCosA;
			rotation.data[1, 2] = y * z * oneMinusCosA - x * sinA;

			rotation.data[2, 0] = z * x * oneMinusCosA - y * sinA;
			rotation.data[2, 1] = z * y * oneMinusCosA + x * sinA;
			rotation.data[2, 2] = cosA + z * z * oneMinusCosA;

			// Комбинируем с трансляцией
			var toOrigin = CreateTranslation(-pointA.X, -pointA.Y, -pointA.Z);
			var fromOrigin = CreateTranslation(pointA.X, pointA.Y, pointA.Z);

			return fromOrigin * rotation * toOrigin;
		}
    }
}
