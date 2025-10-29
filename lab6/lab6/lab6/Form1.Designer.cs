﻿namespace lab6
{
    partial class Form1
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
			pictureBox1 = new PictureBox();
			btnZoomIn = new Button();
			btnZoomOut = new Button();
			btnResetView = new Button();
			btnSwitchProjection = new Button();
			buttonTrans = new Button();
			buttonTetr = new Button();
			buttonGex = new Button();
			buttonOct = new Button();
			buttonIco = new Button();
			buttonDod = new Button();
			buttonRefl = new Button();
			reflOptionsBox = new ListBox();
			textBox1 = new TextBox();
			textBox2 = new TextBox();
			textBoxX = new TextBox();
			textBoxY = new TextBox();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			panel1 = new Panel();
			buttonRotateAroundAxis = new Button();
			comboBoxReflection = new ComboBox();
			label8 = new Label();
			buttonRotate = new Button();
			comboBoxRotateAxis = new ComboBox();
			label7 = new Label();
			textBoxAngle = new TextBox();
			label6 = new Label();
			label5 = new Label();
			label4 = new Label();
			textBoxTransY = new TextBox();
			textBoxTransZ = new TextBox();
			textBoxTransX = new TextBox();
			tableLayoutPanel1 = new TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			panel1.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pictureBox1.BackColor = Color.White;
			pictureBox1.Location = new Point(-4, 58);
			pictureBox1.Margin = new Padding(6, 5, 6, 5);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(1129, 732);
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			pictureBox1.Paint += PictureBox1_Paint;
			pictureBox1.MouseDown += PictureBox1_MouseDown;
			pictureBox1.MouseMove += PictureBox1_MouseMove;
			pictureBox1.MouseUp += PictureBox1_MouseUp;
			pictureBox1.MouseWheel += PictureBox1_MouseWheel;
			// 
			// btnZoomIn
			// 
			btnZoomIn.Font = new Font("Segoe UI", 15F);
			btnZoomIn.Location = new Point(174, 322);
			btnZoomIn.Margin = new Padding(6, 5, 6, 5);
			btnZoomIn.Name = "btnZoomIn";
			btnZoomIn.Size = new Size(50, 50);
			btnZoomIn.TabIndex = 0;
			btnZoomIn.Text = "+";
			btnZoomIn.UseVisualStyleBackColor = true;
			btnZoomIn.Click += BtnZoomIn_Click;
			// 
			// btnZoomOut
			// 
			btnZoomOut.Font = new Font("Segoe UI", 15F);
			btnZoomOut.Location = new Point(9, 322);
			btnZoomOut.Margin = new Padding(6, 5, 6, 5);
			btnZoomOut.Name = "btnZoomOut";
			btnZoomOut.Size = new Size(50, 50);
			btnZoomOut.TabIndex = 2;
			btnZoomOut.Text = "-";
			btnZoomOut.UseVisualStyleBackColor = true;
			btnZoomOut.Click += BtnZoomOut_Click;
			// 
			// btnResetView
			// 
			btnResetView.BackColor = Color.RosyBrown;
			btnResetView.Font = new Font("Segoe UI", 11F);
			btnResetView.Location = new Point(2, 740);
			btnResetView.Margin = new Padding(6, 5, 6, 5);
			btnResetView.Name = "btnResetView";
			btnResetView.Size = new Size(222, 45);
			btnResetView.TabIndex = 3;
			btnResetView.Text = "Сброс";
			btnResetView.UseVisualStyleBackColor = false;
			btnResetView.Click += BtnResetView_Click;
			// 
			// btnSwitchProjection
			// 
			btnSwitchProjection.Font = new Font("Segoe UI", 11F);
			btnSwitchProjection.Location = new Point(5, 48);
			btnSwitchProjection.Margin = new Padding(6, 5, 6, 5);
			btnSwitchProjection.Name = "btnSwitchProjection";
			btnSwitchProjection.Size = new Size(219, 41);
			btnSwitchProjection.TabIndex = 4;
			btnSwitchProjection.Text = "Аксонометрия";
			btnSwitchProjection.UseVisualStyleBackColor = true;
			btnSwitchProjection.Click += BtnSwitchProjection_Click;
			// 
			// buttonTrans
			// 
			buttonTrans.Font = new Font("Segoe UI", 11F);
			buttonTrans.Location = new Point(6, 186);
			buttonTrans.Margin = new Padding(6, 5, 6, 5);
			buttonTrans.Name = "buttonTrans";
			buttonTrans.Size = new Size(219, 45);
			buttonTrans.TabIndex = 5;
			buttonTrans.Text = "Сместить";
			buttonTrans.UseVisualStyleBackColor = true;
			buttonTrans.Click += ButtonTrans_Click;
			// 
			// buttonTetr
			// 
			buttonTetr.Font = new Font("Segoe UI", 12F);
			buttonTetr.Location = new Point(6, 5);
			buttonTetr.Margin = new Padding(6, 5, 6, 5);
			buttonTetr.Name = "buttonTetr";
			buttonTetr.Size = new Size(212, 42);
			buttonTetr.TabIndex = 6;
			buttonTetr.Text = "Тетраэдр";
			buttonTetr.TextAlign = ContentAlignment.TopCenter;
			buttonTetr.UseVisualStyleBackColor = true;
			buttonTetr.Click += ButtonTetr_Click;
			// 
			// buttonGex
			// 
			buttonGex.Font = new Font("Segoe UI", 12F);
			buttonGex.Location = new Point(232, 5);
			buttonGex.Margin = new Padding(6, 5, 6, 5);
			buttonGex.Name = "buttonGex";
			buttonGex.Size = new Size(212, 42);
			buttonGex.TabIndex = 7;
			buttonGex.Text = "Гексаэдр";
			buttonGex.UseVisualStyleBackColor = true;
			buttonGex.Click += ButtonGex_Click;
			// 
			// buttonOct
			// 
			buttonOct.Font = new Font("Segoe UI", 12F);
			buttonOct.Location = new Point(458, 5);
			buttonOct.Margin = new Padding(6, 5, 6, 5);
			buttonOct.Name = "buttonOct";
			buttonOct.Size = new Size(212, 42);
			buttonOct.TabIndex = 8;
			buttonOct.Text = "Октаэдр";
			buttonOct.UseVisualStyleBackColor = true;
			buttonOct.Click += ButtonOct_Click;
			// 
			// buttonIco
			// 
			buttonIco.Font = new Font("Segoe UI", 12F);
			buttonIco.Location = new Point(684, 5);
			buttonIco.Margin = new Padding(6, 5, 6, 5);
			buttonIco.Name = "buttonIco";
			buttonIco.Size = new Size(212, 42);
			buttonIco.TabIndex = 9;
			buttonIco.Text = "Икосаэдр";
			buttonIco.UseVisualStyleBackColor = true;
			buttonIco.Click += ButtonIco_Click;
			// 
			// buttonDod
			// 
			buttonDod.Font = new Font("Segoe UI", 12F);
			buttonDod.Location = new Point(910, 5);
			buttonDod.Margin = new Padding(6, 5, 6, 5);
			buttonDod.Name = "buttonDod";
			buttonDod.Size = new Size(214, 42);
			buttonDod.TabIndex = 10;
			buttonDod.Text = "Додекаэдр";
			buttonDod.UseVisualStyleBackColor = true;
			buttonDod.Click += ButtonDod_Click;
			// 
			// buttonRefl
			// 
			buttonRefl.Font = new Font("Segoe UI", 11F);
			buttonRefl.Location = new Point(6, 669);
			buttonRefl.Margin = new Padding(6, 5, 6, 5);
			buttonRefl.Name = "buttonRefl";
			buttonRefl.Size = new Size(215, 45);
			buttonRefl.TabIndex = 11;
			buttonRefl.Text = "Отразить";
			buttonRefl.UseVisualStyleBackColor = true;
			buttonRefl.Click += ButtonRefl_Click;
			// 
			// reflOptionsBox
			// 
			reflOptionsBox.Location = new Point(0, 0);
			reflOptionsBox.Name = "reflOptionsBox";
			reflOptionsBox.Size = new Size(120, 96);
			reflOptionsBox.TabIndex = 0;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(0, 0);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(100, 31);
			textBox1.TabIndex = 0;
			// 
			// textBox2
			// 
			textBox2.Location = new Point(0, 0);
			textBox2.Name = "textBox2";
			textBox2.Size = new Size(100, 31);
			textBox2.TabIndex = 0;
			// 
			// textBoxX
			// 
			textBoxX.Location = new Point(0, 0);
			textBoxX.Name = "textBoxX";
			textBoxX.Size = new Size(100, 31);
			textBoxX.TabIndex = 0;
			// 
			// textBoxY
			// 
			textBoxY.Location = new Point(0, 0);
			textBoxY.Name = "textBoxY";
			textBoxY.Size = new Size(100, 31);
			textBoxY.TabIndex = 0;
			// 
			// label1
			// 
			label1.Font = new Font("Segoe UI", 12F);
			label1.Location = new Point(4, 11);
			label1.Margin = new Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new Size(222, 31);
			label1.TabIndex = 5;
			label1.Text = "Проекция:";
			label1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Segoe UI", 12F);
			label2.Location = new Point(26, 105);
			label2.Margin = new Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new Size(25, 32);
			label2.TabIndex = 6;
			label2.Text = "x";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new Font("Segoe UI", 12F);
			label3.Location = new Point(100, 105);
			label3.Margin = new Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new Size(26, 32);
			label3.TabIndex = 7;
			label3.Text = "y";
			// 
			// panel1
			// 
			panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			panel1.Controls.Add(buttonRotateAroundAxis);
			panel1.Controls.Add(comboBoxReflection);
			panel1.Controls.Add(label8);
			panel1.Controls.Add(buttonRotate);
			panel1.Controls.Add(comboBoxRotateAxis);
			panel1.Controls.Add(label7);
			panel1.Controls.Add(textBoxAngle);
			panel1.Controls.Add(label6);
			panel1.Controls.Add(label5);
			panel1.Controls.Add(label4);
			panel1.Controls.Add(buttonRefl);
			panel1.Controls.Add(textBoxTransY);
			panel1.Controls.Add(buttonTrans);
			panel1.Controls.Add(textBoxTransZ);
			panel1.Controls.Add(btnZoomOut);
			panel1.Controls.Add(textBoxTransX);
			panel1.Controls.Add(label3);
			panel1.Controls.Add(btnZoomIn);
			panel1.Controls.Add(label1);
			panel1.Controls.Add(btnResetView);
			panel1.Controls.Add(btnSwitchProjection);
			panel1.Controls.Add(label2);
			panel1.Location = new Point(1130, 0);
			panel1.Margin = new Padding(4, 4, 4, 4);
			panel1.Name = "panel1";
			panel1.Size = new Size(230, 790);
			panel1.TabIndex = 12;
			// 
			// buttonRotateAroundAxis
			// 
			buttonRotateAroundAxis.Font = new Font("Segoe UI", 10F);
			buttonRotateAroundAxis.Location = new Point(2, 522);
			buttonRotateAroundAxis.Margin = new Padding(6, 5, 6, 5);
			buttonRotateAroundAxis.Name = "buttonRotateAroundAxis";
			buttonRotateAroundAxis.Size = new Size(221, 46);
			buttonRotateAroundAxis.TabIndex = 25;
			buttonRotateAroundAxis.Text = "Произвольная ось";
			buttonRotateAroundAxis.UseVisualStyleBackColor = true;
			buttonRotateAroundAxis.Click += ButtonRotateAroundAxis_Click;
			// 
			// comboBoxReflection
			// 
			comboBoxReflection.FormattingEnabled = true;
			comboBoxReflection.Items.AddRange(new object[] { "XY", "XZ", "YZ" });
			comboBoxReflection.Location = new Point(6, 625);
			comboBoxReflection.Margin = new Padding(4, 4, 4, 4);
			comboBoxReflection.Name = "comboBoxReflection";
			comboBoxReflection.Size = new Size(216, 33);
			comboBoxReflection.TabIndex = 24;
			comboBoxReflection.SelectedIndexChanged += ComboBoxReflection_SelectedIndexChanged;
			// 
			// label8
			// 
			label8.Font = new Font("Segoe UI", 12F);
			label8.Location = new Point(9, 582);
			label8.Margin = new Padding(4, 0, 4, 0);
			label8.Name = "label8";
			label8.Size = new Size(218, 39);
			label8.TabIndex = 23;
			label8.Text = "Плоскость";
			label8.TextAlign = ContentAlignment.TopCenter;
			// 
			// buttonRotate
			// 
			buttonRotate.Font = new Font("Segoe UI", 11F);
			buttonRotate.Location = new Point(2, 466);
			buttonRotate.Margin = new Padding(6, 5, 6, 5);
			buttonRotate.Name = "buttonRotate";
			buttonRotate.Size = new Size(221, 46);
			buttonRotate.TabIndex = 22;
			buttonRotate.Text = "Повернуть";
			buttonRotate.UseVisualStyleBackColor = true;
			buttonRotate.Click += ButtonRotate_Click;
			// 
			// comboBoxRotateAxis
			// 
			comboBoxRotateAxis.FormattingEnabled = true;
			comboBoxRotateAxis.Items.AddRange(new object[] { "X", "Y", "Z" });
			comboBoxRotateAxis.Location = new Point(78, 422);
			comboBoxRotateAxis.Margin = new Padding(4, 4, 4, 4);
			comboBoxRotateAxis.Name = "comboBoxRotateAxis";
			comboBoxRotateAxis.Size = new Size(145, 33);
			comboBoxRotateAxis.TabIndex = 21;
			// 
			// label7
			// 
			label7.Font = new Font("Segoe UI", 10F);
			label7.Location = new Point(78, 391);
			label7.Margin = new Padding(4, 0, 4, 0);
			label7.Name = "label7";
			label7.Size = new Size(146, 29);
			label7.TabIndex = 20;
			label7.Text = "Направление";
			// 
			// textBoxAngle
			// 
			textBoxAngle.Location = new Point(4, 424);
			textBoxAngle.Margin = new Padding(4, 4, 4, 4);
			textBoxAngle.Name = "textBoxAngle";
			textBoxAngle.Size = new Size(62, 31);
			textBoxAngle.TabIndex = 19;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new Font("Segoe UI", 10F);
			label6.Location = new Point(6, 391);
			label6.Margin = new Padding(4, 0, 4, 0);
			label6.Name = "label6";
			label6.Size = new Size(54, 28);
			label6.TabIndex = 18;
			label6.Text = "Угол";
			// 
			// label5
			// 
			label5.Font = new Font("Segoe UI", 12F);
			label5.Location = new Point(2, 280);
			label5.Margin = new Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new Size(221, 38);
			label5.TabIndex = 17;
			label5.Text = "Масштаб:";
			label5.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new Font("Segoe UI", 12F);
			label4.Location = new Point(178, 105);
			label4.Margin = new Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new Size(25, 32);
			label4.TabIndex = 13;
			label4.Text = "z";
			// 
			// textBoxTransY
			// 
			textBoxTransY.Location = new Point(81, 144);
			textBoxTransY.Margin = new Padding(4, 4, 4, 4);
			textBoxTransY.Name = "textBoxTransY";
			textBoxTransY.Size = new Size(62, 31);
			textBoxTransY.TabIndex = 16;
			textBoxTransY.Text = "0";
			// 
			// textBoxTransZ
			// 
			textBoxTransZ.Location = new Point(162, 144);
			textBoxTransZ.Margin = new Padding(4, 4, 4, 4);
			textBoxTransZ.Name = "textBoxTransZ";
			textBoxTransZ.Size = new Size(62, 31);
			textBoxTransZ.TabIndex = 15;
			textBoxTransZ.Text = "0";
			// 
			// textBoxTransX
			// 
			textBoxTransX.Location = new Point(6, 144);
			textBoxTransX.Margin = new Padding(4, 4, 4, 4);
			textBoxTransX.Name = "textBoxTransX";
			textBoxTransX.Size = new Size(62, 31);
			textBoxTransX.TabIndex = 14;
			textBoxTransX.Text = "0";
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			tableLayoutPanel1.ColumnCount = 5;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			tableLayoutPanel1.Controls.Add(buttonTetr, 0, 0);
			tableLayoutPanel1.Controls.Add(buttonGex, 1, 0);
			tableLayoutPanel1.Controls.Add(buttonDod, 4, 0);
			tableLayoutPanel1.Controls.Add(buttonOct, 2, 0);
			tableLayoutPanel1.Controls.Add(buttonIco, 3, 0);
			tableLayoutPanel1.Location = new Point(-4, 0);
			tableLayoutPanel1.Margin = new Padding(4, 4, 4, 4);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 1;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Size = new Size(1130, 55);
			tableLayoutPanel1.TabIndex = 18;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1360, 790);
			Controls.Add(tableLayoutPanel1);
			Controls.Add(panel1);
			Controls.Add(pictureBox1);
			Margin = new Padding(6, 5, 6, 5);
			Name = "Form1";
			Text = "3D Viewer - Лабораторная работа 6";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnResetView;
        private System.Windows.Forms.Button btnSwitchProjection;
		private Button buttonTrans;
		private Button buttonTetr;
		private Button buttonGex;
		private Button buttonOct;
		private Button buttonIco;
		private Button buttonDod;
		private Button buttonRefl;
		private ListBox reflOptionsBox;
		private TextBox textBox1;
		private TextBox textBox2;
		private TextBox textBoxX;
		private TextBox textBoxY;
        private Label label1;
        private Label label3;
        private Label label2;
        private Panel panel1;
        private Label label4;
        private TextBox textBoxTransY;
        private TextBox textBoxTransZ;
        private TextBox textBoxTransX;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textBoxAngle;
        private Label label6;
        private Label label7;
        private ComboBox comboBoxRotateAxis;
        private ComboBox comboBoxReflection;
        private Label label8;
        private Button buttonRotate;
        private Button buttonRotateAroundAxis;
    }
}