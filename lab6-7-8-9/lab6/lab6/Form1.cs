using System.Numerics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lab6
{
    public partial class Form1 : Form
    {
        private enum shadingState
        {
            off = 0,
            guro,
            phong
        }

        private Camera camera = new Camera();
        private Viewport viewport = new Viewport();
        private List<Point3D> points = new List<Point3D>();
        private bool isRotatingCamera = false;
        private bool isRotatingObject = false;
        private Point lastMousePosition;
        private List<Polyhedron> polyhedrons = new List<Polyhedron>();
        public Polyhedron currentPolyhedron;
        private Matrix4x4 objectRotation = new Matrix4x4();
        private string chosenOption = "XY";
        private double dx = 1;
        private double dy = 1;
        private double dz = 1;
        public PictureBox MainPictureBox => pictureBox1;
        private ZBuffer zBuffer;
        private bool backfaceCullingEnabled = true;
        private bool zBufferEnabled = false;
        private LightSource lightSource = new LightSource();
        private shadingState currShadingState = shadingState.off;
        private bool isTexturingEnabled = false;
        private Texture currentTexture;
        private bool usingSecondPolyhedron;

        public Point3D AxisPointA { get; set; } = null;
        public Point3D AxisPointB { get; set; } = null;

        public Form1()
        {
            InitializeComponent();
            InitializePoints();
            this.Resize += (s, e) => pictureBox1.Invalidate();
            comboBoxReflection.Items.Clear();
            comboBoxReflection.Items.AddRange(["XY", "XZ", "YZ"]);
            comboBoxReflection.SelectedIndex = 0;
            comboBoxShading.SelectedIndex = 0;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.Clear(Color.White);

            DrawCoordinateAxes(e.Graphics);
            if (currentPolyhedron != null)
            {
                DrawPolyherdon(e.Graphics, pictureBox1.Width, pictureBox1.Height);
            }

            DrawInfo(e.Graphics);

            if (AxisPointA != null && AxisPointB != null)
            {
                PointF screenA = viewport.WorldToScreen(AxisPointA, camera, pictureBox1.Width, pictureBox1.Height);
                PointF screenB = viewport.WorldToScreen(AxisPointB, camera, pictureBox1.Width, pictureBox1.Height);

                using Pen pen = new Pen(Color.Orange, 2);
                e.Graphics.DrawLine(pen, screenA, screenB);
            }
        }

        private void DrawPolyherdon(Graphics g, int screenWidth, int screenHeight)
        {
            if (currentPolyhedron == null || currentPolyhedron.Vertices.Count == 0 || currentPolyhedron.Faces.Count == 0)
                return;

            var polyhedronsToDraw = new List<Polyhedron> { currentPolyhedron };

            if (usingSecondPolyhedron)
            {
                var secondPolyhedron = Polyhedron.CreateIcosahedron();
                secondPolyhedron.Material.DiffuseColor = Color.Blue;
                var translation = Matrix4x4.CreateTranslation(3, 0, 0);
                secondPolyhedron.Transform(translation);
                polyhedronsToDraw.Add(secondPolyhedron);
            }

            if (zBufferEnabled)
            {
                if (zBuffer == null || screenWidth != zBuffer.Width || screenHeight != zBuffer.Height)
                {
                    zBuffer = new ZBuffer(screenWidth, screenHeight);
                }
                zBuffer.Clear();
            }

            float maxCoord = Math.Max(screenWidth, screenHeight) * 2f;

            foreach (var polyhedron in polyhedronsToDraw)
            {
                if (polyhedron.Vertices.Count == 0 || polyhedron.Faces.Count == 0)
                    continue;

                using (Pen pen = new Pen(polyhedron.Material.DiffuseColor, 2))
                {
                    Matrix4x4 rotX = Matrix4x4.CreateRotationX(camera.RotateX * Math.PI / 180.0);
                    Matrix4x4 rotY = Matrix4x4.CreateRotationY(camera.RotateY * Math.PI / 180.0);
                    Matrix4x4 rotationMatrix = rotY * rotX;

                    var sortedFaces = new List<(List<int> face, double depth, PointF[] screenPoints, List<PointF> texCoords)>();

                    var screenVertices = new PointF[polyhedron.Vertices.Count];
                    var vertexDepths = new float[polyhedron.Vertices.Count];

                    for (int i = 0; i < polyhedron.Vertices.Count; i++)
                    {
                        var projection = camera.ProjectTo2DWithDepth(polyhedron.Vertices[i], screenWidth, screenHeight, viewport.Scale);
                        screenVertices[i] = projection.screenPos;
                        vertexDepths[i] = projection.depth;
                    }

                    foreach (var face in polyhedron.Faces)
                    {
                        if (face.Count < 3) continue;

                        List<PointF> texCoords = GetTextureCoordsForFace(face, polyhedron);

                        var points = new PointF[face.Count];
                        bool validFace = true;
                        double avgDepth = 0;

                        for (int i = 0; i < face.Count; i++)
                        {
                            points[i] = screenVertices[face[i]];
                            avgDepth += vertexDepths[face[i]];

                            if (Math.Abs(points[i].X) > maxCoord || Math.Abs(points[i].Y) > maxCoord)
                            {
                                validFace = false;
                                break;
                            }
                        }

                        if (!validFace) continue;
                        avgDepth /= face.Count;

                        sortedFaces.Add((face, avgDepth, points, texCoords));
                    }

                    if (zBufferEnabled)
                    {
                        sortedFaces = sortedFaces.OrderByDescending(f => f.depth).ToList();
                    }
                    else
                    {
                        sortedFaces = sortedFaces.OrderByDescending(f => f.depth).ToList();
                    }

                    foreach (var (face, depth, screenPoints, texCoords) in sortedFaces)
                    {
                        if (isTexturingEnabled && currentTexture != null)
                        {
                            DrawTexturedPolygon(g, screenPoints, texCoords, polyhedron);
                        }
                        else
                        {
                            Color faceColor = GetFaceColor(face, rotationMatrix, polyhedron);
                            using (Brush faceBrush = new SolidBrush(faceColor))
                            {
                                if (zBufferEnabled)
                                {
                                    var triangleData = new (PointF pos, float depth, Point3D worldPos, int index)[face.Count];
                                    for (int i = 0; i < face.Count; i++)
                                    {
                                        int idx = face[i];
                                        triangleData[i] = (screenVertices[idx], vertexDepths[idx], polyhedron.Vertices[idx], idx);
                                    }

                                    for (int i = 1; i < face.Count - 1; i++)
                                    {
                                        var triangle = new[]
                                        {
                                    triangleData[0],
                                    triangleData[i],
                                    triangleData[i + 1]
                                };
                                        DrawTriangleWithZBuffer(g, triangle, faceBrush, polyhedron);
                                    }
                                }
                                else
                                {
                                    g.FillPolygon(faceBrush, screenPoints);
                                    g.DrawPolygon(pen, screenPoints);
                                }
                            }
                        }
                    }
                }
            }

            DrawLightSource(g, screenWidth, screenHeight);
        }

        private List<PointF> GetTextureCoordsForFace(List<int> face, Polyhedron polyhedron)
        {
            if (polyhedron.Name.Contains("тетра") || polyhedron.Name.Contains("Tetra") || polyhedron.Faces.Count == 4)
            {
                return GetTetrahedronUVs(face, polyhedron);
            }
            else if (polyhedron.Name.Contains("куб") || polyhedron.Name.Contains("cube") || polyhedron.Name.Contains("Куб") || polyhedron.Faces.Count == 6)
            {
                return GetCubeUVs(face, polyhedron);
            }
            else if (polyhedron.Name.Contains("окта") || polyhedron.Name.Contains("octa") || polyhedron.Name.Contains("Окта"))
            {
                return GetOctahedronUVs(face, polyhedron);
            }
            var texCoords = new List<PointF>();

            if (polyhedron.TextureCoords == null || polyhedron.TextureCoords.Count == 0)
            {
                foreach (int vertexIndex in face)
                {
                    if (vertexIndex < polyhedron.Vertices.Count)
                    {
                        var vertex = polyhedron.Vertices[vertexIndex];
                        float u = (float)((vertex.X + 1) / 2);
                        float v = (float)((vertex.Y + 1) / 2);
                        texCoords.Add(new PointF(u, v));
                    }
                    else
                    {
                        texCoords.Add(new PointF(0.5f, 0.5f));
                    }
                }
                return texCoords;
            }

            int faceIndex = polyhedron.Faces.IndexOf(face);
            if (faceIndex == -1)
            {
                for (int i = 0; i < polyhedron.Faces.Count; i++)
                {
                    if (polyhedron.Faces[i].SequenceEqual(face))
                    {
                        faceIndex = i;
                        break;
                    }
                }
            }

            if (faceIndex >= 0 && polyhedron.TextureIndices != null &&
                faceIndex < polyhedron.TextureIndices.Count)
            {
                var texIndices = polyhedron.TextureIndices[faceIndex];

                if (texIndices.Count == face.Count)
                {
                    foreach (int texIndex in texIndices)
                    {
                        if (texIndex >= 0 && texIndex < polyhedron.TextureCoords.Count)
                        {
                            texCoords.Add(polyhedron.TextureCoords[texIndex]);
                        }
                        else
                        {
                            texCoords.Add(new PointF(0.5f, 0.5f));
                        }
                    }
                }
                else
                {
                    foreach (int vertexIndex in face)
                    {
                        if (vertexIndex >= 0 && vertexIndex < polyhedron.TextureCoords.Count)
                        {
                            texCoords.Add(polyhedron.TextureCoords[vertexIndex]);
                        }
                        else
                        {
                            texCoords.Add(new PointF(0.5f, 0.5f));
                        }
                    }
                }
            }
            else
            {
                foreach (int vertexIndex in face)
                {
                    if (vertexIndex >= 0 && vertexIndex < polyhedron.TextureCoords.Count)
                    {
                        texCoords.Add(polyhedron.TextureCoords[vertexIndex]);
                    }
                    else
                    {
                        texCoords.Add(new PointF(0.5f, 0.5f));
                    }
                }
            }

            return texCoords;
        }

        private List<PointF> GetCubeUVs(List<int> face, Polyhedron polyhedron)
        {
            var uvCoords = new List<PointF>();

            int faceIndex = polyhedron.Faces.IndexOf(face);
            if (faceIndex == -1)
            {
                for (int i = 0; i < polyhedron.Faces.Count; i++)
                {
                    if (polyhedron.Faces[i].SequenceEqual(face))
                    {
                        faceIndex = i;
                        break;
                    }
                }
            }

            if (faceIndex >= 0 && polyhedron.TextureIndices != null &&
                faceIndex < polyhedron.TextureIndices.Count)
            {
                var texIndices = polyhedron.TextureIndices[faceIndex];

                foreach (int texIndex in texIndices)
                {
                    if (texIndex >= 0 && texIndex < polyhedron.TextureCoords.Count)
                    {
                        uvCoords.Add(polyhedron.TextureCoords[texIndex]);
                    }
                    else
                    {
                        uvCoords.Add(new PointF(0.5f, 0.5f));
                    }
                }
            }

            if (uvCoords.Count != face.Count)
            {
                uvCoords.Clear();
                uvCoords.Add(new PointF(0.0f, 0.0f));
                uvCoords.Add(new PointF(1.0f, 0.0f));
                uvCoords.Add(new PointF(1.0f, 1.0f));
                uvCoords.Add(new PointF(0.0f, 1.0f));
            }

            return uvCoords;
        }

        private List<PointF> GetOctahedronUVs(List<int> face, Polyhedron polyhedron)
        {
            var uvCoords = new List<PointF>();

            int faceIndex = polyhedron.Faces.IndexOf(face);
            if (faceIndex == -1)
            {
                for (int i = 0; i < polyhedron.Faces.Count; i++)
                {
                    if (polyhedron.Faces[i].SequenceEqual(face))
                    {
                        faceIndex = i;
                        break;
                    }
                }
            }

            if (faceIndex >= 0 && polyhedron.TextureIndices != null &&
                faceIndex < polyhedron.TextureIndices.Count)
            {
                var texIndices = polyhedron.TextureIndices[faceIndex];

                foreach (int texIndex in texIndices)
                {
                    if (texIndex >= 0 && texIndex < polyhedron.TextureCoords.Count)
                    {
                        uvCoords.Add(polyhedron.TextureCoords[texIndex]);
                    }
                }
            }

            if (uvCoords.Count != face.Count)
            {
                uvCoords.Clear();
                uvCoords.Add(new PointF(0.0f, 0.0f));
                uvCoords.Add(new PointF(1.0f, 0.0f));
                uvCoords.Add(new PointF(0.5f, 1.0f));
            }

            return uvCoords;
        }

        private List<PointF> GetTetrahedronUVs(List<int> face, Polyhedron polyhedron)
        {
            var uvCoords = new List<PointF>();

            int faceIndex = polyhedron.Faces.IndexOf(face);
            if (faceIndex == -1)
            {
                for (int i = 0; i < polyhedron.Faces.Count; i++)
                {
                    if (polyhedron.Faces[i].SequenceEqual(face))
                    {
                        faceIndex = i;
                        break;
                    }
                }
            }

            if (faceIndex >= 0 && polyhedron.TextureIndices != null &&
                faceIndex < polyhedron.TextureIndices.Count)
            {
                var texIndices = polyhedron.TextureIndices[faceIndex];

                foreach (int texIndex in texIndices)
                {
                    if (texIndex >= 0 && texIndex < polyhedron.TextureCoords.Count)
                    {
                        uvCoords.Add(polyhedron.TextureCoords[texIndex]);
                    }
                }
            }

            if (uvCoords.Count != face.Count)
            {
                uvCoords.Clear();
                uvCoords.Add(new PointF(0.0f, 0.0f));
                uvCoords.Add(new PointF(1.0f, 0.0f));
                uvCoords.Add(new PointF(0.5f, 1.0f));
            }

            return uvCoords;
        }

        private void DrawTexturedTriangle(Graphics graphics, PointF[] points, List<PointF> texCoords, Polyhedron polyhedron)
        {
            if (points.Length != 3 || texCoords.Count != 3)
                return;

            float minX = Math.Min(Math.Min(points[0].X, points[1].X), points[2].X);
            float maxX = Math.Max(Math.Max(points[0].X, points[1].X), points[2].X);
            float minY = Math.Min(Math.Min(points[0].Y, points[1].Y), points[2].Y);
            float maxY = Math.Max(Math.Max(points[0].Y, points[1].Y), points[2].Y);

            int startX = Math.Max(0, (int)Math.Floor(minX));
            int endX = Math.Min(pictureBox1.Width - 1, (int)Math.Ceiling(maxX));
            int startY = Math.Max(0, (int)Math.Floor(minY));
            int endY = Math.Min(pictureBox1.Height - 1, (int)Math.Ceiling(maxY));

            PointF p0 = points[0], p1 = points[1], p2 = points[2];
            PointF t0 = texCoords[0], t1 = texCoords[1], t2 = texCoords[2];

            float det = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(det) < 1e-10f) return;
            float invDet = 1.0f / det;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    float w0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) * invDet;
                    float w1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) * invDet;
                    float w2 = 1.0f - w0 - w1;

                    if (w0 >= -1e-4f && w1 >= -1e-4f && w2 >= -1e-4f)
                    {
                        float u = w0 * t0.X + w1 * t1.X + w2 * t2.X;
                        float v = w0 * t0.Y + w1 * t1.Y + w2 * t2.Y;

                        Color texColor = currentTexture.GetColor(u, v);

                        float intensity = 0.7f;
                        int r = (int)(texColor.R * intensity);
                        int g = (int)(texColor.G * intensity);
                        int b = (int)(texColor.B * intensity);

                        Color finalColor = Color.FromArgb(
                            Math.Clamp(r, 0, 255),
                            Math.Clamp(g, 0, 255),
                            Math.Clamp(b, 0, 255));

                        using (var brush = new SolidBrush(finalColor))
                        {
                            graphics.FillRectangle(brush, x, y, 1, 1);
                        }
                    }
                }
            }
        }

        private void DrawTexturedPolygon(Graphics graphics, PointF[] points, List<PointF> texCoords, Polyhedron polyhedron)
        {
            if (points.Length < 3 || texCoords.Count < 3 || points.Length != texCoords.Count)
                return;

            if (points.Length == 3)
            {
                DrawTexturedTriangle(graphics, points, texCoords, polyhedron);
            }
            else
            {
                DrawTexturedPolygonTriangulated(graphics, points, texCoords, polyhedron);
            }
        }

        private void DrawTexturedPolygonTriangulated(Graphics graphics, PointF[] points, List<PointF> texCoords, Polyhedron polyhedron)
        {
            for (int i = 1; i < points.Length - 1; i++)
            {
                PointF[] trianglePoints = { points[0], points[i], points[i + 1] };
                List<PointF> triangleTexCoords = new List<PointF> { texCoords[0], texCoords[i], texCoords[i + 1] };

                DrawTexturedTriangle(graphics, trianglePoints, triangleTexCoords, polyhedron);
            }
        }

        private class EdgeIntersection
        {
            public float X, U, V;

            public EdgeIntersection(float x, float u, float v)
            {
                X = x;
                U = u;
                V = v;
            }
        }

        private void ScanlineTexturedPolygon(Graphics g, PointF[] points, List<PointF> texCoords, Polyhedron polyhedron)
        {
            float minY = points.Min(p => p.Y);
            float maxY = points.Max(p => p.Y);

            int startY = Math.Max(0, (int)Math.Floor(minY));
            int endY = Math.Min(pictureBox1.Height - 1, (int)Math.Ceiling(maxY));

            if (startY > endY) return;

            var edges = new List<Edge>();

            for (int i = 0; i < points.Length; i++)
            {
                int next = (i + 1) % points.Length;
                Edge edge = new Edge(points[i], texCoords[i], points[next], texCoords[next]);
                if (Math.Abs(edge.DeltaY) > 0.0001f)
                {
                    edges.Add(edge);
                }
            }

            for (int y = startY; y <= endY; y++)
            {
                var activeEdges = edges.Where(e => y >= Math.Min(e.YStart, e.YEnd) &&
                                                  y <= Math.Max(e.YStart, e.YEnd)).ToList();

                if (activeEdges.Count < 2) continue;

                var intersections = new List<EdgeIntersection>();
                foreach (var edge in activeEdges)
                {
                    if (edge.DeltaY != 0)
                    {
                        float t = (y - edge.YStart) / edge.DeltaY;
                        if (t >= 0 && t <= 1)
                        {
                            float x = edge.XStart + t * edge.DeltaX;
                            float u = edge.UStart + t * edge.DeltaU;
                            float v = edge.VStart + t * edge.DeltaV;
                            intersections.Add(new EdgeIntersection(x, u, v));
                        }
                    }
                }

                intersections.Sort((a, b) => a.X.CompareTo(b.X));

                for (int i = 0; i < intersections.Count - 1; i += 2)
                {
                    float x1 = intersections[i].X;
                    float x2 = intersections[i + 1].X;
                    float u1 = intersections[i].U;
                    float v1 = intersections[i].V;
                    float u2 = intersections[i + 1].U;
                    float v2 = intersections[i + 1].V;

                    int startX = Math.Max(0, (int)Math.Floor(x1));
                    int endX = Math.Min(pictureBox1.Width - 1, (int)Math.Ceiling(x2));

                    if (startX > endX) continue;

                    for (int x = startX; x <= endX; x++)
                    {
                        float t = (x - x1) / (x2 - x1);
                        float u = u1 + t * (u2 - u1);
                        float v = v1 + t * (v2 - v1);

                        Color texColor = currentTexture.GetColor(u, v);
                        Color finalColor = ApplyLightingToTexel(texColor, polyhedron);

                        using (var brush = new SolidBrush(finalColor))
                        {
                            g.FillRectangle(brush, x, y, 1, 1);
                        }
                    }
                }
            }
        }

        private Color ApplyLightingToTexel(Color texColor, Polyhedron polyhedron)
        {
            float intensity = 0.7f;
            int r = (int)(texColor.R * intensity);
            int g = (int)(texColor.G * intensity);
            int b = (int)(texColor.B * intensity);

            return Color.FromArgb(
                Math.Clamp(r, 0, 255),
                Math.Clamp(g, 0, 255),
                Math.Clamp(b, 0, 255));
        }

        private Color GetFaceColor(List<int> face, Matrix4x4 rotationMatrix, Polyhedron polyhedron)
        {
            if (face.Count < 3)
                return polyhedron.Material.DiffuseColor;

            var v0 = polyhedron.Vertices[face[0]];
            var v1 = polyhedron.Vertices[face[1]];
            var v2 = polyhedron.Vertices[face[2]];

            var edge1 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
            var edge2 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);
            var faceNormal = Point3D.CrossProduct(edge1, edge2);
            faceNormal.Normalize();

            Point3D faceCenter = new Point3D(0, 0, 0);
            foreach (int vertexIndex in face)
            {
                faceCenter.X += polyhedron.Vertices[vertexIndex].X;
                faceCenter.Y += polyhedron.Vertices[vertexIndex].Y;
                faceCenter.Z += polyhedron.Vertices[vertexIndex].Z;
            }
            faceCenter.X /= face.Count;
            faceCenter.Y /= face.Count;
            faceCenter.Z /= face.Count;

            Vector3 lightDirection = lightSource.GetDirectionTo(faceCenter);

            Vector3 worldNormal = new Vector3((float)faceNormal.X, (float)faceNormal.Y, (float)faceNormal.Z);
            float dot = Vector3.Dot(worldNormal, lightDirection);

            dot = Math.Max(0, Math.Min(1, dot));

            float intensity = polyhedron.Material.AmbientIntensity +
                             polyhedron.Material.DiffuseIntensity * dot;
            intensity = Math.Max(0, Math.Min(1, intensity));

            Color baseColor = polyhedron.Material.DiffuseColor;
            int r = (int)(baseColor.R * intensity);
            int g = (int)(baseColor.G * intensity);
            int b = (int)(baseColor.B * intensity);

            return Color.FromArgb(
                Math.Min(255, Math.Max(0, r)),
                Math.Min(255, Math.Max(0, g)),
                Math.Min(255, Math.Max(0, b)));
        }

        private class Edge
        {
            public float XStart, YStart, XEnd, YEnd;
            public float UStart, VStart, UEnd, VEnd;
            public float DeltaX, DeltaY, DeltaU, DeltaV;

            public Edge(PointF p1, PointF uv1, PointF p2, PointF uv2)
            {
                XStart = p1.X;
                YStart = p1.Y;
                XEnd = p2.X;
                YEnd = p2.Y;

                UStart = uv1.X;
                VStart = uv1.Y;
                UEnd = uv2.X;
                VEnd = uv2.Y;

                DeltaX = XEnd - XStart;
                DeltaY = YEnd - YStart;
                DeltaU = UEnd - UStart;
                DeltaV = VEnd - VStart;
            }
        }

        private void DrawTriangleWithZBuffer(Graphics g, (PointF pos, float depth, Point3D worldPos, int index)[] triangle,
    Brush brush, Polyhedron polyhedron)
        {
            if (triangle.Length != 3) return;

            if (backfaceCullingEnabled)
            {
                var v0 = triangle[0].worldPos;
                var v1 = triangle[1].worldPos;
                var v2 = triangle[2].worldPos;

                var edge1 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
                var edge2 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);

                var normal = Point3D.CrossProduct(edge1, edge2);
                normal.Normalize();

                var center = new Point3D(
                    (v0.X + v1.X + v2.X) / 3,
                    (v0.Y + v1.Y + v2.Y) / 3,
                    (v0.Z + v1.Z + v2.Z) / 3
                );

                var cameraPos = new Point3D(0, 0, 10);
                var viewDir = new Point3D(
                    center.X - cameraPos.X,
                    center.Y - cameraPos.Y,
                    center.Z - cameraPos.Z
                );
                viewDir.Normalize();

                float dot = (float)(normal.X * viewDir.X + normal.Y * viewDir.Y + normal.Z * viewDir.Z);

                if (dot < 0)
                {
                    return;
                }
            }

            float minX = Math.Min(Math.Min(triangle[0].pos.X, triangle[1].pos.X), triangle[2].pos.X);
            float maxX = Math.Max(Math.Max(triangle[0].pos.X, triangle[1].pos.X), triangle[2].pos.X);
            float minY = Math.Min(Math.Min(triangle[0].pos.Y, triangle[1].pos.Y), triangle[2].pos.Y);
            float maxY = Math.Max(Math.Max(triangle[0].pos.Y, triangle[1].pos.Y), triangle[2].pos.Y);

            int startX = Math.Max(0, (int)Math.Floor(minX));
            int endX = Math.Min(zBuffer.Width - 1, (int)Math.Ceiling(maxX));
            int startY = Math.Max(0, (int)Math.Floor(minY));
            int endY = Math.Min(zBuffer.Height - 1, (int)Math.Ceiling(maxY));

            PointF p0 = triangle[0].pos, p1 = triangle[1].pos, p2 = triangle[2].pos;
            float z0 = triangle[0].depth, z1 = triangle[1].depth, z2 = triangle[2].depth;

            float area = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(area) < 1e-10f) return;
            float invArea = 1.0f / area;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    float w0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) * invArea;
                    float w1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) * invArea;
                    float w2 = 1.0f - w0 - w1;

                    if (w0 >= -1e-4f && w1 >= -1e-4f && w2 >= -1e-4f)
                    {
                        float depth = w0 * z0 + w1 * z1 + w2 * z2;

                        depth += 0.0001f;

                        if (zBuffer.TestAndSet(x, y, depth))
                        {
                            g.FillRectangle(brush, x, y, 1, 1);
                        }
                    }
                }
            }
        }

        private Vector3 ComputeCelShading(Vector3 interpNormal, Vector3 interpPos, Polyhedron polyhedron)
        {
            Vector3 ColorToVec(Color c) => new Vector3(c.R / 255f, c.G / 255f, c.B / 255f);

            Vector3 baseCol = ColorToVec(polyhedron.Material.DiffuseColor);
            Vector3 lightCol = ColorToVec(lightSource.Color) * lightSource.Intensity;

            Point3D worldPos = new Point3D(interpPos.X, interpPos.Y, interpPos.Z);
            Vector3 L = lightSource.GetDirectionTo(worldPos);

            float diff = Math.Max(Vector3.Dot(interpNormal, L), 0f);
            diff = 0.2f + diff;

            if (diff < 0.4f)
                diff = 0.3f;
            else if (diff < 0.7f)
                diff = 1.0f;
            else
                diff = 1.3f;

            return baseCol * diff * lightCol;
        }

        private void DrawArrow(Graphics g, PointF start, PointF end, Color color)
        {
            float maxX = pictureBox1.Width * 2f;
            float maxY = pictureBox1.Height * 2f;

            if (Math.Abs(start.X) > maxX || Math.Abs(start.Y) > maxY ||
                Math.Abs(end.X) > maxX || Math.Abs(end.Y) > maxY)
            {
                return;
            }

            using (Pen pen = new Pen(color, 2))
            {
                g.DrawLine(pen, start, end);
            }

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            if (length > 0)
            {
                dx /= length;
                dy /= length;

                float arrowSize = 10f;
                float angle = (float)(Math.PI / 6);

                float leftX = end.X - arrowSize * (float)Math.Cos(angle) * dx + arrowSize * (float)Math.Sin(angle) * dy;
                float leftY = end.Y - arrowSize * (float)Math.Cos(angle) * dy - arrowSize * (float)Math.Sin(angle) * dx;

                float rightX = end.X - arrowSize * (float)Math.Cos(angle) * dx - arrowSize * (float)Math.Sin(angle) * dy;
                float rightY = end.Y - arrowSize * (float)Math.Cos(angle) * dy + arrowSize * (float)Math.Sin(angle) * dx;

                if (Math.Abs(leftX) <= maxX && Math.Abs(leftY) <= maxY &&
                    Math.Abs(rightX) <= maxX && Math.Abs(rightY) <= maxY)
                {
                    using (Pen arrowPen = new Pen(color, 2))
                    {
                        g.DrawLine(arrowPen, end, new PointF(leftX, leftY));
                        g.DrawLine(arrowPen, end, new PointF(rightX, rightY));
                    }
                }
            }

        }

        private void DrawCoordinateAxes(Graphics g)
        {
            Point3D origin = new Point3D(0, 0, 0);
            Point3D xAxis = new Point3D(3, 0, 0);
            Point3D yAxis = new Point3D(0, 3, 0);
            Point3D zAxis = new Point3D(0, 0, 3);

            PointF originScreen = viewport.WorldToScreen(origin, camera, pictureBox1.Width, pictureBox1.Height);
            PointF xScreen = viewport.WorldToScreen(xAxis, camera, pictureBox1.Width, pictureBox1.Height);
            PointF yScreen = viewport.WorldToScreen(yAxis, camera, pictureBox1.Width, pictureBox1.Height);
            PointF zScreen = viewport.WorldToScreen(zAxis, camera, pictureBox1.Width, pictureBox1.Height);

            DrawArrow(g, originScreen, xScreen, Color.Red);
            DrawArrow(g, originScreen, yScreen, Color.Green);
            DrawArrow(g, originScreen, zScreen, Color.Blue);

            g.DrawString("X", this.Font, Brushes.Red, xScreen.X + 5, xScreen.Y + 5);
            g.DrawString("Y", this.Font, Brushes.Green, yScreen.X + 5, yScreen.Y + 5);
            g.DrawString("Z", this.Font, Brushes.Blue, zScreen.X + 5, zScreen.Y + 5);
        }

        private void DrawInfo(Graphics g)
        {
            string projection = camera.CurrentProjection == Camera.ProjectionType.Axonometric
             ? "Аксонометрическая"
             : "Перспективная";

            string modelInfo = currentPolyhedron != null ?
               $" | Вершин: {currentPolyhedron.Vertices.Count} | Граней: {currentPolyhedron.Faces.Count}" :
               "";
            string modelName = currentPolyhedron?.Name != null ? $"Имя: {currentPolyhedron.Name} | " : "";

            string info = $"{modelName}Проекция: {projection} | Масштаб: {viewport.Scale:F2}x{modelInfo}";

            g.DrawString(info, Font, Brushes.Black, 10, pictureBox1.Height - 30);
        }

        private void DrawLightSource(Graphics g, int screenWidth, int screenHeight)
        {
            PointF lightScreenPos = viewport.WorldToScreen(
                lightSource.Position, camera, screenWidth, screenHeight);

            float lightSize = 8f;

            using (var lightBrush = new SolidBrush(Color.Yellow))
            using (var lightPen = new Pen(Color.Orange, 1))
            {
                g.FillEllipse(lightBrush,
                    lightScreenPos.X - lightSize / 2,
                    lightScreenPos.Y - lightSize / 2,
                    lightSize, lightSize);
                g.DrawEllipse(lightPen,
                    lightScreenPos.X - lightSize / 2,
                    lightScreenPos.Y - lightSize / 2,
                    lightSize, lightSize);
            }

            PointF centerScreen = viewport.WorldToScreen(
                new Point3D(0, 0, 0), camera, screenWidth, screenHeight);
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            viewport.Zoom(zoomFactor, new System.Drawing.PointF(e.X, e.Y), pictureBox1.Width, pictureBox1.Height); pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && camera.CurrentProjection == Camera.ProjectionType.Axonometric)
            {
                isRotatingCamera = true;
                lastMousePosition = e.Location;
                pictureBox1.Cursor = Cursors.SizeAll;
            }
            else if (e.Button == MouseButtons.Left)
            {
                isRotatingObject = true;
                lastMousePosition = e.Location;
                pictureBox1.Cursor = Cursors.Hand;
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRotatingCamera)
            {
                float deltaX = e.X - lastMousePosition.X;
                float deltaY = e.Y - lastMousePosition.Y;

                camera.Rotate(deltaX, deltaY);

                lastMousePosition = e.Location;
                pictureBox1.Invalidate();
            }
            else if (isRotatingObject)
            {
                float deltaX = e.X - lastMousePosition.X;
                float deltaY = e.Y - lastMousePosition.Y;

                RotateObject(deltaX, deltaY);

                lastMousePosition = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isRotatingCamera = false;
                pictureBox1.Cursor = Cursors.Default;
            }
            else if (e.Button == MouseButtons.Left)
            {
                isRotatingObject = false;
                pictureBox1.Cursor = Cursors.Default;
            }
        }

        private void RotateObject(double deltaX, double deltaY)
        {
            if (currentPolyhedron == null) return;

            Point3D center = currentPolyhedron.GetCenter();

            Matrix4x4 toOrigin = Matrix4x4.CreateTranslation(-center.X, -center.Y, -center.Z);
            Matrix4x4 fromOrigin = Matrix4x4.CreateTranslation(center.X, center.Y, center.Z);

            Matrix4x4 rotY = Matrix4x4.CreateRotationY(deltaX * 0.01);
            Matrix4x4 rotX = Matrix4x4.CreateRotationX(deltaY * 0.01);

            Matrix4x4 rotation = fromOrigin * rotX * rotY * toOrigin;

            currentPolyhedron.Transform(rotation);
            currentPolyhedron.TransformNormals(rotation);

            objectRotation = rotation * objectRotation;
        }

        private void ZoomPolyhedron(float scaleFactor)
        {
            if (currentPolyhedron == null) return;
            Matrix4x4 mat = Matrix4x4.CreateScaleAroundCenter(scaleFactor, currentPolyhedron.GetCenter());
            currentPolyhedron.Transform(mat);
            currentPolyhedron.TransformNormals(mat);

            pictureBox1.Invalidate();
        }

        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomPolyhedron(1.2f);
        }

        private void BtnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomPolyhedron(0.8f);
        }

        private void BtnResetView_Click(object sender, EventArgs e)
        {
            comboBoxShading.SelectedIndex = 0;
            currShadingState = shadingState.off;
            viewport.Reset();
            InitializePoints();
            AxisPointA = null;
            AxisPointB = null;
            isTexturingEnabled = false;
            currentTexture = null;
            pictureBox1.Invalidate();
        }

        private void BtnSwitchProjection_Click(object sender, EventArgs e)
        {
            camera.CurrentProjection = camera.CurrentProjection == Camera.ProjectionType.Axonometric
                ? Camera.ProjectionType.Perspective
                : Camera.ProjectionType.Axonometric;

            btnSwitchProjection.Text = camera.CurrentProjection == Camera.ProjectionType.Axonometric
                  ? "Перспектива"
                  : "Аксонометрия";

            pictureBox1.Invalidate();
        }

        private void ButtonOct_Click(object sender, EventArgs e)
        {
            currentPolyhedron = polyhedrons[2];
            objectRotation.MakeIdentity();
            pictureBox1.Invalidate();
        }

        private void ButtonTetr_Click(object sender, EventArgs e)
        {
            currentPolyhedron = polyhedrons[0];
            objectRotation.MakeIdentity();
            pictureBox1.Invalidate();
        }

        private void ButtonGex_Click(object sender, EventArgs e)
        {
            currentPolyhedron = polyhedrons[1];
            objectRotation.MakeIdentity();
            pictureBox1.Invalidate();
        }

        private void ButtonIco_Click(object sender, EventArgs e)
        {
            currentPolyhedron = polyhedrons[3];
            objectRotation.MakeIdentity();
            pictureBox1.Invalidate();
        }

        private void ButtonDod_Click(object sender, EventArgs e)
        {
            currentPolyhedron = polyhedrons[4];
            objectRotation.MakeIdentity();
            pictureBox1.Invalidate();
        }

        private void ComboBoxReflection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxReflection.SelectedItem != null)
                chosenOption = comboBoxReflection.SelectedItem.ToString().Trim().ToUpperInvariant();
        }

        private void ButtonRefl_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            var matrix = Matrix4x4.CreateReflection(chosenOption);
            currentPolyhedron.Transform(matrix);
            currentPolyhedron.TransformNormals(matrix);
            pictureBox1.Invalidate();
        }

        private void ButtonTrans_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            if (!double.TryParse(textBoxTransX.Text, out dx))
            {
                MessageBox.Show("Введите корректное число для смещения по X!", "Ошибка ввода",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxTransY.Text, out dy))
            {
                MessageBox.Show("Введите корректное число для смещения по Y!", "Ошибка ввода",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxTransZ.Text, out dz))
            {
                MessageBox.Show("Введите корректное число для смещения по Z!", "Ошибка ввода",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var matrix = Matrix4x4.CreateTranslation(dx, dy, dz);
            currentPolyhedron.Transform(matrix);
            currentPolyhedron.TransformNormals(matrix);
            pictureBox1.Invalidate();
        }

        private void ButtonRotate_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            if (!double.TryParse(textBoxAngle.Text, out double angleDeg))
            {
                MessageBox.Show("Введите корректный угол!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string axis = comboBoxRotateAxis.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(axis))
            {
                MessageBox.Show("Выберите ось вращения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double angle = Math.PI * angleDeg / 180.0;
            Matrix4x4 rotation = new Matrix4x4();
            Point3D center = currentPolyhedron.GetCenter();

            Matrix4x4 toOrigin = Matrix4x4.CreateTranslation(-center.X, -center.Y, -center.Z);
            Matrix4x4 fromOrigin = Matrix4x4.CreateTranslation(center.X, center.Y, center.Z);

            switch (axis)
            {
                case "X":
                    rotation = Matrix4x4.CreateRotationX(angle);
                    break;
                case "Y":
                    rotation = Matrix4x4.CreateRotationY(angle);
                    break;
                case "Z":
                    rotation = Matrix4x4.CreateRotationZ(angle);
                    break;
                case "Ось (по точкам)":
                    MessageBox.Show("Для этого варианта используйте поля для координат двух точек оси.",
                     "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                default:
                    MessageBox.Show("Неизвестная ось!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            Matrix4x4 result = fromOrigin * rotation * toOrigin;
            currentPolyhedron.Transform(result);
            currentPolyhedron.TransformNormals(result);
            pictureBox1.Invalidate();
        }

        private void ButtonRotateAroundAxis_Click(object sender, EventArgs e)
        {
            var rotateForm = new RotateAroundAxisForm(this);
            rotateForm.ShowDialog();
        }

        private void ButtonFigRotate_Click(object sender, EventArgs e)
        {
            using var form = new FormFigureOfRevolution();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var fig = Polyhedron.CreateFigOfRevolution(
                    form.Generatrix,
                    form.Axis,
                    form.Segments
                );
                currentPolyhedron = fig;
                objectRotation.MakeIdentity();
                pictureBox1.Invalidate();
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OBJ files (*.obj)|*.obj|All files (*.*)|*.*";
            openFileDialog.Title = "Загрузить модель OBJ";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var loadedPolyhedron = ObjFileHandler.LoadFromFile(openFileDialog.FileName);

                    if (loadedPolyhedron.Vertices.Count > 0 && loadedPolyhedron.Faces.Count > 0)
                    {
                        currentPolyhedron = loadedPolyhedron;
                        objectRotation.MakeIdentity();

                        polyhedrons.Add(currentPolyhedron);

                        pictureBox1.Invalidate();
                        MessageBox.Show($"Модель загружена успешно!\nВершин: {currentPolyhedron.Vertices.Count}\nГраней: {currentPolyhedron.Faces.Count}",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Файл не содержит корректных данных модели.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null || currentPolyhedron.Vertices.Count == 0)
            {
                MessageBox.Show("Нет модели для сохранения.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "OBJ files (*.obj)|*.obj|All files (*.*)|*.*";
            saveFileDialog.Title = "Сохранить модель OBJ";
            saveFileDialog.DefaultExt = "obj";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ObjFileHandler.SaveToFile(currentPolyhedron, saveFileDialog.FileName);
                    MessageBox.Show($"Модель сохранена успешно!\nВершин: {currentPolyhedron.Vertices.Count}\nГраней: {currentPolyhedron.Faces.Count}",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonFunctionGraph_Click(object sender, EventArgs e)
        {
            using (var form = new FormFunctionGraph())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var surface = Polyhedron.CreateFunctionSurface(
                        form.Function,
                        form.XMin, form.XMax,
                        form.YMin, form.YMax,
                        form.Steps
                    );
                    currentPolyhedron = surface;
                    objectRotation.MakeIdentity();
                    pictureBox1.Invalidate();
                }
            }
        }

        private void buttonZB_Click(object sender, EventArgs e)
        {
            zBufferEnabled = !zBufferEnabled;
            backfaceCullingEnabled = !backfaceCullingEnabled;
            if (zBufferEnabled && zBuffer == null)
            {
                zBuffer = new ZBuffer(pictureBox1.Width, pictureBox1.Height);
            }
            else
            {
                zBuffer.Clear();
            }
            pictureBox1.Invalidate();
        }

        private void buttonLighting_Click(object sender, EventArgs e)
        {
            using (var lightForm = new LightSettingsForm(lightSource))
            {
                if (lightForm.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Invalidate();
                }
            }
        }

        private void ButtonTexture_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null)
            {
                MessageBox.Show("Сначала выберите или создайте фигуру", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var textureForm = new TextureSettingsForm())
            {
                if (textureForm.ShowDialog() == DialogResult.OK)
                {
                    currentTexture = textureForm.SelectedTexture;
                    isTexturingEnabled = true;
                    pictureBox1.Invalidate();
                }
            }
        }

        private void FillTexturedTriangle(Graphics g, PointF[] points, PointF[] texCoords, Bitmap texture)
        {
            if (points.Length != 3 || texCoords.Length != 3)
                return;

            float minX = Math.Min(Math.Min(points[0].X, points[1].X), points[2].X);
            float maxX = Math.Max(Math.Max(points[0].X, points[1].X), points[2].X);
            float minY = Math.Min(Math.Min(points[0].Y, points[1].Y), points[2].Y);
            float maxY = Math.Max(Math.Max(points[0].Y, points[1].Y), points[2].Y);

            int startX = Math.Max(0, (int)Math.Floor(minX));
            int endX = Math.Min(pictureBox1.Width - 1, (int)Math.Ceiling(maxX));
            int startY = Math.Max(0, (int)Math.Floor(minY));
            int endY = Math.Min(pictureBox1.Height - 1, (int)Math.Ceiling(maxY));

            float det = (points[1].Y - points[2].Y) * (points[0].X - points[2].X) +
                       (points[2].X - points[1].X) * (points[0].Y - points[2].Y);

            if (Math.Abs(det) < 1e-10f) return;
            float invDet = 1.0f / det;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    float w0 = ((points[1].Y - points[2].Y) * (x - points[2].X) +
                               (points[2].X - points[1].X) * (y - points[2].Y)) * invDet;
                    float w1 = ((points[2].Y - points[0].Y) * (x - points[2].X) +
                               (points[0].X - points[2].X) * (y - points[2].Y)) * invDet;
                    float w2 = 1.0f - w0 - w1;

                    if (w0 >= -1e-4f && w1 >= -1e-4f && w2 >= -1e-4f)
                    {
                        float u = w0 * texCoords[0].X + w1 * texCoords[1].X + w2 * texCoords[2].X;
                        float v = w0 * texCoords[0].Y + w1 * texCoords[1].Y + w2 * texCoords[2].Y;

                        int texX = (int)((u - (int)u) * texture.Width);
                        int texY = (int)((v - (int)v) * texture.Height);

                        texX = (texX % texture.Width + texture.Width) % texture.Width;
                        texY = (texY % texture.Height + texture.Height) % texture.Height;

                        Color pixelColor = texture.GetPixel(texX, texY);

                        using (var brush = new SolidBrush(pixelColor))
                        {
                            g.FillRectangle(brush, x, y, 1, 1);
                        }
                    }
                }
            }
        }
        private PointF GenerateTextureCoord(int vertexIndex, List<Point3D> vertices)
        {
            if (vertexIndex < 0 || vertexIndex >= vertices.Count)
                return new PointF(0, 0);

            var vertex = vertices[vertexIndex];

            double x = vertex.X, y = vertex.Y, z = vertex.Z;
            double r = Math.Sqrt(x * x + y * y + z * z);

            if (Math.Abs(r) < 1e-10)
                return new PointF(0.5f, 0.5f);

            double u = 0.5 + Math.Atan2(z, x) / (2 * Math.PI);
            double v = 0.5 - Math.Asin(y / r) / Math.PI;

            return new PointF((float)u, (float)v);
        }

        private void InitializePoints()
        {
            points.Clear();
            polyhedrons.Clear();

            var tetra = CreateTetrahedronWithObjUVs();
            polyhedrons.Add(tetra);

            var cube = CreateHexahedronWithSphericalUVs();
            polyhedrons.Add(cube);

            var octa = CreateOctahedronWithSphericalUVs();
            polyhedrons.Add(octa);

            var ico = Polyhedron.CreateIcosahedron();
            ico.CalculateTriangleTextureCoords();
            polyhedrons.Add(ico);

            var dode = Polyhedron.CreateDodecaedr();
            dode.CalculateTriangleTextureCoords();
            polyhedrons.Add(dode);

            currentPolyhedron = polyhedrons[0];
            currentTexture = null;
            isTexturingEnabled = false;
        }

        public static Polyhedron CreateOctahedronWithSphericalUVs(double size = 1.0)
        {
            var poly = new Polyhedron();
            poly.Name = "Октаэдр";
            double s = size;

            poly.Vertices.AddRange(
            [
                new Point3D(0, s, 0),     // 0: верх
                new Point3D(s, 0, 0),     // 1: право
                new Point3D(0, 0, s),     // 2: перед
                new Point3D(-s, 0, 0),    // 3: лево
                new Point3D(0, 0, -s),    // 4: зад
                new Point3D(0, -s, 0)     // 5: низ
                    ]);

            poly.Faces.AddRange(
            [
                new List<int> { 0, 2, 1 },
                new List<int> { 0, 1, 4 }, 
                new List<int> { 0, 4, 3 }, 
                new List<int> { 0, 3, 2 }, 
                new List<int> { 5, 1, 2 },  
                new List<int> { 5, 4, 1 }, 
                new List<int> { 5, 3, 4 },  
                new List<int> { 5, 2, 3 }  
                    ]);

            poly.TextureCoords = new List<PointF>();

            foreach (var vertex in poly.Vertices)
            {
                double x = vertex.X, y = vertex.Y, z = vertex.Z;
                double length = Math.Sqrt(x * x + y * y + z * z);

                if (length < 1e-10)
                {
                    poly.TextureCoords.Add(new PointF(0.5f, 0.5f));
                    continue;
                }

                x /= length;
                y /= length;
                z /= length;

                double u = 0.5 + Math.Atan2(z, x) / (2 * Math.PI);
                double v = 0.5 - Math.Asin(y) / Math.PI;

                if (u < 0) u += 1.0;
                if (u > 1) u -= 1.0;
                v = Math.Max(0.0, Math.Min(1.0, v));

                poly.TextureCoords.Add(new PointF((float)u, (float)v));
            }

            poly.TextureIndices = new List<List<int>>();
            foreach (var face in poly.Faces)
            {
                poly.TextureIndices.Add(new List<int>(face));
            }

            poly.CalculateVertexNormals();
            return poly;
        }

        public static Polyhedron CreateHexahedronWithSphericalUVs(double size = 1.0)
        {
            var poly = new Polyhedron();
            poly.Name = "Куб";
            double s = size;

            poly.Vertices.AddRange(
            [
                new Point3D(-s, -s, s),
                new Point3D(s, -s, s),
                new Point3D(s, s, s),
                new Point3D(-s, s, s),
                new Point3D(-s, -s, -s),
                new Point3D(s, -s, -s),
                new Point3D(s, s, -s),
                new Point3D(-s, s, -s)
            ]);

            poly.Faces.AddRange(
            [
                new List<int> { 0, 1, 2, 3 },
                new List<int> { 1, 5, 6, 2 },
                new List<int> { 5, 4, 7, 6 },
                new List<int> { 4, 0, 3, 7 },
                new List<int> { 3, 2, 6, 7 },
                new List<int> { 4, 5, 1, 0 }
            ]);

            poly.TextureCoords = new List<PointF>();

            foreach (var vertex in poly.Vertices)
            {
                double x = vertex.X, y = vertex.Y, z = vertex.Z;
                double length = Math.Sqrt(x * x + y * y + z * z);

                if (length < 1e-10)
                {
                    poly.TextureCoords.Add(new PointF(0.5f, 0.5f));
                    continue;
                }

                x /= length;
                y /= length;
                z /= length;

                double u = 0.5 + Math.Atan2(z, x) / (2 * Math.PI);
                double v = 0.5 - Math.Asin(y) / Math.PI;

                poly.TextureCoords.Add(new PointF((float)u, (float)v));
            }

            poly.TextureIndices = new List<List<int>>();
            foreach (var face in poly.Faces)
            {
                poly.TextureIndices.Add(new List<int>(face));
            }

            poly.CalculateVertexNormals();
            return poly;
        }

        public static Polyhedron CreateTetrahedronWithObjUVs(double size = 1.0)
        {
            var poly = new Polyhedron();
            poly.Name = "Тетраэдр (OBJ UV)";
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

            poly.TextureCoords.Add(new PointF(0.5f, 1.0f));
            poly.TextureCoords.Add(new PointF(0.0f, 0.0f));
            poly.TextureCoords.Add(new PointF(1.0f, 0.0f));
            poly.TextureCoords.Add(new PointF(0.5f, 0.5f));

            foreach (var face in poly.Faces)
            {
                poly.TextureIndices.Add(new List<int>(face));
            }

            poly.CalculateVertexNormals();
            return poly;
        }

        private void comboBoxShading_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = comboBoxShading.SelectedIndex;
            currShadingState = (shadingState)mode;
            pictureBox1.Invalidate();
        }

        private void buttonSecondPoly_Click(object sender, EventArgs e)
        {
            usingSecondPolyhedron = !usingSecondPolyhedron;
            pictureBox1.Invalidate();
        }
    }
}