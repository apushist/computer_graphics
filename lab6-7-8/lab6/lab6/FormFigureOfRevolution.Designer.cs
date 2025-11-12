namespace lab6
{
    partial class FormFigureOfRevolution
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
            label1 = new Label();
            label2 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            comboBoxAxis = new ComboBox();
            numericUpDownSegments = new NumericUpDown();
            buttonOK = new Button();
            buttonCancel = new Button();
            label3 = new Label();
            dataGridViewPoints = new DataGridView();
            X = new DataGridViewTextBoxColumn();
            Y = new DataGridViewTextBoxColumn();
            Z = new DataGridViewTextBoxColumn();
            buttonAddPoint = new Button();
            buttonRemovePoint = new Button();
            textBoxX = new TextBox();
            textBoxZ = new TextBox();
            textBoxY = new TextBox();
            buttonLoad = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSegments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPoints).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(207, 38);
            label1.TabIndex = 0;
            label1.Text = "Ось вращения:";
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(3, 55);
            label2.Name = "label2";
            label2.Size = new Size(291, 38);
            label2.TabIndex = 2;
            label2.Text = "Количество разбиений:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60.98563F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39.0143738F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(comboBoxAxis, 1, 0);
            tableLayoutPanel1.Controls.Add(numericUpDownSegments, 1, 1);
            tableLayoutPanel1.Location = new Point(159, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.522728F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 32.386364F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(487, 110);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // comboBoxAxis
            // 
            comboBoxAxis.Font = new Font("Segoe UI", 15F);
            comboBoxAxis.FormattingEnabled = true;
            comboBoxAxis.Items.AddRange(new object[] { "X", "Y", "Z" });
            comboBoxAxis.Location = new Point(300, 3);
            comboBoxAxis.Name = "comboBoxAxis";
            comboBoxAxis.Size = new Size(61, 43);
            comboBoxAxis.TabIndex = 1;
            // 
            // numericUpDownSegments
            // 
            numericUpDownSegments.Font = new Font("Segoe UI", 15F);
            numericUpDownSegments.Location = new Point(300, 58);
            numericUpDownSegments.Name = "numericUpDownSegments";
            numericUpDownSegments.Size = new Size(61, 41);
            numericUpDownSegments.TabIndex = 3;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            buttonOK.BackColor = Color.LightGreen;
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.Font = new Font("Segoe UI", 15F);
            buttonOK.ImageAlign = ContentAlignment.MiddleRight;
            buttonOK.Location = new Point(12, 391);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(162, 47);
            buttonOK.TabIndex = 4;
            buttonOK.Text = "Построить";
            buttonOK.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            buttonCancel.BackColor = Color.LightSalmon;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Font = new Font("Segoe UI", 15F);
            buttonCancel.ImageAlign = ContentAlignment.MiddleRight;
            buttonCancel.Location = new Point(638, 391);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(150, 47);
            buttonCancel.TabIndex = 5;
            buttonCancel.Text = "Отмена";
            buttonCancel.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label3.Font = new Font("Segoe UI", 15F);
            label3.Location = new Point(108, 128);
            label3.Name = "label3";
            label3.Size = new Size(95, 38);
            label3.TabIndex = 4;
            label3.Text = "Точки:";
            // 
            // dataGridViewPoints
            // 
            dataGridViewPoints.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewPoints.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPoints.Columns.AddRange(new DataGridViewColumn[] { X, Y, Z });
            dataGridViewPoints.Location = new Point(209, 128);
            dataGridViewPoints.Name = "dataGridViewPoints";
            dataGridViewPoints.RowHeadersWidth = 51;
            dataGridViewPoints.Size = new Size(407, 164);
            dataGridViewPoints.TabIndex = 5;
            // 
            // X
            // 
            X.HeaderText = "X";
            X.MinimumWidth = 6;
            X.Name = "X";
            X.Width = 125;
            // 
            // Y
            // 
            Y.HeaderText = "Y";
            Y.MinimumWidth = 6;
            Y.Name = "Y";
            Y.Width = 125;
            // 
            // Z
            // 
            Z.HeaderText = "Z";
            Z.MinimumWidth = 6;
            Z.Name = "Z";
            Z.Width = 125;
            // 
            // buttonAddPoint
            // 
            buttonAddPoint.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonAddPoint.BackColor = Color.PaleGreen;
            buttonAddPoint.Font = new Font("Segoe UI", 11F);
            buttonAddPoint.ImageAlign = ContentAlignment.MiddleRight;
            buttonAddPoint.Location = new Point(207, 338);
            buttonAddPoint.Name = "buttonAddPoint";
            buttonAddPoint.Size = new Size(162, 47);
            buttonAddPoint.TabIndex = 6;
            buttonAddPoint.Text = "Добавить точку";
            buttonAddPoint.UseVisualStyleBackColor = false;
            buttonAddPoint.Click += ButtonAddPoint_Click;
            // 
            // buttonRemovePoint
            // 
            buttonRemovePoint.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonRemovePoint.BackColor = Color.PeachPuff;
            buttonRemovePoint.Font = new Font("Segoe UI", 11F);
            buttonRemovePoint.ImageAlign = ContentAlignment.MiddleRight;
            buttonRemovePoint.Location = new Point(466, 338);
            buttonRemovePoint.Name = "buttonRemovePoint";
            buttonRemovePoint.Size = new Size(150, 47);
            buttonRemovePoint.TabIndex = 7;
            buttonRemovePoint.Text = "Удалить точку";
            buttonRemovePoint.UseVisualStyleBackColor = false;
            buttonRemovePoint.Click += ButtonRemovePoint_Click;
            // 
            // textBoxX
            // 
            textBoxX.Font = new Font("Segoe UI", 15F);
            textBoxX.Location = new Point(209, 297);
            textBoxX.Name = "textBoxX";
            textBoxX.Size = new Size(61, 41);
            textBoxX.TabIndex = 11;
            // 
            // textBoxZ
            // 
            textBoxZ.Font = new Font("Segoe UI", 15F);
            textBoxZ.Location = new Point(555, 297);
            textBoxZ.Name = "textBoxZ";
            textBoxZ.Size = new Size(61, 41);
            textBoxZ.TabIndex = 12;
            // 
            // textBoxY
            // 
            textBoxY.Font = new Font("Segoe UI", 15F);
            textBoxY.Location = new Point(392, 298);
            textBoxY.Name = "textBoxY";
            textBoxY.Size = new Size(61, 41);
            textBoxY.TabIndex = 13;
            // 
            // buttonLoad
            // 
            buttonLoad.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            buttonLoad.BackColor = Color.LightYellow;
            buttonLoad.Font = new Font("Segoe UI", 11F);
            buttonLoad.ImageAlign = ContentAlignment.MiddleRight;
            buttonLoad.Location = new Point(298, 394);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(238, 47);
            buttonLoad.TabIndex = 14;
            buttonLoad.Text = "Загрузить образующую...";
            buttonLoad.UseVisualStyleBackColor = false;
            buttonLoad.Click += ButtonLoad_Click;
            // 
            // FormFigureOfRevolution
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonLoad);
            Controls.Add(textBoxY);
            Controls.Add(textBoxZ);
            Controls.Add(textBoxX);
            Controls.Add(buttonRemovePoint);
            Controls.Add(buttonAddPoint);
            Controls.Add(label3);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(dataGridViewPoints);
            Name = "FormFigureOfRevolution";
            Text = "Создание фигуры вращения";
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDownSegments).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPoints).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonOK;
        private Button buttonCancel;
        private Label label3;
        private ComboBox comboBoxAxis;
        private NumericUpDown numericUpDownSegments;
        private DataGridView dataGridViewPoints;
        private DataGridViewTextBoxColumn X;
        private DataGridViewTextBoxColumn Y;
        private DataGridViewTextBoxColumn Z;
        public Button buttonAddPoint;
        private Button buttonRemovePoint;
        private TextBox textBoxX;
        private TextBox textBoxZ;
        private TextBox textBoxY;
        private Button buttonLoad;
    }
}