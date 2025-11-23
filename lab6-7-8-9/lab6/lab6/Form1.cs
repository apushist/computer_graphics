using System.Numerics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lab6
{
    public partial class Form1 : Form
    {
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
        private bool shadingEnabled = false;
        private bool usePhongShading = false;
        private bool isTexturingEnabled = false;
        private Texture currentTexture;

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
        }

        private void InitializePoints()
        {
            points.Clear();
            polyhedrons.Clear();

            polyhedrons.Add(Polyhedron.CreateTetrahedron());
            polyhedrons.Add(Polyhedron.CreateHexahedron());
            polyhedrons.Add(Polyhedron.CreateOctahedron());
            polyhedrons.Add(Polyhedron.CreateIcosahedron());
            polyhedrons.Add(Polyhedron.CreateDodecaedr());

            currentPolyhedron = polyhedrons[0];

            currentTexture = Texture.CreateTestTexture();
        }


        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
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

        public void DrawPolyherdon(Graphics g, int screenWidth, int screenHeight)
        {
            if (currentPolyhedron.Vertices.Count == 0 || currentPolyhedron.Faces.Count == 0) return;

            if (currentPolyhedron.Normals.Count == 0)
            {
                currentPolyhedron.CalculateVertexNormals();
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

            using (Pen pen = new Pen(Color.Black, 2))
            {
                Matrix4x4 rotX = Matrix4x4.CreateRotationX(camera.RotateX * Math.PI / 180.0);
                Matrix4x4 rotY = Matrix4x4.CreateRotationY(camera.RotateY * Math.PI / 180.0);
                Matrix4x4 rotationMatrix = rotY * rotX;

                Vector3 view;
                if (camera.CurrentProjection == Camera.ProjectionType.Perspective)
                {
                    view = Vector3.Normalize(camera.Target - camera.Position);
                }
                else
                {
                    view = new Vector3(0, 0, -1);
                }

                var sortedFaces = new List<(List<int> face, double depth)>();

                foreach (var face in currentPolyhedron.Faces)
                {
                    if (face.Count < 3) continue;

                    double avgDepth = 0;
                    foreach (int vertexIndex in face)
                    {
                        var vertex = currentPolyhedron.Vertices[vertexIndex];
                        avgDepth += Math.Sqrt(vertex.X * vertex.X + vertex.Y * vertex.Y + vertex.Z * vertex.Z);
                    }
                    avgDepth /= face.Count;

                    sortedFaces.Add((face, avgDepth));
                }

                if (!zBufferEnabled)
                {
                    sortedFaces = sortedFaces.OrderByDescending(f => f.depth).ToList();
                }

                foreach (var (face, depth) in sortedFaces)
                {
                    if (face.Count < 3) continue;

                    if (backfaceCullingEnabled)
                    {
                        var v0 = currentPolyhedron.Vertices[face[0]];
                        var v1 = currentPolyhedron.Vertices[face[1]];
                        var v2 = currentPolyhedron.Vertices[face[2]];

                        var a = new Vector3((float)(v1.X - v0.X), (float)(v1.Y - v0.Y), (float)(v1.Z - v0.Z));
                        var b = new Vector3((float)(v2.X - v0.X), (float)(v2.Y - v0.Y), (float)(v2.Z - v0.Z));

                        var normal = Vector3.Cross(a, b);
                        Vector3 transformedNormal = normal;

                        if (camera.CurrentProjection == Camera.ProjectionType.Axonometric)
                        {
                            transformedNormal = rotationMatrix.TransformVector(normal);
                        }

                        float dot = Vector3.Dot(transformedNormal, view);
                        if (dot >= 0) continue;
                    }

                    Color faceColor;
                    if (shadingEnabled)
                    {
                        faceColor = CalculateFaceColor(face, rotationMatrix);
                    }
                    else
                    {
                        faceColor = currentPolyhedron.Material.DiffuseColor;
                    }

                    var points = new PointF[face.Count];
                    bool validFace = true;

                    for (int i = 0; i < face.Count; i++)
                    {
                        var vertex = currentPolyhedron.Vertices[face[i]];
                        points[i] = viewport.WorldToScreen(vertex, camera, screenWidth, screenHeight);

                        if (Math.Abs(points[i].X) > maxCoord || Math.Abs(points[i].Y) > maxCoord)
                        {
                            validFace = false;
                            break;
                        }
                    }

                    if (!validFace) continue;

                    try
                    {
                        if (isTexturingEnabled && currentTexture != null)
                        {
                            var texCoords = CalculateDynamicTextureCoords(face, currentPolyhedron.Vertices);
                            DrawTexturedFace(g, face, points, texCoords);
                        }
                        else
                        {
                            using (Brush faceBrush = new SolidBrush(faceColor))
                            {
                                if (zBufferEnabled)
                                {
                                    DrawPolygonWithZBuffer(g, points, face, screenWidth, screenHeight, faceBrush, pen);
                                }
                                else
                                {
                                    g.DrawPolygon(pen, points);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка отрисовки: {ex.Message}");
                    }
                }
            }
        }


        private void DrawPolygonWithZBuffer(Graphics g, PointF[] points, List<int> face,
            int screenWidth, int screenHeight, Brush brush, Pen pen)
        {
            if (face.Count < 3) return;

            var vertices = new (PointF pos, float depth, Point3D worldPos, int index)[face.Count];
            for (int i = 0; i < face.Count; i++)
            {
                var idx = face[i];
                var vertex = currentPolyhedron.Vertices[idx];
                var projection = camera.ProjectTo2DWithDepth(vertex, screenWidth, screenHeight, viewport.Scale);
                vertices[i] = (projection.screenPos, projection.depth, vertex, idx);
            }

            for (int i = 1; i < face.Count - 1; i++)
            {
                var triangle = new[] { vertices[0], vertices[i], vertices[i + 1] };
                DrawTriangleWithZBuffer(g, triangle, brush);
            }

            g.DrawPolygon(pen, vertices.Select(v => v.pos).ToArray());
        }

        private void DrawTriangleWithZBuffer(Graphics g, (PointF pos, float depth, Point3D worldPos, int index)[] triangle, Brush brush)
        {
            if (triangle.Length != 3) return;

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

            float det = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(det) < 1e-10f) return;
            float invDet = 1.0f / det;

            Vector3 ColorToVec(Color c) => new Vector3(c.R / 255f, c.G / 255f, c.B / 255f);
            Color VecToColor(Vector3 v)
            {
                v = Vector3.Clamp(v, Vector3.Zero, new Vector3(1, 1, 1));
                return Color.FromArgb((int)(v.X * 255), (int)(v.Y * 255), (int)(v.Z * 255));
            }

            Vector3[] vColors = new Vector3[3];
            Vector3[] vNormals = new Vector3[3];
            Vector3[] vWorldPos = new Vector3[3];

            for (int i = 0; i < 3; i++)
            {
                var t = triangle[i];
                vWorldPos[i] = new Vector3((float)t.worldPos.X, (float)t.worldPos.Y, (float)t.worldPos.Z);

                if (currentPolyhedron.Normals != null && currentPolyhedron.Normals.Count > t.index)
                {
                    var vn = currentPolyhedron.Normals[t.index];
                    vNormals[i] = Vector3.Normalize(new Vector3((float)vn.X, (float)vn.Y, (float)vn.Z));
                }
                else
                {
                    vNormals[i] = Vector3.UnitZ;
                }

                Vector3 lightDir = lightSource.GetDirectionTo(new Point3D(vWorldPos[i].X, vWorldPos[i].Y, vWorldPos[i].Z));
                float diff = Math.Max(Vector3.Dot(vNormals[i], lightDir), 0f);
                float intensity = currentPolyhedron.Material.AmbientIntensity + currentPolyhedron.Material.DiffuseIntensity * diff;
                Vector3 baseCol = ColorToVec(currentPolyhedron.Material.DiffuseColor);
                Vector3 lightCol = ColorToVec(lightSource.Color) * lightSource.Intensity;
                vColors[i] = baseCol * intensity * lightCol;
            }

            bool usePhong = false;
            try { usePhong = this.usePhongShading; } catch { usePhong = false; }

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    float w0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) * invDet;
                    float w1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) * invDet;
                    float w2 = 1.0f - w0 - w1;

                    if (w0 >= -1e-4f && w1 >= -1e-4f && w2 >= -1e-4f)
                    {
                        float depth = w0 * z0 + w1 * z1 + w2 * z2;
                        if (!zBuffer.TestAndSet(x, y, depth)) continue;

                        Vector3 finalColorVec;

                        if (!shadingEnabled)
                        {
                            finalColorVec = ColorToVec(((SolidBrush)brush).Color);
                        }
                        else if (!usePhong)
                        {
                            finalColorVec = vColors[0] * w0 + vColors[1] * w1 + vColors[2] * w2;
                        }
                        else
                        {
                            Vector3 interpNormal = Vector3.Normalize(vNormals[0] * w0 + vNormals[1] * w1 + vNormals[2] * w2);
                            Vector3 interpPos = vWorldPos[0] * w0 + vWorldPos[1] * w1 + vWorldPos[2] * w2;

                            Vector3 baseCol = ColorToVec(currentPolyhedron.Material.DiffuseColor);
                            Vector3 lightCol = ColorToVec(lightSource.Color) * lightSource.Intensity;
                            Vector3 ambient = baseCol * currentPolyhedron.Material.AmbientIntensity;

                            Vector3 L = Vector3.Normalize(new Vector3(
                                (float)(lightSource.Position.X - interpPos.X),
                                (float)(lightSource.Position.Y - interpPos.Y),
                                (float)(lightSource.Position.Z - interpPos.Z)
                            ));

                            float diff = Math.Max(Vector3.Dot(interpNormal, L), 0f);
                            Vector3 diffuse = baseCol * currentPolyhedron.Material.DiffuseIntensity * diff;

                            Vector3 V = Vector3.Normalize(-interpPos);
                            Vector3 R = Vector3.Reflect(-L, interpNormal);
                            float spec = 0f;
                            if (diff > 0f)
                            {
                                float shininess = 32f;
                                spec = (float)Math.Pow(Math.Max(Vector3.Dot(R, V), 0f), shininess);
                            }
                            Vector3 specular = new Vector3(1, 1, 1) * spec * 0.5f;

                            finalColorVec = (ambient + diffuse + specular) * lightCol;
                        }

                        Color finalColor = VecToColor(finalColorVec);
                        using (var pixelBrush = new SolidBrush(finalColor))
                        {
                            g.FillRectangle(pixelBrush, x, y, 1, 1);
                        }
                    }
                }
            }
        }

        // Вычисление цвета грани по модели Ламберта
        private Color CalculateFaceColor(List<int> face, Matrix4x4 rotationMatrix)
        {
            var v0 = currentPolyhedron.Vertices[face[0]];
            var v1 = currentPolyhedron.Vertices[face[1]];
            var v2 = currentPolyhedron.Vertices[face[2]];

            var edge1 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z, 0);
            var edge2 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z, 0);

            var faceNormal = Point3D.CrossProduct(edge1, edge2);
            faceNormal.Normalize();
            Point3D faceCenter = new Point3D(
                (v0.X + v1.X + v2.X) / 3,
                (v0.Y + v1.Y + v2.Y) / 3,
                (v0.Z + v1.Z + v2.Z) / 3
            );

            Vector3 lightDirection = lightSource.GetDirectionTo(faceCenter);

            Vector3 worldNormal = new Vector3((float)faceNormal.X, (float)faceNormal.Y, (float)faceNormal.Z);
            float dot = Vector3.Dot(worldNormal, lightDirection);


            dot = Math.Max(0, Math.Min(1, dot));

            float intensity = currentPolyhedron.Material.AmbientIntensity +
                             currentPolyhedron.Material.DiffuseIntensity * dot;
            intensity = Math.Max(0, Math.Min(1, intensity));

            Color baseColor = currentPolyhedron.Material.DiffuseColor;
            int r = (int)(baseColor.R * intensity);
            int g = (int)(baseColor.G * intensity);
            int b = (int)(baseColor.B * intensity);

            return Color.FromArgb(200,
                Math.Min(255, Math.Max(0, r)),
                Math.Min(255, Math.Max(0, g)),
                Math.Min(255, Math.Max(0, b)));
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
            viewport.Reset();
            InitializePoints();
            AxisPointA = null;
            AxisPointB = null;
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

        private void buttonShading_Click(object sender, EventArgs e)
        {
            shadingEnabled = !shadingEnabled;
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

        private void CheckBoxPhong_CheckedChanged(object sender, EventArgs e)
        {
            usePhongShading = checkBoxPhong.Checked;
            pictureBox1.Invalidate();
        }

        private void ButtonTexture_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            isTexturingEnabled = !isTexturingEnabled;

            if (isTexturingEnabled)
            {
                currentTexture = Texture.CreateTestTexture();
                buttonTexture.Text = "Выключить текстурирование";

                MessageBox.Show("Текстура теперь должна вращаться с объектом!\nПовращайте фигуру.", "Текстурирование");
            }
            else
            {
                buttonTexture.Text = "Включить текстурирование";
            }

            pictureBox1.Invalidate();
        }

        private List<PointF> CalculateDynamicTextureCoords(List<int> face, List<Point3D> vertices)
        {
            var texCoords = new List<PointF>();

            if (currentPolyhedron.TextureCoords != null && currentPolyhedron.TextureCoords.Count == currentPolyhedron.Vertices.Count)
            {
                Console.WriteLine($"Используем предварительные координаты для {currentPolyhedron.Name}");

                foreach (int vertexIndex in face)
                {
                    if (vertexIndex < currentPolyhedron.TextureCoords.Count)
                    {
                        var texCoord = currentPolyhedron.TextureCoords[vertexIndex];
                        texCoords.Add(texCoord);

                        if (currentPolyhedron.Name == "Гексаэдр")
                        {
                            var vertex = vertices[vertexIndex];
                            Console.WriteLine($"Куб: вершина {vertexIndex} ({vertex.X:F1},{vertex.Y:F1},{vertex.Z:F1}) -> UV ({texCoord.X:F2},{texCoord.Y:F2})");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ОШИБКА: вершина {vertexIndex} выходит за границы TextureCoords");
                        texCoords.Add(new PointF(0.5f, 0.5f));
                    }
                }
            }
            else
            {
                Console.WriteLine($"НЕТ предварительных координат для {currentPolyhedron.Name}, используем fallback");

                var faceVertices = face.Select(idx => vertices[idx]).ToList();

                double minX = faceVertices.Min(v => v.X);
                double maxX = faceVertices.Max(v => v.X);
                double minY = faceVertices.Min(v => v.Y);
                double maxY = faceVertices.Max(v => v.Y);
                double minZ = faceVertices.Min(v => v.Z);
                double maxZ = faceVertices.Max(v => v.Z);

                foreach (var vertex in faceVertices)
                {
                    float u, v;

                    if (Math.Abs(maxX - minX) >= Math.Abs(maxY - minY) && Math.Abs(maxX - minX) >= Math.Abs(maxZ - minZ))
                    {
                        u = (float)((vertex.Z - minZ) / (maxZ - minZ));
                        v = (float)((vertex.Y - minY) / (maxY - minY));
                    }
                    else if (Math.Abs(maxY - minY) >= Math.Abs(maxZ - minZ))
                    {
                        u = (float)((vertex.X - minX) / (maxX - minX));
                        v = (float)((vertex.Z - minZ) / (maxZ - minZ));
                    }
                    else
                    {
                        u = (float)((vertex.X - minX) / (maxX - minX));
                        v = (float)((vertex.Y - minY) / (maxY - minY));
                    }

                    texCoords.Add(new PointF(u, v));
                }
            }

            return texCoords;
        }

        private void DrawTexturedFace(Graphics g, List<int> face, PointF[] screenPoints, List<PointF> texCoords)
        {
            if (currentTexture == null || currentTexture.Bitmap == null) return;

            try
            {
                using (var textureBrush = new TextureBrush(currentTexture.Bitmap))
                {
                    SetupTextureMapping(textureBrush, screenPoints, texCoords);

                    g.FillPolygon(textureBrush, screenPoints);
                }

                using (var pen = new Pen(Color.DarkGray, 1))
                {
                    g.DrawPolygon(pen, screenPoints);
                }
            }
            catch (Exception ex)
            {
                using (var brush = new SolidBrush(Color.FromArgb(150, 255, 255, 0)))
                using (var pen = new Pen(Color.Black, 1))
                {
                    g.FillPolygon(brush, screenPoints);
                    g.DrawPolygon(pen, screenPoints);
                }
            }
        }

        private void SetupTextureMapping(TextureBrush brush, PointF[] screenPoints, List<PointF> texCoords)
        {
            if (screenPoints.Length < 3 || texCoords.Count < 3) return;

            try
            {
                PointF[] sourcePoints = new PointF[3];
                PointF[] destPoints = new PointF[3];

                for (int i = 0; i < 3; i++)
                {
                    sourcePoints[i] = new PointF(
                        texCoords[i].X * currentTexture.Bitmap.Width,
                        texCoords[i].Y * currentTexture.Bitmap.Height
                    );
                    destPoints[i] = screenPoints[i];
                }

                using (Matrix transform = new Matrix(
                    new RectangleF(0, 0, currentTexture.Bitmap.Width, currentTexture.Bitmap.Height),
                    sourcePoints
                ))
                {
                    transform.Reset();

                    PointF p0 = sourcePoints[0];
                    PointF p1 = sourcePoints[1];
                    PointF p2 = sourcePoints[2];
                    PointF q0 = destPoints[0];
                    PointF q1 = destPoints[1];
                    PointF q2 = destPoints[2];

                    float det = (p1.X - p0.X) * (p2.Y - p0.Y) - (p1.Y - p0.Y) * (p2.X - p0.X);
                    if (Math.Abs(det) < 1e-10f) return;

                    float a11 = ((q1.X - q0.X) * (p2.Y - p0.Y) - (q2.X - q0.X) * (p1.Y - p0.Y)) / det;
                    float a12 = ((q2.X - q0.X) * (p1.X - p0.X) - (q1.X - q0.X) * (p2.X - p0.X)) / det;
                    float a21 = ((q1.Y - q0.Y) * (p2.Y - p0.Y) - (q2.Y - q0.Y) * (p1.Y - p0.Y)) / det;
                    float a22 = ((q2.Y - q0.Y) * (p1.X - p0.X) - (q1.Y - q0.Y) * (p2.X - p0.X)) / det;
                    float dx = q0.X - a11 * p0.X - a12 * p0.Y;
                    float dy = q0.Y - a21 * p0.X - a22 * p0.Y;

                    transform.Reset();
                    transform.Multiply(new Matrix(a11, a21, a12, a22, dx, dy));

                    brush.Transform = transform;
                }
            }
            catch (Exception ex)
            {
                brush.ResetTransform();

                float centerX = screenPoints.Average(p => p.X);
                float centerY = screenPoints.Average(p => p.Y);

                brush.TranslateTransform(centerX - currentTexture.Bitmap.Width / 2,
                                       centerY - currentTexture.Bitmap.Height / 2);

                Console.WriteLine($"Texture mapping error: {ex.Message}");
            }
        }

    }
}