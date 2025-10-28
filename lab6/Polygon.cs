using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public class Polygon
	{
		public List<Point3D> Vertices { get; private set; }
		public Color FillColor { get; set; }
		public Color BorderColor { get; set; }
		public bool IsVisible { get; set; } = true;

		public Polygon()
		{
			Vertices = new List<Point3D>();
			FillColor = Color.LightBlue;
			BorderColor = Color.DarkBlue;
		}

		public Polygon(IEnumerable<Point3D> vertices) : this()
		{
			Vertices.AddRange(vertices);
		}

		// Добавление вершины
		public void AddVertex(Point3D vertex)
		{
			Vertices.Add(vertex);
		}

		// Применение матричного преобразования ко всем вершинам
		public void Transform(Matrix4x4 matrix)
		{
			for (int i = 0; i < Vertices.Count; i++)
			{
				Vertices[i] = Vertices[i].Transform(matrix);
			}
		}

		// Вычисление нормали грани (для будущих ЛР)
		public Point3D CalculateNormal()
		{
			if (Vertices.Count < 3)
				return new Point3D(0, 0, 0);

			// Векторы из первых трех точек
			Point3D v1 = Vertices[1] - Vertices[0];
			Point3D v2 = Vertices[2] - Vertices[0];

			// Векторное произведение для получения нормали
			Point3D normal = Point3D.CrossProduct(v1, v2);
			return normal.Normalize();
		}

		// Отрисовка грани на Graphics
		public void Draw(Graphics g, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix,
						int canvasWidth, int canvasHeight)
		{
			if (!IsVisible || Vertices.Count < 3) return;

			// Применяем преобразования ко всем вершинам
			var transformedPoints = new List<PointF>();
			foreach (var vertex in Vertices)
			{
				var viewPoint = vertex.Transform(viewMatrix);
				var projectedPoint = viewPoint.Transform(projectionMatrix);

				// Преобразование в координаты экрана
				float screenX = (float)(projectedPoint.X * canvasWidth / 2 + canvasWidth / 2);
				float screenY = (float)(-projectedPoint.Y * canvasHeight / 2 + canvasHeight / 2);

				transformedPoints.Add(new PointF(screenX, screenY));
			}

			// Рисуем заполненный полигон
			using (var fillBrush = new SolidBrush(FillColor))
			using (var borderPen = new Pen(BorderColor, 2))
			{
				g.FillPolygon(fillBrush, transformedPoints.ToArray());
				g.DrawPolygon(borderPen, transformedPoints.ToArray());
			}
		}

		// Получение центра грани
		public Point3D GetCenter()
		{
			double x = 0, y = 0, z = 0;
			foreach (var vertex in Vertices)
			{
				x += vertex.X;
				y += vertex.Y;
				z += vertex.Z;
			}

			int count = Vertices.Count;
			return new Point3D(x / count, y / count, z / count);
		}
	}
}
