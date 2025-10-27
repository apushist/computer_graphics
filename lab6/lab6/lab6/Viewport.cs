using System;

namespace lab6
{
    public class Viewport
    {
        public float Scale { get; set; } = 1.0f;
        public float MinScale { get; set; } = 0.1f;
        public float MaxScale { get; set; } = 5.0f;

        public void Zoom(float delta, System.Drawing.PointF mousePosition, int screenWidth, int screenHeight)
        {
            Scale = Math.Max(MinScale, Math.Min(MaxScale, Scale * delta));
        }

        public void Reset()
        {
            Scale = 1.0f;
        }

        public System.Drawing.PointF WorldToScreen(Point3D worldPoint, Camera camera, int screenWidth, int screenHeight)
        {
            System.Drawing.PointF projected = camera.ProjectTo2D(worldPoint, screenWidth, screenHeight);

            float centerX = screenWidth / 2;
            float centerY = screenHeight / 2;

            return new System.Drawing.PointF(
                (projected.X - centerX) * Scale + centerX,
                (projected.Y - centerY) * Scale + centerY
            );
        }
    }
}