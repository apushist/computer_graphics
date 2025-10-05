using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
        private LineShape? firstEdge = null;
        private LineShape? secondEdge = null;
        private Point? intersectionPoint = null;
        private readonly List<LineShape> secondEdges = [];

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
            labelMessage.Text = "";
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
                labelMessage.Text = "";
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
        }

        private void DrawingBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (isIntersectionMode)
            {
                HandleIntersectionClick(e.Location);
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
                if (selectedShape is PolygonShape poly)
                {
                    for (int i = 0; i < poly.Points.Count - 1; i++)
                    {
                        if (Geometry.IsNearLine(clickPoint, poly.Points[i], poly.Points[i + 1]))
                        {
                            firstEdge = new LineShape { Points = { poly.Points[i], poly.Points[i + 1] } };
                            labelMessage.Text = "Нарисуйте второе ребро";
                            drawingBox.Invalidate();
                            return;
                        }
                    }
                }
                else if (selectedShape is LineShape line && line.Points.Count == 2)
                {
                    if (Geometry.IsNearLine(clickPoint, line.Points[0], line.Points[1]))
                    {
                        firstEdge = new LineShape { Points = { line.Points[0], line.Points[1] } };
                        labelMessage.Text = "Нарисуйте второе ребро";
                        drawingBox.Invalidate();
                        return;
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
                }
                else
                {
                    intersectionPoint = null;
                    labelMessage.Text = "Пересечения нет";
                }

                secondEdge = null;
                firstEdge = null;
                drawingBox.Invalidate();
            }
        }
    }
}
