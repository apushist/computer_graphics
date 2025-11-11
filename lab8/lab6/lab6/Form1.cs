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

		public Point3D AxisPointA { get; set; } = null;
		public Point3D AxisPointB { get; set; } = null;

		private List<Polyhedron> allPolyhedrons = new List<Polyhedron>();
		private int selectedPolyhedronIndex = 0;


		public Form1()
		{
			InitializeComponent();
			InitializePoints();
			viewport.EnableZBuffer(false);
			this.Resize += (s, e) =>
			{
				viewport.InitializeZBuffer(pictureBox1.Width, pictureBox1.Height);
				pictureBox1.Invalidate();
			};
			comboBoxReflection.Items.Clear();
			comboBoxReflection.Items.AddRange(["XY", "XZ", "YZ"]);
			comboBoxReflection.SelectedIndex = 0;
		}

		private void InitializePoints()
		{
			points.Clear();
			polyhedrons.Clear();

			allPolyhedrons.Clear();

			// Создаем базовые полиэдры
			var tetra = Polyhedron.CreateTetrahedron();
			tetra.Color = Color.Red;
			tetra.IsVisible = true;

			var hexa = Polyhedron.CreateHexahedron();
			hexa.Color = Color.Blue;
			hexa.IsVisible = false;

			var octa = Polyhedron.CreateOctahedron();
			octa.Color = Color.Green;
			octa.IsVisible = false;

			var ico = Polyhedron.CreateIcosahedron();
			ico.Color = Color.Orange;
			ico.IsVisible = false;

			var dodeca = Polyhedron.CreateDodecaedr();
			dodeca.Color = Color.Purple;
			dodeca.IsVisible = false;

			// Добавляем в общий список
			allPolyhedrons.AddRange(new[] { tetra, hexa, octa, ico, dodeca });


			selectedPolyhedronIndex = 0;
			currentPolyhedron = allPolyhedrons[0];

			viewport.InitializeZBuffer(pictureBox1.Width, pictureBox1.Height);

		}


		private void PictureBox1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			e.Graphics.Clear(Color.White);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			e.Graphics.Clear(Color.White);

			viewport.ClearZBuffer();

			if (viewport.IsZBufferEnabled())
			{
				DrawWithZBuffer(e.Graphics);
			}
			else
			{
				DrawWithoutZBuffer(e.Graphics);
			}

			DrawCoordinateAxes(e.Graphics);
			DrawInfo(e.Graphics);

			if (AxisPointA != null && AxisPointB != null)
			{
				PointF screenA = viewport.WorldToScreen(AxisPointA, camera, pictureBox1.Width, pictureBox1.Height);
				PointF screenB = viewport.WorldToScreen(AxisPointB, camera, pictureBox1.Width, pictureBox1.Height);

				using Pen pen = new Pen(Color.Orange, 2);
				e.Graphics.DrawLine(pen, screenA, screenB);
			}
		}

		private void DrawWithZBuffer(Graphics g)
		{
			var viewMatrix = camera.GetViewMatrix();

			// Сортируем только видимые полиэдры по удаленности от камеры
			var sortedPolyhedrons = allPolyhedrons
				.Where(p => p.IsVisible)
				.OrderByDescending(p =>
				{
					var center = p.GetCenter();
					var viewCenter = new Point3D(center.X, center.Y, center.Z);
					viewCenter.Transform(viewMatrix);
					return viewCenter.Z;
				})
				.ToList();

			foreach (var poly in sortedPolyhedrons)
			{
				poly.DrawWithZBuffer(g, camera, viewport, pictureBox1.Width, pictureBox1.Height);
			}
		}

		// добавляем проверку видимости
		private void DrawWithoutZBuffer(Graphics g)
		{
			DrawCoordinateAxes(g);

			foreach (var poly in allPolyhedrons.Where(p => p.IsVisible))
			{
				DrawPolyherdon(g, poly, pictureBox1.Width, pictureBox1.Height);
			}
		}

		public void DrawPolyherdon(Graphics g, Polyhedron polyhedron, int screenWidth, int screenHeight)
		{
			if (polyhedron == null || !polyhedron.IsVisible) return;
			if (polyhedron.Vertices.Count == 0 || polyhedron.Faces.Count == 0) return;

			// Используем цвет из полиэдра
			using (Pen pen = new Pen(polyhedron.Color, 2))
			{
				foreach (var face in polyhedron.Faces)
				{
					if (face.Count < 2) continue;

					var points = new PointF[face.Count];
					bool validFace = true;

					for (int i = 0; i < face.Count; i++)
					{
						var vertex = polyhedron.Vertices[face[i]];
						points[i] = viewport.WorldToScreen(vertex, camera, screenWidth, screenHeight);

						if (Math.Abs(points[i].X) > float.MaxValue / 2 || Math.Abs(points[i].Y) > float.MaxValue / 2)
						{
							validFace = false;
							break;
						}
					}

					if (validFace)
					{
						g.DrawPolygon(pen, points);
					}
				}
			}

			// Отрисовка вершин (опционально)
			foreach (var vertex in polyhedron.Vertices)
			{
				var screenPoint = viewport.WorldToScreen(vertex, camera, screenWidth, screenHeight);
				try
				{
					g.FillEllipse(Brushes.Black, screenPoint.X - 3, screenPoint.Y - 3, 6, 6);
				}
				catch (Exception)
				{
					// Игнорируем ошибки отрисовки
				}
			}
		}


		private void DrawArrow(Graphics g, PointF start, PointF end, Color color)
		{
			try
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
			catch (Exception)
			{
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
			 ? "Аксонометрическая" : "Перспективная";

			string zBufferInfo = viewport.IsZBufferEnabled() ? " | Z-буфер: Вкл" : " | Z-буфер: Выкл";
			string selectedObjInfo = currentPolyhedron != null ? $" | Объект: {currentPolyhedron.Name}" : "";
			string objectsCount = $" | Объектов: {allPolyhedrons.Count}";

			string modelInfo = currentPolyhedron != null ?
			   $" | Вершин: {currentPolyhedron.Vertices.Count} | Граней: {currentPolyhedron.Faces.Count}" : "";

			string info = $"Проекция: {projection}{zBufferInfo}{selectedObjInfo}{objectsCount}{modelInfo}";

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

			objectRotation = rotation * objectRotation;
		}

		private void ZoomPolyhedron(float scaleFactor)
		{
			if (currentPolyhedron == null) return;

			currentPolyhedron.Transform(Matrix4x4.CreateScaleAroundCenter(scaleFactor,currentPolyhedron.GetCenter()));

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

		private void ButtonTetr_Click(object sender, EventArgs e)
		{
			selectedPolyhedronIndex = 0;
			TogglePolyhedronVisibility(0);
		}

		private void ButtonGex_Click(object sender, EventArgs e)
		{
			selectedPolyhedronIndex = 1;
			TogglePolyhedronVisibility(1);
		}

		private void ButtonOct_Click(object sender, EventArgs e)
		{
			selectedPolyhedronIndex = 2;
			TogglePolyhedronVisibility(2);
		}

		private void ButtonIco_Click(object sender, EventArgs e)
		{
			selectedPolyhedronIndex = 3;
			TogglePolyhedronVisibility(3);
		}

		private void ButtonDod_Click(object sender, EventArgs e)
		{
			selectedPolyhedronIndex = 4;
			TogglePolyhedronVisibility(4);
		}

		// Новый метод для переключения видимости полиэдра
		private void TogglePolyhedronVisibility(int index)
		{
			if (index >= 0 && index < allPolyhedrons.Count)
			{
				// Переключаем видимость выбранного полиэдра
				allPolyhedrons[index].IsVisible = !allPolyhedrons[index].IsVisible;

				// Устанавливаем его как текущий (для операций трансформации)
				selectedPolyhedronIndex = index;
				currentPolyhedron = allPolyhedrons[index];

				pictureBox1.Invalidate();

			}
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




		//------------------------------------------------------------------------------------------
		// Кнопка копирования объекта
		private void ButtonCopyObject_Click(object sender, EventArgs e)
		{
			if (currentPolyhedron != null)
			{
				var copy = currentPolyhedron.Clone();
				// Смещаем копию для видимости
				copy.Transform(Matrix4x4.CreateTranslation(2, 2, 0));
				allPolyhedrons.Add(copy);
				polyhedrons.Add(copy); // для обратной совместимости

				selectedPolyhedronIndex = allPolyhedrons.Count - 1;
				currentPolyhedron = copy;
				pictureBox1.Invalidate();
			}
		}

		// Кнопка переключения Z-буфера
		private void ButtonToggleZBuffer_Click(object sender, EventArgs e)
		{
			viewport.EnableZBuffer(!viewport.IsZBufferEnabled());
			//buttonToggleZBuffer.Text = viewport.IsZBufferEnabled() ? "Z-буфер: Вкл" : "Z-буфер: Выкл";
			pictureBox1.Invalidate();
		}

		// Кнопка следующего объекта
		private void ButtonNextObject_Click(object sender, EventArgs e)
		{
			if (allPolyhedrons.Count == 0) return;

			selectedPolyhedronIndex = (selectedPolyhedronIndex + 1) % allPolyhedrons.Count;
			currentPolyhedron = allPolyhedrons[selectedPolyhedronIndex];
			pictureBox1.Invalidate();
		}

		// Кнопка предыдущего объекта
		private void ButtonPrevObject_Click(object sender, EventArgs e)
		{
			if (allPolyhedrons.Count == 0) return;

			selectedPolyhedronIndex = (selectedPolyhedronIndex - 1 + allPolyhedrons.Count) % allPolyhedrons.Count;
			currentPolyhedron = allPolyhedrons[selectedPolyhedronIndex];
			pictureBox1.Invalidate();
		}
	}
}