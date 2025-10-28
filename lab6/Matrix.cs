using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public class Matrix4x4
	{
		private readonly double[,] _matrix = new double[4, 4];

		public double this[int row, int col]
		{
			get => _matrix[row, col];
			set => _matrix[row, col] = value;
		}

		// Конструкторы
		public Matrix4x4() => MakeIdentity();

		public Matrix4x4(double[,] values)
		{
			if (values.GetLength(0) != 4 || values.GetLength(1) != 4)
				throw new ArgumentException("Matrix must be 4x4");
			_matrix = values.Clone() as double[,];
		}

		// Создание единичной матрицы
		public void MakeIdentity()
		{
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					_matrix[i, j] = (i == j) ? 1.0 : 0.0;
		}

		// Умножение матриц
		public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
		{
			var result = new Matrix4x4();
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
				{
					result[i, j] = 0;
					for (int k = 0; k < 4; k++)
						result[i, j] += a[i, k] * b[k, j];
				}
			return result;
		}

		// Умножение матрицы на вектор/точку (в однородных координатах)
		public static Point3D operator *(Matrix4x4 matrix, Point3D point)
		{
			double x = point.X * matrix[0, 0] + point.Y * matrix[0, 1] +
					   point.Z * matrix[0, 2] + point.W * matrix[0, 3];
			double y = point.X * matrix[1, 0] + point.Y * matrix[1, 1] +
					   point.Z * matrix[1, 2] + point.W * matrix[1, 3];
			double z = point.X * matrix[2, 0] + point.Y * matrix[2, 1] +
					   point.Z * matrix[2, 2] + point.W * matrix[2, 3];
			double w = point.X * matrix[3, 0] + point.Y * matrix[3, 1] +
					   point.Z * matrix[3, 2] + point.W * matrix[3, 3];

			// Нормализация однородных координат
			if (w != 0 && w != 1)
			{
				x /= w;
				y /= w;
				z /= w;
				w = 1;
			}

			return new Point3D(x, y, z, w);
		}

		// Статические методы создания матриц преобразований

		public static Matrix4x4 CreateTranslation(double dx, double dy, double dz)
		{
			var matrix = new Matrix4x4();
			matrix[0, 3] = dx;
			matrix[1, 3] = dy;
			matrix[2, 3] = dz;
			return matrix;
		}

		public static Matrix4x4 CreateScale(double sx, double sy, double sz)
		{
			var matrix = new Matrix4x4();
			matrix[0, 0] = sx;
			matrix[1, 1] = sy;
			matrix[2, 2] = sz;
			return matrix;
		}

		public static Matrix4x4 CreateRotationX(double angle)
		{
			double cos = Math.Cos(angle);
			double sin = Math.Sin(angle);

			var matrix = new Matrix4x4();
			matrix[1, 1] = cos;
			matrix[1, 2] = -sin;
			matrix[2, 1] = sin;
			matrix[2, 2] = cos;
			return matrix;
		}

		public static Matrix4x4 CreateRotationY(double angle)
		{
			double cos = Math.Cos(angle);
			double sin = Math.Sin(angle);

			var matrix = new Matrix4x4();
			matrix[0, 0] = cos;
			matrix[0, 2] = sin;
			matrix[2, 0] = -sin;
			matrix[2, 2] = cos;
			return matrix;
		}

		public static Matrix4x4 CreateRotationZ(double angle)
		{
			double cos = Math.Cos(angle);
			double sin = Math.Sin(angle);

			var matrix = new Matrix4x4();
			matrix[0, 0] = cos;
			matrix[0, 1] = -sin;
			matrix[1, 0] = sin;
			matrix[1, 1] = cos;
			return matrix;
		}

		public static Matrix4x4 CreateReflection(string plane)
		{
			var matrix = new Matrix4x4();
			switch (plane.ToUpper())
			{
				case "XY": matrix[2, 2] = -1; break;  // Отражение по Z
				case "XZ": matrix[1, 1] = -1; break;  // Отражение по Y
				case "YZ": matrix[0, 0] = -1; break;  // Отражение по X
				default: throw new ArgumentException("Invalid plane");
			}
			return matrix;
		}

		// Транспонирование матрицы (полезно для нормалей)
		public Matrix4x4 Transpose()
		{
			var result = new Matrix4x4();
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					result[i, j] = _matrix[j, i];
			return result;
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

		// Поворот вокруг произвольной оси
		public static Matrix4x4 CreateRotationAroundAxis(Point3D pointA, Point3D pointB, double angle)
		{
			// Вектор оси
			Point3D axisVector = pointB - pointA;
			Point3D unitAxis = axisVector.Normalize();

			// 1. Перенос оси в начало координат
			var translation = CreateTranslation(-pointA.X, -pointA.Y, -pointA.Z);

			// 2. Совмещение оси с осью Z
			// Вращение вокруг X
			double d = Math.Sqrt(unitAxis.Y * unitAxis.Y + unitAxis.Z * unitAxis.Z);
			Matrix4x4 rotX = new Matrix4x4();
			if (d != 0)
			{
				rotX[1, 1] = unitAxis.Z / d;
				rotX[1, 2] = -unitAxis.Y / d;
				rotX[2, 1] = unitAxis.Y / d;
				rotX[2, 2] = unitAxis.Z / d;
			}

			// Вращение вокруг Y
			Matrix4x4 rotY = new Matrix4x4();
			double len = unitAxis.Length();
			if (len != 0)
			{
				rotY[0, 0] = d;
				rotY[0, 2] = unitAxis.X;
				rotY[2, 0] = -unitAxis.X;
				rotY[2, 2] = d;
			}

			// 3. Поворот вокруг Z на заданный угол
			var rotZ = CreateRotationZ(angle);

			// 4. Обратные преобразования
			var rotYInv = rotY.Transpose(); // Для ортогональных матриц обратная = транспонированная
			var rotXInv = rotX.Transpose();
			var translationInv = CreateTranslation(pointA.X, pointA.Y, pointA.Z);

			// Композиция всех преобразований
			return translationInv * rotXInv * rotYInv * rotZ * rotY * rotX * translation;
		}

	}
}
