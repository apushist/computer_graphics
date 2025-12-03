using System.Numerics;

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
        public List<PointF> TextureCoords { get; set; } = new List<PointF>();
        public List<List<int>> TextureIndices { get; set; } = new List<List<int>>();

        public Polyhedron()
        {
            Vertices = new List<Point3D>();
            Faces = new List<List<int>>();
            Name = "Unnamed";
        }

        public void CalculateSphericalTextureCoords()
        {
            TextureCoords.Clear();

            foreach (var vertex in Vertices)
            {
                double length = Math.Sqrt(vertex.X * vertex.X + vertex.Y * vertex.Y + vertex.Z * vertex.Z);
                if (length < 1e-10)
                {
                    TextureCoords.Add(new PointF(0.5f, 0.5f));
                    continue;
                }

                double nx = vertex.X / length;
                double ny = vertex.Y / length;
                double nz = vertex.Z / length;

                float u = (float)(0.5 + Math.Atan2(nz, nx) / (2 * Math.PI));
                float v = (float)(0.5 - Math.Asin(ny) / Math.PI);

                TextureCoords.Add(new PointF(u, v));
            }
        }

        public void CalculateTriangleTextureCoords()
        {
            TextureCoords.Clear();
            TextureIndices.Clear();

            PointF[] triangleUVs = new PointF[]
            {
                new PointF(0.0f, 0.0f),
                new PointF(1.0f, 0.0f),
                new PointF(0.5f, 1.0f)
            };

            foreach (var face in Faces)
            {
                var texIndices = new List<int>();

                for (int i = 0; i < face.Count; i++)
                {
                    if (face.Count == 3)
                    {
                        int uvIndex = i % 3;
                        TextureCoords.Add(triangleUVs[uvIndex]);
                        texIndices.Add(TextureCoords.Count - 1);
                    }
                    else if (face.Count == 4)
                    {
                        float[][] quadUVs = new float[][]
                        {
                            new float[] { 0.0f, 0.0f },
                            new float[] { 1.0f, 0.0f },
                            new float[] { 1.0f, 1.0f },
                            new float[] { 0.0f, 1.0f }
                        };

                        TextureCoords.Add(new PointF(quadUVs[i % 4][0], quadUVs[i % 4][1]));
                        texIndices.Add(TextureCoords.Count - 1);
                    }
                    else
                    {
                        var vertex = Vertices[face[i]];
                        double length = Math.Sqrt(vertex.X * vertex.X + vertex.Y * vertex.Y + vertex.Z * vertex.Z);

                        if (length < 1e-10)
                        {
                            TextureCoords.Add(new PointF(0.5f, 0.5f));
                        }
                        else
                        {
                            double nx = vertex.X / length;
                            double ny = vertex.Y / length;
                            double nz = vertex.Z / length;

                            float u = (float)(0.5 + Math.Atan2(nz, nx) / (2 * Math.PI));
                            float v = (float)(0.5 - Math.Asin(ny) / Math.PI);
                            TextureCoords.Add(new PointF(u, v));
                        }
                        texIndices.Add(TextureCoords.Count - 1);
                    }
                }

                TextureIndices.Add(texIndices);
            }
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
            var poly = new Polyhedron();
            poly.Name = "Тетраэдр";
            double s = size;

            poly.Vertices.AddRange(
            [
                new Point3D(s, s, s),
                new Point3D(-s, -s, s),
                new Point3D(-s, s, -s),
                new Point3D(s, -s, -s)
            ]);

            poly.Faces.AddRange(
            [
                new List<int> { 0, 2, 1 },
                new List<int> { 0, 1, 3 },
                new List<int> { 0, 3, 2 },
                new List<int> { 1, 2, 3 }
            ]);

            poly.TextureCoords = new List<PointF>();
            poly.TextureIndices = new List<List<int>>();

            PointF[] triangleUVs = new PointF[]
            {
                new PointF(0.0f, 0.0f),
                new PointF(1.0f, 0.0f),
                new PointF(0.5f, 1.0f)
            };

            for (int faceIndex = 0; faceIndex < 4; faceIndex++)
            {
                var texIndices = new List<int>();

                for (int v = 0; v < 3; v++)
                {
                    poly.TextureCoords.Add(triangleUVs[v]);
                    texIndices.Add(poly.TextureCoords.Count - 1);
                }

                poly.TextureIndices.Add(texIndices);
            }

            poly.CalculateVertexNormals();
            return poly;
        }

        public static Polyhedron CreateHexahedron(double size = 1.0)
        {
            var poly = new Polyhedron();
            poly.Name = "Гексаэдр";
            double s = size;

            poly.Vertices.AddRange(
            [
                new Point3D(-s, -s, s),
                new Point3D(-s, s, s),
                new Point3D(s, -s, s),
                new Point3D(s, s, s),
                new Point3D(-s, -s, -s),
                new Point3D(-s, s, -s),
                new Point3D(s, -s, -s),
                new Point3D(s, s, -s)
            ]);

            poly.Faces.AddRange(
            [
                new List<int> { 0, 1, 3, 2 },
                new List<int> { 2, 3, 7, 6 },
                new List<int> { 6, 7, 5, 4 },
                new List<int> { 4, 5, 1, 0 },
                new List<int> { 1, 5, 7, 3 },
                new List<int> { 4, 0, 2, 6 }
            ]);

            poly.TextureCoords = new List<PointF>();

            poly.TextureCoords.Add(new PointF(0.25f, 0.75f));
            poly.TextureCoords.Add(new PointF(0.25f, 0.50f));
            poly.TextureCoords.Add(new PointF(0.50f, 0.75f));
            poly.TextureCoords.Add(new PointF(0.50f, 0.50f));
            poly.TextureCoords.Add(new PointF(0.00f, 0.75f));
            poly.TextureCoords.Add(new PointF(0.00f, 0.50f));
            poly.TextureCoords.Add(new PointF(0.75f, 0.75f));
            poly.TextureCoords.Add(new PointF(0.75f, 0.50f));

            poly.TextureIndices = new List<List<int>>
            {
                new List<int> { 0, 1, 3, 2 },
                new List<int> { 2, 3, 7, 6 },
                new List<int> { 6, 7, 5, 4 },
                new List<int> { 4, 5, 1, 0 },
                new List<int> { 1, 5, 7, 3 },
                new List<int> { 4, 0, 2, 6 }
            };

            poly.CalculateVertexNormals();
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
                new List<int> { 0, 2, 4 },
                new List<int> { 0, 4, 3 },
                new List<int> { 0, 3, 5 },
                new List<int> { 0, 5, 2 },
                new List<int> { 1, 2, 5 },
                new List<int> { 1, 5, 3 },
                new List<int> { 1, 3, 4 },
                new List<int> { 1, 4, 2 }
            ]);

            poly.TextureCoords = new List<PointF>();
            poly.TextureIndices = new List<List<int>>();

            PointF[] triangleUVs = new PointF[]
            {
                new PointF(0.0f, 0.0f),
                new PointF(1.0f, 0.0f),
                new PointF(0.5f, 1.0f)
            };

            for (int faceIndex = 0; faceIndex < 8; faceIndex++)
            {
                var texIndices = new List<int>();

                for (int v = 0; v < 3; v++)
                {
                    poly.TextureCoords.Add(triangleUVs[v]);
                    texIndices.Add(poly.TextureCoords.Count - 1);
                }

                poly.TextureIndices.Add(texIndices);
            }

            poly.CalculateVertexNormals();
            return poly;
        }

        public static Polyhedron CreateIcosahedron(double size = 1.0)
        {
            var poly = new Polyhedron();
            poly.Name = "Икосаэдр";
            double s = size;

            double phi = (1.0 + Math.Sqrt(5.0)) / 2.0;
            double a = s / Math.Sqrt(1.0 + phi * phi);
            double b = a * phi;

            poly.Vertices.AddRange(
            [
                new Point3D(0, b, a),
                new Point3D(0, b, -a),
                new Point3D(0, -b, a),
                new Point3D(0, -b, -a),
                new Point3D(a, 0, b),
                new Point3D(-a, 0, b),
                new Point3D(a, 0, -b),
                new Point3D(-a, 0, -b),
                new Point3D(b, a, 0),
                new Point3D(-b, a, 0),
                new Point3D(b, -a, 0),
                new Point3D(-b, -a, 0)
            ]);

            poly.Faces.AddRange(
            [
                new List<int> { 0, 4, 8 },
                new List<int> { 0, 8, 1 },
                new List<int> { 0, 1, 9 },
                new List<int> { 0, 9, 5 },
                new List<int> { 0, 5, 4 },
                new List<int> { 4, 10, 8 },
                new List<int> { 8, 10, 6 },
                new List<int> { 8, 6, 1 },
                new List<int> { 1, 6, 7 },
                new List<int> { 1, 7, 9 },
                new List<int> { 9, 7, 11 },
                new List<int> { 9, 11, 5 },
                new List<int> { 5, 11, 2 },
                new List<int> { 5, 2, 4 },
                new List<int> { 4, 2, 10 },
                new List<int> { 3, 6, 10 },
                new List<int> { 3, 10, 2 },
                new List<int> { 3, 2, 11 },
                new List<int> { 3, 11, 7 },
                new List<int> { 3, 7, 6 }
            ]);

            poly.TextureCoords = new List<PointF>();
            poly.TextureIndices = new List<List<int>>();

            PointF[] triangleUVs = new PointF[]
            {
                new PointF(0.0f, 0.0f),
                new PointF(1.0f, 0.0f),
                new PointF(0.5f, 1.0f)
            };

            for (int faceIndex = 0; faceIndex < 20; faceIndex++)
            {
                var texIndices = new List<int>();

                for (int v = 0; v < 3; v++)
                {
                    poly.TextureCoords.Add(triangleUVs[v]);
                    texIndices.Add(poly.TextureCoords.Count - 1);
                }

                poly.TextureIndices.Add(texIndices);
            }

            poly.CalculateVertexNormals();
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
                new Point3D(-1, -1, -1) * s,
                new Point3D(-1, -1, 1) * s,
                new Point3D(-1, 1, -1) * s,
                new Point3D(-1, 1, 1) * s,
                new Point3D(1, -1, -1) * s,
                new Point3D(1, -1, 1) * s,
                new Point3D(1, 1, -1) * s,
                new Point3D(1, 1, 1) * s,
                new Point3D(0, -1/phi, -phi) * s,
                new Point3D(0, -1/phi, phi) * s,
                new Point3D(0, 1/phi, -phi) * s,
                new Point3D(0, 1/phi, phi) * s,
                new Point3D(-1/phi, -phi, 0) * s,
                new Point3D(-1/phi, phi, 0) * s,
                new Point3D(1/phi, -phi, 0) * s,
                new Point3D(1/phi, phi, 0) * s,
                new Point3D(-phi, 0, -1/phi) * s,
                new Point3D(-phi, 0, 1/phi) * s,
                new Point3D(phi, 0, -1/phi) * s,
                new Point3D(phi, 0, 1/phi) * s
            ]);

            poly.Faces.AddRange(
            [
                new List<int> { 0, 8, 10, 2, 16 },
                new List<int> { 0, 12, 14, 4, 8 },
                new List<int> { 0, 16, 17, 1, 12 },
                new List<int> { 1, 17, 3, 11, 9 },
                new List<int> { 1, 9, 5, 14, 12 },
                new List<int> { 2, 10, 6, 15, 13 },
                new List<int> { 2, 16, 17, 3, 13 },
                new List<int> { 3, 13, 15, 7, 11 },
                new List<int> { 4, 8, 10, 6, 18 },
                new List<int> { 4, 18, 19, 5, 14 },
                new List<int> { 5, 19, 7, 11, 9 },
                new List<int> { 6, 18, 19, 7, 15 }
            ]);

            poly.CalculateSphericalTextureCoords();

            poly.TextureIndices.Clear();
            foreach (var face in poly.Faces)
            {
                var texIndices = new List<int>();
                for (int i = 0; i < face.Count; i++)
                {
                    texIndices.Add(face[i]);
                }
                poly.TextureIndices.Add(texIndices);
            }

            poly.CalculateVertexNormals();
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

            poly.CalculateTriangleTextureCoords();
            poly.CalculateVertexNormals();

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

            poly.CalculateTriangleTextureCoords();
            poly.CalculateVertexNormals();

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

                var edge1 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
                var edge2 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);

                var faceNormal = Point3D.CrossProduct(edge1, edge2);
                faceNormal.Normalize();

                Point3D faceCenter = new Point3D(0, 0, 0);
                foreach (int vertexIndex in face)
                {
                    faceCenter.X += Vertices[vertexIndex].X;
                    faceCenter.Y += Vertices[vertexIndex].Y;
                    faceCenter.Z += Vertices[vertexIndex].Z;
                }
                faceCenter.X /= face.Count;
                faceCenter.Y /= face.Count;
                faceCenter.Z /= face.Count;

                double dot = faceNormal.X * faceCenter.X + faceNormal.Y * faceCenter.Y + faceNormal.Z * faceCenter.Z;
                if (dot > 0)
                {
                    faceNormal.X = -faceNormal.X;
                    faceNormal.Y = -faceNormal.Y;
                    faceNormal.Z = -faceNormal.Z;
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