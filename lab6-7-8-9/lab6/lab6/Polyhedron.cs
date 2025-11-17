
namespace lab6
{
	public class Polyhedron
	{
		public string Name { get; set; }
		public List<Point3D> Vertices { get; set; }
		public List<List<int>> Faces { get; set; }
		public List<VertexNormal> Normals { get; set; } = new List<VertexNormal>();
		public List<List<int>> NormalIndices { get; set; } = new List<List<int>>();
		public Material Material { get; set; } = new Material();


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

		public static Polyhedron CreateTetrahedron(double size = 1.0)
		{
			var poly = new Polyhedron ();
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
				[0, 1, 2 ],
				[ 0, 2, 3 ],
				[0, 3, 1],
				[ 1, 3, 2 ]
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
				[ 0, 1, 2, 3 ],
                [ 4, 5, 6, 7 ],
                [ 0, 1, 5, 4 ], 
                [ 2, 3, 7, 6 ],
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
				[ 0, 2, 4 ],
				[ 0, 4, 3 ],
				[ 0, 3, 5 ],
				[ 0, 5, 2 ],
				[ 1, 2, 5 ],
				[ 1, 5, 3 ],
				[ 1, 3, 4 ],
				[ 1, 4, 2 ]
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
			var poly = new Polyhedron ();
			poly.Name = "Додекаэдр";

			double phi = (1 + Math.Sqrt(5)) / 2;
			double s = size ;

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
				new Point3D(-phi, 0, -1/phi) * s,  
				new Point3D(-phi, 0, 1/phi) * s,   
				new Point3D(phi, 0, -1/phi) * s,   
				new Point3D(phi, 0, 1/phi) * s     
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
					
					switch(axis)
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
		public void CalculateVertexNormals()
		{
			Normals.Clear();

			for (int i = 0; i < Vertices.Count; i++)
			{
				Normals.Add(new VertexNormal(0, 0, 0));
			}

			foreach (var face in Faces)
			{
				if (face.Count < 3) continue;

				var v0 = Vertices[face[0]];
				var v1 = Vertices[face[1]];
				var v2 = Vertices[face[2]];

				var edge1 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z, 0);
				var edge2 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z, 0);

				var faceNormal = Point3D.CrossProduct(edge1, edge2);
				double length = faceNormal.Length();
				if (length > 0)
				{
					faceNormal.X /= length;
					faceNormal.Y /= length;
					faceNormal.Z /= length;
				}

				foreach (int vertexIndex in face)
				{
					Normals[vertexIndex].X += faceNormal.X;
					Normals[vertexIndex].Y += faceNormal.Y;
					Normals[vertexIndex].Z += faceNormal.Z;
				}
			}

			foreach (var normal in Normals)
			{
				normal.Normalize();
			}
		}

		public void TransformNormals(Matrix4x4 matrix)
		{
			for (int i = 0; i < Normals.Count; i++)
			{
				Normals[i] = matrix.TransformNormal(Normals[i]);
			}
		}
	}
}
