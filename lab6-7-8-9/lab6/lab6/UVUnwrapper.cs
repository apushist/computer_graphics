using System;
using System.Collections.Generic;
using System.Drawing;

namespace lab6
{
    public class UVUnwrapper
    {
        public static Dictionary<int, List<PointF>> CreateCubeUVMap(Polyhedron cube)
        {
            var uvMap = new Dictionary<int, List<PointF>>();

            float[][] faceUVs = new float[][]
            {
                new float[] { 0.25f, 0.333f, 0.5f, 0.333f, 0.5f, 0.667f, 0.25f, 0.667f },
                new float[] { 0.5f, 0.333f, 0.75f, 0.333f, 0.75f, 0.667f, 0.5f, 0.667f },
                new float[] { 0.75f, 0.333f, 1.0f, 0.333f, 1.0f, 0.667f, 0.75f, 0.667f },
                new float[] { 0.0f, 0.333f, 0.25f, 0.333f, 0.25f, 0.667f, 0.0f, 0.667f },
                new float[] { 0.25f, 0.0f, 0.5f, 0.0f, 0.5f, 0.333f, 0.25f, 0.333f },
                new float[] { 0.25f, 0.667f, 0.5f, 0.667f, 0.5f, 1.0f, 0.25f, 1.0f }
            };

            for (int faceIndex = 0; faceIndex < 6; faceIndex++)
            {
                var uvs = new List<PointF>();
                for (int i = 0; i < 4; i++)
                {
                    float u = faceUVs[faceIndex][i * 2];
                    float v = faceUVs[faceIndex][i * 2 + 1];
                    uvs.Add(new PointF(u, v));
                }
                uvMap[faceIndex] = uvs;
            }

            return uvMap;
        }
        public static Dictionary<int, List<PointF>> CreateTetrahedronUVMap(Polyhedron tetra)
        {
            var uvMap = new Dictionary<int, List<PointF>>();

            float[][] faceUVs = new float[][]
            {
                new float[] { 0.5f, 0.0f, 1.0f, 0.866f, 0.0f, 0.866f },
                new float[] { 0.5f, 0.0f, 1.0f, 0.866f, 0.5f, 0.866f * 2 },
                new float[] { 0.5f, 0.0f, 0.0f, 0.866f, 0.5f, 0.866f * 2 },
                new float[] { 1.0f, 0.866f, 0.0f, 0.866f, 0.5f, 0.866f * 2 }
            };

            float maxX = 1.0f;
            float maxY = 0.866f * 2;

            for (int faceIndex = 0; faceIndex < 4; faceIndex++)
            {
                var uvs = new List<PointF>();
                for (int i = 0; i < 3; i++)
                {
                    float u = faceUVs[faceIndex][i * 2] / maxX;
                    float v = faceUVs[faceIndex][i * 2 + 1] / maxY;
                    uvs.Add(new PointF(u, v));
                }
                uvMap[faceIndex] = uvs;
            }

            return uvMap;
        }
        public static Dictionary<int, List<PointF>> CreateSphericalUVMap(Polyhedron polyhedron)
        {
            var uvMap = new Dictionary<int, List<PointF>>();

            for (int faceIndex = 0; faceIndex < polyhedron.Faces.Count; faceIndex++)
            {
                var face = polyhedron.Faces[faceIndex];
                var uvs = new List<PointF>();

                foreach (int vertexIndex in face)
                {
                    var vertex = polyhedron.Vertices[vertexIndex];

                    double x = vertex.X, y = vertex.Y, z = vertex.Z;
                    double r = Math.Sqrt(x * x + y * y + z * z);

                    if (r < 1e-10)
                    {
                        uvs.Add(new PointF(0.5f, 0.5f));
                        continue;
                    }

                    double theta = Math.Atan2(z, x); 
                    double phi = Math.Asin(y / r); 

                    float u = (float)((theta + Math.PI) / (2 * Math.PI));
                    float v = (float)((phi + Math.PI / 2) / Math.PI);

                    uvs.Add(new PointF(u, v));
                }

                uvMap[faceIndex] = uvs;
            }

            return uvMap;
        }

        public static Dictionary<int, List<PointF>> CreateCylindricalUVMap(Polyhedron polyhedron)
        {
            var uvMap = new Dictionary<int, List<PointF>>();

            double minY = double.MaxValue, maxY = double.MinValue;
            foreach (var vertex in polyhedron.Vertices)
            {
                if (vertex.Y < minY) minY = vertex.Y;
                if (vertex.Y > maxY) maxY = vertex.Y;
            }
            double yRange = maxY - minY;

            for (int faceIndex = 0; faceIndex < polyhedron.Faces.Count; faceIndex++)
            {
                var face = polyhedron.Faces[faceIndex];
                var uvs = new List<PointF>();

                foreach (int vertexIndex in face)
                {
                    var vertex = polyhedron.Vertices[vertexIndex];

                    double x = vertex.X, z = vertex.Z;
                    double theta = Math.Atan2(z, x); 

                    float u = (float)((theta + Math.PI) / (2 * Math.PI));
                    float v = yRange > 0 ? (float)((vertex.Y - minY) / yRange) : 0.5f;

                    uvs.Add(new PointF(u, v));
                }

                uvMap[faceIndex] = uvs;
            }

            return uvMap;
        }

        public static Dictionary<int, List<PointF>> CreatePlanarUVMap(Polyhedron polyhedron, string plane = "XY")
        {
            var uvMap = new Dictionary<int, List<PointF>>();

            double minU = double.MaxValue, maxU = double.MinValue;
            double minV = double.MaxValue, maxV = double.MinValue;

            foreach (var vertex in polyhedron.Vertices)
            {
                double u = 0, v = 0;

                switch (plane)
                {
                    case "XY":
                        u = vertex.X; v = vertex.Y;
                        break;
                    case "XZ":
                        u = vertex.X; v = vertex.Z;
                        break;
                    case "YZ":
                        u = vertex.Y; v = vertex.Z;
                        break;
                }

                if (u < minU) minU = u;
                if (u > maxU) maxU = u;
                if (v < minV) minV = v;
                if (v > maxV) maxV = v;
            }

            double uRange = maxU - minU;
            double vRange = maxV - minV;

            for (int faceIndex = 0; faceIndex < polyhedron.Faces.Count; faceIndex++)
            {
                var face = polyhedron.Faces[faceIndex];
                var uvs = new List<PointF>();

                foreach (int vertexIndex in face)
                {
                    var vertex = polyhedron.Vertices[vertexIndex];

                    double u = 0, v = 0;
                    switch (plane)
                    {
                        case "XY":
                            u = vertex.X; v = vertex.Y;
                            break;
                        case "XZ":
                            u = vertex.X; v = vertex.Z;
                            break;
                        case "YZ":
                            u = vertex.Y; v = vertex.Z;
                            break;
                    }

                    float uvU = uRange > 0 ? (float)((u - minU) / uRange) : 0.5f;
                    float uvV = vRange > 0 ? (float)((v - minV) / vRange) : 0.5f;

                    uvs.Add(new PointF(uvU, uvV));
                }

                uvMap[faceIndex] = uvs;
            }

            return uvMap;
        }

        public static void ApplyUVMapToPolyhedron(Polyhedron poly, Dictionary<int, List<PointF>> uvMap)
        {
            if (poly.TextureCoords == null)
                poly.TextureCoords = new List<PointF>();

            if (poly.TextureIndices == null)
                poly.TextureIndices = new List<List<int>>();
            else
                poly.TextureIndices.Clear();

            for (int faceIndex = 0; faceIndex < poly.Faces.Count; faceIndex++)
            {
                if (uvMap.ContainsKey(faceIndex))
                {
                    var uvs = uvMap[faceIndex];
                    var texIndices = new List<int>();

                    foreach (var uv in uvs)
                    {
                        poly.TextureCoords.Add(uv);
                        texIndices.Add(poly.TextureCoords.Count - 1);
                    }

                    poly.TextureIndices.Add(texIndices);
                }
                else
                {
                    var texIndices = new List<int>();
                    for (int i = 0; i < poly.Faces[faceIndex].Count; i++)
                    {
                        poly.TextureCoords.Add(new PointF(0.5f, 0.5f));
                        texIndices.Add(poly.TextureCoords.Count - 1);
                    }
                    poly.TextureIndices.Add(texIndices);
                }
            }
        }

        public static Dictionary<int, List<PointF>> AutoCreateUVMap(Polyhedron poly)
        {
            if (poly.Name != null)
            {
                if (poly.Name.Contains("cube") || poly.Faces.Count == 6)
                    return CreateCubeUVMap(poly);
                if (poly.Name.Contains("tetra") || poly.Faces.Count == 4)
                    return CreateTetrahedronUVMap(poly);
                if (poly.Name.Contains("octa") || poly.Name.Contains("ico"))
                    return CreateSphericalUVMap(poly);
            }

            return CreateCylindricalUVMap(poly);
        }
    }
}