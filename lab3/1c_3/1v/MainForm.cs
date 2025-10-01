namespace _1v
{
    public partial class MainForm : Form
    {
        private Bitmap? _originalImage;
        private Bitmap? _image;
        private List<Point> _boundaryPoints = [];
        private const int FixedSize = 400;
        private Color _activeColor = Color.Black;
        private Color _boundaryColor = Color.Red;
        private bool _pipetteMode = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Изображения|*.png;*.jpg;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap original = new Bitmap(ofd.FileName);

                _originalImage = new Bitmap(FixedSize, FixedSize);
                using (Graphics g = Graphics.FromImage(_originalImage))
                {
                    g.DrawImage(original, 0, 0, FixedSize, FixedSize);
                }

                _image = new Bitmap(_originalImage);

                _boundaryPoints.Clear();
                _pipetteMode = false;
                Invalidate();
            }
        }

        private void BtnColor_Click(object? sender, EventArgs e)
        {
            using var cd = new ColorDialog();
            cd.Color = _boundaryColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                _boundaryColor = cd.Color;
                MessageBox.Show($"Цвет границы изменён на: {_boundaryColor}");
            }
        }

        private void BtnPipette_Click(object? sender, EventArgs e)
        {
            if (_image == null) return;
            _pipetteMode = true;
            MessageBox.Show("Кликните на изображении, чтобы выбрать цвет пипеткой.");
        }

        private void BtnReset_Click(object? sender, EventArgs e)
        {
            if (_originalImage != null)
            {
                _image = new Bitmap(_originalImage);
                _boundaryPoints.Clear();
                _pipetteMode = false;
                Invalidate();
            }
        }

        private void BtnTrace_Click(object? sender, EventArgs e)
        {
            if (_image == null) return;

            Point? start = BoundaryTracer.FindStartPoint(_image, _activeColor);
            if (start == null)
            {
                MessageBox.Show("Не найдена область выбранного цвета.");
                return;
            }

            var newBoundary = BoundaryTracer.TraceBoundary(_image, start.Value, _activeColor);

            using (Graphics g = Graphics.FromImage(_image))
            using (Brush brush = new SolidBrush(_boundaryColor))
            {
                foreach (var p in newBoundary)
                {
                    g.FillEllipse(brush, p.X - 1, p.Y - 1, 3, 3);
                }
            }

            _boundaryPoints.AddRange(newBoundary);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_pipetteMode && _image != null)
            {
                int x = e.X - 10;
                int y = e.Y - 60;

                if (x >= 0 && y >= 0 && x < _image.Width && y < _image.Height)
                {
                    _activeColor = _image.GetPixel(x, y);
                    _pipetteMode = false;
                    MessageBox.Show($"Цвет выбран пипеткой: {_activeColor}");
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_image != null)
            {
                e.Graphics.DrawImage(_image, 10, 60, FixedSize, FixedSize);
            }
        }
    }
}
