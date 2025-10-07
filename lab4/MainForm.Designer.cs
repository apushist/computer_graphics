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
			((System.ComponentModel.ISupportInitialize)drawingBox).BeginInit();
			tableLayoutPanelMode.SuspendLayout();
			panel1.SuspendLayout();
			panelScaling.SuspendLayout();
			SuspendLayout();
			// 
			// buttonPoint
			// 
			buttonPoint.Location = new Point(3, 29);
			buttonPoint.Margin = new Padding(3, 2, 3, 2);
			buttonPoint.Name = "buttonPoint";
			buttonPoint.Size = new Size(260, 23);
			buttonPoint.TabIndex = 0;
			buttonPoint.Text = "Точка";
			buttonPoint.UseVisualStyleBackColor = true;
			// 
			// drawingBox
			// 
			drawingBox.BorderStyle = BorderStyle.FixedSingle;
			drawingBox.Location = new Point(10, 9);
			drawingBox.Margin = new Padding(3, 2, 3, 2);
			drawingBox.Name = "drawingBox";
			drawingBox.Size = new Size(776, 442);
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
			tableLayoutPanelMode.Location = new Point(791, 9);
			tableLayoutPanelMode.Margin = new Padding(3, 2, 3, 2);
			tableLayoutPanelMode.Name = "tableLayoutPanelMode";
			tableLayoutPanelMode.RowCount = 5;
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
			tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
			tableLayoutPanelMode.Size = new Size(267, 139);
			tableLayoutPanelMode.TabIndex = 2;
			// 
			// buttonFix
			// 
			buttonFix.BackColor = SystemColors.ActiveCaption;
			buttonFix.Location = new Point(3, 110);
			buttonFix.Margin = new Padding(3, 2, 3, 2);
			buttonFix.Name = "buttonFix";
			buttonFix.Size = new Size(260, 23);
			buttonFix.TabIndex = 4;
			buttonFix.Text = "Зафиксировать полигон";
			buttonFix.UseVisualStyleBackColor = false;
			// 
			// buttonPolygon
			// 
			buttonPolygon.Location = new Point(3, 83);
			buttonPolygon.Margin = new Padding(3, 2, 3, 2);
			buttonPolygon.Name = "buttonPolygon";
			buttonPolygon.Size = new Size(260, 23);
			buttonPolygon.TabIndex = 2;
			buttonPolygon.Text = "Полигон";
			buttonPolygon.UseVisualStyleBackColor = true;
			// 
			// buttonLine
			// 
			buttonLine.Location = new Point(3, 56);
			buttonLine.Margin = new Padding(3, 2, 3, 2);
			buttonLine.Name = "buttonLine";
			buttonLine.Size = new Size(260, 23);
			buttonLine.TabIndex = 1;
			buttonLine.Text = "Линия";
			buttonLine.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			textBox1.BackColor = SystemColors.Control;
			textBox1.Location = new Point(3, 2);
			textBox1.Margin = new Padding(3, 2, 3, 2);
			textBox1.Name = "textBox1";
			textBox1.ReadOnly = true;
			textBox1.Size = new Size(258, 23);
			textBox1.TabIndex = 3;
			textBox1.Text = "Режим рисования";
			textBox1.TextAlign = HorizontalAlignment.Center;
			// 
			// buttonClear
			// 
			buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonClear.BackColor = Color.RosyBrown;
			buttonClear.Location = new Point(794, 427);
			buttonClear.Margin = new Padding(3, 2, 3, 2);
			buttonClear.Name = "buttonClear";
			buttonClear.Size = new Size(260, 23);
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
			panel1.Location = new Point(791, 156);
			panel1.Margin = new Padding(3, 2, 3, 2);
			panel1.Name = "panel1";
			panel1.Size = new Size(260, 88);
			panel1.TabIndex = 4;
			// 
			// buttonTranslate
			// 
			buttonTranslate.BackColor = SystemColors.ActiveCaption;
			buttonTranslate.Location = new Point(149, 33);
			buttonTranslate.Margin = new Padding(3, 2, 3, 2);
			buttonTranslate.Name = "buttonTranslate";
			buttonTranslate.Size = new Size(108, 45);
			buttonTranslate.TabIndex = 5;
			buttonTranslate.Text = "Сместить";
			buttonTranslate.UseVisualStyleBackColor = false;
			// 
			// textBoxDy
			// 
			textBoxDy.Location = new Point(42, 58);
			textBoxDy.Margin = new Padding(3, 2, 3, 2);
			textBoxDy.Name = "textBoxDy";
			textBoxDy.Size = new Size(102, 23);
			textBoxDy.TabIndex = 4;
			// 
			// textBoxDx
			// 
			textBoxDx.Location = new Point(42, 33);
			textBoxDx.Margin = new Padding(3, 2, 3, 2);
			textBoxDx.Name = "textBoxDx";
			textBoxDx.Size = new Size(102, 23);
			textBoxDx.TabIndex = 3;
			// 
			// textBox4
			// 
			textBox4.Location = new Point(8, 58);
			textBox4.Margin = new Padding(3, 2, 3, 2);
			textBox4.Name = "textBox4";
			textBox4.ReadOnly = true;
			textBox4.Size = new Size(29, 23);
			textBox4.TabIndex = 2;
			textBox4.Text = "dy:";
			// 
			// textBox3
			// 
			textBox3.Location = new Point(8, 33);
			textBox3.Margin = new Padding(3, 2, 3, 2);
			textBox3.Name = "textBox3";
			textBox3.ReadOnly = true;
			textBox3.Size = new Size(29, 23);
			textBox3.TabIndex = 1;
			textBox3.Text = "dx:";
			// 
			// textBox2
			// 
			textBox2.BackColor = SystemColors.Control;
			textBox2.Location = new Point(3, 2);
			textBox2.Margin = new Padding(3, 2, 3, 2);
			textBox2.Name = "textBox2";
			textBox2.ReadOnly = true;
			textBox2.Size = new Size(255, 23);
			textBox2.TabIndex = 0;
			textBox2.Text = "Смещение";
			textBox2.TextAlign = HorizontalAlignment.Center;
			// 
			// labelMessage
			// 
			labelMessage.Location = new Point(15, 13);
			labelMessage.Name = "labelMessage";
			labelMessage.Size = new Size(747, 19);
			labelMessage.TabIndex = 5;
			// 
			// buttonIntersection
			// 
			buttonIntersection.BackColor = SystemColors.ActiveCaption;
			buttonIntersection.Location = new Point(791, 248);
			buttonIntersection.Margin = new Padding(3, 2, 3, 2);
			buttonIntersection.Name = "buttonIntersection";
			buttonIntersection.Size = new Size(260, 23);
			buttonIntersection.TabIndex = 6;
			buttonIntersection.Text = "Найти пересечение";
			buttonIntersection.UseVisualStyleBackColor = false;
			// 
			// buttonScaling
			// 
			buttonScaling.BackColor = SystemColors.ActiveCaption;
			buttonScaling.Location = new Point(791, 275);
			buttonScaling.Margin = new Padding(3, 2, 3, 2);
			buttonScaling.Name = "buttonScaling";
			buttonScaling.Size = new Size(260, 23);
			buttonScaling.TabIndex = 7;
			buttonScaling.Text = "Масштабировать относительно точки";
			buttonScaling.UseVisualStyleBackColor = false;
			buttonScaling.Click += buttonScaling_Click;
			// 
			// buttonScalingCenter
			// 
			buttonScalingCenter.BackColor = SystemColors.ActiveCaption;
			buttonScalingCenter.Location = new Point(791, 302);
			buttonScalingCenter.Margin = new Padding(3, 2, 3, 2);
			buttonScalingCenter.Name = "buttonScalingCenter";
			buttonScalingCenter.Size = new Size(260, 23);
			buttonScalingCenter.TabIndex = 8;
			buttonScalingCenter.Text = "Масштабировать относительно центра";
			buttonScalingCenter.UseVisualStyleBackColor = false;
			buttonScalingCenter.Click += buttonScalingCenter_Click;
			// 
			// buttonLeftRight
			// 
			buttonLeftRight.BackColor = SystemColors.ActiveCaption;
			buttonLeftRight.Location = new Point(791, 329);
			buttonLeftRight.Margin = new Padding(3, 2, 3, 2);
			buttonLeftRight.Name = "buttonLeftRight";
			buttonLeftRight.Size = new Size(260, 23);
			buttonLeftRight.TabIndex = 9;
			buttonLeftRight.Text = "Относительное положение точки";
			buttonLeftRight.UseVisualStyleBackColor = false;
			buttonLeftRight.Click += buttonLeftRight_Click;
			// 
			// textBox5
			// 
			textBox5.Location = new Point(3, 3);
			textBox5.Name = "textBox5";
			textBox5.Size = new Size(88, 23);
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
			panelScaling.Location = new Point(586, 419);
			panelScaling.Name = "panelScaling";
			panelScaling.Size = new Size(189, 32);
			panelScaling.TabIndex = 11;
			panelScaling.Visible = false;
			// 
			// buttonOK
			// 
			buttonOK.BackColor = SystemColors.ActiveBorder;
			buttonOK.Location = new Point(149, 2);
			buttonOK.Margin = new Padding(3, 2, 3, 2);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(35, 24);
			buttonOK.TabIndex = 6;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = false;
			buttonOK.Click += buttonOK_Click;
			// 
			// textBoxScaling
			// 
			textBoxScaling.Location = new Point(97, 3);
			textBoxScaling.Margin = new Padding(3, 2, 3, 2);
			textBoxScaling.Name = "textBoxScaling";
			textBoxScaling.Size = new Size(46, 23);
			textBoxScaling.TabIndex = 6;
			textBoxScaling.Tag = "";
			textBoxScaling.Text = "100";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1061, 459);
			Controls.Add(panelScaling);
			Controls.Add(buttonLeftRight);
			Controls.Add(buttonScalingCenter);
			Controls.Add(buttonScaling);
			Controls.Add(buttonIntersection);
			Controls.Add(labelMessage);
			Controls.Add(panel1);
			Controls.Add(buttonClear);
			Controls.Add(tableLayoutPanelMode);
			Controls.Add(drawingBox);
			Margin = new Padding(3, 2, 3, 2);
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
	}
}
