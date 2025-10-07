using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;

namespace lab4
{
    public abstract class Shape
    {
        public List<Point> Points { get; set; } = new();
        public List<Point> ogPoints { get; set; } = new();//for correct scaling
        public Color Color { get; set; } = Color.Purple;

        public abstract void Draw(Graphics g);
        public virtual bool Contains(Point point)
        {
            const int eps = 5;
            return Points.Any(p => Math.Abs(p.X - point.X) < eps && Math.Abs(p.Y - point.Y) < eps);
        }

		public Point GetCenter()
		{
			if (Points == null || Points.Count == 0)
			{
				return Point.Empty;
			}

			double sumX = 0;
			double sumY = 0;

			foreach (Point p in Points)
			{
				sumX += p.X;
				sumY += p.Y;
			}

			int centerX = (int)(sumX / Points.Count);
			int centerY = (int)(sumY / Points.Count);

			return new Point(centerX, centerY);
		}
		
	}

    public class PointShape : Shape
    {
        public override void Draw(Graphics g)
        {
            if (Points.Count == 0) return;
            var p = Points[0];
            g.FillEllipse(Brushes.Chocolate, p.X - 2, p.Y - 2, 8, 8);
        }
    }

    public class LineShape : Shape
    {
        public override void Draw(Graphics g)
        {
            if (Points.Count < 2) return;
            g.DrawLine(new Pen(Color, 2), Points[0], Points[1]);
        }
    }

    public class PolygonShape : Shape
    {
        public override void Draw(Graphics g)
        {
            if (Points.Count == 1)
                g.FillEllipse(Brushes.Blue, Points[0].X - 2, Points[0].Y - 2, 4, 4);
            else if (Points.Count == 2)
                g.DrawLine(Pens.Blue, Points[0], Points[1]);
            else
                g.DrawPolygon(Pens.Blue, Points.ToArray());
        }

        public override bool Contains(Point p)
        {
            if (Points.Count < 3) return base.Contains(p);
            using var path = new GraphicsPath();
            path.AddPolygon(Points.ToArray());
            return path.IsVisible(p);
        }		
	}
}
