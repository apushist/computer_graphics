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
	public partial class Form2 : Form
	{
		private Form1 mainForm;
		private Polyhedron currentPolyhedron => mainForm?.currentPolyhedron;

		public Form2(Form1 mainForm)
		{
			this.mainForm = mainForm;
			InitializeComponentCustom();
		}

		private void InitializeComponentCustom()
		{
			this.SuspendLayout();
			this.Text = "Управление преобразованиями";
			this.Size = new Size(300, 600);
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(800, 0);

			var mainPanel = new Panel
			{
				Dock = DockStyle.Fill,
				AutoScroll = true
			};

			var yOffset = 10;

			// Группа базовых преобразований
			var basicGroup = CreateGroupBox("Базовые преобразования", 10, yOffset, 260, 150);
			yOffset += 160;

			// Смещение
			AddLabeledControl(basicGroup, "Смещение X:", CreateNumericUpDown(-10, 10, 0, 1), 10, 20);
			AddLabeledControl(basicGroup, "Смещение Y:", CreateNumericUpDown(-10, 10, 0, 1), 10, 50);
			AddLabeledControl(basicGroup, "Смещение Z:", CreateNumericUpDown(-10, 10, 0, 1), 10, 80);
			var btnTranslate = new Button { Text = "Применить смещение", Width = 120, Top = 110, Left = 70 };
			btnTranslate.Click += ApplyTranslation;
			basicGroup.Controls.Add(btnTranslate);

			// Группа масштабирования
			var scaleGroup = CreateGroupBox("Масштабирование", 10, yOffset, 260, 120);
			yOffset += 130;

			AddLabeledControl(scaleGroup, "Масштаб X:", CreateNumericUpDown(0.1, 5, 1, 0.1), 10, 20);
			AddLabeledControl(scaleGroup, "Масштаб Y:", CreateNumericUpDown(0.1, 5, 1, 0.1), 10, 50);
			AddLabeledControl(scaleGroup, "Масштаб Z:", CreateNumericUpDown(0.1, 5, 1, 0.1), 10, 80);
			var btnScale = new Button { Text = "Масштаб от центра", Width = 120, Top = 80, Left = 70 };
			btnScale.Click += ApplyScale;
			scaleGroup.Controls.Add(btnScale);

			// Группа поворотов вокруг осей
			var rotationGroup = CreateGroupBox("Повороты вокруг осей", 10, yOffset, 260, 150);
			yOffset += 160;

			AddLabeledControl(rotationGroup, "Поворот X (°):", CreateNumericUpDown(-180, 180, 0, 15), 10, 20);
			AddLabeledControl(rotationGroup, "Поворот Y (°):", CreateNumericUpDown(-180, 180, 0, 15), 10, 50);
			AddLabeledControl(rotationGroup, "Поворот Z (°):", CreateNumericUpDown(-180, 180, 0, 15), 10, 80);
			var btnRotate = new Button { Text = "Применить повороты", Width = 120, Top = 110, Left = 70 };
			btnRotate.Click += ApplyRotation;
			rotationGroup.Controls.Add(btnRotate);

			// Группа отражений
			var reflectionGroup = CreateGroupBox("Отражение", 10, yOffset, 260, 80);
			yOffset += 90;

			var btnReflectXY = new Button { Text = "XY плоскость", Width = 70, Top = 20, Left = 10 };
			var btnReflectXZ = new Button { Text = "XZ плоскость", Width = 70, Top = 20, Left = 90 };
			var btnReflectYZ = new Button { Text = "YZ плоскость", Width = 70, Top = 20, Left = 170 };

			btnReflectXY.Click += (s, e) => ApplyReflection("XY");
			btnReflectXZ.Click += (s, e) => ApplyReflection("XZ");
			btnReflectYZ.Click += (s, e) => ApplyReflection("YZ");

			reflectionGroup.Controls.AddRange(new Control[] { btnReflectXY, btnReflectXZ, btnReflectYZ });

			// Группа поворота вокруг произвольной оси
			var arbitraryAxisGroup = CreateGroupBox("Поворот вокруг произвольной оси", 10, yOffset, 260, 200);

			AddLabeledControl(arbitraryAxisGroup, "Точка A X:", CreateNumericUpDown(-5, 5, 0, 0.5), 10, 20);
			AddLabeledControl(arbitraryAxisGroup, "Точка A Y:", CreateNumericUpDown(-5, 5, 0, 0.5), 10, 50);
			AddLabeledControl(arbitraryAxisGroup, "Точка A Z:", CreateNumericUpDown(-5, 5, 0, 0.5), 10, 80);

			AddLabeledControl(arbitraryAxisGroup, "Точка B X:", CreateNumericUpDown(-5, 5, 1, 0.5), 130, 20);
			AddLabeledControl(arbitraryAxisGroup, "Точка B Y:", CreateNumericUpDown(-5, 5, 1, 0.5), 130, 50);
			AddLabeledControl(arbitraryAxisGroup, "Точка B Z:", CreateNumericUpDown(-5, 5, 1, 0.5), 130, 80);

			AddLabeledControl(arbitraryAxisGroup, "Угол (°):", CreateNumericUpDown(-180, 180, 45, 15), 10, 110);

			var btnArbitraryRotate = new Button { Text = "Повернуть вокруг оси", Width = 140, Top = 140, Left = 60 };
			btnArbitraryRotate.Click += ApplyArbitraryRotation;
			arbitraryAxisGroup.Controls.Add(btnArbitraryRotate);

			// Сборка интерфейса
			mainPanel.Controls.AddRange(new Control[] {
			basicGroup, scaleGroup, rotationGroup, reflectionGroup, arbitraryAxisGroup
		});
			this.Controls.Add(mainPanel);
			this.ResumeLayout();
		}

		// Вспомогательные методы создания элементов управления
		private GroupBox CreateGroupBox(string text, int x, int y, int width, int height)
		{
			return new GroupBox
			{
				Text = text,
				Location = new Point(x, y),
				Size = new Size(width, height)
			};
		}

		private void AddLabeledControl(Control parent, string labelText, Control control, int x, int y)
		{
			var label = new Label
			{
				Text = labelText,
				Location = new Point(x, y),
				Width = 80,
				Height = 20
			};

			control.Location = new Point(x + 85, y);
			control.Width = 60;

			parent.Controls.Add(label);
			parent.Controls.Add(control);
		}

		private NumericUpDown CreateNumericUpDown(decimal min, decimal max, decimal value, decimal increment)
		{
			return new NumericUpDown
			{
				Minimum = min,
				Maximum = max,
				Value = value,
				Increment = increment,
				DecimalPlaces = 2
			};
		}

		// Обработчики преобразований
		private void ApplyTranslation(object sender, EventArgs e)
		{
			if (currentPolyhedron == null) return;

			decimal dx = ((NumericUpDown)FindControl(this, "Смещение X:")).Value;
			decimal dy = ((NumericUpDown)FindControl(this, "Смещение Y:")).Value;
			decimal dz = ((NumericUpDown)FindControl(this, "Смещение Z:")).Value;

			var translation = Matrix4x4.CreateTranslation((double)dx, (double)dy, (double)dz);
			currentPolyhedron.Transform(translation);
			mainForm.RefreshView();
		}

		private void ApplyScale(object sender, EventArgs e)
		{
			if (currentPolyhedron == null) return;

			decimal sx = ((NumericUpDown)FindControl(this, "Масштаб X:")).Value;
			decimal sy = ((NumericUpDown)FindControl(this, "Масштаб Y:")).Value;
			decimal sz = ((NumericUpDown)FindControl(this, "Масштаб Z:")).Value;

			var scale = Matrix4x4.CreateScaleAroundCenter((double)sx, (double)sy, (double)sz,
														 currentPolyhedron.Center);
			currentPolyhedron.Transform(scale);
			mainForm.RefreshView();
		}

		private void ApplyRotation(object sender, EventArgs e)
		{
			if (currentPolyhedron == null) return;

			decimal rx = ((NumericUpDown)FindControl(this, "Поворот X (°):")).Value;
			decimal ry = ((NumericUpDown)FindControl(this, "Поворот Y (°):")).Value;
			decimal rz = ((NumericUpDown)FindControl(this, "Поворот Z (°):")).Value;

			// Поворот относительно центра
			var toCenter = Matrix4x4.CreateTranslation(-currentPolyhedron.Center.X,
													  -currentPolyhedron.Center.Y,
													  -currentPolyhedron.Center.Z);

			var rotX = Matrix4x4.CreateRotationX(MathUtils.DegreesToRadians((double)rx));
			var rotY = Matrix4x4.CreateRotationY(MathUtils.DegreesToRadians((double)ry));
			var rotZ = Matrix4x4.CreateRotationZ(MathUtils.DegreesToRadians((double)rz));

			var fromCenter = Matrix4x4.CreateTranslation(currentPolyhedron.Center.X,
														currentPolyhedron.Center.Y,
														currentPolyhedron.Center.Z);

			var rotation = fromCenter * rotZ * rotY * rotX * toCenter;
			currentPolyhedron.Transform(rotation);
			mainForm.RefreshView();
		}

		private void ApplyReflection(string plane)
		{
			if (currentPolyhedron == null) return;

			var reflection = Matrix4x4.CreateReflection(plane);
			currentPolyhedron.Transform(reflection);
			mainForm.RefreshView();
		}

		private void ApplyArbitraryRotation(object sender, EventArgs e)
		{
			if (currentPolyhedron == null) return;

			decimal ax = ((NumericUpDown)FindControl(this, "Точка A X:")).Value;
			decimal ay = ((NumericUpDown)FindControl(this, "Точка A Y:")).Value;
			decimal az = ((NumericUpDown)FindControl(this, "Точка A Z:")).Value;

			decimal bx = ((NumericUpDown)FindControl(this, "Точка B X:")).Value;
			decimal by = ((NumericUpDown)FindControl(this, "Точка B Y:")).Value;
			decimal bz = ((NumericUpDown)FindControl(this, "Точка B Z:")).Value;

			decimal angle = ((NumericUpDown)FindControl(this, "Угол (°):")).Value;

			var pointA = new Point3D((double)ax, (double)ay, (double)az);
			var pointB = new Point3D((double)bx, (double)by, (double)bz);

			var rotation = Matrix4x4.CreateRotationAroundAxis(pointA, pointB,
															MathUtils.DegreesToRadians((double)angle));
			currentPolyhedron.Transform(rotation);
			mainForm.RefreshView();
		}

		// Вспомогательный метод для поиска контролов
		private Control FindControl(Control parent, string labelText)
		{
			foreach (Control control in parent.Controls)
			{
				if (control is Label label && label.Text == labelText)
				{
					// Возвращаем следующий контрол (NumericUpDown)
					int index = parent.Controls.IndexOf(control);
					return parent.Controls[index + 1];
				}

				if (control.HasChildren)
				{
					var found = FindControl(control, labelText);
					if (found != null) return found;
				}
			}
			return null;
		}
	}
}

