using System.Numerics;

namespace lab6
{

    public class Matrix4x4
    {
		const int c = 10;

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
            matrix.data[0, 0] = 1;
            matrix.data[1, 1] = cos;
            matrix.data[1, 2] = -sin;
            matrix.data[2, 1] = sin;
            matrix.data[2, 2] = cos;
			matrix.data[3, 3] = 1;

			return matrix;
        }

        public static Matrix4x4 CreateRotationY(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.data[0, 0] = cos;
            matrix.data[1, 1] = 1;
            matrix.data[0, 2] = sin;
            matrix.data[2, 0] = -sin;
            matrix.data[2, 2] = cos;
            matrix.data[3, 3] = 1;
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

		public static Matrix4x4 CreateAxonMatrix()
        {
			Matrix4x4 matrix = new Matrix4x4();
			matrix.data[0, 0] = 1;
			matrix.data[1, 1] = 1;
			matrix.data[3,3] = 1;
			return matrix;
		}

		public static Matrix4x4 CreatePerspectMatrix()
		{
			Matrix4x4 matrix = new Matrix4x4();
			matrix.data[0, 0] = 1;
			matrix.data[1, 1] = 1;
			matrix.data[2, 2] = 0;
			matrix.data[3, 2] = -1.0/c;
			matrix.data[3, 3] = 1;
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

		public static Matrix4x4 CreateScaleAroundCenter(double scaleFactor, Point3D center)
		{
			var toOrigin = CreateTranslation(-center.X, -center.Y, -center.Z);
			
			var scale = CreateScale(scaleFactor, scaleFactor, scaleFactor);
		
			var fromOrigin = CreateTranslation(center.X, center.Y, center.Z);

			return fromOrigin * scale * toOrigin;
		}

		public static Matrix4x4 CreateRotationAroundAxis(Point3D pointA, Point3D pointB, double angle)
		{
			Point3D axis = pointB - pointA;
			Point3D unitAxis = axis.Normalize();

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

			var toOrigin = CreateTranslation(-pointA.X, -pointA.Y, -pointA.Z);
			var fromOrigin = CreateTranslation(pointA.X, pointA.Y, pointA.Z);

			return fromOrigin * rotation * toOrigin;
		}

        public Vector3 TransformVector(Vector3 v)
        {
            float x = (float)(v.X * data[0, 0] + v.Y * data[0, 1] + v.Z * data[0, 2] + data[0, 3]);
            float y = (float)(v.X * data[1, 0] + v.Y * data[1, 1] + v.Z * data[1, 2] + data[1, 3]);
            float z = (float)(v.X * data[2, 0] + v.Y * data[2, 1] + v.Z * data[2, 2] + data[2, 3]);
            return new Vector3(x, y, z);
        }

		public VertexNormal TransformNormal(VertexNormal normal)
		{
			double x = data[0, 0] * normal.X + data[0, 1] * normal.Y + data[0, 2] * normal.Z;
			double y = data[1, 0] * normal.X + data[1, 1] * normal.Y + data[1, 2] * normal.Z;
			double z = data[2, 0] * normal.X + data[2, 1] * normal.Y + data[2, 2] * normal.Z;

			VertexNormal result = new VertexNormal(x, y, z);
			result.Normalize();
			return result;
		}

        public double M11 { get { return data[0, 0]; } set { data[0, 0] = value; } }
        public double M12 { get { return data[0, 1]; } set { data[0, 1] = value; } }
        public double M13 { get { return data[0, 2]; } set { data[0, 2] = value; } }
        public double M14 { get { return data[0, 3]; } set { data[0, 3] = value; } }

        public double M21 { get { return data[1, 0]; } set { data[1, 0] = value; } }
        public double M22 { get { return data[1, 1]; } set { data[1, 1] = value; } }
        public double M23 { get { return data[1, 2]; } set { data[1, 2] = value; } }
        public double M24 { get { return data[1, 3]; } set { data[1, 3] = value; } }

        public double M31 { get { return data[2, 0]; } set { data[2, 0] = value; } }
        public double M32 { get { return data[2, 1]; } set { data[2, 1] = value; } }
        public double M33 { get { return data[2, 2]; } set { data[2, 2] = value; } }
        public double M34 { get { return data[2, 3]; } set { data[2, 3] = value; } }

        public double M41 { get { return data[3, 0]; } set { data[3, 0] = value; } }
        public double M42 { get { return data[3, 1]; } set { data[3, 1] = value; } }
        public double M43 { get { return data[3, 2]; } set { data[3, 2] = value; } }
        public double M44 { get { return data[3, 3]; } set { data[3, 3] = value; } }

        public static Matrix4x4 Transpose(Matrix4x4 matrix)
        {
            var result = new Matrix4x4();

            result.data[0, 0] = matrix.data[0, 0];
            result.data[0, 1] = matrix.data[1, 0];
            result.data[0, 2] = matrix.data[2, 0];
            result.data[0, 3] = matrix.data[3, 0];

            result.data[1, 0] = matrix.data[0, 1];
            result.data[1, 1] = matrix.data[1, 1];
            result.data[1, 2] = matrix.data[2, 1];
            result.data[1, 3] = matrix.data[3, 1];

            result.data[2, 0] = matrix.data[0, 2];
            result.data[2, 1] = matrix.data[1, 2];
            result.data[2, 2] = matrix.data[2, 2];
            result.data[2, 3] = matrix.data[3, 2];

            result.data[3, 0] = matrix.data[0, 3];
            result.data[3, 1] = matrix.data[1, 3];
            result.data[3, 2] = matrix.data[2, 3];
            result.data[3, 3] = matrix.data[3, 3];

            return result;
        }
    }
}
