using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab6
{
    public partial class Form1 : Form
    {
        private Camera camera = new Camera();
        private Viewport viewport = new Viewport();
        private List<Point3D> points = new List<Point3D>();
        private bool isPanning = false;
        private Point lastMousePosition;
        private Button btnResetRotation;

        

        public Form1()
        {
            InitializeComponent();
            InitializeTestPoints();
        }

        private void InitializeTestPoints()
        {
            points.Clear();

            double size = 1.0;

            points.Add(new Point3D(0, size, 0));               
            points.Add(new Point3D(size, -size / 2, 0));           
            points.Add(new Point3D(-size, -size / 2, 0)); 
            points.Add(new Point3D(0, -size / 2, size));

            btnResetRotation = new Button();
            btnResetRotation.Text = "Сброс вращ";
            btnResetRotation.Size = new Size(80, 30);
            btnResetRotation.Location = new Point(255, 10);
            btnResetRotation.Click += btnResetRotation_Click;
            this.Controls.Add(btnResetRotation);

        }

        private void btnResetRotation_Click(object sender, EventArgs e)
        {
            camera.RotateX = 30.0;
            camera.RotateY = 45.0;
            pictureBox1.Invalidate();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);

            DrawCoordinateAxes(e.Graphics);
            DrawTetrahedron(e.Graphics);
            DrawInfo(e.Graphics);
        }

        private void DrawArrow(Graphics g, PointF start, PointF end, Color color)
        {
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

                using (Pen arrowPen = new Pen(color, 2))
                {
                    g.DrawLine(arrowPen, end, new PointF(leftX, leftY));
                    g.DrawLine(arrowPen, end, new PointF(rightX, rightY));
                }
            }
        }


        private void DrawCoordinateAxes(Graphics g)
        {
            Point3D origin = new Point3D(0, 0, 0);
            Point3D xAxis = new Point3D(3, 0, 0); 
            Point3D yAxis = new Point3D(0, 3, 0);
            Point3D zAxis = new Point3D(0, 0, 3);

            System.Drawing.PointF originScreen = viewport.WorldToScreen(origin, camera, pictureBox1.Width, pictureBox1.Height);
            System.Drawing.PointF xScreen = viewport.WorldToScreen(xAxis, camera, pictureBox1.Width, pictureBox1.Height);
            System.Drawing.PointF yScreen = viewport.WorldToScreen(yAxis, camera, pictureBox1.Width, pictureBox1.Height);
            System.Drawing.PointF zScreen = viewport.WorldToScreen(zAxis, camera, pictureBox1.Width, pictureBox1.Height);

            DrawArrow(g, originScreen, xScreen, Color.Red);
            DrawArrow(g, originScreen, yScreen, Color.Green);
            DrawArrow(g, originScreen, zScreen, Color.Blue);

            g.DrawString("X", this.Font, Brushes.Red, xScreen.X + 5, xScreen.Y + 5);
            g.DrawString("Y", this.Font, Brushes.Green, yScreen.X + 5, yScreen.Y + 5);
            g.DrawString("Z", this.Font, Brushes.Blue, zScreen.X + 5, zScreen.Y + 5);
        }



        private void DrawTetrahedron(Graphics g)
        {
            if (points.Count < 4) return;

            System.Drawing.PointF[] screenPoints = new System.Drawing.PointF[4];
            for (int i = 0; i < 4; i++)
            {
                screenPoints[i] = viewport.WorldToScreen(points[i], camera, pictureBox1.Width, pictureBox1.Height);
            }

            using (Pen blackPen = new Pen(Color.Black, 2))
            {
                g.DrawLine(blackPen, screenPoints[1], screenPoints[2]);
                g.DrawLine(blackPen, screenPoints[2], screenPoints[3]);
                g.DrawLine(blackPen, screenPoints[3], screenPoints[1]);

                g.DrawLine(blackPen, screenPoints[0], screenPoints[1]);
                g.DrawLine(blackPen, screenPoints[0], screenPoints[2]);
                g.DrawLine(blackPen, screenPoints[0], screenPoints[3]);
            }

            for (int i = 0; i < 4; i++)
            {
                g.FillEllipse(Brushes.Black, screenPoints[i].X - 4, screenPoints[i].Y - 4, 8, 8);
            }
        }

        private void DrawInfo(Graphics g)
        {
            string projection = camera.CurrentProjection == Camera.ProjectionType.Axonometric
                ? "Аксонометрическая"
                : "Перспективная";

            string info = $"Проекция: {projection} | Масштаб: {viewport.Scale:F2}x";

            g.DrawString(info, this.Font, Brushes.Black, 10, pictureBox1.Height - 30);
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            viewport.Zoom(zoomFactor, new System.Drawing.PointF(e.X, e.Y), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isPanning = true;
                lastMousePosition = e.Location;
                pictureBox1.Cursor = Cursors.SizeAll;
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                float deltaX = e.X - lastMousePosition.X;
                float deltaY = e.Y - lastMousePosition.Y;

                camera.Rotate(deltaX, deltaY);

                lastMousePosition = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isPanning = false;
                pictureBox1.Cursor = Cursors.Default;
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            viewport.Zoom(1.2f, new System.Drawing.PointF(pictureBox1.Width / 2, pictureBox1.Height / 2),
                pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            viewport.Zoom(0.8f, new System.Drawing.PointF(pictureBox1.Width / 2, pictureBox1.Height / 2),
                pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void btnResetView_Click(object sender, EventArgs e)
        {
            viewport.Reset();
            pictureBox1.Invalidate();
        }

        private void btnSwitchProjection_Click(object sender, EventArgs e)
        {
            camera.CurrentProjection = camera.CurrentProjection == Camera.ProjectionType.Axonometric
                ? Camera.ProjectionType.Perspective
                : Camera.ProjectionType.Axonometric;

            btnSwitchProjection.Text = camera.CurrentProjection == Camera.ProjectionType.Axonometric
                ? "Аксонометрия"
                : "Перспектива";

            pictureBox1.Invalidate();
        }
    }
}