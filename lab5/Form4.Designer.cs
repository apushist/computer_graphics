namespace lab5
{
	partial class Form4
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			button1 = new Button();
			button2 = new Button();
			button3 = new Button();
			pictureBox1 = new PictureBox();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new Point(644, 3);
			button1.Name = "button1";
			button1.Size = new Size(154, 23);
			button1.TabIndex = 2;
			button1.Text = "Вернуться в меню";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new Point(644, 32);
			button2.Name = "button2";
			button2.Size = new Size(154, 23);
			button2.TabIndex = 3;
			button2.Text = "Удалить точку";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new Point(644, 61);
			button3.Name = "button3";
			button3.Size = new Size(154, 23);
			button3.TabIndex = 4;
			button3.Text = "Очистить поле";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// pictureBox1
			// 
			pictureBox1.BackColor = SystemColors.Window;
			pictureBox1.Location = new Point(6, 4);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(632, 441);
			pictureBox1.TabIndex = 5;
			pictureBox1.TabStop = false;
			pictureBox1.Paint += pictureBox1_Paint;
			pictureBox1.MouseDown += pictureBox1_MouseDown;
			pictureBox1.MouseMove += pictureBox1_MouseMove;
			// 
			// Form4
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(pictureBox1);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "Form4";
			Text = "Кубические сплайны Безье";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Button button1;
		private Button button2;
		private Button button3;
		private PictureBox pictureBox1;
	}
}