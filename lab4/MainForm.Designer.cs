namespace lab4
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			buttonPoint = new Button();
			drawingBox = new PictureBox();
			tableLayoutPanelMode = new TableLayoutPanel();
			buttonFix = new Button();
			buttonPolygon = new Button();
			buttonLine = new Button();
			textBox1 = new TextBox();
			buttonClear = new Button();
			panel1 = new Panel();
			buttonTranslate = new Button();
			textBoxDy = new TextBox();
			textBoxDx = new TextBox();
			textBox4 = new TextBox();
			textBox3 = new TextBox();
			textBox2 = new TextBox();
			labelMessage = new Label();
			buttonIntersection = new Button();
			buttonScaling = new Button();
			buttonScalingCenter = new Button();
			buttonLeftRight = new Button();
			textBox5 = new TextBox();
			panelScaling = new Panel();
			buttonOK = new Button();
			textBoxScaling = new TextBox();
			buttonRotatePoint = new Button();
			buttonRotateCenter = new Button();
			buttonCheckPointInPolygon = new Button();
			labelRotationAngle = new Label();
			textBoxRotationAngle = new TextBox();
			label2 = new Label();
			((System.ComponentModel.ISupportInitialize)drawingBox).BeginInit();
			tableLayoutPanelMode.SuspendLayout();
			panel1.SuspendLayout();
			panelScaling.SuspendLayout();
			SuspendLayout();
			// 
			// buttonPoint
			// 
			buttonPoint.Location = new Point(4, 50);
			buttonPoint.Margin = new Padding(4);
			buttonPoint.Name = "buttonPoint";
			buttonPoint.Size = new Size(371, 38);
			buttonPoint.TabIndex = 0;
			buttonPoint.Text = "Точка";
			buttonPoint.UseVisualStyleBackColor = true;
			// 
			// drawingBox
			// 
			drawingBox.BorderStyle = BorderStyle.FixedSingle;
			drawingBox.Location = new Point(14, 15);
			drawingBox.Margin = new Padding(4);
			drawingBox.Name = "drawingBox";
			drawingBox.Size = new Size(1108, 756);
			drawingBox.TabIndex = 1;
			drawingBox.TabStop = false;
			drawingBox.Paint += DrawingBox_Paint;
			drawingBox.MouseClick += DrawingBox_MouseClick;
			// 
			// tableLayoutPanelMode
			// 
			tableLayoutPanelMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			tableLayoutPanelMode.BackColor = SystemColors.ControlDark;
			tableLayoutPanelMode.CausesValidation = false;
			tableLayoutPanelMode.ColumnCount = 1;
			tableLayoutPanelMode.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanelMode.Controls.Add(buttonFix, 0, 4);
			tableLayoutPanelMode.Controls.Add(buttonPolygon, 0, 3);
			tableLayoutPanelMode.Controls.Add(buttonLine, 0, 2);
			tableLayoutPanelMode.Controls.Add(buttonPoint, 0, 1);
			tableLayoutPanelMode.Controls.Add(textBox1, 0, 0);
			tableLayoutPanelMode.Location = new Point(1130, 15);
			tableLayoutPanelMode.Margin = new Padding(4);
			tableLayoutPanelMode.Name = "tableLayoutPanelMode";
			tableLayoutPanelMode.RowCount = 5;
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
			tableLayoutPanelMode.Size = new Size(381, 231);
			tableLayoutPanelMode.TabIndex = 2;
			// 
			// buttonFix
			// 
			buttonFix.BackColor = SystemColors.ActiveCaption;
			buttonFix.Location = new Point(4, 188);
			buttonFix.Margin = new Padding(4);
			buttonFix.Name = "buttonFix";
			buttonFix.Size = new Size(371, 39);
			buttonFix.TabIndex = 4;
			buttonFix.Text = "Зафиксировать полигон";
			buttonFix.UseVisualStyleBackColor = false;
			// 
			// buttonPolygon
			// 
			buttonPolygon.Location = new Point(4, 142);
			buttonPolygon.Margin = new Padding(4);
			buttonPolygon.Name = "buttonPolygon";
			buttonPolygon.Size = new Size(371, 38);
			buttonPolygon.TabIndex = 2;
			buttonPolygon.Text = "Полигон";
			buttonPolygon.UseVisualStyleBackColor = true;
			// 
			// buttonLine
			// 
			buttonLine.Location = new Point(4, 96);
			buttonLine.Margin = new Padding(4);
			buttonLine.Name = "buttonLine";
			buttonLine.Size = new Size(371, 38);
			buttonLine.TabIndex = 1;
			buttonLine.Text = "Линия";
			buttonLine.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			textBox1.BackColor = SystemColors.Control;
			textBox1.Location = new Point(4, 4);
			textBox1.Margin = new Padding(4);
			textBox1.Name = "textBox1";
			textBox1.ReadOnly = true;
			textBox1.Size = new Size(366, 31);
			textBox1.TabIndex = 3;
			textBox1.Text = "Режим рисования";
			textBox1.TextAlign = HorizontalAlignment.Center;
			// 
			// buttonClear
			// 
			buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonClear.BackColor = Color.RosyBrown;
			buttonClear.Location = new Point(1130, 732);
			buttonClear.Margin = new Padding(4);
			buttonClear.Name = "buttonClear";
			buttonClear.Size = new Size(371, 39);
			buttonClear.TabIndex = 3;
			buttonClear.Text = "Очистить сцену";
			buttonClear.UseVisualStyleBackColor = false;
			// 
			// panel1
			// 
			panel1.BackColor = SystemColors.ControlDark;
			panel1.Controls.Add(buttonTranslate);
			panel1.Controls.Add(textBoxDy);
			panel1.Controls.Add(textBoxDx);
			panel1.Controls.Add(textBox4);
			panel1.Controls.Add(textBox3);
			panel1.Controls.Add(textBox2);
			panel1.Location = new Point(1130, 260);
			panel1.Margin = new Padding(4);
			panel1.Name = "panel1";
			panel1.Size = new Size(371, 146);
			panel1.TabIndex = 4;
			// 
			// buttonTranslate
			// 
			buttonTranslate.BackColor = SystemColors.ActiveCaption;
			buttonTranslate.Location = new Point(212, 55);
			buttonTranslate.Margin = new Padding(4);
			buttonTranslate.Name = "buttonTranslate";
			buttonTranslate.Size = new Size(154, 75);
			buttonTranslate.TabIndex = 5;
			buttonTranslate.Text = "Сместить";
			buttonTranslate.UseVisualStyleBackColor = false;
			// 
			// textBoxDy
			// 
			textBoxDy.Location = new Point(60, 96);
			textBoxDy.Margin = new Padding(4);
			textBoxDy.Name = "textBoxDy";
			textBoxDy.Size = new Size(144, 31);
			textBoxDy.TabIndex = 4;
			// 
			// textBoxDx
			// 
			textBoxDx.Location = new Point(60, 55);
			textBoxDx.Margin = new Padding(4);
			textBoxDx.Name = "textBoxDx";
			textBoxDx.Size = new Size(144, 31);
			textBoxDx.TabIndex = 3;
			// 
			// textBox4
			// 
			textBox4.Location = new Point(11, 96);
			textBox4.Margin = new Padding(4);
			textBox4.Name = "textBox4";
			textBox4.ReadOnly = true;
			textBox4.Size = new Size(40, 31);
			textBox4.TabIndex = 2;
			textBox4.Text = "dy:";
			// 
			// textBox3
			// 
			textBox3.Location = new Point(11, 55);
			textBox3.Margin = new Padding(4);
			textBox3.Name = "textBox3";
			textBox3.ReadOnly = true;
			textBox3.Size = new Size(40, 31);
			textBox3.TabIndex = 1;
			textBox3.Text = "dx:";
			// 
			// textBox2
			// 
			textBox2.BackColor = SystemColors.Control;
			textBox2.Location = new Point(4, 4);
			textBox2.Margin = new Padding(4);
			textBox2.Name = "textBox2";
			textBox2.ReadOnly = true;
			textBox2.Size = new Size(363, 31);
			textBox2.TabIndex = 0;
			textBox2.Text = "Смещение";
			textBox2.TextAlign = HorizontalAlignment.Center;
			// 
			// labelMessage
			// 
			labelMessage.Location = new Point(21, 21);
			labelMessage.Margin = new Padding(4, 0, 4, 0);
			labelMessage.Name = "labelMessage";
			labelMessage.Size = new Size(1068, 31);
			labelMessage.TabIndex = 5;
			// 
			// buttonIntersection
			// 
			buttonIntersection.BackColor = SystemColors.ActiveCaption;
			buttonIntersection.Location = new Point(1130, 412);
			buttonIntersection.Margin = new Padding(4);
			buttonIntersection.Name = "buttonIntersection";
			buttonIntersection.Size = new Size(371, 39);
			buttonIntersection.TabIndex = 6;
			buttonIntersection.Text = "Найти пересечение";
			buttonIntersection.UseVisualStyleBackColor = false;
			// 
			// buttonScaling
			// 
			buttonScaling.BackColor = SystemColors.ActiveCaption;
			buttonScaling.Location = new Point(1130, 459);
			buttonScaling.Margin = new Padding(4);
			buttonScaling.Name = "buttonScaling";
			buttonScaling.Size = new Size(371, 39);
			buttonScaling.TabIndex = 7;
			buttonScaling.Text = "Масштабировать относительно точки";
			buttonScaling.UseVisualStyleBackColor = false;
			buttonScaling.Click += buttonScaling_Click;
			// 
			// buttonScalingCenter
			// 
			buttonScalingCenter.BackColor = SystemColors.ActiveCaption;
			buttonScalingCenter.Location = new Point(1130, 504);
			buttonScalingCenter.Margin = new Padding(4);
			buttonScalingCenter.Name = "buttonScalingCenter";
			buttonScalingCenter.Size = new Size(371, 39);
			buttonScalingCenter.TabIndex = 8;
			buttonScalingCenter.Text = "Масштабировать относительно центра";
			buttonScalingCenter.UseVisualStyleBackColor = false;
			buttonScalingCenter.Click += buttonScalingCenter_Click;
			// 
			// buttonLeftRight
			// 
			buttonLeftRight.BackColor = SystemColors.ActiveCaption;
			buttonLeftRight.Location = new Point(1130, 549);
			buttonLeftRight.Margin = new Padding(4);
			buttonLeftRight.Name = "buttonLeftRight";
			buttonLeftRight.Size = new Size(371, 39);
			buttonLeftRight.TabIndex = 9;
			buttonLeftRight.Text = "Относительное положение точки";
			buttonLeftRight.UseVisualStyleBackColor = false;
			buttonLeftRight.Click += buttonLeftRight_Click;
			// 
			// textBox5
			// 
			textBox5.Location = new Point(4, 5);
			textBox5.Margin = new Padding(4, 5, 4, 5);
			textBox5.Name = "textBox5";
			textBox5.Size = new Size(124, 31);
			textBox5.TabIndex = 10;
			textBox5.Text = "Масштаб(%):";
			textBox5.TextAlign = HorizontalAlignment.Center;
			// 
			// panelScaling
			// 
			panelScaling.BackColor = SystemColors.ControlDark;
			panelScaling.BorderStyle = BorderStyle.FixedSingle;
			panelScaling.Controls.Add(buttonOK);
			panelScaling.Controls.Add(textBoxScaling);
			panelScaling.Controls.Add(textBox5);
			panelScaling.Location = new Point(852, 719);
			panelScaling.Margin = new Padding(4, 5, 4, 5);
			panelScaling.Name = "panelScaling";
			panelScaling.Size = new Size(270, 52);
			panelScaling.TabIndex = 11;
			panelScaling.Visible = false;
			// 
			// buttonOK
			// 
			buttonOK.BackColor = SystemColors.ActiveBorder;
			buttonOK.Location = new Point(212, 4);
			buttonOK.Margin = new Padding(4);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(50, 40);
			buttonOK.TabIndex = 6;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = false;
			buttonOK.Click += buttonOK_Click;
			// 
			// textBoxScaling
			// 
			textBoxScaling.Location = new Point(139, 5);
			textBoxScaling.Margin = new Padding(4);
			textBoxScaling.Name = "textBoxScaling";
			textBoxScaling.Size = new Size(64, 31);
			textBoxScaling.TabIndex = 6;
			textBoxScaling.Tag = "";
			textBoxScaling.Text = "100";
			// 
			// buttonRotatePoint
			// 
			buttonRotatePoint.BackColor = SystemColors.ActiveCaption;
			buttonRotatePoint.Location = new Point(1130, 592);
			buttonRotatePoint.Margin = new Padding(4);
			buttonRotatePoint.Name = "buttonRotatePoint";
			buttonRotatePoint.Size = new Size(238, 46);
			buttonRotatePoint.TabIndex = 12;
			buttonRotatePoint.Text = "Поворот вокруг точки";
			buttonRotatePoint.UseVisualStyleBackColor = false;
			// 
			// buttonRotateCenter
			// 
			buttonRotateCenter.BackColor = SystemColors.ActiveCaption;
			buttonRotateCenter.Location = new Point(1130, 638);
			buttonRotateCenter.Margin = new Padding(4);
			buttonRotateCenter.Name = "buttonRotateCenter";
			buttonRotateCenter.Size = new Size(238, 44);
			buttonRotateCenter.TabIndex = 13;
			buttonRotateCenter.Text = "Поворот вокруг центра";
			buttonRotateCenter.UseVisualStyleBackColor = false;
			// 
			// buttonCheckPointInPolygon
			// 
			buttonCheckPointInPolygon.BackColor = SystemColors.ActiveCaption;
			buttonCheckPointInPolygon.Location = new Point(1130, 685);
			buttonCheckPointInPolygon.Margin = new Padding(4);
			buttonCheckPointInPolygon.Name = "buttonCheckPointInPolygon";
			buttonCheckPointInPolygon.Size = new Size(371, 39);
			buttonCheckPointInPolygon.TabIndex = 14;
			buttonCheckPointInPolygon.Text = "Проверить точку в полигоне";
			buttonCheckPointInPolygon.UseVisualStyleBackColor = false;
			// 
			// labelRotationAngle
			// 
			labelRotationAngle.Location = new Point(989, 450);
			labelRotationAngle.Margin = new Padding(4, 0, 4, 0);
			labelRotationAngle.Name = "labelRotationAngle";
			labelRotationAngle.Size = new Size(125, 25);
			labelRotationAngle.TabIndex = 16;
			labelRotationAngle.Text = "Угол поворота:";
			// 
			// textBoxRotationAngle
			// 
			textBoxRotationAngle.Location = new Point(1375, 638);
			textBoxRotationAngle.Margin = new Padding(4);
			textBoxRotationAngle.Name = "textBoxRotationAngle";
			textBoxRotationAngle.Size = new Size(104, 31);
			textBoxRotationAngle.TabIndex = 15;
			textBoxRotationAngle.Text = "30";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(1368, 601);
			label2.Margin = new Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new Size(138, 25);
			label2.TabIndex = 18;
			label2.Text = "Угол поворота:";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1516, 785);
			Controls.Add(label2);
			Controls.Add(panelScaling);
			Controls.Add(buttonCheckPointInPolygon);
			Controls.Add(buttonRotateCenter);
			Controls.Add(buttonRotatePoint);
			Controls.Add(buttonLeftRight);
			Controls.Add(buttonScalingCenter);
			Controls.Add(textBoxRotationAngle);
			Controls.Add(buttonIntersection);
			Controls.Add(buttonScaling);
			Controls.Add(labelMessage);
			Controls.Add(panel1);
			Controls.Add(buttonClear);
			Controls.Add(tableLayoutPanelMode);
			Controls.Add(drawingBox);
			Controls.Add(labelRotationAngle);
			Margin = new Padding(4);
			Name = "MainForm";
			Text = "Аффинные преобразования";
			((System.ComponentModel.ISupportInitialize)drawingBox).EndInit();
			tableLayoutPanelMode.ResumeLayout(false);
			tableLayoutPanelMode.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panelScaling.ResumeLayout(false);
			panelScaling.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button buttonPoint;
        private PictureBox drawingBox;
        private TableLayoutPanel tableLayoutPanelMode;
        private Button buttonLine;
        private Button buttonPolygon;
        private Button buttonFix;
        private Button buttonClear;
        private Panel panel1;
        private Button buttonTranslate;
        private TextBox textBoxDy;
        private TextBox textBoxDx;
        private TextBox textBox4;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label labelMessage;
        private Button buttonIntersection;
		private Button buttonScaling;
		private Button buttonScalingCenter;
		private Button buttonLeftRight;
		private TextBox textBox5;
		private Panel panelScaling;
		private TextBox textBoxScaling;
		private Button buttonOK;
        private Button buttonRotatePoint;
        private Button buttonRotateCenter;
        private Button buttonCheckPointInPolygon;
        private TextBox textBoxRotationAngle;
        private Label labelRotationAngle;
        private Label label2;
    }
}
