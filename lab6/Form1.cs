namespace lab6
{
	public partial class Form1 : Form
	{
		public Polyhedron currentPolyhedron;
		private Matrix4x4 viewMatrix;
		private Matrix4x4 projectionMatrix;
		private bool isPerspectiveProjection = false;

		public Form1()
		{
			InitializeComponent();
			InitializeComponentCustom();
			SetupView();
		}

		private void InitializeComponentCustom()
		{
			this.SuspendLayout();

			// PictureBox ��� ���������
			var pictureBox = new PictureBox
			{
				Dock = DockStyle.Fill,
				BackColor = Color.White,
				SizeMode = PictureBoxSizeMode.Zoom
			};
			pictureBox.Paint += PictureBox_Paint;
			this.Controls.Add(pictureBox);

			// ������ ����������
			var controlPanel = new Panel
			{
				Dock = DockStyle.Right,
				Width = 200,
				BackColor = Color.LightGray
			};

			// ������ ������ �������������
			var btnTetrahedron = new Button { Text = "��������", Top = 10, Left = 10, Width = 80 };
			var btnCube = new Button { Text = "���", Top = 40, Left = 10, Width = 80 };
			var btnOctahedron = new Button { Text = "�������", Top = 70, Left = 10, Width = 80 };

			btnTetrahedron.Click += (s, e) => SetPolyhedron(Polyhedron.CreateTetrahedron());
			btnCube.Click += (s, e) => SetPolyhedron(Polyhedron.CreateHexahedron());
			btnOctahedron.Click += (s, e) => SetPolyhedron(Polyhedron.CreateOctahedron());

			// ������������ ��������
			var btnToggleProjection = new Button { Text = "����������� ��������", Top = 110, Left = 10, Width = 120 };
			btnToggleProjection.Click += (s, e) => ToggleProjection();

			controlPanel.Controls.AddRange(new Control[] { btnTetrahedron, btnCube, btnOctahedron, btnToggleProjection });
			this.Controls.Add(controlPanel);

			var btnTransformations = new Button
			{
				Text = "��������������",
				Top = 140,
				Left = 10,
				Width = 120
			};
			btnTransformations.Click += (s, e) => ShowTransformationForm();

			// ��������� � controlPanel
			controlPanel.Controls.Add(btnTransformations);

			this.Text = "3D Viewer - ������������ ������ �6";
			this.Size = new Size(800, 600);
			this.ResumeLayout();
		}

		private void SetupView()
		{
			viewMatrix = new Matrix4x4(); // ��������� ������� ����
			projectionMatrix = Projection.CreateAxonometricProjection();

			// ��������� ������������
			SetPolyhedron(Polyhedron.CreateHexahedron());
		}

		private void SetPolyhedron(Polyhedron polyhedron)
		{
			currentPolyhedron = polyhedron;
			this.Text = $"3D Viewer - {polyhedron.Name}";
			RefreshView();
		}

		private void ToggleProjection()
		{
			isPerspectiveProjection = !isPerspectiveProjection;

			if (isPerspectiveProjection)
				projectionMatrix = Projection.CreateSimplePerspectiveProjection(5.0);
			else
				projectionMatrix = Projection.CreateAxonometricProjection();

			RefreshView();
		}

		public void RefreshView()
		{
			foreach (Control control in this.Controls)
			{
				if (control is PictureBox)
				{
					control.Invalidate();
					break;
				}
			}
		}

		private void ShowTransformationForm()
		{
			var transformForm = new Form2(this);
			transformForm.Show();
		}

		private void PictureBox_Paint(object sender, PaintEventArgs e)
		{
			if (currentPolyhedron == null) return;

			var g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// �������
			g.Clear(Color.White);

			// ��������� �������������
			var pictureBox = (PictureBox)sender;
			currentPolyhedron.Draw(g, viewMatrix, projectionMatrix,
								 pictureBox.Width, pictureBox.Height);

			// ����������� ���������� � ��������
			string projectionInfo = isPerspectiveProjection ? "������������� ��������" : "����������������� ��������";
			g.DrawString(projectionInfo, new Font("Arial", 10), Brushes.Black, 10, 10);
		}
	}
}
