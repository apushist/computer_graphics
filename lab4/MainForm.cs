using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xaml.Permissions;

namespace lab4
{
    public partial class MainForm : Form
    {
        private enum DrawMode { None, Point, Line, Polygon }
        private DrawMode currentMode = DrawMode.None;

        private const int MAX_SHAPES = 10;
        private readonly Queue<Shape> shapes = new();
        private Shape? tempShape = null;
        private Shape? selectedShape = null;

        private bool isIntersectionMode = false;
        private bool isScalingMode = false;
        private bool isLeftRightMode = false;
        private LineShape? firstEdge = null;
        private LineShape? secondEdge = null;
        private Point? intersectionPoint = null;
        private Point? leftRightPoint = null;
        private Point scalingPoint = new Point(-10, -10);
        private readonly List<LineShape> secondEdges = [];

        private Point? rotationCenter;
        private bool waitingForRotationCenter = false;
        private float rotationAngle = 30f;
        private bool checkingPointInPolygon = false;
        private Point? testPoint = null;

        public MainForm()
        {
            InitializeComponent();

            buttonPoint.Click += (s, e) => SetMode(DrawMode.Point);
            buttonLine.Click += (s, e) => SetMode(DrawMode.Line);
            buttonPolygon.Click += (s, e) => SetMode(DrawMode.Polygon);

            buttonFix.Click += (s, e) => FixPolygon();
            buttonClear.Click += (s, e) => ClearScene();
            buttonTranslate.Click += (s, e) => ApplyTranslation();
            buttonIntersection.Click += (s, e) => StartIntersectionMode();

            buttonRotatePoint.Click += buttonRotatePoint_Click;
            buttonRotateCenter.Click += buttonRotateCenter_Click;
            buttonCheckPointInPolygon.Click += buttonCheckPointInPolygon_Click;

            UpdateModeHighlight();
        }

        private void SetMode(DrawMode mode)
        {
            currentMode = mode;
            tempShape = null;
            selectedShape = null;
            UpdateModeHighlight();
            drawingBox.Invalidate();
        }

        private void ClearScene()
        {
            shapes.Clear();
            tempShape = null;
            selectedShape = null;
            firstEdge = null;
            secondEdge = null;
            secondEdges.Clear();
            intersectionPoint = null;
            currentMode = DrawMode.None;
            isIntersectionMode = false;
            labelMessage.Visible = false;
            UpdateModeHighlight();
            drawingBox.Invalidate();

            rotationCenter = null;
            waitingForRotationCenter = false;
            testPoint = null;
            checkingPointInPolygon = false;
            leftRightPoint = null;
            isScalingMode = false;
            panelScaling.Visible = false;

            labelMessage.Visible = false;
            UpdateModeHighlight();
            drawingBox.Invalidate();
        }

        private void FixPolygon()
        {
            if (tempShape is PolygonShape polygon && polygon.Points.Count > 2)
            {
                if (Geometry.Distance(polygon.Points.First(), polygon.Points.Last()) > 5)
                    polygon.Points.Add(polygon.Points.First());

                AddShape(polygon);
                tempShape = null;
                currentMode = DrawMode.None;
                UpdateModeHighlight();
            }
        }

        private void AddShape(Shape shape)
        {
            if (shapes.Count >= MAX_SHAPES)
                shapes.Dequeue();
            shapes.Enqueue(shape);
            drawingBox.Invalidate();
        }

        private void ApplyTranslation()
        {
            if (selectedShape == null)
            {
                MessageBox.Show("Сначала выберите фигуру для смещения.", "Нет выбранной фигуры",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            float dx = float.TryParse(textBoxDx.Text, out float tempDx) ? tempDx : 0;
            float dy = float.TryParse(textBoxDy.Text, out float tempDy) ? tempDy : 0;

            AffineTransform.ApplyTranslation(selectedShape, dx, dy);
            drawingBox.Invalidate();
        }

        private void UpdateModeHighlight()
        {
            buttonPoint.BackColor = SystemColors.Control;
            buttonLine.BackColor = SystemColors.Control;
            buttonPolygon.BackColor = SystemColors.Control;

            switch (currentMode)
            {
                case DrawMode.Point:
                    buttonPoint.BackColor = Color.LightGreen;
                    break;
                case DrawMode.Line:
                    buttonLine.BackColor = Color.LightGreen;
                    break;
                case DrawMode.Polygon:
                    buttonPolygon.BackColor = Color.LightGreen;
                    break;
            }
        }

        private void StartIntersectionMode()
        {
            if (isIntersectionMode)
            {
                isIntersectionMode = false;
                firstEdge = null;
                secondEdge = null;
                secondEdges.Clear();
                intersectionPoint = null;
                labelMessage.Visible = false;
                drawingBox.Invalidate();
                return;
            }

            if (selectedShape == null)
            {
                MessageBox.Show("Сначала выберите фигуру.", "Нет выбранной фигуры",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isIntersectionMode = true;
            firstEdge = null;
            secondEdge = null;
            intersectionPoint = null;
            secondEdges.Clear();
            labelMessage.Text = "Выберите ребро первой фигуры";
            labelMessage.Visible = true;
        }

        private void DrawingBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var shape in shapes)
                shape.Draw(e.Graphics);

            if (tempShape is PolygonShape polygon)
            {
                for (int i = 0; i < polygon.Points.Count; i++)
                {
                    var pt = polygon.Points[i];
                    e.Graphics.FillEllipse(Brushes.Blue, pt.X - 3, pt.Y - 3, 6, 6);
                    if (i > 0)
                        e.Graphics.DrawLine(Pens.Blue, polygon.Points[i - 1], pt);
                }
            }
            else
            {
                tempShape?.Draw(e.Graphics);
            }

            DrawSelectedShape(e.Graphics);
            DrawEdgesAndIntersection(e.Graphics);
        }

        private void DrawSelectedShape(Graphics g)
        {
            if (selectedShape == null || selectedShape.Points.Count == 0) return;

            var pts = selectedShape.Points;
            using (var pen = new Pen(Color.Red, 2))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                if (pts.Count == 1)
                    g.DrawEllipse(pen, pts[0].X - 8, pts[0].Y - 8, 16, 16);
                else if (pts.Count == 2)
                    g.DrawLine(pen, pts[0], pts[1]);
                else
                    g.DrawPolygon(pen, pts.ToArray());
            }

            foreach (var p in pts)
                g.FillEllipse(Brushes.Red, p.X - 4, p.Y - 4, 8, 8);

            var r = Geometry.GetBounds(selectedShape);
            if (r != Rectangle.Empty)
            {
                using var pen2 = new Pen(Color.Red, 1);
                pen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawRectangle(pen2, r);
            }
        }

        private void DrawEdgesAndIntersection(Graphics g)
        {
            if (firstEdge != null)
                g.DrawLine(new Pen(Color.Orange, 3), firstEdge.Points[0], firstEdge.Points[1]);

            foreach (var edge in secondEdges)
            {
                g.DrawLine(Pens.Green, edge.Points[0], edge.Points[1]);
                foreach (var pt in edge.Points)
                    g.FillEllipse(Brushes.Green, pt.X - 3, pt.Y - 3, 6, 6);
            }

            if (secondEdge != null && secondEdge.Points.Count > 0)
            {
                Point p0 = secondEdge.Points[0];
                Point p1 = secondEdge.Points.Count > 1 ? secondEdge.Points[1] : secondEdge.Points[0];
                g.DrawLine(Pens.Green, p0, p1);
                foreach (var pt in secondEdge.Points)
                    g.FillEllipse(Brushes.Green, pt.X - 3, pt.Y - 3, 6, 6);
            }

            if (intersectionPoint != null)
                g.FillEllipse(Brushes.Black, intersectionPoint.Value.X - 4, intersectionPoint.Value.Y - 4, 8, 8);

            if (leftRightPoint != null)
                g.FillEllipse(Brushes.Black, leftRightPoint.Value.X - 4, leftRightPoint.Value.Y - 4, 8, 8);

            if (panelScaling.Visible)
                g.FillEllipse(Brushes.Black, scalingPoint.X - 4, scalingPoint.Y - 4, 8, 8);

            if (testPoint.HasValue)
            {
                g.FillEllipse(Brushes.Red, testPoint.Value.X - 3, testPoint.Value.Y - 3, 6, 6);
            }

            if (rotationCenter.HasValue)
            {
                g.DrawEllipse(Pens.Green, rotationCenter.Value.X - 3, rotationCenter.Value.Y - 3, 6, 6);
            }
        }

        private void DrawingBox_MouseClick(object sender, MouseEventArgs e)
        {
            var point = e.Location;

            if (waitingForRotationCenter)
            {
                rotationCenter = point;
                if (selectedShape != null && rotationCenter.HasValue)
                {
                    AffineTransform.ApplyRotation(selectedShape, rotationAngle * (float)Math.PI / 180, rotationCenter.Value);
                    drawingBox.Invalidate();
                    labelMessage.Text = "Поворот вокруг точки выполнен";
                    labelMessage.Visible = true;
                }
                waitingForRotationCenter = false;
                return;
            }

            if (checkingPointInPolygon)
            {
                CheckPointInPolygon(point);
                return;
            }

            if (isScalingMode)
            {
                HandleScalingClick(e.Location);
                return;
            }
            labelMessage.Visible = false;
            panelScaling.Visible = false;
            leftRightPoint = null;
            if (isIntersectionMode)
            {
                HandleIntersectionClick(e.Location);
                return;
            }
            if (isLeftRightMode)
            {
                HandleLeftRightClick(e.Location);
                return;
            }


            if (currentMode == DrawMode.None)
            {
                Shape? foundShape = null;
                foreach (var shape in shapes.Reverse())
                {
                    if (shape is LineShape line && line.Points.Count == 2)
                    {
                        if (Geometry.IsNearLine(e.Location, line.Points[0], line.Points[1]))
                        {
                            foundShape = shape;
                            break;
                        }
                    }
                    else if (shape.Contains(e.Location))
                    {
                        foundShape = shape;
                        break;
                    }
                }
                selectedShape = foundShape;
                drawingBox.Invalidate();
                return;
            }

            switch (currentMode)
            {
                case DrawMode.Point:
                    AddShape(new PointShape { Points = { e.Location } });
                    currentMode = DrawMode.None;
                    UpdateModeHighlight();
                    break;

                case DrawMode.Line:
                    if (tempShape == null)
                    {
                        tempShape = new LineShape();
                        tempShape.Points.Add(e.Location);
                    }
                    else
                    {
                        tempShape.Points.Add(e.Location);
                        AddShape(tempShape);
                        tempShape = null;
                        currentMode = DrawMode.None;
                        UpdateModeHighlight();
                    }
                    using (Graphics g = drawingBox.CreateGraphics())
                        g.FillEllipse(Brushes.Blue, e.X - 3, e.Y - 3, 6, 6);
                    break;

                case DrawMode.Polygon:
                    tempShape ??= new PolygonShape();
                    tempShape.Points.Add(e.Location);
                    drawingBox.Invalidate();
                    break;
            }
        }

        private void HandleIntersectionClick(Point clickPoint)
        {
            if (firstEdge == null)
            {
                if (selectedShape is not PointShape)
                {
                    for (int i = 0; i < selectedShape.Points.Count - 1; i++)
                    {
                        if (Geometry.IsNearLine(clickPoint, selectedShape.Points[i], selectedShape.Points[i + 1]))
                        {
                            firstEdge = new LineShape { Points = { selectedShape.Points[i], selectedShape.Points[i + 1] } };
                            labelMessage.Text = "Нарисуйте второе ребро";
                            labelMessage.Visible = true;
                            drawingBox.Invalidate();
                            return;
                        }
                    }
                }
                MessageBox.Show("Кликните ближе к ребру выбранной фигуры.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            secondEdge ??= new LineShape();
            secondEdge.Points.Add(clickPoint);
            drawingBox.Invalidate();

            if (secondEdge.Points.Count == 2)
            {
                secondEdges.Add(secondEdge);
                if (Geometry.TryGetIntersection(firstEdge.Points[0], firstEdge.Points[1],
                                                     secondEdge.Points[0], secondEdge.Points[1],
                                                     out Point inter))
                {
                    intersectionPoint = inter;
                    labelMessage.Text = "Пересечение найдено!";
                    labelMessage.Visible = true;
                }
                else
                {
                    intersectionPoint = null;
                    labelMessage.Text = "Пересечения нет";
                    labelMessage.Visible = true;
                }
                isIntersectionMode = false;
                firstEdge = null;
                secondEdge = null;
                drawingBox.Invalidate();
            }
        }

        private void StartScalingMode()
        {
            if (isScalingMode)
            {
                isScalingMode = false;
                labelMessage.Visible = false;
                drawingBox.Invalidate();
                return;
            }

            if (selectedShape == null)
            {
                MessageBox.Show("Сначала выберите фигуру.", "Нет выбранной фигуры",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            isScalingMode = true;
            selectedShape.ogPoints = selectedShape.Points;

        }

        public void HandleScalingClick(Point clickPoint)
        {
            labelMessage.Text = "Введите желаемый масштаб";
            labelMessage.Visible = true;
            panelScaling.Visible = true;
            scalingPoint = clickPoint;
            drawingBox.Invalidate();
            isScalingMode = false;


        }

        private void buttonScaling_Click(object sender, EventArgs e)
        {
            StartScalingMode();
            if (isScalingMode)
            {
                labelMessage.Text = "Выберите точку, относительно которой будет происходить масштабирование";
                labelMessage.Visible = true;
            }
        }

        private void buttonScalingCenter_Click(object sender, EventArgs e)
        {
            StartScalingMode();

            if (isScalingMode)
                HandleScalingClick(selectedShape.GetCenter());
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            int scale = 100;
            if (int.TryParse(textBoxScaling.Text, out scale))
            {
                AffineTransform.ApplyScale(selectedShape, scalingPoint, 1.0f * scale / 100);
                drawingBox.Invalidate();
                labelMessage.Text = "Масштаб изменен на " + scale + "% изначального";
                labelMessage.Visible = true;
            }
            else
            {
                labelMessage.Text = "Невозможное значение масштаба";
                labelMessage.Visible = true;
            }
        }


        private void StartLeftRightMode()
        {
            if (isLeftRightMode)
            {
                isLeftRightMode = false;
                firstEdge = null;
                leftRightPoint = null;
                labelMessage.Visible = false;
                drawingBox.Invalidate();
                return;
            }

            if (selectedShape == null)
            {
                MessageBox.Show("Сначала выберите фигуру.", "Нет выбранной фигуры",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isLeftRightMode = true;
            firstEdge = null;
            leftRightPoint = null;
            labelMessage.Text = "Выберите ребро фигуры";
            labelMessage.Visible = true;
        }

        private void HandleLeftRightClick(Point clickPoint)
        {
            if (firstEdge == null)
            {
                if (selectedShape is not PointShape)
                {
                    for (int i = 0; i < selectedShape.Points.Count - 1; i++)
                    {
                        if (Geometry.IsNearLine(clickPoint, selectedShape.Points[i], selectedShape.Points[i + 1]))
                        {
                            firstEdge = new LineShape { Points = { selectedShape.Points[i], selectedShape.Points[i + 1] } };
                            labelMessage.Text = "Выберите точку";
                            labelMessage.Visible = true;
                            drawingBox.Invalidate();
                            return;
                        }
                    }
                }
                MessageBox.Show("Кликните ближе к ребру выбранной фигуры.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            leftRightPoint = clickPoint;
            drawingBox.Invalidate();
            int x0 = firstEdge.Points[1].X;
            int y0 = firstEdge.Points[1].Y;
            int xa = firstEdge.Points[0].X - x0;
            int ya = firstEdge.Points[0].Y - y0;
            int xb = leftRightPoint.Value.X - x0;
            int yb = leftRightPoint.Value.Y - y0;

            if (yb * xa - xb * ya < 0)
            {
                labelMessage.Text = "Точка лежит справа от ребра";
                labelMessage.Visible = true;
            }
            else if (yb * xa - xb * ya > 0)
            {
                labelMessage.Text = "Точка лежит слева от ребра";
                labelMessage.Visible = true;
            }
            else
            {
                labelMessage.Text = "Точка лежит на ребре";
                labelMessage.Visible = true;
            }

            isLeftRightMode = false;
            firstEdge = null;

            drawingBox.Invalidate();

        }

        private void buttonLeftRight_Click(object sender, EventArgs e)
        {
            StartLeftRightMode();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonRotatePoint_Click(object sender, EventArgs e)
        {
            if (selectedShape == null)
            {
                MessageBox.Show("Сначала выберите фигуру");
                return;
            }

            if (float.TryParse(textBoxRotationAngle.Text, out float angle))
            {
                rotationAngle = angle;
            }
            else
            {
                MessageBox.Show("Введите корректный угол поворота", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            waitingForRotationCenter = true;
            rotationCenter = null;
            labelMessage.Text = $"Выберите центр вращения (угол: {rotationAngle}°)";
            labelMessage.Visible = true;
        }

        private void buttonRotateCenter_Click(object sender, EventArgs e)
        {
            if (selectedShape == null)
            {
                MessageBox.Show("Сначала выберите фигуру");
                return;
            }

            if (float.TryParse(textBoxRotationAngle.Text, out float angle))
            {
                rotationAngle = angle;
            }
            else
            {
                MessageBox.Show("Введите корректный угол поворота", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Point center = selectedShape.GetCenter();
            AffineTransform.ApplyRotation(selectedShape, rotationAngle * (float)Math.PI / 180, center);
            drawingBox.Invalidate();
            labelMessage.Text = $"Поворот вокруг центра на {rotationAngle}° выполнен";
            labelMessage.Visible = true;
        }

        private void buttonCheckPointInPolygon_Click(object sender, EventArgs e)
        {
            checkingPointInPolygon = true;
            testPoint = null;
            labelMessage.Text = "Выберите точку для проверки принадлежности полигонам";
            labelMessage.Visible = true;
        }

        private void CheckPointInPolygon(Point point)
        {
            testPoint = point;
            bool foundInAnyPolygon = false;

            foreach (var shape in shapes)
            {
                if (shape is PolygonShape polygon && polygon.Points.Count >= 3)
                {
                    bool isInside = Geometry.IsPointInPolygon(point, polygon.Points);
                    bool isConvex = Geometry.IsConvexPolygon(polygon.Points);

                    if (isInside)
                    {
                        foundInAnyPolygon = true;
                        string polygonType = isConvex ? "выпуклого" : "невыпуклого";
                        MessageBox.Show($"Точка находится внутри {polygonType} полигона",
                                      "Результат проверки",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                }
            }

            if (!foundInAnyPolygon)
            {
                MessageBox.Show("Точка не находится внутри любого полигона",
                               "Результат проверки",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }

            checkingPointInPolygon = false;
            testPoint = null;
            drawingBox.Invalidate();
        }
    }
}
