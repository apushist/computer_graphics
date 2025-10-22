using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form3 : Form
    {
        private class Edge
        {
            public PointF left;
            public PointF right;
            public Edge(PointF _left, PointF _right) { left = _left; right = _right; }
        }

        private Bitmap bmp;
        private Graphics g;
        private List<Edge> originalEdges = new List<Edge>();
        private List<Edge> displayEdges = new List<Edge>();
        private Random rnd = new Random();
        private double R;
        private const int MAX_STEPS = 10;
        private const double MIN_SEGMENT_LENGTH = 2.0;
        private int currentStep = 0;
        private Size originalPictureBoxSize;

        public Form3()
        {
            InitializeComponent();

            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            this.plusBtn.Click += new System.EventHandler(this.PlusBtn_Click);
            this.minusBtn.Click += new System.EventHandler(this.minusBtn_Click);
            this.autoGenerateBtn.Click += new System.EventHandler(this.AutoGenerate_Click);
            this.Resize += new System.EventHandler(this.Form3_Resize);

            originalPictureBoxSize = pictureBox1.Size;

            InitializeBitmap();

            initLLength.Minimum = 10;
            initLLength.Maximum = pictureBox1.Height - 10;
            initLLength.Value = pictureBox1.Height / 3;

            initRLength.Minimum = 10;
            initRLength.Maximum = pictureBox1.Height - 10;
            initRLength.Value = pictureBox1.Height / 4;
        }

        private void InitializeBitmap()
        {
            if (bmp != null)
            {
                bmp.Dispose();
            }
            if (g != null)
            {
                g.Dispose();
            }

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            pictureBox1.Refresh();
        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            RedrawOnResize();
        }

        private void RedrawOnResize()
        {
            if (originalEdges.Count > 0)
            {
                InitializeBitmap();
                ScaleEdgesToCurrentSize();
                DrawEdges();
            }
            else
            {
                InitializeBitmap();
            }

            initLLength.Maximum = pictureBox1.Height - 10;
            initRLength.Maximum = pictureBox1.Height - 10;
        }

        private void ScaleEdgesToCurrentSize()
        {
            displayEdges.Clear();

            if (originalPictureBoxSize.Width == 0 || originalPictureBoxSize.Height == 0)
                return;

            float scaleX = (float)pictureBox1.Width / originalPictureBoxSize.Width;
            float scaleY = (float)pictureBox1.Height / originalPictureBoxSize.Height;

            foreach (Edge originalEdge in originalEdges)
            {
                PointF scaledLeft = new PointF(
                    originalEdge.left.X * scaleX,
                    originalEdge.left.Y * scaleY);

                PointF scaledRight = new PointF(
                    originalEdge.right.X * scaleX,
                    originalEdge.right.Y * scaleY);

                displayEdges.Add(new Edge(scaledLeft, scaledRight));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NextStep_Click(object sender, EventArgs e)
        {
            if (currentStep >= MAX_STEPS)
            {
                MessageBox.Show($"Достигнут предел в {MAX_STEPS} шагов!", "Информация",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (originalEdges.Count == 0)
            {
                double lLength = (double)initLLength.Value;
                double rLength = (double)initRLength.Value;

                if (!double.TryParse(initRoughness.Text, out R))
                {
                    R = 0.4;
                }

                if (R < 0.1 || R > 2.0)
                {
                    MessageBox.Show("Шероховатость должна быть от 0.1 до 2.0", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    R = Math.Max(0.1, Math.Min(2.0, R));
                    initRoughness.Text = R.ToString("F1");
                }

                initLLength.Enabled = false;
                initRLength.Enabled = false;
                initRoughness.Enabled = false;

                originalPictureBoxSize = pictureBox1.Size;

                Edge first = new Edge(
                    new PointF(0, pictureBox1.Height - (float)lLength),
                    new PointF(pictureBox1.Width, pictureBox1.Height - (float)rLength));

                originalEdges.Add(first);
                displayEdges.Add(new Edge(first.left, first.right)); 
                currentStep = 1;
                DrawEdges();
            }
            else
            {
                List<Edge> scattered = new List<Edge>();
                bool addedNewSegments = false;

                foreach (Edge edge in originalEdges)
                {
                    double length = Math.Sqrt(
                        Math.Pow(edge.right.X - edge.left.X, 2) +
                        Math.Pow(edge.right.Y - edge.left.Y, 2));

                    if (length < MIN_SEGMENT_LENGTH)
                    {
                        scattered.Add(edge);
                        continue;
                    }

                    double newHeight = (edge.left.Y + edge.right.Y) / 2 +
                                     (rnd.NextDouble() - 0.5) * R * length;

                    float minY = 10;
                    float maxY = originalPictureBoxSize.Height - 10; 
                    newHeight = Math.Max(minY, Math.Min(maxY, newHeight));

                    PointF middle = new PointF(
                        (edge.left.X + edge.right.X) / 2,
                        (float)newHeight);

                    scattered.Add(new Edge(edge.left, middle));
                    scattered.Add(new Edge(middle, edge.right));
                    addedNewSegments = true;
                }

                if (!addedNewSegments && scattered.Count == originalEdges.Count)
                {
                    MessageBox.Show("Достигнута максимальная детализация!", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                originalEdges = scattered;
                ScaleEdgesToCurrentSize(); 
                currentStep++;
                DrawEdges();
            }
        }

        private void DrawEdges()
        {
            g.Clear(Color.White);

            foreach (Edge edge in displayEdges)
            {
                g.DrawLine(Pens.Black, edge.left, edge.right);
            }

            pictureBox1.Refresh();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            currentStep = 0;
            initLLength.Enabled = true;
            initRLength.Enabled = true;
            initRoughness.Enabled = true;
            originalEdges = new List<Edge>();
            displayEdges = new List<Edge>();
            InitializeBitmap();
            R = 0;
        }

        private void PlusBtn_Click(object sender, EventArgs e)
        {
            if (double.TryParse(initRoughness.Text, out double R_tmp))
            {
                R_tmp += 0.1;
                R_tmp = Math.Min(2.0, R_tmp);
                initRoughness.Text = R_tmp.ToString("F1");
            }
        }

        private void minusBtn_Click(object sender, EventArgs e)
        {
            if (double.TryParse(initRoughness.Text, out double R_tmp))
            {
                R_tmp -= 0.1;
                R_tmp = Math.Max(0.1, R_tmp);
                initRoughness.Text = R_tmp.ToString("F1");
            }
        }

        private void AutoGenerate_Click(object sender, EventArgs e)
        {
            Clear_Click(sender, e);

            for (int i = 0; i < MAX_STEPS; i++)
            {
                NextStep_Click(sender, e);

                if (currentStep >= MAX_STEPS)
                    break;

                Application.DoEvents();
                System.Threading.Thread.Sleep(200);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (bmp != null)
            {
                bmp.Dispose();
            }
            if (g != null)
            {
                g.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}