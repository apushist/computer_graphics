using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
	public partial class Form4 : Form
	{
		private List<PointF> points = new List<PointF>();
		private PointF? selectedPoint;
		private PointF dragOffset;
		private bool isMoving = false;
		private const int NUMPOINTS = 50;

		public Form4()
		{
			InitializeComponent();
		}

		private PointF? IsNearPoint(float x, float y)
		{
			foreach (PointF p in points)
			{
				float distance = (float)Math.Sqrt(Math.Pow(p.X - x, 2) + Math.Pow(p.Y - y, 2));
				if (distance < 10)
				{
					return p;
				}
			}
			return null;
		}

		private float CalculateBezierPoint(float t, float p0, float p1, float p2, float p3)
		{
			return (p0 + t * (-3 * p0 + 3 * p1) + t * t * (3 * p0 - 6 * p1 + 3 * p2) + t * t * t * (-p0 + 3 * p1 - 3 * p2 + p3));
		}


		private void CalculateCurve(Graphics g)
		{
			if (points.Count > 3)
			{
				Pen curvePen = new Pen(Color.Blue, 2f);

				for (int segment = 0; segment < (points.Count - 1) / 3; segment++)
				{
					int startIndex = segment * 3;

					PointF p0 = points[startIndex];
					PointF p1 = points[startIndex + 1];
					PointF p2 = points[startIndex + 2];
					PointF p3 = points[startIndex + 3];


					PointF prevPoint = p0;

					for (int i = 1; i <= NUMPOINTS; i++)
					{
						float t = (float)i / NUMPOINTS;
						float x = CalculateBezierPoint(t, p0.X, p1.X, p2.X, p3.X);
						float y = CalculateBezierPoint(t, p0.Y, p1.Y, p2.Y, p3.Y);

						PointF currentPoint = new PointF(x, y);

						g.DrawLine(curvePen, prevPoint, currentPoint);

						prevPoint = currentPoint;
					}
				}

				curvePen.Dispose();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				float x = e.X;
				float y = e.Y;

				PointF? hoveredPoint = IsNearPoint(x, y);

				if (hoveredPoint != null)
				{
					if (selectedPoint == hoveredPoint)
					{
						if (isMoving)
						{
							isMoving = false;
						}
						else
						{
							isMoving = true;
							dragOffset = new PointF(x - selectedPoint.Value.X, y - selectedPoint.Value.Y);
						}
					}
					else
					{
						selectedPoint = hoveredPoint;
						isMoving = true;
						dragOffset = new PointF(x - selectedPoint.Value.X, y - selectedPoint.Value.Y);
					}
				}
				else
				{
					points.Add(new PointF(x, y));
					selectedPoint = null;
				}
			}
			else
			{
				selectedPoint = null;
			}
			pictureBox1.Invalidate();
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (selectedPoint != null && isMoving)
			{
				PointF newPoint = new PointF(e.X - dragOffset.X, e.Y - dragOffset.Y);

				int index = points.IndexOf(selectedPoint.Value);
				if (index != -1)
				{
					points[index] = newPoint;
					selectedPoint = newPoint;
				}
				pictureBox1.Invalidate();
			}
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			foreach (PointF p in points)
			{
				g.FillEllipse(Brushes.Gray, p.X - 4, p.Y - 4, 8, 8);
			}

			if (points.Count > 1)
			{
				for (int i = 0; i < points.Count - 1; i++)
				{
					g.DrawLine(Pens.Gray, points[i], points[i + 1]);
				}
			}

			if (selectedPoint != null)
			{
				g.DrawEllipse(Pens.Red, selectedPoint.Value.X - 4, selectedPoint.Value.Y - 4, 8, 8);
			}

			CalculateCurve(g);

		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (selectedPoint != null)
			{
				points.Remove(selectedPoint.Value);
				selectedPoint = null;
				pictureBox1.Invalidate();
			}
			else
			{
				MessageBox.Show("Нет выбранной точки", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			points.Clear();
			selectedPoint = null;
			pictureBox1.Invalidate();
		}
	}
}
