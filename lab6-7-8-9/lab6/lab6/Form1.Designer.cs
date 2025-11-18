namespace lab6
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
            buttonTexture = new Button();
            buttonLighting = new Button();
            buttonShading = new Button();
            buttonZB = new Button();
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
            buttonFunctionGraph = new Button();
            buttonSave = new Button();
            buttonLoad = new Button();
            buttonFigRotate = new Button();
            checkBoxPhong = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(-3, 95);
            pictureBox1.Margin = new Padding(5, 4, 5, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(903, 665);
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
            btnZoomIn.Location = new Point(139, 267);
            btnZoomIn.Margin = new Padding(5, 4, 5, 4);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new Size(40, 40);
            btnZoomIn.TabIndex = 0;
            btnZoomIn.Text = "+";
            btnZoomIn.UseVisualStyleBackColor = true;
            btnZoomIn.Click += BtnZoomIn_Click;
            // 
            // btnZoomOut
            // 
            btnZoomOut.Font = new Font("Segoe UI", 15F);
            btnZoomOut.Location = new Point(7, 267);
            btnZoomOut.Margin = new Padding(5, 4, 5, 4);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new Size(40, 40);
            btnZoomOut.TabIndex = 2;
            btnZoomOut.Text = "-";
            btnZoomOut.UseVisualStyleBackColor = true;
            btnZoomOut.Click += BtnZoomOut_Click;
            // 
            // btnResetView
            // 
            btnResetView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnResetView.BackColor = Color.RosyBrown;
            btnResetView.Font = new Font("Segoe UI", 11F);
            btnResetView.Location = new Point(2, 720);
            btnResetView.Margin = new Padding(5, 4, 5, 4);
            btnResetView.Name = "btnResetView";
            btnResetView.Size = new Size(177, 36);
            btnResetView.TabIndex = 3;
            btnResetView.Text = "Сброс";
            btnResetView.UseVisualStyleBackColor = false;
            btnResetView.Click += BtnResetView_Click;
            // 
            // btnSwitchProjection
            // 
            btnSwitchProjection.Font = new Font("Segoe UI", 11F);
            btnSwitchProjection.Location = new Point(5, 39);
            btnSwitchProjection.Margin = new Padding(5, 4, 5, 4);
            btnSwitchProjection.Name = "btnSwitchProjection";
            btnSwitchProjection.Size = new Size(175, 33);
            btnSwitchProjection.TabIndex = 4;
            btnSwitchProjection.Text = "Перспектива";
            btnSwitchProjection.UseVisualStyleBackColor = true;
            btnSwitchProjection.Click += BtnSwitchProjection_Click;
            // 
            // buttonTrans
            // 
            buttonTrans.Font = new Font("Segoe UI", 11F);
            buttonTrans.Location = new Point(5, 149);
            buttonTrans.Margin = new Padding(5, 4, 5, 4);
            buttonTrans.Name = "buttonTrans";
            buttonTrans.Size = new Size(175, 36);
            buttonTrans.TabIndex = 5;
            buttonTrans.Text = "Сместить";
            buttonTrans.UseVisualStyleBackColor = true;
            buttonTrans.Click += ButtonTrans_Click;
            // 
            // buttonTetr
            // 
            buttonTetr.Font = new Font("Segoe UI", 12F);
            buttonTetr.Location = new Point(5, 4);
            buttonTetr.Margin = new Padding(5, 4, 5, 4);
            buttonTetr.Name = "buttonTetr";
            buttonTetr.Size = new Size(169, 35);
            buttonTetr.TabIndex = 6;
            buttonTetr.Text = "Тетраэдр";
            buttonTetr.TextAlign = ContentAlignment.TopCenter;
            buttonTetr.UseVisualStyleBackColor = true;
            buttonTetr.Click += ButtonTetr_Click;
            // 
            // buttonGex
            // 
            buttonGex.Font = new Font("Segoe UI", 12F);
            buttonGex.Location = new Point(185, 4);
            buttonGex.Margin = new Padding(5, 4, 5, 4);
            buttonGex.Name = "buttonGex";
            buttonGex.Size = new Size(169, 35);
            buttonGex.TabIndex = 7;
            buttonGex.Text = "Гексаэдр";
            buttonGex.UseVisualStyleBackColor = true;
            buttonGex.Click += ButtonGex_Click;
            // 
            // buttonOct
            // 
            buttonOct.Font = new Font("Segoe UI", 12F);
            buttonOct.Location = new Point(365, 4);
            buttonOct.Margin = new Padding(5, 4, 5, 4);
            buttonOct.Name = "buttonOct";
            buttonOct.Size = new Size(169, 35);
            buttonOct.TabIndex = 8;
            buttonOct.Text = "Октаэдр";
            buttonOct.UseVisualStyleBackColor = true;
            buttonOct.Click += ButtonOct_Click;
            // 
            // buttonIco
            // 
            buttonIco.Font = new Font("Segoe UI", 12F);
            buttonIco.Location = new Point(545, 4);
            buttonIco.Margin = new Padding(5, 4, 5, 4);
            buttonIco.Name = "buttonIco";
            buttonIco.Size = new Size(169, 35);
            buttonIco.TabIndex = 9;
            buttonIco.Text = "Икосаэдр";
            buttonIco.UseVisualStyleBackColor = true;
            buttonIco.Click += ButtonIco_Click;
            // 
            // buttonDod
            // 
            buttonDod.Font = new Font("Segoe UI", 12F);
            buttonDod.Location = new Point(725, 4);
            buttonDod.Margin = new Padding(5, 4, 5, 4);
            buttonDod.Name = "buttonDod";
            buttonDod.Size = new Size(171, 35);
            buttonDod.TabIndex = 10;
            buttonDod.Text = "Додекаэдр";
            buttonDod.UseVisualStyleBackColor = true;
            buttonDod.Click += ButtonDod_Click;
            // 
            // buttonRefl
            // 
            buttonRefl.Font = new Font("Segoe UI", 11F);
            buttonRefl.Location = new Point(3, 524);
            buttonRefl.Margin = new Padding(5, 4, 5, 4);
            buttonRefl.Name = "buttonRefl";
            buttonRefl.Size = new Size(171, 36);
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
            textBox1.Size = new Size(100, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(0, 0);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 27);
            textBox2.TabIndex = 0;
            // 
            // textBoxX
            // 
            textBoxX.Location = new Point(0, 0);
            textBoxX.Name = "textBoxX";
            textBoxX.Size = new Size(100, 27);
            textBoxX.TabIndex = 0;
            // 
            // textBoxY
            // 
            textBoxY.Location = new Point(0, 0);
            textBoxY.Name = "textBoxY";
            textBoxY.Size = new Size(100, 27);
            textBoxY.TabIndex = 0;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(177, 25);
            label1.TabIndex = 5;
            label1.Text = "Проекция:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(21, 84);
            label2.Name = "label2";
            label2.Size = new Size(21, 28);
            label2.TabIndex = 6;
            label2.Text = "x";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(80, 84);
            label3.Name = "label3";
            label3.Size = new Size(22, 28);
            label3.TabIndex = 7;
            label3.Text = "y";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panel1.Controls.Add(buttonTexture);
            panel1.Controls.Add(buttonLighting);
            panel1.Controls.Add(buttonShading);
            panel1.Controls.Add(buttonZB);
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
            panel1.Location = new Point(904, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(184, 760);
            panel1.TabIndex = 12;
            // 
            // buttonTexture
            // 
            buttonTexture.Location = new Point(5, 684);
            buttonTexture.Name = "buttonTexture";
            buttonTexture.Size = new Size(172, 29);
            buttonTexture.TabIndex = 20;
            buttonTexture.Text = "Текстурирование";
            buttonTexture.UseVisualStyleBackColor = true;
            buttonTexture.Click += ButtonTexture_Click;
            // 
            // buttonLighting
            // 
            buttonLighting.Font = new Font("Segoe UI", 10F);
            buttonLighting.Location = new Point(5, 643);
            buttonLighting.Margin = new Padding(5, 4, 5, 4);
            buttonLighting.Name = "buttonLighting";
            buttonLighting.Size = new Size(171, 36);
            buttonLighting.TabIndex = 28;
            buttonLighting.Text = "Настройки света";
            buttonLighting.UseVisualStyleBackColor = true;
            buttonLighting.Click += buttonLighting_Click;
            // 
            // buttonShading
            // 
            buttonShading.Font = new Font("Segoe UI", 10F);
            buttonShading.Location = new Point(3, 568);
            buttonShading.Margin = new Padding(5, 4, 5, 4);
            buttonShading.Name = "buttonShading";
            buttonShading.Size = new Size(171, 68);
            buttonShading.TabIndex = 27;
            buttonShading.Text = "Переключить затенение";
            buttonShading.UseVisualStyleBackColor = true;
            buttonShading.Click += buttonShading_Click;
            // 
            // buttonZB
            // 
            buttonZB.Font = new Font("Segoe UI", 11F);
            buttonZB.Location = new Point(5, 193);
            buttonZB.Margin = new Padding(5, 4, 5, 4);
            buttonZB.Name = "buttonZB";
            buttonZB.Size = new Size(175, 36);
            buttonZB.TabIndex = 26;
            buttonZB.Text = "Z-буфер";
            buttonZB.UseVisualStyleBackColor = true;
            buttonZB.Click += buttonZB_Click;
            // 
            // buttonRotateAroundAxis
            // 
            buttonRotateAroundAxis.Font = new Font("Segoe UI", 10F);
            buttonRotateAroundAxis.Location = new Point(1, 417);
            buttonRotateAroundAxis.Margin = new Padding(5, 4, 5, 4);
            buttonRotateAroundAxis.Name = "buttonRotateAroundAxis";
            buttonRotateAroundAxis.Size = new Size(177, 37);
            buttonRotateAroundAxis.TabIndex = 25;
            buttonRotateAroundAxis.Text = "Произвольная ось";
            buttonRotateAroundAxis.UseVisualStyleBackColor = true;
            buttonRotateAroundAxis.Click += ButtonRotateAroundAxis_Click;
            // 
            // comboBoxReflection
            // 
            comboBoxReflection.FormattingEnabled = true;
            comboBoxReflection.Items.AddRange(new object[] { "XY", "XZ", "YZ" });
            comboBoxReflection.Location = new Point(5, 492);
            comboBoxReflection.Name = "comboBoxReflection";
            comboBoxReflection.Size = new Size(173, 28);
            comboBoxReflection.TabIndex = 24;
            comboBoxReflection.SelectedIndexChanged += ComboBoxReflection_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(1, 458);
            label8.Name = "label8";
            label8.Size = new Size(175, 31);
            label8.TabIndex = 23;
            label8.Text = "Плоскость";
            label8.TextAlign = ContentAlignment.TopCenter;
            // 
            // buttonRotate
            // 
            buttonRotate.Font = new Font("Segoe UI", 11F);
            buttonRotate.Location = new Point(1, 373);
            buttonRotate.Margin = new Padding(5, 4, 5, 4);
            buttonRotate.Name = "buttonRotate";
            buttonRotate.Size = new Size(177, 37);
            buttonRotate.TabIndex = 22;
            buttonRotate.Text = "Повернуть";
            buttonRotate.UseVisualStyleBackColor = true;
            buttonRotate.Click += ButtonRotate_Click;
            // 
            // comboBoxRotateAxis
            // 
            comboBoxRotateAxis.FormattingEnabled = true;
            comboBoxRotateAxis.Items.AddRange(new object[] { "X", "Y", "Z" });
            comboBoxRotateAxis.Location = new Point(63, 337);
            comboBoxRotateAxis.Name = "comboBoxRotateAxis";
            comboBoxRotateAxis.Size = new Size(117, 28);
            comboBoxRotateAxis.TabIndex = 21;
            // 
            // label7
            // 
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(63, 313);
            label7.Name = "label7";
            label7.Size = new Size(117, 23);
            label7.TabIndex = 20;
            label7.Text = "Направление";
            // 
            // textBoxAngle
            // 
            textBoxAngle.Location = new Point(3, 339);
            textBoxAngle.Name = "textBoxAngle";
            textBoxAngle.Size = new Size(51, 27);
            textBoxAngle.TabIndex = 19;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(5, 313);
            label6.Name = "label6";
            label6.Size = new Size(46, 23);
            label6.TabIndex = 18;
            label6.Text = "Угол";
            // 
            // label5
            // 
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(1, 233);
            label5.Name = "label5";
            label5.Size = new Size(177, 31);
            label5.TabIndex = 17;
            label5.Text = "Масштаб:";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(143, 84);
            label4.Name = "label4";
            label4.Size = new Size(21, 28);
            label4.TabIndex = 13;
            label4.Text = "z";
            // 
            // textBoxTransY
            // 
            textBoxTransY.Location = new Point(65, 115);
            textBoxTransY.Name = "textBoxTransY";
            textBoxTransY.Size = new Size(51, 27);
            textBoxTransY.TabIndex = 16;
            textBoxTransY.Text = "0";
            // 
            // textBoxTransZ
            // 
            textBoxTransZ.Location = new Point(129, 115);
            textBoxTransZ.Name = "textBoxTransZ";
            textBoxTransZ.Size = new Size(51, 27);
            textBoxTransZ.TabIndex = 15;
            textBoxTransZ.Text = "0";
            // 
            // textBoxTransX
            // 
            textBoxTransX.Location = new Point(5, 115);
            textBoxTransX.Name = "textBoxTransX";
            textBoxTransX.Size = new Size(51, 27);
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
            tableLayoutPanel1.Controls.Add(buttonFunctionGraph, 3, 1);
            tableLayoutPanel1.Controls.Add(buttonSave, 2, 1);
            tableLayoutPanel1.Controls.Add(buttonLoad, 1, 1);
            tableLayoutPanel1.Controls.Add(buttonFigRotate, 0, 1);
            tableLayoutPanel1.Controls.Add(buttonTetr, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonGex, 1, 0);
            tableLayoutPanel1.Controls.Add(buttonDod, 4, 0);
            tableLayoutPanel1.Controls.Add(buttonOct, 2, 0);
            tableLayoutPanel1.Controls.Add(buttonIco, 3, 0);
            tableLayoutPanel1.Controls.Add(checkBoxPhong, 4, 1);
            tableLayoutPanel1.Location = new Point(-3, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.Size = new Size(904, 88);
            tableLayoutPanel1.TabIndex = 18;
            // 
            // buttonFunctionGraph
            // 
            buttonFunctionGraph.Font = new Font("Segoe UI", 12F);
            buttonFunctionGraph.Location = new Point(545, 47);
            buttonFunctionGraph.Margin = new Padding(5, 4, 5, 4);
            buttonFunctionGraph.Name = "buttonFunctionGraph";
            buttonFunctionGraph.Size = new Size(169, 37);
            buttonFunctionGraph.TabIndex = 14;
            buttonFunctionGraph.Text = "Поверхность функции";
            buttonFunctionGraph.TextAlign = ContentAlignment.TopCenter;
            buttonFunctionGraph.UseVisualStyleBackColor = true;
            buttonFunctionGraph.Click += buttonFunctionGraph_Click;
            // 
            // buttonSave
            // 
            buttonSave.Font = new Font("Segoe UI", 12F);
            buttonSave.Location = new Point(365, 47);
            buttonSave.Margin = new Padding(5, 4, 5, 4);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(169, 37);
            buttonSave.TabIndex = 13;
            buttonSave.Text = "Сохранить";
            buttonSave.TextAlign = ContentAlignment.TopCenter;
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Font = new Font("Segoe UI", 12F);
            buttonLoad.Location = new Point(185, 47);
            buttonLoad.Margin = new Padding(5, 4, 5, 4);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(169, 37);
            buttonLoad.TabIndex = 12;
            buttonLoad.Text = "Загрузить";
            buttonLoad.TextAlign = ContentAlignment.TopCenter;
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonFigRotate
            // 
            buttonFigRotate.Font = new Font("Segoe UI", 10F);
            buttonFigRotate.Location = new Point(5, 47);
            buttonFigRotate.Margin = new Padding(5, 4, 5, 4);
            buttonFigRotate.Name = "buttonFigRotate";
            buttonFigRotate.Size = new Size(169, 37);
            buttonFigRotate.TabIndex = 11;
            buttonFigRotate.Text = "Фигура вращения";
            buttonFigRotate.TextAlign = ContentAlignment.TopCenter;
            buttonFigRotate.UseVisualStyleBackColor = true;
            buttonFigRotate.Click += ButtonFigRotate_Click;
            // 
            // checkBoxPhong
            // 
            checkBoxPhong.AutoSize = true;
            checkBoxPhong.Font = new Font("Segoe UI", 11F);
            checkBoxPhong.Location = new Point(723, 46);
            checkBoxPhong.Name = "checkBoxPhong";
            checkBoxPhong.Size = new Size(170, 29);
            checkBoxPhong.TabIndex = 15;
            checkBoxPhong.Text = "Шейдинг Фонга";
            checkBoxPhong.UseVisualStyleBackColor = true;
            checkBoxPhong.CheckedChanged += CheckBoxPhong_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 760);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Margin = new Padding(5, 4, 5, 4);
            Name = "Form1";
            Text = "3D Viewer - Лабораторная работа 9";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
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
        private Button buttonFigRotate;
		private Button buttonLoad;
		private Button buttonSave;
		private Button buttonFunctionGraph;
		private Button buttonZB;
		private Button buttonLighting;
		private Button buttonShading;
        private CheckBox checkBoxPhong;
        private Button buttonTexture;
    }
}