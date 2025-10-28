using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public static class Projection
	{
		// Аксонометрическая (ортографическая) проекция
		public static Matrix4x4 CreateAxonometricProjection()
		{
			// Простая ортографическая проекция - отбрасываем Z координату
			var matrix = new Matrix4x4();
			matrix[2, 2] = 0;  // Z не влияет на проекцию
			return matrix;
		}

		// Перспективная проекция
		public static Matrix4x4 CreatePerspectiveProjection(double fov = 60, double aspectRatio = 1.0,
														   double near = 0.1, double far = 100.0)
		{
			double fovRad = fov * Math.PI / 180.0;
			double tanHalfFov = Math.Tan(fovRad / 2.0);

			var matrix = new Matrix4x4();

			matrix[0, 0] = 1.0 / (aspectRatio * tanHalfFov);
			matrix[1, 1] = 1.0 / tanHalfFov;
			matrix[2, 2] = far / (far - near);
			matrix[2, 3] = -far * near / (far - near);
			matrix[3, 2] = 1.0;
			matrix[3, 3] = 0;

			return matrix;
		}

		// Упрощенная перспективная проекция (для начальной реализации)
		public static Matrix4x4 CreateSimplePerspectiveProjection(double distance = 5.0)
		{
			var matrix = new Matrix4x4();
			matrix[3, 2] = -1.0 / distance;  // Простая перспектива
			return matrix;
		}
	}
}
