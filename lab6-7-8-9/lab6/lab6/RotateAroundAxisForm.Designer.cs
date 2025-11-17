namespace lab6
{
    partial class RotateAroundAxisForm
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
            textBoxX1 = new TextBox();
            buttonRotate = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBoxZ1 = new TextBox();
            textBoxY1 = new TextBox();
            textBoxX2 = new TextBox();
            textBoxZ2 = new TextBox();
            textBoxY2 = new TextBox();
            label4 = new Label();
            textBoxAngle = new TextBox();
            SuspendLayout();
            // 
            // textBoxX1
            // 
            textBoxX1.Font = new Font("Segoe UI", 13F);
            textBoxX1.Location = new Point(23, 155);
            textBoxX1.Name = "textBoxX1";
            textBoxX1.Size = new Size(47, 36);
            textBoxX1.TabIndex = 0;
            // 
            // buttonRotate
            // 
            buttonRotate.Font = new Font("Segoe UI", 14F);
            buttonRotate.Location = new Point(187, 220);
            buttonRotate.Name = "buttonRotate";
            buttonRotate.Size = new Size(182, 52);
            buttonRotate.TabIndex = 1;
            buttonRotate.Text = "Повернуть";
            buttonRotate.UseVisualStyleBackColor = true;
            buttonRotate.Click += ButtonRotate_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(23, 59);
            label1.Name = "label1";
            label1.Size = new Size(159, 35);
            label1.TabIndex = 2;
            label1.Text = "Координаты";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13F);
            label2.Location = new Point(23, 109);
            label2.Name = "label2";
            label2.Size = new Size(90, 30);
            label2.TabIndex = 3;
            label2.Text = "Точка 1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.Location = new Point(333, 109);
            label3.Name = "label3";
            label3.Size = new Size(90, 30);
            label3.TabIndex = 4;
            label3.Text = "Точка 2";
            // 
            // textBoxZ1
            // 
            textBoxZ1.Font = new Font("Segoe UI", 13F);
            textBoxZ1.Location = new Point(129, 155);
            textBoxZ1.Name = "textBoxZ1";
            textBoxZ1.Size = new Size(47, 36);
            textBoxZ1.TabIndex = 5;
            // 
            // textBoxY1
            // 
            textBoxY1.Font = new Font("Segoe UI", 13F);
            textBoxY1.Location = new Point(76, 155);
            textBoxY1.Name = "textBoxY1";
            textBoxY1.Size = new Size(47, 36);
            textBoxY1.TabIndex = 6;
            // 
            // textBoxX2
            // 
            textBoxX2.Font = new Font("Segoe UI", 13F);
            textBoxX2.Location = new Point(333, 155);
            textBoxX2.Name = "textBoxX2";
            textBoxX2.Size = new Size(47, 36);
            textBoxX2.TabIndex = 7;
            // 
            // textBoxZ2
            // 
            textBoxZ2.Font = new Font("Segoe UI", 13F);
            textBoxZ2.Location = new Point(439, 155);
            textBoxZ2.Name = "textBoxZ2";
            textBoxZ2.Size = new Size(47, 36);
            textBoxZ2.TabIndex = 8;
            // 
            // textBoxY2
            // 
            textBoxY2.Font = new Font("Segoe UI", 13F);
            textBoxY2.Location = new Point(386, 155);
            textBoxY2.Name = "textBoxY2";
            textBoxY2.Size = new Size(47, 36);
            textBoxY2.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15F);
            label4.Location = new Point(23, 9);
            label4.Name = "label4";
            label4.Size = new Size(67, 35);
            label4.TabIndex = 10;
            label4.Text = "Угол";
            // 
            // textBoxAngle
            // 
            textBoxAngle.Font = new Font("Segoe UI", 13F);
            textBoxAngle.Location = new Point(96, 10);
            textBoxAngle.Name = "textBoxAngle";
            textBoxAngle.Size = new Size(80, 36);
            textBoxAngle.TabIndex = 11;
            // 
            // RotateAroundAxisForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(565, 284);
            Controls.Add(textBoxAngle);
            Controls.Add(label4);
            Controls.Add(textBoxY2);
            Controls.Add(textBoxZ2);
            Controls.Add(textBoxX2);
            Controls.Add(textBoxY1);
            Controls.Add(textBoxZ1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonRotate);
            Controls.Add(textBoxX1);
            Name = "RotateAroundAxisForm";
            Text = "RotateAroundAxisForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxX1;
        private Button buttonRotate;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBoxZ1;
        private TextBox textBoxY1;
        private TextBox textBoxX2;
        private TextBox textBoxZ2;
        private TextBox textBoxY2;
        private Label label4;
        private TextBox textBoxAngle;
    }
}