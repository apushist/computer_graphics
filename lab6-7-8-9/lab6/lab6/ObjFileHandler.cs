using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public class ObjFileHandler
	{
		public static Polyhedron LoadFromFile(string filePath)
		{
			var polyhedron = new Polyhedron();
			var vertices = new List<Point3D>();
			var faces = new List<List<int>>();

			
			var vertexNormals = new List<VertexNormal>();
			var faceNormalIndices = new List<List<int>>();

			//параметры существующие в файле, но пока что не использующиеся в нашем проекте.
			//Нужны для освещения и т п, так что пусть будут тут для следующих лаб
			var parameterVertices = new List<Point3D>();
			var textureVertices = new List<Point3D>();


			try
			{
				var lines = File.ReadAllLines(filePath);

				foreach (var line in lines)
				{
					if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
						continue;

					var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length == 0) continue;

					switch (parts[0])
					{
						case "o":
							if (parts.Length >= 2)
							{
								polyhedron.Name = string.Join(" ", parts.Skip(1));
							}
							break;
						case "vt": // texture vertex
							if (parts.Length >= 2)
							{
								double u = 0, v = 0, w = 0;
								if (parts.Length >= 2) double.TryParse(parts[1], out u);
								if (parts.Length >= 3) double.TryParse(parts[2], out v);
								if (parts.Length >= 4) double.TryParse(parts[3], out w);

								textureVertices.Add(new Point3D(u, v, w));
							}
							break;

						case "vn": // vertex normal
							if (parts.Length >= 4)
							{
								if (double.TryParse(parts[1], out double x) &&
									double.TryParse(parts[2], out double y) &&
									double.TryParse(parts[3], out double z))
								{
									vertexNormals.Add(new VertexNormal(x, y, z));
								}
							}
							break;
						case "vp": // parameter space vertex
							if (parts.Length >= 2)
							{
								double u = 0, v = 0, w = 0;
								if (parts.Length >= 2) double.TryParse(parts[1], out u);
								if (parts.Length >= 3) double.TryParse(parts[2], out v);
								if (parts.Length >= 4) double.TryParse(parts[3], out w);

								parameterVertices.Add(new Point3D(u, v, w));
							}
							break;
						case "v": // vertex
							if (parts.Length >= 4)
							{
								if (double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) &&
									double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double y) &&
									double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double z))
								{
									double w = 1.0;
									if (parts.Length >= 5)
										double.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out w);

									vertices.Add(new Point3D(x, y, z, w));
								}
							}
							break;

						case "f": // face
							if (parts.Length >= 4)
							{
								var faceIndices = new List<int>();
								var normalIndices = new List<int>();

								for (int i = 1; i < parts.Length; i++)
								{
									var vertexParts = parts[i].Split('/');
									if (vertexParts.Length >= 1 && int.TryParse(vertexParts[0], out int vertexIndex))
									{
										faceIndices.Add(vertexIndex - 1);
									}

									if (vertexParts.Length >= 3 && int.TryParse(vertexParts[2], out int normalIndex))
									{
										normalIndices.Add(normalIndex - 1);
									}
									else
									{
										normalIndices.Add(-1);
									}
								}
								if (faceIndices.Count >= 3)
								{
									faces.Add(faceIndices);
									faceNormalIndices.Add(normalIndices);
								}
							}
							break;
					}
				}

				polyhedron.Vertices = vertices;
				polyhedron.Faces = faces;
				polyhedron.Normals = vertexNormals;
				polyhedron.NormalIndices = faceNormalIndices;

				// Если нормали не были загружены из файла, вычисляем их
				if (polyhedron.Normals.Count == 0)
				{
					polyhedron.CalculateVertexNormals();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error loading OBJ file: {ex.Message}", ex);
			}

			return polyhedron;
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

				foreach (var face in polyhedron.Faces)
				{
					if (face.Count > 0)
					{
						var faceIndices = string.Join(" ", face.Select(index => (index + 1).ToString()));
						writer.WriteLine($"f {faceIndices}");
					}
				}

				foreach (var normal in polyhedron.Normals)
				{
					writer.WriteLine(string.Format(CultureInfo.InvariantCulture,
						 "vn {0:F6} {1:F6} {2:F6}", normal.X, normal.Y, normal.Z));
				}

				//сюда нужно будет добавить штуки для записи в файл других компонентов когда они будут
			}
			catch (Exception ex)
			{
				throw new Exception($"Error saving OBJ file: {ex.Message}", ex);
			}
		}

	}
}
