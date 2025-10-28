using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public class Polyhedron
	{
		public List<Polygon> Faces { get; private set; }
		public string Name { get; set; }
		public Point3D Center { get; private set; }

		public Polyhedron()
		{
			Faces = new List<Polygon>();
			Center = new Point3D(0, 0, 0);
		}

		// Добавление грани
		public void AddFace(Polygon face)
		{
			Faces.Add(face);
			UpdateCenter();
		}

		// Применение матричного преобразования ко всему многограннику
		public void Transform(Matrix4x4 matrix)
		{
			foreach (var face in Faces)
			{
				face.Transform(matrix);
			}
			UpdateCenter();
		}

		// Обновление центра многогранника
		private void UpdateCenter()
		{
			if (Faces.Count == 0) return;

			double x = 0, y = 0, z = 0;
			int vertexCount = 0;

			foreach (var face in Faces)
			{
				foreach (var vertex in face.Vertices)
				{
					x += vertex.X;
					y += vertex.Y;
					z += vertex.Z;
					vertexCount++;
				}
			}

			Center = new Point3D(x / vertexCount, y / vertexCount, z / vertexCount);
		}

		// Отрисовка всего многогранника
		public void Draw(Graphics g, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix,
						int canvasWidth, int canvasHeight)
		{
			foreach (var face in Faces)
			{
				face.Draw(g, viewMatrix, projectionMatrix, canvasWidth, canvasHeight);
			}
		}

		// Генераторы правильных многогранников

		public static Polyhedron CreateTetrahedron(double size = 1.0)
		{
			var polyhedron = new Polyhedron { Name = "Тетраэдр" };

			double s = size / Math.Sqrt(2);
			var vertices = new Point3D[]
			{
			new Point3D(s, s, s),
			new Point3D(s, -s, -s),
			new Point3D(-s, s, -s),
			new Point3D(-s, -s, s)
			};

			// Грани тетраэдра
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[1], vertices[2] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[2], vertices[3] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[3], vertices[1] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[3], vertices[2] }));

			return polyhedron;
		}

		public static Polyhedron CreateHexahedron(double size = 1.0)
		{
			var polyhedron = new Polyhedron { Name = "Гексаэдр (Куб)" };

			double s = size / 2;
			var vertices = new Point3D[]
			{
			new Point3D(-s, -s, -s), // 0
            new Point3D(s, -s, -s),  // 1
            new Point3D(s, s, -s),   // 2
            new Point3D(-s, s, -s),  // 3
            new Point3D(-s, -s, s),  // 4
            new Point3D(s, -s, s),   // 5
            new Point3D(s, s, s),    // 6
            new Point3D(-s, s, s)    // 7
			};

			// Грани куба
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[1], vertices[2], vertices[3] })); // задняя
			polyhedron.AddFace(new Polygon(new[] { vertices[4], vertices[5], vertices[6], vertices[7] })); // передняя
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[1], vertices[5], vertices[4] })); // нижняя
			polyhedron.AddFace(new Polygon(new[] { vertices[2], vertices[3], vertices[7], vertices[6] })); // верхняя
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[3], vertices[7], vertices[4] })); // левая
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[2], vertices[6], vertices[5] })); // правая

			return polyhedron;
		}

		public static Polyhedron CreateOctahedron(double size = 1.0)
		{
			var polyhedron = new Polyhedron { Name = "Октаэдр" };

			double s = size;
			var vertices = new Point3D[]
			{
			new Point3D(s, 0, 0),   // 0
            new Point3D(-s, 0, 0),  // 1
            new Point3D(0, s, 0),   // 2
            new Point3D(0, -s, 0),  // 3
            new Point3D(0, 0, s),   // 4
            new Point3D(0, 0, -s)   // 5
			};

			// Грани октаэдра
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[2], vertices[4] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[4], vertices[3] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[3], vertices[5] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[5], vertices[2] }));

			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[2], vertices[5] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[5], vertices[3] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[3], vertices[4] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[4], vertices[2] }));

			return polyhedron;
		}

		public static Polyhedron CreateIcosahedron(double size = 1.0)
		{
			var polyhedron = new Polyhedron { Name = "Икосаэдр" };

			// Золотое сечение
			double phi = (1.0 + Math.Sqrt(5.0)) / 2.0;
			double s = size / (2 * Math.Sqrt(phi * Math.Sqrt(5)));

			// Вершины икосаэдра
			var vertices = new Point3D[]
			{
				// Верхняя и нижняя вершины
				new Point3D(0, s * phi, s),        // 0: верх
				new Point3D(0, s * phi, -s),       // 1: низ
        
				// Верхнее кольцо
				new Point3D(s, s, 0),              // 2
				new Point3D(-s, s, 0),             // 3
				new Point3D(s * phi, 0, s),        // 4
				new Point3D(-s * phi, 0, s),       // 5
				new Point3D(s * phi, 0, -s),       // 6
				new Point3D(-s * phi, 0, -s),      // 7
        
				// Нижнее кольцо
				new Point3D(s, -s, 0),             // 8
				new Point3D(-s, -s, 0),            // 9
				new Point3D(0, -s * phi, s),       // 10
				new Point3D(0, -s * phi, -s)       // 11
			};

			// Грани икосаэдра (20 треугольников)

			// Верхняя "шапка" - 5 треугольников вокруг вершины 0
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[2], vertices[4] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[4], vertices[5] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[5], vertices[3] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[3], vertices[6] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[0], vertices[6], vertices[2] }));

			// Средний пояс - 10 треугольников
			polyhedron.AddFace(new Polygon(new[] { vertices[2], vertices[8], vertices[4] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[4], vertices[8], vertices[10] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[4], vertices[10], vertices[5] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[5], vertices[10], vertices[9] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[5], vertices[9], vertices[3] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[3], vertices[9], vertices[7] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[3], vertices[7], vertices[6] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[6], vertices[7], vertices[11] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[6], vertices[11], vertices[2] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[2], vertices[11], vertices[8] }));

			// Нижняя "шапка" - 5 треугольников вокруг вершины 1
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[4], vertices[2] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[5], vertices[4] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[3], vertices[5] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[6], vertices[3] }));
			polyhedron.AddFace(new Polygon(new[] { vertices[1], vertices[2], vertices[6] }));

			return polyhedron;
		}
	}
}
