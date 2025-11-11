namespace lab6
{
    public class Camera
    {
        public enum ProjectionType
        {
            Axonometric,
            Perspective
        }

        public ProjectionType CurrentProjection { get; set; }
        public double FieldOfView { get; set; } = 80.0;

        public double RotateX { get; set; } = 30.0;
        public double RotateY { get; set; } = 45.0;

        public Camera()
        {
            CurrentProjection = ProjectionType.Axonometric;
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            return CurrentProjection switch
            {
                ProjectionType.Axonometric => CreateAxonometricProjection(),
                ProjectionType.Perspective => CreatePerspectiveProjection(),
                _ => CreateAxonometricProjection()
            };
        }

        private Matrix4x4 CreatePerspectiveProjection()
        {            
			Matrix4x4 matrix = Matrix4x4.CreatePerspectMatrix();
			Matrix4x4 rotationX = Matrix4x4.CreateRotationX(RotateX * Math.PI / 180.0);
			Matrix4x4 rotationY = Matrix4x4.CreateRotationY(RotateY * Math.PI / 180.0);

			return rotationY * rotationX * matrix;
        }

        private Matrix4x4 CreateAxonometricProjection()
        {
			Matrix4x4 rotationX = Matrix4x4.CreateRotationX(RotateX * Math.PI / 180.0);
			Matrix4x4 rotationY = Matrix4x4.CreateRotationY(RotateY * Math.PI / 180.0);
            Matrix4x4 matrix = Matrix4x4.CreateAxonMatrix();

			return rotationY * rotationX * matrix;
        }

        public PointF ProjectTo2D(Point3D point3D, int screenWidth, int screenHeight)
        {
            Point3D transformed = new Point3D(point3D.X, point3D.Y, point3D.Z);

            Matrix4x4 projection = GetProjectionMatrix();
            transformed.Transform(projection);

            if (transformed.W != 0)
            {
                transformed.X /= transformed.W;
                transformed.Y /= transformed.W;
                transformed.Z /= transformed.W;
            }

            float scale = 80f;
            float x = (float)(transformed.X * scale + screenWidth / 2);
            float y = (float)(-transformed.Y * scale + screenHeight / 2);

            return new PointF(x, y);
        }

        public void Rotate(double deltaX, double deltaY)
        {
            RotateY += deltaX * 0.5;
            RotateX += deltaY * 0.5;
        }

		public Point3D ViewDirection { get; set; } = new Point3D(0, 0, -1);

		public Matrix4x4 GetViewMatrix()
		{
			return Matrix4x4.CreateTranslation(0, 0, -5);
		}

		public double CalculateDepth(Point3D point, Matrix4x4 viewMatrix)
		{
			Point3D viewPoint = new Point3D(point.X, point.Y, point.Z);
			viewPoint.Transform(viewMatrix);
			return viewPoint.Z;
		}

		public Point3D GetViewDirection()
		{
			// Для аксонометрической проекции - фиксированное направление
			if (CurrentProjection == ProjectionType.Axonometric)
			{
				Matrix4x4 rotationX = Matrix4x4.CreateRotationX(RotateX * Math.PI / 180.0);
				Matrix4x4 rotationY = Matrix4x4.CreateRotationY(RotateY * Math.PI / 180.0);
				Point3D direction = new Point3D(0, 0, -1);
				direction.Transform(rotationY * rotationX);
				return direction;
			}
			else
			{
				// Для перспективной - направление от камеры
				return new Point3D(0, 0, -1);
			}
		}
	}
}