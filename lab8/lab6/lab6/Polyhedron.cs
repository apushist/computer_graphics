using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace lab6
{
	public class Polyhedron
	{
		public string Name { get; set; }
		public List<Point3D> Vertices { get; set; }
		public List<List<int>> Faces { get; set; }
		public Color Color { get; set; } = Color.Black;
		public bool IsVisible { get; set; } = true;

		public Polyhedron()
		{
			Vertices = new List<Point3D>();
			Faces = new List<List<int>>();
			Name = "Unnamed";
		}

		public Point3D GetCenter()
		{
			if (Vertices.Count == 0) return new Point3D(0, 0, 0);

			double x = 0, y = 0, z = 0;
			foreach (var vertex in Vertices)
			{
				x += vertex.X;
				y += vertex.Y;
				z += vertex.Z;
			}

			return new Point3D(x / Vertices.Count, y / Vertices.Count, z / Vertices.Count);
		}

		public void Transform(Matrix4x4 matrix)
		{
			foreach (var vertex in Vertices)
			{
				vertex.Transform(matrix);
			}
		}

		public Point3D CalculateFaceNormal(List<int> face)
		{
			if (face.Count < 3)
				return new Point3D(0, 0, 0);

			Point3D v1 = Vertices[face[0]];
			Point3D v2 = Vertices[face[1]];
			Point3D v3 = Vertices[face[2]];

			Point3D u = v2 - v1;
			Point3D v = v3 - v1;

			return Point3D.CrossProduct(u, v).Normalize();
		}

		public bool IsFaceVisible(List<int> face, Point3D viewDirection)
		{
			Point3D normal = CalculateFaceNormal(face);
			return Point3D.DotProduct(normal, viewDirection) < 0;
		}

		public Polyhedron Clone()
		{
			var cloned = new Polyhedron
			{
				Name = this.Name + " (копия)",
				Color = this.Color,
				IsVisible = this.IsVisible,
				Vertices = new List<Point3D>(),
				Faces = new List<List<int>>()
			};

			// Копируем вершины
			foreach (var vertex in this.Vertices)
			{
				cloned.Vertices.Add(new Point3D(vertex.X, vertex.Y, vertex.Z, vertex.W));
			}

			// Копируем грани
			foreach (var face in this.Faces)
			{
				cloned.Faces.Add(new List<int>(face));
			}

			return cloned;
		}

		public void DrawWithZBuffer(Graphics g, Camera camera, Viewport viewport, int screenWidth, int screenHeight)
		{
			if (Vertices.Count == 0 || Faces.Count == 0 || !IsVisible) return;

			var viewMatrix = camera.GetViewMatrix();
			var viewDirection = camera.GetViewDirection();

			// Преобразуем все вершины в экранные координаты и вычисляем глубину
			var screenPoints = new PointF[Vertices.Count];
			var depths = new double[Vertices.Count];

			for (int i = 0; i < Vertices.Count; i++)
			{
				screenPoints[i] = viewport.WorldToScreen(Vertices[i], camera, screenWidth, screenHeight);
				depths[i] = camera.CalculateDepth(Vertices[i], viewMatrix);
			}

			// Рисуем грани с использованием Z-буфера
			using (var brush = new SolidBrush(Color.FromArgb(100, Color)))
			using (var pen = new Pen(Color, 2))
			{
				foreach (var face in Faces)
				{
					if (face.Count < 3) continue;

					// Backface culling
					if (!IsFaceVisible(face, viewDirection))
						continue;

					// Создаем полигон для текущей грани
					var facePoints = new PointF[face.Count];
					for (int i = 0; i < face.Count; i++)
					{
						facePoints[i] = screenPoints[face[i]];
					}

					// Закрашиваем полигон с использованием Z-буфера
					FillPolygonWithZBuffer(g, facePoints, face, depths, brush, viewport, screenWidth, screenHeight);

					// Рисуем контур
					g.DrawPolygon(pen, facePoints);
				}
			}
		}

		private void FillPolygonWithZBuffer(Graphics g, PointF[] points, List<int> face, double[] depths, Brush brush, Viewport viewport, int screenWidth, int screenHeight)
		{
			if (!viewport.IsZBufferEnabled()) return;

			// Находим ограничивающий прямоугольник
			float minX = Math.Max(0, points.Min(p => p.X));
			float maxX = Math.Min(screenWidth - 1, points.Max(p => p.X));
			float minY = Math.Max(0, points.Min(p => p.Y));
			float maxY = Math.Min(screenHeight - 1, points.Max(p => p.Y));

			// Простой алгоритм закраски с Z-буфером
			for (int y = (int)minY; y <= maxY; y++)
			{
				for (int x = (int)minX; x <= maxX; x++)
				{
					if (IsPointInPolygon(x, y, points))
					{
						// Вычисляем глубину для текущей точки
						double depth = CalculateDepthAtPoint(x, y, points, face, depths);

						if (viewport.TestAndSetZBuffer(x, y, depth))
						{
							// Точка видима - рисуем её
							using (var pixelBrush = new SolidBrush(((SolidBrush)brush).Color))
							{
								g.FillRectangle(pixelBrush, x, y, 1, 1);
							}
						}
					}
				}
			}
		}

		private bool IsPointInPolygon(int x, int y, PointF[] polygon)
		{
			int n = polygon.Length;
			bool inside = false;
			for (int i = 0, j = n - 1; i < n; j = i++)
			{
				if (((polygon[i].Y > y) != (polygon[j].Y > y)) &&
					(x < (polygon[j].X - polygon[i].X) * (y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
				{
					inside = !inside;
				}
			}
			return inside;
		}

		private double CalculateDepthAtPoint(int x, int y, PointF[] points, List<int> face, double[] depths)
		{
			// Упрощенная интерполяция глубины - используем среднюю глубину вершин грани
			double totalDepth = 0;
			foreach (int vertexIndex in face)
			{
				totalDepth += depths[vertexIndex];
			}
			return totalDepth / face.Count;
		}

		public static Polyhedron CreateTetrahedron(double size = 1.0)
		{
			var poly = new Polyhedron();
			poly.Name = "Тетраэдр";
			double s = size;

			poly.Vertices.AddRange(
			[
				new Point3D(s, s, s),
				new Point3D(s, -s, -s),
				new Point3D(-s, s, -s),
				new Point3D(-s, -s, s)
			]);

			poly.Faces.AddRange(
			[
				[0, 1, 2],
				[0, 2, 3],
				[0, 3, 1],
				[1, 3, 2]
			]);

			return poly;
		}

		public static Polyhedron CreateHexahedron(double size = 1.0)
		{
			var poly = new Polyhedron();
			poly.Name = "Гексаэдр";

			double s = size;

			poly.Vertices.AddRange(
			[
				new Point3D(-s, -s, -s),
				new Point3D(s, -s, -s),
				new Point3D(s, s, -s),
				new Point3D(-s, s, -s),
				new Point3D(-s, -s, s),
				new Point3D(s, -s, s),
				new Point3D(s, s, s),
				new Point3D(-s, s, s)
			]);

			poly.Faces.AddRange(
			[
				[0, 1, 2, 3],
				[4, 5, 6, 7],
				[0, 1, 5, 4],
				[2, 3, 7, 6],
				[0, 3, 7, 4],
				[1, 2, 6, 5]
			]);

			return poly;
		}

		public static Polyhedron CreateOctahedron(double size = 1.0)
		{
			var poly = new Polyhedron();
			poly.Name = "Октаэдр";

			double s = size;

			poly.Vertices.AddRange(
			[
				new Point3D(s, 0, 0),
				new Point3D(-s, 0, 0),
				new Point3D(0, s, 0),
				new Point3D(0, -s, 0),
				new Point3D(0, 0, s),
				new Point3D(0, 0, -s)
			]);

			poly.Faces.AddRange(
			[
				[0, 2, 4],
				[0, 4, 3],
				[0, 3, 5],
				[0, 5, 2],
				[1, 2, 5],
				[1, 5, 3],
				[1, 3, 4],
				[1, 4, 2]
			]);

			return poly;
		}

		public static Polyhedron CreateIcosahedron(double size = 1.0)
		{
			var poly = new Polyhedron();
			poly.Name = "Икосаэдр";

			double s = size;
			// Золотое сечение
			double phi = (1.0 + Math.Sqrt(5.0)) / 2.0;
			double a = s / Math.Sqrt(1.0 + phi * phi);
			double b = a * phi;

			poly.Vertices.AddRange(
			[
				new Point3D(0, b, a),    // 0
                new Point3D(0, b, -a),   // 1
                new Point3D(0, -b, a),   // 2
                new Point3D(0, -b, -a),  // 3
        
                new Point3D(a, 0, b),    // 4
                new Point3D(-a, 0, b),   // 5
                new Point3D(a, 0, -b),   // 6
                new Point3D(-a, 0, -b),  // 7
        
                new Point3D(b, a, 0),    // 8
                new Point3D(-b, a, 0),   // 9
                new Point3D(b, -a, 0),   // 10
                new Point3D(-b, -a, 0)   // 11
            ]);

			poly.Faces.AddRange(
			[
				[0, 4, 8],
				[0, 8, 1],
				[0, 1, 9],
				[0, 9, 5],
				[0, 5, 4],

				[4, 10, 8],
				[8, 10, 6],
				[8, 6, 1],
				[1, 6, 7],
				[1, 7, 9],
				[9, 7, 11],
				[9, 11, 5],
				[5, 11, 2],
				[5, 2, 4],
				[4, 2, 10],

				[3, 6, 10],
				[3, 10, 2],
				[3, 2, 11],
				[3, 11, 7],
				[3, 7, 6]
			]);
			return poly;
		}

		public static Polyhedron CreateDodecaedr(double size = 1.0)
		{
			var poly = new Polyhedron();
			poly.Name = "Додекаэдр";

			double phi = (1 + Math.Sqrt(5)) / 2;
			double s = size;

			poly.Vertices.AddRange(
			[
                // (±1, ±1, ±1)
                new Point3D(-1, -1, -1) * s,  // 0
                new Point3D(-1, -1, 1) * s,   // 1
                new Point3D(-1, 1, -1) * s,   // 2
                new Point3D(-1, 1, 1) * s,    // 3
                new Point3D(1, -1, -1) * s,   // 4
                new Point3D(1, -1, 1) * s,    // 5
                new Point3D(1, 1, -1) * s,    // 6
                new Point3D(1, 1, 1) * s,     // 7
        
                // (0, ±1/φ, ±φ)
                new Point3D(0, -1/phi, -phi) * s,  // 8
                new Point3D(0, -1/phi, phi) * s,   // 9
                new Point3D(0, 1/phi, -phi) * s,   // 10
                new Point3D(0, 1/phi, phi) * s,    // 11
        
                // (±1/φ, ±φ, 0)
                new Point3D(-1/phi, -phi, 0) * s,  // 12
                new Point3D(-1/phi, phi, 0) * s,   // 13
                new Point3D(1/phi, -phi, 0) * s,   // 14
                new Point3D(1/phi, phi, 0) * s,    // 15
        
                // (±φ, 0, ±1/φ)
                new Point3D(-phi, 0, -1/phi) * s,  // 16
                new Point3D(-phi, 0, 1/phi) * s,   // 17
                new Point3D(phi, 0, -1/phi) * s,   // 18
                new Point3D(phi, 0, 1/phi) * s     // 19
            ]);

			poly.Faces.AddRange(
			[
				[0, 8, 10, 2, 16],
				[0, 12, 14, 4, 8],
				[0, 16, 17, 1, 12],
				[1, 17, 3, 11, 9],
				[1, 9, 5, 14, 12],
				[2, 10, 6, 15, 13],
				[2, 16, 17, 3, 13],
				[3, 13, 15, 7, 11],
				[4, 8, 10, 6, 18],
				[4, 18, 19, 5, 14],
				[5, 19, 7, 11, 9],
				[6, 18, 19, 7, 15]
			]);
			return poly;
		}

		public static Polyhedron CreateFigOfRevolution(List<Point3D> generatrix, char axis, int segments)
		{
			var poly = new Polyhedron();
			double angleStep = 2 * Math.PI / segments;

			for (int i = 0; i < segments; i++)
			{
				double angle = i * angleStep;
				double cosA = Math.Cos(angle);
				double sinA = Math.Sin(angle);

				foreach (var p in generatrix)
				{
					double x = p.X, y = p.Y, z = p.Z;

					switch (axis)
					{
						case 'X':
							poly.Vertices.Add(new Point3D(
								x,
								y * cosA - z * sinA,
								y * sinA + z * cosA
							));
							break;
						case 'Y':
							poly.Vertices.Add(new Point3D(
								x * cosA + z * sinA,
								y,
								-x * sinA + z * cosA
							));
							break;
						case 'Z':
							poly.Vertices.Add(new Point3D(
								x * cosA - y * sinA,
								x * sinA + y * cosA,
								z
							));
							break;
					}
				}
			}

			int m = generatrix.Count;
			for (int i = 0; i < segments; i++)
			{
				int next = (i + 1) % segments;
				for (int j = 0; j < m - 1; j++)
				{
					poly.Faces.Add(new List<int>
					{
						i * m + j,
						next * m + j,
						next * m + (j + 1),
						i * m + (j + 1)
					});
				}
			}

			return poly;
		}

		public static Polyhedron CreateFunctionSurface(Func<double, double, double> function,
			double xMin, double xMax, double yMin, double yMax, int steps)
		{
			var poly = new Polyhedron();
			var vertices = new List<Point3D>();
			var faces = new List<List<int>>();

			double xStep = (xMax - xMin) / steps;
			double yStep = (yMax - yMin) / steps;

			for (int i = 0; i <= steps; i++)
			{
				for (int j = 0; j <= steps; j++)
				{
					double x = xMin + i * xStep;
					double y = yMin + j * yStep;
					double z = function(x, y);
					vertices.Add(new Point3D(x, y, z));
				}
			}

			for (int i = 0; i < steps; i++)
			{
				for (int j = 0; j < steps; j++)
				{
					int v1 = i * (steps + 1) + j;
					int v2 = v1 + 1;
					int v3 = (i + 1) * (steps + 1) + j + 1;
					int v4 = (i + 1) * (steps + 1) + j;

					faces.Add(new List<int> { v1, v2, v3 });
					faces.Add(new List<int> { v1, v3, v4 });
				}
			}

			poly.Vertices = vertices;
			poly.Faces = faces;
			return poly;
		}
	}
}