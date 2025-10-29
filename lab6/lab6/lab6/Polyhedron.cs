﻿
namespace lab6
{
	public class Polyhedron
	{
		public List<Point3D> Vertices { get; private set; }
		public List<List<int>> Faces { get; private set; }

		public Polyhedron()
		{
			Vertices = new List<Point3D>();
			Faces = new List<List<int>>();
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
	}
}
