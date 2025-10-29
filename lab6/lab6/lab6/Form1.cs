using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        //private Button btnResetRotation;
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

        private void BtnResetRotation_Click(object sender, EventArgs e)
        {
            camera.RotateX = 30.0;
            camera.RotateY = 45.0;

            if (currentPolyhedron != null)
            {
                ResetCurrentPolyhedron();
            }

            pictureBox1.Invalidate();
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

            float maxCoord = Math.Max(screenWidth, screenHeight) * 2f;

            using (Pen pen = new Pen(Color.Black, 2))
            {
                foreach (var face in currentPolyhedron.Faces)
                {
                    if (face.Count < 2) continue;

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

                    if (validFace)
                    {
                        try
                        {
                            g.DrawPolygon(pen, points);
                        }
                        catch (Exception)
                        {
                           
                        }
                    }
                }
            }

            foreach (var vertex in currentPolyhedron.Vertices)
            {
                var screenPoint = viewport.WorldToScreen(vertex, camera, screenWidth, screenHeight);
                if (Math.Abs(screenPoint.X) <= maxCoord && Math.Abs(screenPoint.Y) <= maxCoord)
                {
                    try
                    {
                        g.FillEllipse(Brushes.Black, screenPoint.X - 3, screenPoint.Y - 3, 6, 6);
                    }
                    catch (Exception)
                    {
                    }
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
                ? "�����������������"
                : "�������������";

            string info = $"��������: {projection} | �������: {viewport.Scale:F2}x";

            g.DrawString(info, Font, Brushes.Black, 10, pictureBox1.Height - 30);
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            viewport.Zoom(zoomFactor, new System.Drawing.PointF(e.X, e.Y), pictureBox1.Width, pictureBox1.Height); pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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

        private void ResetCurrentPolyhedron()
        {
            if (currentPolyhedron == null) return;

            int index = polyhedrons.IndexOf(currentPolyhedron);
            if (index >= 0)
            {
                switch (index)
                {
                    case 0: currentPolyhedron = Polyhedron.CreateTetrahedron(); break;
                    case 1: currentPolyhedron = Polyhedron.CreateHexahedron(); break;
                    case 2: currentPolyhedron = Polyhedron.CreateOctahedron(); break;
                    case 3: currentPolyhedron = Polyhedron.CreateIcosahedron(); break;
                    case 4: currentPolyhedron = Polyhedron.CreateDodecaedr(); break;
                }
                polyhedrons[index] = currentPolyhedron;
            }

            objectRotation.MakeIdentity(); 
        }

        private void ZoomPolyhedron(float scaleFactor)
        {
            if (currentPolyhedron == null) return;

            Point3D center = currentPolyhedron.GetCenter();

            Matrix4x4 scaleMatrix =
                Matrix4x4.CreateTranslation(-center.X, -center.Y, -center.Z) *
                Matrix4x4.CreateScale(scaleFactor, scaleFactor, scaleFactor) *
                Matrix4x4.CreateTranslation(center.X, center.Y, center.Z);

            currentPolyhedron.Transform(scaleMatrix);

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
                ? "������������"
                : "�����������";

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

        //����� ���������
        private void ButtonRefl_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            var matrix = Matrix4x4.CreateReflection(chosenOption);
            currentPolyhedron.Transform(matrix);
            pictureBox1.Invalidate();
        }

        //����� ��������
        private void ButtonTrans_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            if (!double.TryParse(textBoxTransX.Text, out dx))
            {
                MessageBox.Show("������� ���������� ����� ��� �������� �� X!", "������ �����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxTransY.Text, out dy))
            {
                MessageBox.Show("������� ���������� ����� ��� �������� �� Y!", "������ �����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxTransZ.Text, out dz))
            {
                MessageBox.Show("������� ���������� ����� ��� �������� �� Z!", "������ �����",
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
                MessageBox.Show("������� ���������� ����!", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string axis = comboBoxRotateAxis.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(axis))
            {
                MessageBox.Show("�������� ��� ��������!", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                case "��� (�� ������)":
                    MessageBox.Show("��� ����� �������� ����������� ���� ��� ��������� ���� ����� ���.",
                        "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                default:
                    MessageBox.Show("����������� ���!", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}