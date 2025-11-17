using System.Numerics;

namespace lab6
{
	public class LightSource
	{
		public Point3D Position { get; set; } = new Point3D(3,3,3);
		public Color Color { get; set; } = Color.White;
		public float Intensity { get; set; } = 1.0f;

		public Vector3 GetDirectionTo(Point3D point)
		{
			Vector3 direction = new Vector3(
				(float)(Position.X - point.X),
				(float)(Position.Y - point.Y),
				(float)(Position.Z - point.Z)
			);
			return Vector3.Normalize(direction);
		}
	}
}
