using System;

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

        private Matrix4x4 CreateAxonometricProjection()
        {

            Matrix4x4 rotationX = Matrix4x4.CreateRotationX(RotateX * Math.PI / 180.0);
            Matrix4x4 rotationY = Matrix4x4.CreateRotationY(RotateY * Math.PI / 180.0);

            return rotationY * rotationX;
        }

        private Matrix4x4 CreatePerspectiveProjection()
        {
            double fov = FieldOfView * Math.PI / 180.0;
            double aspect = 1.0;
            double near = 1.0;
            double far = 100.0;

            double f = 1.0 / Math.Tan(fov / 2.0);

            return new Matrix4x4(new double[4, 4]
            {
                { f / aspect, 0, 0, 0 },
                { 0, f, 0, 0 },
                { 0, 0, (far + near) / (near - far), (2 * far * near) / (near - far) },
                { 0, 0, -1, 0 }
            });
        }

        public PointF ProjectTo2D(Point3D point3D, int screenWidth, int screenHeight)
        {
            Point3D transformed = new Point3D(point3D.X, point3D.Y, point3D.Z);

            Matrix4x4 rotationX = Matrix4x4.CreateRotationX(RotateX * Math.PI / 180.0);
            Matrix4x4 rotationY = Matrix4x4.CreateRotationY(RotateY * Math.PI / 180.0);
            transformed.Transform(rotationY * rotationX);

            Matrix4x4 projection = GetProjectionMatrix();
            transformed.Transform(projection);

            if (transformed.W != 0)
            {
                transformed.X /= transformed.W;
                transformed.Y /= transformed.W;
                transformed.Z /= transformed.W;
            }

            // УМЕНЬШИЛ масштаб с 200f до 80f
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
    }
}