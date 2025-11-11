using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public partial class FormFigureOfRevolution : Form
    {
        public char Axis => comboBoxAxis.SelectedItem.ToString()[0];
        public int Segments => (int)numericUpDownSegments.Value;

        public List<Point3D> Generatrix { get; private set; } = [];

        private double x = 1;
        private double y = 1;
        private double z = 1;

        public FormFigureOfRevolution()
        {
            InitializeComponent();

            comboBoxAxis.SelectedIndex = 0;
            numericUpDownSegments.Value = 12;

            dataGridViewPoints.Rows.Add(1, 0, 0);
            dataGridViewPoints.Rows.Add(0.8, 1, 0);
            dataGridViewPoints.Rows.Add(0.4, 2, 0);
        }

        private void ButtonAddPoint_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBoxX.Text, out x))
            {
                MessageBox.Show("Введите корректное число для X!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxY.Text, out y))
            {
                MessageBox.Show("Введите корректное число для Y!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxZ.Text, out z))
            {
                MessageBox.Show("Введите корректное число для Z!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dataGridViewPoints.Rows.Add(x, y, z);
        }

        private void ButtonRemovePoint_Click(object sender, EventArgs e)
        {
            if (dataGridViewPoints.SelectedRows.Count > 0)
                dataGridViewPoints.Rows.RemoveAt(dataGridViewPoints.SelectedRows[0].Index);
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            ofd.Title = "Выберите файл с точками образующей";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(ofd.FileName);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] parts = line.Replace(',', '.').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 3)
                        {
                            MessageBox.Show($"Строка '{line}' имеет неверный формат. Ожидается 3 числа.", "Ошибка формата", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        if (double.TryParse(parts[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double x) &&
                            double.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double y) &&
                            double.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double z))
                        {
                            dataGridViewPoints.Rows.Add(x, y, z);
                        }
                        else
                        {
                            MessageBox.Show($"Не удалось распознать числа в строке: '{line}'", "Ошибка парсинга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (DialogResult == DialogResult.OK)
            {
                Generatrix.Clear();

                foreach (DataGridViewRow row in dataGridViewPoints.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (double.TryParse(row.Cells[0].Value?.ToString(), out double x) &&
                        double.TryParse(row.Cells[1].Value?.ToString(), out double y) &&
                        double.TryParse(row.Cells[2].Value?.ToString(), out double z))
                    {
                        Generatrix.Add(new Point3D(x, y, z));
                    }
                }
            }
        }
    }
}
