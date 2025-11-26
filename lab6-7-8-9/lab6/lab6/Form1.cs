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

		public void DrawPolyherdon(Graphics g, int screenWidth, int screenHeight)
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

				if (polyhedron.Normals.Count == 0)
				{
					polyhedron.CalculateVertexNormals();
				}

				using (Pen pen = new Pen(polyhedron.Material.DiffuseColor, 2))
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

					var sortedFaces = new List<(List<int> face, double depth, bool isVisible)>();

					foreach (var face in polyhedron.Faces)
					{
						if (face.Count < 3) continue;

						bool isVisible = true;

						var v0 = polyhedron.Vertices[face[0]];
						var v1 = polyhedron.Vertices[face[1]];
						var v2 = polyhedron.Vertices[face[2]];

						var a = new Vector3((float)(v1.X - v0.X), (float)(v1.Y - v0.Y), (float)(v1.Z - v0.Z));
						var b = new Vector3((float)(v2.X - v0.X), (float)(v2.Y - v0.Y), (float)(v2.Z - v0.Z));

						var normal = Vector3.Cross(a, b);
						Vector3 transformedNormal = normal;

						if (camera.CurrentProjection == Camera.ProjectionType.Axonometric)
						{
							transformedNormal = rotationMatrix.TransformVector(normal);
						}

						float dot = Vector3.Dot(transformedNormal, view);
						isVisible = dot < 0; // Грань видима если нормаль направлена от камеры

						if (isVisible)
						{
							double avgDepth = 0;
							foreach (int vertexIndex in face)
							{
								var vertex = polyhedron.Vertices[vertexIndex];
								avgDepth += vertex.Z;
							}
							avgDepth /= face.Count;

							sortedFaces.Add((face, avgDepth, isVisible));
						}
					}

					if (!zBufferEnabled)
					{
						sortedFaces = sortedFaces.OrderByDescending(f => f.depth).ToList();
					}

					foreach (var (face, depth, isVisible) in sortedFaces)
					{
						if (!isVisible) continue;

						Color faceColor = Color.White;
						switch (currShadingState)
						{
							case shadingState.off:
								faceColor = polyhedron.Material.DiffuseColor;
								break;
							case shadingState.guro:
								faceColor = CalculateFaceColor(face, rotationMatrix, polyhedron);
								break;
							case shadingState.phong:
								faceColor = Color.White;
								break;
						}

						var points = new PointF[face.Count];
						bool validFace = true;

						for (int i = 0; i < face.Count; i++)
						{
							var vertex = polyhedron.Vertices[face[i]];
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
								var texCoords = CalculateDynamicTextureCoords(face, polyhedron.Vertices);
								DrawTexturedFace(g, face, points, texCoords, polyhedron);
							}
							else
							{
								using (Brush faceBrush = new SolidBrush(faceColor))
								{
									if (zBufferEnabled)
									{
										DrawPolygonWithZBuffer(g, points, face, screenWidth, screenHeight, faceBrush, pen, polyhedron);
									}
									else
									{
										g.FillPolygon(faceBrush, points);
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

			DrawLightSource(g, screenWidth, screenHeight);
		}


		private void DrawPolygonWithZBuffer(Graphics g, PointF[] points, List<int> face,
	int screenWidth, int screenHeight, Brush brush, Pen pen, Polyhedron polyhedron)
		{
			if (face.Count < 3) return;

			var vertices = new (PointF pos, float depth, Point3D worldPos, int index)[face.Count];
			for (int i = 0; i < face.Count; i++)
			{
				var idx = face[i];
				var vertex = polyhedron.Vertices[idx];
				var projection = camera.ProjectTo2DWithDepth(vertex, screenWidth, screenHeight, viewport.Scale);
				vertices[i] = (projection.screenPos, projection.depth, vertex, idx);
			}

			for (int i = 1; i < face.Count - 1; i++)
			{
				var triangle = new[] { vertices[0], vertices[i], vertices[i + 1] };
				DrawTriangleWithZBuffer(g, triangle, brush, polyhedron);
			}
		}

		private void DrawTriangleWithZBuffer(Graphics g, (PointF pos, float depth, Point3D worldPos, int index)[] triangle,
			Brush brush, Polyhedron polyhedron)
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

				if (polyhedron.Normals != null && polyhedron.Normals.Count > t.index)
				{
					var vn = polyhedron.Normals[t.index];
					vNormals[i] = Vector3.Normalize(new Vector3((float)vn.X, (float)vn.Y, (float)vn.Z));
				}
				else
				{
					vNormals[i] = Vector3.UnitZ;
				}

				Vector3 lightDir = lightSource.GetDirectionTo(new Point3D(vWorldPos[i].X, vWorldPos[i].Y, vWorldPos[i].Z));
				float diff = Math.Max(Vector3.Dot(vNormals[i], lightDir), 0f);
				float intensity = polyhedron.Material.AmbientIntensity + polyhedron.Material.DiffuseIntensity * diff;
				Vector3 baseCol = ColorToVec(polyhedron.Material.DiffuseColor);
				Vector3 lightCol = ColorToVec(lightSource.Color) * lightSource.Intensity;
				vColors[i] = baseCol * intensity * lightCol;
			}

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

						if (zBuffer.TestAndSet(x, y, depth))
						{
							Vector3 finalColorVec = new(0, 0, 0);
							switch (currShadingState)
							{
								case shadingState.off:
									finalColorVec = ColorToVec(((SolidBrush)brush).Color);
									break;
								case shadingState.guro:
									finalColorVec = vColors[0] * w0 + vColors[1] * w1 + vColors[2] * w2;
									break;
								case shadingState.phong:
									Vector3 interpNormal = Vector3.Normalize(vNormals[0] * w0 + vNormals[1] * w1 + vNormals[2] * w2);
									Vector3 interpPos = vWorldPos[0] * w0 + vWorldPos[1] * w1 + vWorldPos[2] * w2;

									finalColorVec = ComputeCelShading(interpNormal, interpPos, polyhedron);
									break;
							}

							Color finalColor = VecToColor(finalColorVec);
							using var pixelBrush = new SolidBrush(finalColor);
							g.FillRectangle(pixelBrush, x, y, 1, 1);
						}
					}
				}
			}
		}

		// Вычисление цвета грани по модели Ламберта
		private Color CalculateFaceColor(List<int> face, Matrix4x4 rotationMatrix, Polyhedron polyhedron)
		{
			var v0 = polyhedron.Vertices[face[0]];
			var v1 = polyhedron.Vertices[face[1]];
			var v2 = polyhedron.Vertices[face[2]];

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

		private List<PointF> CalculateDynamicTextureCoords(List<int> face, List<Point3D> vertices)
		{
			var worldPoints = face.Select(idx => vertices[idx]).ToList();
			Matrix4x4 inverseRotation = Matrix4x4.Transpose(objectRotation);
			return CalculateFaceTextureCoordinates(worldPoints, inverseRotation);
		}

		private void DrawTexturedFace(Graphics g, List<int> face, PointF[] screenPoints, List<PointF> texCoords, Polyhedron polyhedron)
		{
			if (currentTexture == null || currentTexture.Bitmap == null)
			{
				using (var brush = new SolidBrush(polyhedron.Material.DiffuseColor))
				using (var pen = new Pen(Color.DarkGray, 1))
				{
					g.FillPolygon(brush, screenPoints);
					g.DrawPolygon(pen, screenPoints);
				}
				return;
			}

			try
			{
				using (var textureBrush = new TextureBrush(currentTexture.Bitmap))
				{
					textureBrush.WrapMode = WrapMode.Tile;

					SetupFaceTextureMapping(textureBrush, texCoords, screenPoints);

					g.FillPolygon(textureBrush, screenPoints);
				}

				using (var pen = new Pen(Color.FromArgb(100, Color.Black), 1))
				{
					g.DrawPolygon(pen, screenPoints);
				}
			}
			catch (Exception ex)
			{
				using (var brush = new SolidBrush(polyhedron.Material.DiffuseColor))
				using (var pen = new Pen(Color.DarkGray, 1))
				{
					g.FillPolygon(brush, screenPoints);
					g.DrawPolygon(pen, screenPoints);
				}
			}
		}

		private List<PointF> CalculateFaceTextureCoordinates(List<Point3D> worldPoints, Matrix4x4 inverseRotation)
		{
			var texCoords = new List<PointF>();

			var localPoints = worldPoints.Select(p =>
			{
				var pointCopy = new Point3D(p.X, p.Y, p.Z, p.W);
				pointCopy.Transform(inverseRotation);
				return pointCopy;
			}).ToList();

			double minX = localPoints.Min(p => p.X);
			double maxX = localPoints.Max(p => p.X);
			double minY = localPoints.Min(p => p.Y);
			double maxY = localPoints.Max(p => p.Y);
			double minZ = localPoints.Min(p => p.Z);
			double maxZ = localPoints.Max(p => p.Z);

			var v0 = localPoints[0];
			var v1 = localPoints[1];
			var v2 = localPoints[2];

			var edge1 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
			var edge2 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);

			var normal = Point3D.CrossProduct(edge1, edge2);
			normal.Normalize();

			double absX = Math.Abs(normal.X);
			double absY = Math.Abs(normal.Y);
			double absZ = Math.Abs(normal.Z);

			foreach (var point in localPoints)
			{
				float u, v;

				if (absX >= absY && absX >= absZ)
				{
					u = (float)((point.Z - minZ) / (maxZ - minZ));
					v = (float)((point.Y - minY) / (maxY - minY));
				}
				else if (absY >= absZ)
				{
					u = (float)((point.X - minX) / (maxX - minX));
					v = (float)((point.Z - minZ) / (maxZ - minZ));
				}
				else
				{
					u = (float)((point.X - minX) / (maxX - minX));
					v = (float)((point.Y - minY) / (maxY - minY));
				}

				texCoords.Add(new PointF(u, v));
			}

			return texCoords;
		}

		private void SetupFaceTextureMapping(TextureBrush brush, List<PointF> texCoords, PointF[] screenPoints)
		{
			try
			{
				if (texCoords.Count < 3 || screenPoints.Length < 3) return;

				PointF[] sourcePoints = new PointF[3];
				PointF[] destPoints = new PointF[3];

				for (int i = 0; i < 3; i++)
				{
					sourcePoints[i] = new PointF(
						texCoords[i].X * currentTexture.Width,
						texCoords[i].Y * currentTexture.Height
					);
					destPoints[i] = screenPoints[i];
				}

				PointF p0 = sourcePoints[0];
				PointF p1 = sourcePoints[1];
				PointF p2 = sourcePoints[2];

				PointF q0 = destPoints[0];
				PointF q1 = destPoints[1];
				PointF q2 = destPoints[2];

				float det = (p1.X - p0.X) * (p2.Y - p0.Y) - (p1.Y - p0.Y) * (p2.X - p0.X);
				if (Math.Abs(det) < 1e-10f)
				{
					brush.ResetTransform();
					return;
				}

				float a11 = ((q1.X - q0.X) * (p2.Y - p0.Y) - (q2.X - q0.X) * (p1.Y - p0.Y)) / det;
				float a12 = ((q2.X - q0.X) * (p1.X - p0.X) - (q1.X - q0.X) * (p2.X - p0.X)) / det;
				float a21 = ((q1.Y - q0.Y) * (p2.Y - p0.Y) - (q2.Y - q0.Y) * (p1.Y - p0.Y)) / det;
				float a22 = ((q2.Y - q0.Y) * (p1.X - p0.X) - (q1.Y - q0.Y) * (p2.X - p0.X)) / det;
				float dx = q0.X - a11 * p0.X - a12 * p0.Y;
				float dy = q0.Y - a21 * p0.X - a22 * p0.Y;

				using (Matrix transform = new Matrix(a11, a21, a12, a22, dx, dy))
				{
					brush.Transform = transform;
				}
			}
			catch
			{
				brush.ResetTransform();
			}
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