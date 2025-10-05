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
            buttonClear = new Button();
            panel1 = new Panel();
            buttonTranslate = new Button();
            textBoxDy = new TextBox();
            textBoxDx = new TextBox();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            labelMessage = new Label();
            buttonIntersection = new Button();
            ((System.ComponentModel.ISupportInitialize)drawingBox).BeginInit();
            tableLayoutPanelMode.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPoint
            // 
            buttonPoint.Location = new Point(3, 40);
            buttonPoint.Name = "buttonPoint";
            buttonPoint.Size = new Size(297, 31);
            buttonPoint.TabIndex = 0;
            buttonPoint.Text = "Точка";
            buttonPoint.UseVisualStyleBackColor = true;
            // 
            // drawingBox
            // 
            drawingBox.BorderStyle = BorderStyle.FixedSingle;
            drawingBox.Location = new Point(12, 12);
            drawingBox.Name = "drawingBox";
            drawingBox.Size = new Size(886, 588);
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
            tableLayoutPanelMode.Location = new Point(904, 12);
            tableLayoutPanelMode.Name = "tableLayoutPanelMode";
            tableLayoutPanelMode.RowCount = 5;
            tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanelMode.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanelMode.Size = new Size(305, 185);
            tableLayoutPanelMode.TabIndex = 2;
            // 
            // buttonFix
            // 
            buttonFix.BackColor = SystemColors.ActiveCaption;
            buttonFix.Location = new Point(3, 151);
            buttonFix.Name = "buttonFix";
            buttonFix.Size = new Size(297, 31);
            buttonFix.TabIndex = 4;
            buttonFix.Text = "Зафиксировать полигон";
            buttonFix.UseVisualStyleBackColor = false;
            // 
            // buttonPolygon
            // 
            buttonPolygon.Location = new Point(3, 114);
            buttonPolygon.Name = "buttonPolygon";
            buttonPolygon.Size = new Size(297, 31);
            buttonPolygon.TabIndex = 2;
            buttonPolygon.Text = "Полигон";
            buttonPolygon.UseVisualStyleBackColor = true;
            // 
            // buttonLine
            // 
            buttonLine.Location = new Point(3, 77);
            buttonLine.Name = "buttonLine";
            buttonLine.Size = new Size(297, 31);
            buttonLine.TabIndex = 1;
            buttonLine.Text = "Линия";
            buttonLine.UseVisualStyleBackColor = true;
            // 
            // buttonClear
            // 
            buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClear.BackColor = Color.RosyBrown;
            buttonClear.Location = new Point(907, 569);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(297, 31);
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
            panel1.Location = new Point(904, 208);
            panel1.Name = "panel1";
            panel1.Size = new Size(297, 117);
            panel1.TabIndex = 4;
            // 
            // buttonTranslate
            // 
            buttonTranslate.BackColor = SystemColors.ActiveCaption;
            buttonTranslate.Location = new Point(170, 44);
            buttonTranslate.Name = "buttonTranslate";
            buttonTranslate.Size = new Size(124, 60);
            buttonTranslate.TabIndex = 5;
            buttonTranslate.Text = "Сместить";
            buttonTranslate.UseVisualStyleBackColor = false;
            // 
            // textBoxDy
            // 
            textBoxDy.Location = new Point(48, 77);
            textBoxDy.Name = "textBoxDy";
            textBoxDy.Size = new Size(116, 27);
            textBoxDy.TabIndex = 4;
            // 
            // textBoxDx
            // 
            textBoxDx.Location = new Point(48, 44);
            textBoxDx.Name = "textBoxDx";
            textBoxDx.Size = new Size(116, 27);
            textBoxDx.TabIndex = 3;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(9, 77);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(33, 27);
            textBox4.TabIndex = 2;
            textBox4.Text = "dy:";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(9, 44);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(33, 27);
            textBox3.TabIndex = 1;
            textBox3.Text = "dx:";
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.Control;
            textBox2.Location = new Point(3, 3);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(291, 27);
            textBox2.TabIndex = 0;
            textBox2.Text = "Смещение";
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Control;
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(294, 27);
            textBox1.TabIndex = 3;
            textBox1.Text = "Режим рисования";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // labelMessage
            // 
            labelMessage.Location = new Point(17, 17);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(854, 25);
            labelMessage.TabIndex = 5;
            // 
            // buttonIntersection
            // 
            buttonIntersection.BackColor = SystemColors.ActiveCaption;
            buttonIntersection.Location = new Point(904, 331);
            buttonIntersection.Name = "buttonIntersection";
            buttonIntersection.Size = new Size(297, 31);
            buttonIntersection.TabIndex = 6;
            buttonIntersection.Text = "Найти пересечение";
            buttonIntersection.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1213, 612);
            Controls.Add(buttonIntersection);
            Controls.Add(labelMessage);
            Controls.Add(panel1);
            Controls.Add(buttonClear);
            Controls.Add(tableLayoutPanelMode);
            Controls.Add(drawingBox);
            Name = "MainForm";
            Text = "Аффинные преобразования";
            ((System.ComponentModel.ISupportInitialize)drawingBox).EndInit();
            tableLayoutPanelMode.ResumeLayout(false);
            tableLayoutPanelMode.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
    }
}
