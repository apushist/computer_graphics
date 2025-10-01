namespace lab2
{
    public partial class MainForm : Form
    {
        private PointF? firstPoint = null;
        private List<Tuple<PointF, PointF>> lines = [];

        public MainForm()
        {
            InitializeComponent();
            this.Resize += (s, e) => this.Invalidate();

            modeSelector.Items.AddRange(["Обычный Ву", "Ву с шаблоном"]);
            modeSelector.SelectedIndex = 0;
            modeSelector.SelectedIndexChanged += (s, e) =>
            {
                LineAlgorithms.PatternOn = (modeSelector.SelectedIndex == 1);
                this.Invalidate();
            };
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            int midX = this.ClientSize.Width / 2;
            if (e.X >= midX) return;

            float relX = (float)e.X / this.ClientSize.Width;
            float relY = (float)e.Y / this.ClientSize.Height;
            PointF clicked = new(relX, relY);

            if (firstPoint == null)
            {
                firstPoint = clicked;
            }
            else
            {
                lines.Add(Tuple.Create(firstPoint.Value, clicked));
                firstPoint = null;
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;
            int midX = width / 2;

            e.Graphics.DrawLine(Pens.Gray, midX, 0, midX, height);

            foreach (var line in lines)
            {
                Point leftP0 = new((int)(line.Item1.X * width), (int)(line.Item1.Y * height));
                Point leftP1 = new((int)(line.Item2.X * width), (int)(line.Item2.Y * height));
                LineAlgorithms.DrawLineBresenham(e.Graphics, leftP0, leftP1, new Rectangle(0, 0, midX, height), Color.Black);

                DrawPoint(e.Graphics, leftP0, Color.Red);
                DrawPoint(e.Graphics, leftP1, Color.Red);

                LineAlgorithms.DrawLineWu(e.Graphics, leftP0, leftP1, new Rectangle(midX, 0, midX, height), Color.Black);

                DrawPoint(e.Graphics, leftP0, Color.Red, midX);
                DrawPoint(e.Graphics, leftP1, Color.Red, midX);
            }

            if (firstPoint != null)
            {
                Point p = new((int)(firstPoint.Value.X * width), (int)(firstPoint.Value.Y * height));
                DrawPoint(e.Graphics, p, Color.Red);
            }
        }

        private void DrawPoint(Graphics g, Point p, Color color, int offsetX = 0)
        {
            int size = 6;
            g.FillEllipse(new SolidBrush(color), p.X - size / 2 + offsetX, p.Y - size / 2, size, size);
        }
    }
}
