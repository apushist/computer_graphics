using System;
using System.Windows.Forms;

namespace lab6
{
    public partial class RotateAroundAxisForm : Form
    {

        private Form1 mainForm;

        public RotateAroundAxisForm(Form1 form)
        {
            InitializeComponent();
            this.mainForm = form;
            this.Text = "Вращение вокруг произвольной оси";
        }

        private void ButtonRotate_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBoxX1.Text, out double ax) ||
                !double.TryParse(textBoxY1.Text, out double ay) ||
                !double.TryParse(textBoxZ1.Text, out double az) ||
                !double.TryParse(textBoxX2.Text, out double bx) ||
                !double.TryParse(textBoxY2.Text, out double by) ||
                !double.TryParse(textBoxZ2.Text, out double bz))
            {
                MessageBox.Show("Введите корректные числа для всех координат", "Ошибка ввода");
                return;
            }

            if (!double.TryParse(textBoxAngle.Text, out double angle))
            {
                MessageBox.Show("Введите корректный угол", "Ошибка ввода");
                return;
            }

            mainForm.AxisPointA = new Point3D(ax, ay, az);
            mainForm.AxisPointB = new Point3D(bx, by, bz);

            // Выполняем вращение
            var rotation = Matrix4x4.CreateRotationAroundAxis(mainForm.AxisPointA, mainForm.AxisPointB, angle * Math.PI / 180.0);
            mainForm.currentPolyhedron.Transform(rotation);

            // Перерисовываем основное PictureBox
            mainForm.MainPictureBox.Invalidate();

        }
    }
}
