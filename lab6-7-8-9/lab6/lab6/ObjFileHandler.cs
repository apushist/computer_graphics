using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;

namespace lab6
{
    public class ObjFileHandler
    {
        public static Polyhedron LoadFromFile(string filePath)
        {
            try
            {
                var vertices = new List<Point3D>();
                var textureCoords = new List<PointF>();
                var normals = new List<VertexNormal>();
                var faces = new List<List<int>>();
                var textureIndices = new List<List<int>>();
                var normalIndices = new List<List<int>>();

                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    int vertexCount = 0;
                    int texCoordCount = 0;
                    int normalCount = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                            continue;

                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 0)
                            continue;

                        switch (parts[0])
                        {
                            case "v": 
                                if (parts.Length >= 4)
                                {
                                    double x = ParseDouble(parts[1]);
                                    double y = ParseDouble(parts[2]);
                                    double z = ParseDouble(parts[3]);
                                    vertices.Add(new Point3D(x, y, z));
                                }
                                break;

                            case "vt": 
                                if (parts.Length >= 3)
                                {
                                    float u = ParseFloat(parts[1]);
                                    float v = ParseFloat(parts[2]);
                                    textureCoords.Add(new PointF(u, v));
                                }
                                break;

                            case "vn": 
                                if (parts.Length >= 4)
                                {
                                    double nx = ParseDouble(parts[1]);
                                    double ny = ParseDouble(parts[2]);
                                    double nz = ParseDouble(parts[3]);
                                    normals.Add(new VertexNormal(nx, ny, nz));
                                }
                                break;

                            case "f": 
                                if (parts.Length >= 4)
                                {
                                    var faceVertices = new List<int>();
                                    var faceTexCoords = new List<int>();
                                    var faceNormals = new List<int>();

                                    for (int i = 1; i < parts.Length; i++)
                                    {
                                        var vertexParts = parts[i].Split('/');

                                        if (vertexParts.Length > 0 && !string.IsNullOrEmpty(vertexParts[0]))
                                        {
                                            int vertexIndex = int.Parse(vertexParts[0]);
                                            faceVertices.Add(vertexIndex - 1);
                                        }

                                        if (vertexParts.Length > 1 && !string.IsNullOrEmpty(vertexParts[1]))
                                        {
                                            int texIndex = int.Parse(vertexParts[1]);
                                            faceTexCoords.Add(texIndex - 1);
                                        }

                                        if (vertexParts.Length > 2 && !string.IsNullOrEmpty(vertexParts[2]))
                                        {
                                            int normalIndex = int.Parse(vertexParts[2]);
                                            faceNormals.Add(normalIndex - 1);
                                        }
                                    }

                                    faces.Add(faceVertices);
                                    textureIndices.Add(faceTexCoords);
                                    normalIndices.Add(faceNormals);
                                }
                                break;
                        }
                    }
                }

                var polyhedron = new Polyhedron
                {
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    Vertices = vertices,
                    Faces = faces,
                    TextureCoords = textureCoords,
                    TextureIndices = textureIndices.Count > 0 ? textureIndices : null,
                    Normals = normals.Count > 0 ? normals : null,
                    NormalIndices = normalIndices.Count > 0 ? normalIndices : null
                };

                if (polyhedron.Normals == null || polyhedron.Normals.Count == 0)
                {
                    polyhedron.CalculateVertexNormals();
                }

                Console.WriteLine($"Загружено из OBJ: {polyhedron.Name}");
                Console.WriteLine($"  Вершин: {polyhedron.Vertices.Count}");
                Console.WriteLine($"  Граней: {polyhedron.Faces.Count}");
                Console.WriteLine($"  UV координат: {polyhedron.TextureCoords?.Count ?? 0}");

                return polyhedron;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки OBJ файла: {ex.Message}", ex);
            }
        }

        private static double ParseDouble(string s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        private static float ParseFloat(string s)
        {
            return float.Parse(s, CultureInfo.InvariantCulture);
        }

        public static void SaveToFile(Polyhedron polyhedron, string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

                writer.WriteLine("# Wavefront OBJ file");
                writer.WriteLine($"o {polyhedron.Name}");
                writer.WriteLine();

                foreach (var vertex in polyhedron.Vertices)
                {
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture,
                        "v {0:F6} {1:F6} {2:F6}", vertex.X, vertex.Y, vertex.Z));
                }
                writer.WriteLine();

                if (polyhedron.TextureCoords != null && polyhedron.TextureCoords.Count > 0)
                {
                    foreach (var texCoord in polyhedron.TextureCoords)
                    {
                        writer.WriteLine(string.Format(CultureInfo.InvariantCulture,
                            "vt {0:F6} {1:F6}", texCoord.X, texCoord.Y));
                    }
                    writer.WriteLine();
                }

                if (polyhedron.Normals != null && polyhedron.Normals.Count > 0)
                {
                    foreach (var normal in polyhedron.Normals)
                    {
                        writer.WriteLine(string.Format(CultureInfo.InvariantCulture,
                            "vn {0:F6} {1:F6} {2:F6}", normal.X, normal.Y, normal.Z));
                    }
                    writer.WriteLine();
                }

                for (int i = 0; i < polyhedron.Faces.Count; i++)
                {
                    var face = polyhedron.Faces[i];
                    var faceLine = new List<string>();

                    for (int j = 0; j < face.Count; j++)
                    {
                        var vertexIndex = face[j] + 1;

                        bool hasTexture = polyhedron.TextureIndices != null &&
                                         i < polyhedron.TextureIndices.Count &&
                                         j < polyhedron.TextureIndices[i].Count &&
                                         polyhedron.TextureIndices[i][j] >= 0;

                        bool hasNormal = polyhedron.NormalIndices != null &&
                                        i < polyhedron.NormalIndices.Count &&
                                        j < polyhedron.NormalIndices[i].Count &&
                                        polyhedron.NormalIndices[i][j] >= 0;

                        if (hasTexture && hasNormal)
                        {
                            var texIndex = polyhedron.TextureIndices[i][j] + 1;
                            var normalIndex = polyhedron.NormalIndices[i][j] + 1;
                            faceLine.Add($"{vertexIndex}/{texIndex}/{normalIndex}");
                        }
                        else if (hasTexture)
                        {
                            var texIndex = polyhedron.TextureIndices[i][j] + 1;
                            faceLine.Add($"{vertexIndex}/{texIndex}");
                        }
                        else if (hasNormal)
                        {
                            var normalIndex = polyhedron.NormalIndices[i][j] + 1;
                            faceLine.Add($"{vertexIndex}//{normalIndex}");
                        }
                        else
                        {
                            faceLine.Add(vertexIndex.ToString());
                        }
                    }

                    if (faceLine.Count > 0)
                    {
                        writer.WriteLine($"f {string.Join(" ", faceLine)}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения OBJ файла: {ex.Message}", ex);
            }
        }
    }
}