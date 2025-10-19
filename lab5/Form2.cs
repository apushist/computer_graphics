using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form2 : Form
    {
        private OpenFileDialog ofd;

        private string axiom = "F";
        private double angleDeg = 25.0;
        private double initialDirDeg = 90.0;
        private Dictionary<char, string> rules = [];
        private string currentString = "";
        private List<(PointF a, PointF b)> segments = [];

        private float baseStep = 10f;

        public Form2()
        {
            InitializeComponent();

            trackBarRandomAngle.Scroll += (s, e) => labelRandomAngle.Text = $"Разброс угла (°): {trackBarRandomAngle.Value}";
            trackBarRandomLen.Scroll += (s, e) => labelRandomLen.Text = $"Разброс длины (%): {trackBarRandomLen.Value}";

            ofd = new OpenFileDialog()
            {
                Filter = "Text files|*.txt;*.ls;*.l;*.cfg|All files|*.*"
            };

            rules.Clear();
            axiom = "F";
            angleDeg = 22.5;
            initialDirDeg = 90;
            rules['F'] = "FF-[-F+F+F]+[+F-F-F]";
        }

        private void ButtonReturn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                ParseLFile(File.ReadAllLines(ofd.FileName));
                MessageBox.Show("Файл загружен. Нажмите кнопку \"Сгенерировать\"", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при разборе файла: " + ex.Message);
            }
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            int iter = (int)numericUpDownIter.Value;
            currentString = GenerateLSystem(axiom, rules, iter);
            InterpretToSegments(currentString);
            canvas.Invalidate();
        }

        private void ButtonGenerateTree_Click(object sender, EventArgs e)
        {
            int iter = (int)numericUpDownIter.Value;
            currentString = GenerateLSystem(axiom, rules, iter);
            InterpretToSegmentsTree(currentString);
            canvas.Invalidate();
        }

        private void ParseLFile(string[] lines)
        {
            rules.Clear();
            if (lines.Length == 0) throw new Exception("Пустой файл!");

            int index = 0;
            while (index < lines.Length && string.IsNullOrWhiteSpace(lines[index])) index++;
            if (index >= lines.Length) throw new Exception("Нет данных!");

            var fisrt = lines[index].Trim();
            var parts = fisrt.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 1) throw new Exception("Первая строка должна содержать аксиому");
            axiom = parts[0];

            if (parts.Length >= 2 && double.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double ang))
                angleDeg = ang;
            if (parts.Length >= 3 && double.TryParse(parts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dir))
                initialDirDeg = dir;

            for (int i = index + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string left;
                string right;
                if (line.Contains("->"))
                {
                    var sp = line.Split(["->"], StringSplitOptions.None);
                    left = sp[0].Trim();
                    right = sp[1].Trim();
                }
                else
                {
                    var sp = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
                    if (sp.Length >= 2)
                    {
                        left = sp[0].Trim();
                        right = sp[1].Trim();
                    }
                    else if (sp.Length == 1 && sp[0].Length >= 2)
                    {
                        left = sp[0][0].ToString();
                        right = sp[1].Substring(1);
                    }
                    else
                    {
                        throw new Exception($"Не распознана запись правила: '{line}'");
                    }
                }
                if (left.Length != 1) throw new Exception($"Левая часть правила должна содержать один символ: {left}");
                var key = left[0];
                rules[key] = right;
            }
        }

        private string GenerateLSystem(string axiom, Dictionary<char, string> rules, int iterations)
        {
            var cur = axiom;
            for (int k = 0; k < iterations; k++)
            {
                var sb = new System.Text.StringBuilder();
                foreach (char c in cur)
                {
                    if (rules.TryGetValue(c, out var rep))
                        sb.Append(rep);
                    else
                        sb.Append(c);
                }
                cur = sb.ToString();
                if (cur.Length > 20_000_000)
                {
                    MessageBox.Show($"Строка стала слишком длинной на итерации {k}. Остановка.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
            return cur;
        }

        private void InterpretToSegments(string commands)
        {
            segments.Clear();

            var stack = new Stack<(float x, float y, double angle)>();

            float x = 0f, y = 0f;
            double angle = initialDirDeg * Math.PI / 180.0;

            float step = baseStep;

            var rnd = new Random();
            bool useRandom = checkBoxRandom.Checked;
            double maxAngleJitter = trackBarRandomAngle.Value;
            double maxLenJitter = trackBarRandomLen.Value;

            foreach (char c in commands)
            {
                if (c == 'F' || c == 'f')
                {
                    double angleNoise = 0.0;
                    double lenMultiplier = 1.0;

                    if (useRandom)
                    {
                        angleNoise = (rnd.NextDouble() * 2 - 1) * (maxAngleJitter * Math.PI / 180.0);
                        lenMultiplier = 1.0 + (rnd.NextDouble() * 2 - 1) * (maxLenJitter / 100.0);
                    }

                    double localAngle = angle + angleNoise;

                    float newX = x + (float)(Math.Cos(localAngle) * step * lenMultiplier);
                    float newY = y + (float)(Math.Sin(localAngle) * step * lenMultiplier);

                    if (c == 'F')
                        segments.Add((new PointF(x, y), new PointF(newX, newY)));

                    x = newX;
                    y = newY;
                }
                else if (c == '+')
                {
                    angle += (angleDeg * Math.PI / 180.0);
                }
                else if (c == '-')
                {
                    angle -= (angleDeg * Math.PI / 180.0);
                }
                else if (c == '[')
                {
                    stack.Push((x, y, angle));
                }
                else if (c == ']')
                {
                    if (stack.Count > 0)
                    {
                        var st = stack.Pop();
                        x = st.x;
                        y = st.y;
                        angle = st.angle;
                    }
                }
            }
        }

        private void InterpretToSegmentsTree(string commands)
        {
            segments.Clear();

            var stack = new Stack<(float x, float y, double angle, int depth)>();

            float x = 0f, y = 0f;
            double angle = initialDirDeg * Math.PI / 180.0;
            float step = baseStep;

            int depth = 0;
            int maxDepth = 0;

            foreach (char c in commands)
            {
                if (c == '[') depth++;
                else if (c == ']') depth--;
                maxDepth = Math.Max(maxDepth, depth);
            }

            var rnd = new Random();
            bool useRandom = checkBoxRandom.Checked;
            double maxAngleJitter = trackBarRandomAngle.Value;
            double maxLenJitter = trackBarRandomLen.Value;

            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) { }

            var lines = new List<(PointF a, PointF b, int depth)>();

            depth = 0;
            foreach (char c in commands)
            {
                if (c == 'F' || c == 'f')
                {
                    double angleNoise = 0.0;
                    double lenMultiplier = 1.0;
                    if (useRandom)
                    {
                        angleNoise = (rnd.NextDouble() * 2 - 1) * (maxAngleJitter * Math.PI / 180.0);
                        lenMultiplier = 1.0 + (rnd.NextDouble() * 2 - 1) * (maxLenJitter / 100.0);
                    }

                    double localAngle = angle + angleNoise;

                    float newX = x + (float)(Math.Cos(localAngle) * step * lenMultiplier);
                    float newY = y + (float)(Math.Sin(localAngle) * step * lenMultiplier);

                    if (c == 'F')
                        lines.Add((new PointF(x, y), new PointF(newX, newY), depth));

                    x = newX;
                    y = newY;
                }
                else if (c == '+')
                    angle += (angleDeg * Math.PI / 180.0);
                else if (c == '-')
                    angle -= (angleDeg * Math.PI / 180.0);
                else if (c == '[')
                {
                    stack.Push((x, y, angle, depth));
                    depth++;
                }
                else if (c == ']')
                {
                    if (stack.Count > 0)
                    {
                        var st = stack.Pop();
                        x = st.x;
                        y = st.y;
                        angle = st.angle;
                        depth = st.depth;
                    }
                }
            }

            segments.Clear();
            segments.AddRange(lines.Select(l => (l.a, l.b)));

            canvas.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.Clear(Color.White);

                if (lines.Count == 0) return;

                var xs = lines.SelectMany(s => new[] { s.a.X, s.b.X });
                var ys = lines.SelectMany(s => new[] { s.a.Y, s.b.Y });
                float minX = xs.Min(), maxX = xs.Max(), minY = ys.Min(), maxY = ys.Max();
                float w = maxX - minX, h = maxY - minY;
                if (w == 0) w = 1; if (h == 0) h = 1;
                float pad = 20;
                float scale = Math.Min((canvas.Width - 2 * pad) / w, (canvas.Height - 2 * pad) / h);
                float offsetX = pad - minX * scale + (canvas.Width - 2 * pad - w * scale) / 2;
                float offsetY = pad - minY * scale + (canvas.Height - 2 * pad - h * scale) / 2;

                foreach (var seg in lines)
                {
                    float t = maxDepth == 0 ? 0 : (float)seg.depth / maxDepth;
                    int r = (int)(139 + (0 - 139) * t);
                    int g = (int)(69 + (128 - 69) * t);
                    int b = (int)(19 + (0 - 19) * t);
                    float thickness = Math.Max(1f, 5f * (1f - t));

                    using (var pen = new Pen(Color.FromArgb(r, g, b), thickness))
                    {
                        var a = new PointF(seg.a.X * scale + offsetX, seg.a.Y * scale + offsetY);
                        var bpt = new PointF(seg.b.X * scale + offsetX, seg.b.Y * scale + offsetY);
                        e.Graphics.DrawLine(pen, a, bpt);
                    }
                }
            };
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);

            if (segments.Count == 0) return;

            var xs = segments.SelectMany(s => new[] { s.a.X, s.b.X });
            var ys = segments.SelectMany(s => new[] { s.a.Y, s.b.Y });
            float minX = xs.Min(), maxX = xs.Max(), minY = ys.Min(), maxY = ys.Max();

            float w = maxX - minX;
            float h = maxY - minY;
            if (w == 0) w = 1;
            if (h == 0) h = 1;

            float pad = 20;
            float scaleX = (canvas.ClientSize.Width - 2 * pad) / w;
            float scaleY = (canvas.ClientSize.Height - 2 * pad) / h;
            float scale = Math.Min(scaleX, scaleY);

            float offsetX = pad - minX * scale + (canvas.ClientSize.Width - 2 * pad - w * scale) / 2;
            float offsetY = pad - minY * scale + (canvas.ClientSize.Height - 2 * pad - h * scale) / 2;

            using (var pen = new Pen(Color.Black, 1))
            {
                foreach (var seg in segments)
                {
                    var a = new PointF(seg.a.X * scale + offsetX, seg.a.Y * scale + offsetY);
                    var b = new PointF(seg.b.X * scale + offsetX, seg.b.Y * scale + offsetY);
                    e.Graphics.DrawLine(pen, a, b);
                }
            }
        }
    }
}
