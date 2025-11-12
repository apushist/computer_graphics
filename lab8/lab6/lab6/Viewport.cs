namespace lab6
{
    public class Viewport
    {
        public float Scale { get; set; } = 1.0f;
        public float MinScale { get; set; } = 0.1f;
        public float MaxScale { get; set; } = 5.0f;

		private double[,] zBuffer;
		private int bufferWidth;
		private int bufferHeight;
		private bool useZBuffer = true;

		public void InitializeZBuffer(int width, int height)
		{
			bufferWidth = width;
			bufferHeight = height;
			zBuffer = new double[width, height];
			ClearZBuffer();
		}

		public void ClearZBuffer()
		{
			if (zBuffer == null) return;

			for (int x = 0; x < bufferWidth; x++)
			{
				for (int y = 0; y < bufferHeight; y++)
				{
					zBuffer[x, y] = double.MaxValue;
				}
			}
		}

		public bool TestAndSetZBuffer(int x, int y, double depth)
		{
			if (zBuffer == null || x < 0 || x >= bufferWidth || y < 0 || y >= bufferHeight)
				return false;

			if (depth < zBuffer[x, y])
			{
				zBuffer[x, y] = depth;
				return true;
			}
			return false;
		}

		public void EnableZBuffer(bool enable)
		{
			useZBuffer = enable;
		}

		public bool IsZBufferEnabled()
		{
			return useZBuffer;
		}

		public void Zoom(float delta, PointF mousePosition, int screenWidth, int screenHeight)
        {
            Scale = Math.Max(MinScale, Math.Min(MaxScale, Scale * delta));
        }

        public void Reset()
        {
            Scale = 1.0f;
        }

        public PointF WorldToScreen(Point3D worldPoint, Camera camera, int screenWidth, int screenHeight)
        {
            PointF projected = camera.ProjectTo2D(worldPoint, screenWidth, screenHeight);

            float centerX = screenWidth / 2;
            float centerY = screenHeight / 2;

            return new PointF(
                (projected.X - centerX) * Scale + centerX,
                (projected.Y - centerY) * Scale + centerY
            );
        }
    }
}