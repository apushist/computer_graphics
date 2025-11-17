using lab6;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public class FormFunctionGraph : Form
    {
        private ComboBox comboBoxFunction;
        private TextBox textBoxXMin, textBoxXMax, textBoxYMin, textBoxYMax;
        private TextBox textBoxSteps;
        private Button btnOk, btnCancel;

        public Func<double, double, double> Function { get; private set; }
        public double XMin { get; private set; }
        public double XMax { get; private set; }
        public double YMin { get; private set; }
        public double YMax { get; private set; }
        public int Steps { get; private set; }

        public FormFunctionGraph()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Построение графика функции";
            this.Size = new Size(350, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var labelFunction = new Label { Text = "Функция f(x, y):", Location = new Point(10, 10), Size = new Size(100, 20) };
            comboBoxFunction = new ComboBox { Location = new Point(120, 10), Size = new Size(200, 20) };
            comboBoxFunction.Items.AddRange(new object[]
            {
                "z = x^2 + y^2",
                "z = sin(x) * cos(y)",
                "z = x * y",
                "z = x^2 - y^2",
                "z = sin(sqrt(x^2 + y^2))",
                "z = exp(-(x^2 + y^2))"
            });
            comboBoxFunction.SelectedIndex = 0;

            var labelXRange = new Label { Text = "X диапазон:", Location = new Point(10, 40), Size = new Size(100, 20) };
            textBoxXMin = new TextBox { Text = "-2", Location = new Point(120, 40), Size = new Size(60, 20) };
            var labelXTo = new Label { Text = "до", Location = new Point(185, 40), Size = new Size(20, 20) };
            textBoxXMax = new TextBox { Text = "2", Location = new Point(210, 40), Size = new Size(60, 20) };

            var labelYRange = new Label { Text = "Y диапазон:", Location = new Point(10, 70), Size = new Size(100, 20) };
            textBoxYMin = new TextBox { Text = "-2", Location = new Point(120, 70), Size = new Size(60, 20) };
            var labelYTo = new Label { Text = "до", Location = new Point(185, 70), Size = new Size(20, 20) };
            textBoxYMax = new TextBox { Text = "2", Location = new Point(210, 70), Size = new Size(60, 20) };

            var labelSteps = new Label { Text = "Количество разбиений:", Location = new Point(10, 100), Size = new Size(120, 20) };
            textBoxSteps = new TextBox { Text = "20", Location = new Point(140, 100), Size = new Size(60, 20) };

            btnOk = new Button { Text = "Построить", Location = new Point(80, 140), Size = new Size(80, 30) };
            btnCancel = new Button { Text = "Отмена", Location = new Point(180, 140), Size = new Size(80, 30) };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[]
            {
                labelFunction, comboBoxFunction,
                labelXRange, textBoxXMin, labelXTo, textBoxXMax,
                labelYRange, textBoxYMin, labelYTo, textBoxYMax,
                labelSteps, textBoxSteps,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBoxXMin.Text, out double xMin) ||
                !double.TryParse(textBoxXMax.Text, out double xMax) ||
                !double.TryParse(textBoxYMin.Text, out double yMin) ||
                !double.TryParse(textBoxYMax.Text, out double yMax) ||
                !int.TryParse(textBoxSteps.Text, out int steps))
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (xMin >= xMax || yMin >= yMax)
            {
                MessageBox.Show("Минимальное значение должно быть меньше максимального.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (steps < 2 || steps > 100)
            {
                MessageBox.Show("Количество разбиений должно быть от 2 до 100.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
            Steps = steps;

            Function = comboBoxFunction.SelectedIndex switch
            {
                0 => (x, y) => x * x + y * y,
                1 => (x, y) => Math.Sin(x) * Math.Cos(y),
                2 => (x, y) => x * y,
                3 => (x, y) => x * x - y * y,
                4 => (x, y) => Math.Sin(Math.Sqrt(x * x + y * y)),
                5 => (x, y) => Math.Exp(-(x * x + y * y)),
                _ => (x, y) => x * x + y * y
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
