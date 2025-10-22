namespace lab5
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown initLLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown initRLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.TextBox initRoughness;
        private System.Windows.Forms.Button plusBtn;
        private System.Windows.Forms.Button minusBtn;
        private System.Windows.Forms.Button autoGenerateBtn;

        private void InitializeComponent()
        {
            button1 = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            initLLength = new NumericUpDown();
            label2 = new Label();
            label3 = new Label();
            initRLength = new NumericUpDown();
            label4 = new Label();
            NextStep = new Button();
            Clear = new Button();
            initRoughness = new TextBox();
            plusBtn = new Button();
            minusBtn = new Button();
            autoGenerateBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)initLLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)initRLength).BeginInit();
            SuspendLayout();

            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(719, 635);
            button1.Name = "button1";
            button1.Size = new Size(140, 37);
            button1.TabIndex = 8;
            button1.Text = "Вернуться в меню";
            button1.UseVisualStyleBackColor = true;

            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(16, 18);
            pictureBox1.Margin = new Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(691, 654);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;

            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(715, 14);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(144, 20);
            label1.TabIndex = 2;
            label1.Text = "Начальные высоты";

            initLLength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            initLLength.Location = new Point(775, 38);
            initLLength.Margin = new Padding(4, 5, 4, 5);
            initLLength.Name = "initLLength";
            initLLength.Size = new Size(81, 27);
            initLLength.TabIndex = 3;

            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(715, 43);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(51, 20);
            label2.TabIndex = 4;
            label2.Text = "Левая";

            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(715, 83);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(61, 20);
            label3.TabIndex = 6;
            label3.Text = "Правая";

            initRLength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            initRLength.Location = new Point(775, 78);
            initRLength.Margin = new Padding(4, 5, 4, 5);
            initRLength.Name = "initRLength";
            initRLength.Size = new Size(81, 27);
            initRLength.TabIndex = 5;

            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(716, 125);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(117, 20);
            label4.TabIndex = 7;
            label4.Text = "Шероховатость";

            NextStep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            NextStep.Location = new Point(719, 192);
            NextStep.Margin = new Padding(4, 5, 4, 5);
            NextStep.Name = "NextStep";
            NextStep.Size = new Size(136, 35);
            NextStep.TabIndex = 9;
            NextStep.Text = "Следующий шаг";
            NextStep.UseVisualStyleBackColor = true;

            Clear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Clear.Location = new Point(720, 282);
            Clear.Margin = new Padding(4, 5, 4, 5);
            Clear.Name = "Clear";
            Clear.Size = new Size(135, 35);
            Clear.TabIndex = 10;
            Clear.Text = "Очистить";
            Clear.UseVisualStyleBackColor = true;

            initRoughness.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            initRoughness.Location = new Point(747, 151);
            initRoughness.Margin = new Padding(4, 5, 4, 5);
            initRoughness.Name = "initRoughness";
            initRoughness.Size = new Size(81, 27);
            initRoughness.TabIndex = 11;
            initRoughness.Text = "0,4";

            plusBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            plusBtn.Location = new Point(837, 151);
            plusBtn.Margin = new Padding(4, 5, 4, 5);
            plusBtn.Name = "plusBtn";
            plusBtn.Size = new Size(19, 31);
            plusBtn.TabIndex = 12;
            plusBtn.Text = "+";
            plusBtn.UseVisualStyleBackColor = true;

            minusBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minusBtn.Location = new Point(720, 151);
            minusBtn.Margin = new Padding(4, 5, 4, 5);
            minusBtn.Name = "minusBtn";
            minusBtn.Size = new Size(19, 31);
            minusBtn.TabIndex = 13;
            minusBtn.Text = "-";
            minusBtn.UseVisualStyleBackColor = true;

            autoGenerateBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            autoGenerateBtn.Location = new Point(719, 237);
            autoGenerateBtn.Margin = new Padding(4, 5, 4, 5);
            autoGenerateBtn.Name = "autoGenerateBtn";
            autoGenerateBtn.Size = new Size(136, 35);
            autoGenerateBtn.TabIndex = 14;
            autoGenerateBtn.Text = "Автогенерация";
            autoGenerateBtn.UseVisualStyleBackColor = true;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(872, 691);
            Controls.Add(autoGenerateBtn);
            Controls.Add(minusBtn);
            Controls.Add(plusBtn);
            Controls.Add(initRoughness);
            Controls.Add(Clear);
            Controls.Add(NextStep);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(initRLength);
            Controls.Add(label2);
            Controls.Add(initLLength);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form3";
            Text = "Midpoint Displacement - Линия горизонта";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)initLLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)initRLength).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}