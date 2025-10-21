namespace lab5
{
	partial class Form2
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
            buttonReturn = new Button();
            buttonLoad = new Button();
            buttonGenerate = new Button();
            canvas = new PictureBox();
            labelIter = new Label();
            numericUpDownIter = new NumericUpDown();
            checkBoxRandom = new CheckBox();
            labelRandomAngle = new Label();
            trackBarRandomAngle = new TrackBar();
            labelRandomLen = new Label();
            trackBarRandomLen = new TrackBar();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonGenerateTree = new Button();
            trackBarScale = new TrackBar();
            labelScale = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownIter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarRandomAngle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarRandomLen).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarScale).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonReturn
            // 
            buttonReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonReturn.Location = new Point(3, 4);
            buttonReturn.Margin = new Padding(3, 4, 3, 4);
            buttonReturn.Name = "buttonReturn";
            buttonReturn.Size = new Size(150, 32);
            buttonReturn.TabIndex = 0;
            buttonReturn.Text = "Вернуться в меню";
            buttonReturn.UseVisualStyleBackColor = true;
            buttonReturn.Click += ButtonReturn_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonLoad.Location = new Point(3, 44);
            buttonLoad.Margin = new Padding(3, 4, 3, 4);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(150, 32);
            buttonLoad.TabIndex = 1;
            buttonLoad.Text = "Загрузить файл...";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += ButtonLoad_Click;
            // 
            // buttonGenerate
            // 
            buttonGenerate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonGenerate.Location = new Point(3, 84);
            buttonGenerate.Margin = new Padding(3, 4, 3, 4);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(150, 29);
            buttonGenerate.TabIndex = 2;
            buttonGenerate.Text = "Сгенерировать";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += ButtonGenerate_Click;
            // 
            // canvas
            // 
            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            canvas.Location = new Point(12, 12);
            canvas.Name = "canvas";
            canvas.Size = new Size(742, 535);
            canvas.TabIndex = 3;
            canvas.TabStop = false;
            canvas.Paint += Canvas_Paint;
            // 
            // labelIter
            // 
            labelIter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            labelIter.Location = new Point(3, 203);
            labelIter.Name = "labelIter";
            labelIter.Size = new Size(150, 41);
            labelIter.TabIndex = 4;
            labelIter.Text = "Количество итераций:";
            labelIter.TextAlign = ContentAlignment.TopCenter;
            // 
            // numericUpDownIter
            // 
            numericUpDownIter.Location = new Point(3, 247);
            numericUpDownIter.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            numericUpDownIter.Name = "numericUpDownIter";
            numericUpDownIter.Size = new Size(64, 27);
            numericUpDownIter.TabIndex = 5;
            numericUpDownIter.TextAlign = HorizontalAlignment.Center;
            numericUpDownIter.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // checkBoxRandom
            // 
            checkBoxRandom.AutoSize = true;
            checkBoxRandom.Location = new Point(3, 314);
            checkBoxRandom.Name = "checkBoxRandom";
            checkBoxRandom.Size = new Size(119, 24);
            checkBoxRandom.TabIndex = 6;
            checkBoxRandom.Text = "Случайность";
            checkBoxRandom.UseVisualStyleBackColor = true;
            // 
            // labelRandomAngle
            // 
            labelRandomAngle.Location = new Point(3, 367);
            labelRandomAngle.Name = "labelRandomAngle";
            labelRandomAngle.Size = new Size(150, 25);
            labelRandomAngle.TabIndex = 7;
            labelRandomAngle.Text = "Разброс угла (°): 5";
            labelRandomAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // trackBarRandomAngle
            // 
            trackBarRandomAngle.Location = new Point(3, 401);
            trackBarRandomAngle.Maximum = 60;
            trackBarRandomAngle.Name = "trackBarRandomAngle";
            trackBarRandomAngle.Size = new Size(150, 40);
            trackBarRandomAngle.TabIndex = 8;
            trackBarRandomAngle.Value = 5;
            // 
            // labelRandomLen
            // 
            labelRandomLen.Location = new Point(3, 476);
            labelRandomLen.Name = "labelRandomLen";
            labelRandomLen.Size = new Size(150, 43);
            labelRandomLen.TabIndex = 9;
            labelRandomLen.Text = "Разброс длины (%): 10";
            labelRandomLen.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // trackBarRandomLen
            // 
            trackBarRandomLen.Location = new Point(3, 522);
            trackBarRandomLen.Maximum = 50;
            trackBarRandomLen.Name = "trackBarRandomLen";
            trackBarRandomLen.Size = new Size(150, 37);
            trackBarRandomLen.TabIndex = 10;
            trackBarRandomLen.Value = 10;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(buttonGenerateTree, 0, 3);
            tableLayoutPanel1.Controls.Add(buttonReturn, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonLoad, 0, 1);
            tableLayoutPanel1.Controls.Add(buttonGenerate, 0, 2);
            tableLayoutPanel1.Controls.Add(trackBarRandomLen, 0, 19);
            tableLayoutPanel1.Controls.Add(labelRandomLen, 0, 18);
            tableLayoutPanel1.Controls.Add(trackBarRandomAngle, 0, 15);
            tableLayoutPanel1.Controls.Add(labelRandomAngle, 0, 14);
            tableLayoutPanel1.Controls.Add(checkBoxRandom, 0, 12);
            tableLayoutPanel1.Controls.Add(labelIter, 0, 6);
            tableLayoutPanel1.Controls.Add(numericUpDownIter, 0, 7);
            tableLayoutPanel1.Location = new Point(760, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 21;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 9F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 13F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 13F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(156, 576);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // buttonGenerateTree
            // 
            buttonGenerateTree.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonGenerateTree.Location = new Point(3, 125);
            buttonGenerateTree.Margin = new Padding(3, 4, 3, 4);
            buttonGenerateTree.Name = "buttonGenerateTree";
            buttonGenerateTree.Size = new Size(150, 49);
            buttonGenerateTree.TabIndex = 11;
            buttonGenerateTree.Text = "Сгенерировать дерево";
            buttonGenerateTree.UseVisualStyleBackColor = true;
            buttonGenerateTree.Click += ButtonGenerateTree_Click;
            // 
            // trackBarScale
            // 
            trackBarScale.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBarScale.AutoSize = false;
            trackBarScale.Location = new Point(432, 3);
            trackBarScale.Maximum = 300;
            trackBarScale.Minimum = 1;
            trackBarScale.Name = "trackBarScale";
            trackBarScale.Size = new Size(307, 32);
            trackBarScale.TabIndex = 12;
            trackBarScale.TickFrequency = 10;
            trackBarScale.Value = 100;
            // 
            // labelScale
            // 
            labelScale.AutoSize = true;
            labelScale.Font = new Font("Segoe UI", 15F);
            labelScale.Location = new Point(3, 0);
            labelScale.Name = "labelScale";
            labelScale.Size = new Size(190, 35);
            labelScale.TabIndex = 13;
            labelScale.Text = "Масштаб: 100%";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(labelScale);
            panel1.Controls.Add(trackBarScale);
            panel1.Location = new Point(12, 553);
            panel1.Name = "panel1";
            panel1.Size = new Size(742, 35);
            panel1.TabIndex = 14;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(panel1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(canvas);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form2";
            Text = "L-системы";
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownIter).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarRandomAngle).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarRandomLen).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarScale).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonReturn;
        private Button buttonLoad;
        private Button buttonGenerate;
        private PictureBox canvas;
        private Label labelIter;
        private NumericUpDown numericUpDownIter;
        private CheckBox checkBoxRandom;
        private Label labelRandomAngle;
        private TrackBar trackBarRandomAngle;
        private Label labelRandomLen;
        private TrackBar trackBarRandomLen;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonGenerateTree;
        private TrackBar trackBarScale;
        private Label labelScale;
        private Panel panel1;
    }
}