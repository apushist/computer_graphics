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
    }
}
