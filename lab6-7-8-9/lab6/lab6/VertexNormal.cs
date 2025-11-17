namespace lab6
{
	public class VertexNormal
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public VertexNormal(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
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

		public static VertexNormal operator *(VertexNormal normal, double scalar)
		{
			return new VertexNormal(normal.X * scalar, normal.Y * scalar, normal.Z * scalar);
		}

		public static VertexNormal operator +(VertexNormal a, VertexNormal b)
		{
			return new VertexNormal(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
	}
}
