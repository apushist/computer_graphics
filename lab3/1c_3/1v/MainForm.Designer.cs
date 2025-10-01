namespace _1v
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
            btnLoad = new Button();
            btnColor = new Button();
            btnPipette = new Button();
            btnTrace = new Button();
            btnReset = new Button();
            SuspendLayout();
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoad.Location = new Point(550, 2);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(231, 34);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Загрузить изображение";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += BtnLoad_Click;
            // 
            // btnColor
            // 
            btnColor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnColor.Location = new Point(550, 42);
            btnColor.Name = "btnColor";
            btnColor.Size = new Size(231, 34);
            btnColor.TabIndex = 1;
            btnColor.Text = "Выбрать цвет границы";
            btnColor.UseVisualStyleBackColor = true;
            btnColor.Click += BtnColor_Click;
            // 
            // btnPipette
            // 
            btnPipette.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPipette.Location = new Point(550, 82);
            btnPipette.Name = "btnPipette";
            btnPipette.Size = new Size(231, 53);
            btnPipette.TabIndex = 2;
            btnPipette.Text = "Пипетка (клик по изображению)";
            btnPipette.UseVisualStyleBackColor = true;
            btnPipette.Click += BtnPipette_Click;
            // 
            // btnTrace
            // 
            btnTrace.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTrace.Location = new Point(550, 141);
            btnTrace.Name = "btnTrace";
            btnTrace.Size = new Size(231, 34);
            btnTrace.TabIndex = 3;
            btnTrace.Text = "Обойти границу";
            btnTrace.UseVisualStyleBackColor = true;
            btnTrace.Click += BtnTrace_Click;
            // 
            // btnReset
            // 
            btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReset.Location = new Point(550, 181);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(231, 34);
            btnReset.TabIndex = 4;
            btnReset.Text = "Сбросить границу";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 553);
            Controls.Add(btnReset);
            Controls.Add(btnTrace);
            Controls.Add(btnPipette);
            Controls.Add(btnColor);
            Controls.Add(btnLoad);
            Name = "MainForm";
            Text = "Выделение границы связной области";
            ResumeLayout(false);
        }

        #endregion

        private Button btnLoad;
        private Button btnColor;
        private Button btnPipette;
        private Button btnTrace;
        private Button btnReset;
    }
}
