using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    public static class AffineTransform
    {
        public static void ApplyTranslation(Shape shape, float dx, float dy)
        {
            float[,] matrix = new float[3, 3]
            {
                {1, 0, dx},
                {0, 1, dy},
                {0, 0, 1}
            };

            for (int i = 0; i < shape.Points.Count; i++)
            {
                var p = shape.Points[i];
                float xNew = matrix[0, 0] * p.X + matrix[0, 1] * p.Y + matrix[0, 2];
                float yNew = matrix[1, 0] * p.X + matrix[1, 1] * p.Y + matrix[1, 2];
                shape.Points[i] = new Point((int)xNew, (int)yNew);
            }
        }

		private static float[,] MultiplyMatrix(float[,] a, float[,] b)
		{
			float[,] result = new float[3, 3];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						result[i, j] += a[i, k] * b[k, j];
					}
				}
			}
			return result;
		}

		public static void ApplyScale(Shape shape, Point pivotPoint, float scaleFactor)
		{
			if (shape == null || shape.Points == null || shape.Points.Count == 0) return;

			float[,] translationToOrigin = new float[3, 3]
			{
				{1, 0, -pivotPoint.X},
				{0, 1, -pivotPoint.Y},
				{0, 0, 1}
			};

			float[,] scaleMatrix = new float[3, 3]
			{
			{scaleFactor, 0, 0},
			{0, scaleFactor, 0},
			{0, 0, 1}
			};

			float[,] translationBack = new float[3, 3]
			{
				{1, 0, pivotPoint.X},
				{0, 1, pivotPoint.Y},
				{0, 0, 1}
			};

			float[,] combinedMatrix = MultiplyMatrix(translationBack, MultiplyMatrix(scaleMatrix, translationToOrigin));

			var scaledPoints = new List<Point>();

			foreach (var originalPoint in shape.ogPoints)
			{
				float xNew = combinedMatrix[0, 0] * originalPoint.X + combinedMatrix[0, 1] * originalPoint.Y + combinedMatrix[0, 2];
				float yNew = combinedMatrix[1, 0] * originalPoint.X + combinedMatrix[1, 1] * originalPoint.Y + combinedMatrix[1, 2];
				scaledPoints.Add(new Point((int)Math.Round(xNew), (int)Math.Round(yNew)));
			}
			shape.Points = scaledPoints;
		}

        public static float[,] CreateRotationMatrix(float angle, Point center)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            float centerX = center.X;
            float centerY = center.Y;

            return new float[3, 3] {
        { cos, -sin, centerX * (1 - cos) + centerY * sin },
        { sin, cos, centerY * (1 - cos) - centerX * sin },
        { 0, 0, 1 }
    };
        }

        public static Point TransformPoint(Point point, float[,] matrix)
        {
            float x = matrix[0, 0] * point.X + matrix[0, 1] * point.Y + matrix[0, 2];
            float y = matrix[1, 0] * point.X + matrix[1, 1] * point.Y + matrix[1, 2];
            return new Point((int)x, (int)y);
        }

        public static void ApplyRotation(Shape shape, float angle, Point center)
        {
            float[,] matrix = CreateRotationMatrix(angle, center);

            for (int i = 0; i < shape.Points.Count; i++)
            {
                shape.Points[i] = TransformPoint(shape.Points[i], matrix);
            }

            for (int i = 0; i < shape.ogPoints.Count; i++)
            {
                shape.ogPoints[i] = TransformPoint(shape.ogPoints[i], matrix);
            }
        }
    }
	
}
