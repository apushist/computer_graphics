using System.Numerics;

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
		private bool backfaceCullingEnabled = false;
		private bool zBufferEnabled = false;
		private LightSource lightSource = new LightSource();
		private bool shadingEnabled = false;

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

			// ������� ��� �������������
			polyhedrons.Add(Polyhedron.CreateTetrahedron());
			polyhedrons.Add(Polyhedron.CreateHexahedron());
			polyhedrons.Add(Polyhedron.CreateOctahedron());
			polyhedrons.Add(Polyhedron.CreateIcosahedron());
			polyhedrons.Add(Polyhedron.CreateDodecaedr());

			currentPolyhedron = polyhedrons[0];

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

					using (Brush faceBrush = new SolidBrush(faceColor))
					{
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
							if (zBufferEnabled)
							{
								DrawPolygonWithZBuffer(g, points, face, screenWidth, screenHeight, faceBrush, pen);
							}
							else
							{
								g.FillPolygon(faceBrush, points);
								g.DrawPolygon(pen, points);
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Ошибка отрисовки: {ex.Message}");
						}
					}
				}
			}
		}

		private void DrawPolygonWithZBuffer(Graphics g, PointF[] points, List<int> face,
			int screenWidth, int screenHeight, Brush brush, Pen pen)
		{
			if (face.Count < 3) return;

			var vertices = new (PointF pos, float depth)[face.Count];
			for (int i = 0; i < face.Count; i++)
			{
				var vertex = currentPolyhedron.Vertices[face[i]];
				var projection = camera.ProjectTo2DWithDepth(vertex, screenWidth, screenHeight, viewport.Scale);
				vertices[i] = (projection.screenPos, projection.depth);
			}

			for (int i = 1; i < face.Count - 1; i++)
			{
				var triangle = new[] { vertices[0], vertices[i], vertices[i + 1] };
				DrawTriangleWithZBuffer(g, triangle, brush);
			}

			g.DrawPolygon(pen, vertices.Select(v => v.pos).ToArray());
		}

		private void DrawTriangleWithZBuffer(Graphics g, (PointF pos, float depth)[] triangle, Brush brush)
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

			Color color = ((SolidBrush)brush).Color;

			for (int y = startY; y <= endY; y++)
			{
				for (int x = startX; x <= endX; x++)
				{
					float w0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) * invDet;
					float w1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) * invDet;
					float w2 = 1.0f - w0 - w1;

					if (w0 >= 0 && w1 >= 0 && w2 >= 0)
					{
						float depth = w0 * z0 + w1 * z1 + w2 * z2;

						if (zBuffer.TestAndSet(x, y, depth))
						{
							using (var pixelBrush = new SolidBrush(color))
							{
								g.FillRectangle(pixelBrush, x, y, 1, 1);
							}
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
	}
}