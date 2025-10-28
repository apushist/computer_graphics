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
			btnResetRotation = new Button();
			buttonTrans = new Button();
			buttonTetr = new Button();
			buttonGex = new Button();
			buttonOct = new Button();
			buttonIco = new Button();
			buttonDod = new Button();
			buttonRefl = new Button();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.BackColor = Color.White;
			pictureBox1.Dock = DockStyle.Fill;
			pictureBox1.Location = new Point(0, 0);
			pictureBox1.Margin = new Padding(4, 3, 4, 3);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(915, 647);
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
			btnZoomIn.Location = new Point(14, 14);
			btnZoomIn.Margin = new Padding(4, 3, 4, 3);
			btnZoomIn.Name = "btnZoomIn";
			btnZoomIn.Size = new Size(35, 35);
			btnZoomIn.TabIndex = 1;
			btnZoomIn.Text = "+";
			btnZoomIn.UseVisualStyleBackColor = true;
			btnZoomIn.Click += btnZoomIn_Click;
			// 
			// btnZoomOut
			// 
			btnZoomOut.Location = new Point(56, 14);
			btnZoomOut.Margin = new Padding(4, 3, 4, 3);
			btnZoomOut.Name = "btnZoomOut";
			btnZoomOut.Size = new Size(35, 35);
			btnZoomOut.TabIndex = 2;
			btnZoomOut.Text = "-";
			btnZoomOut.UseVisualStyleBackColor = true;
			btnZoomOut.Click += btnZoomOut_Click;
			// 
			// btnResetView
			// 
			btnResetView.Location = new Point(98, 14);
			btnResetView.Margin = new Padding(4, 3, 4, 3);
			btnResetView.Name = "btnResetView";
			btnResetView.Size = new Size(70, 35);
			btnResetView.TabIndex = 3;
			btnResetView.Text = "Сброс";
			btnResetView.UseVisualStyleBackColor = true;
			btnResetView.Click += btnResetView_Click;
			// 
			// btnSwitchProjection
			// 
			btnSwitchProjection.Location = new Point(175, 14);
			btnSwitchProjection.Margin = new Padding(4, 3, 4, 3);
			btnSwitchProjection.Name = "btnSwitchProjection";
			btnSwitchProjection.Size = new Size(117, 35);
			btnSwitchProjection.TabIndex = 4;
			btnSwitchProjection.Text = "Аксонометрия";
			btnSwitchProjection.UseVisualStyleBackColor = true;
			btnSwitchProjection.Click += btnSwitchProjection_Click;
			// 
			// btnResetRotation
			// 
			btnResetRotation.Location = new Point(255, 12);
			btnResetRotation.Name = "btnResetRotation";
			btnResetRotation.Size = new Size(80, 30);
			btnResetRotation.TabIndex = 5;
			btnResetRotation.Text = "Сброс вращ";
			btnResetRotation.UseVisualStyleBackColor = true;
			btnResetRotation.Click += btnResetRotation_Click;
			// 
			// buttonTrans
			// 
			buttonTrans.Location = new Point(300, 14);
			buttonTrans.Margin = new Padding(4, 3, 4, 3);
			buttonTrans.Name = "buttonTrans";
			buttonTrans.Size = new Size(70, 35);
			buttonTrans.TabIndex = 5;
			buttonTrans.Text = "Сместить";
			buttonTrans.UseVisualStyleBackColor = true;
			buttonTrans.Click += buttonTrans_Click;
			// 
			// buttonTetr
			// 
			buttonTetr.Location = new Point(13, 55);
			buttonTetr.Margin = new Padding(4, 3, 4, 3);
			buttonTetr.Name = "buttonTetr";
			buttonTetr.Size = new Size(78, 26);
			buttonTetr.TabIndex = 6;
			buttonTetr.Text = "Тетраэдр";
			buttonTetr.UseVisualStyleBackColor = true;
			buttonTetr.Click += buttonTetr_Click;
			// 
			// buttonGex
			// 
			buttonGex.Location = new Point(13, 87);
			buttonGex.Margin = new Padding(4, 3, 4, 3);
			buttonGex.Name = "buttonGex";
			buttonGex.Size = new Size(78, 26);
			buttonGex.TabIndex = 7;
			buttonGex.Text = "Гексаэдр";
			buttonGex.UseVisualStyleBackColor = true;
			buttonGex.Click += buttonGex_Click;
			// 
			// buttonOct
			// 
			buttonOct.Location = new Point(13, 119);
			buttonOct.Margin = new Padding(4, 3, 4, 3);
			buttonOct.Name = "buttonOct";
			buttonOct.Size = new Size(78, 26);
			buttonOct.TabIndex = 8;
			buttonOct.Text = "Октаэдр";
			buttonOct.UseVisualStyleBackColor = true;
			buttonOct.Click += buttonOct_Click;
			// 
			// buttonIco
			// 
			buttonIco.Location = new Point(14, 151);
			buttonIco.Margin = new Padding(4, 3, 4, 3);
			buttonIco.Name = "buttonIco";
			buttonIco.Size = new Size(78, 26);
			buttonIco.TabIndex = 9;
			buttonIco.Text = "Икосаэдр";
			buttonIco.UseVisualStyleBackColor = true;
			buttonIco.Click += buttonIco_Click;
			// 
			// buttonDod
			// 
			buttonDod.Location = new Point(14, 183);
			buttonDod.Margin = new Padding(4, 3, 4, 3);
			buttonDod.Name = "buttonDod";
			buttonDod.Size = new Size(78, 26);
			buttonDod.TabIndex = 10;
			buttonDod.Text = "Додекаэдр";
			buttonDod.UseVisualStyleBackColor = true;
			buttonDod.Click += buttonDod_Click;
			// 
			// buttonRefl
			// 
			buttonRefl.Location = new Point(451, 14);
			buttonRefl.Margin = new Padding(4, 3, 4, 3);
			buttonRefl.Name = "buttonRefl";
			buttonRefl.Size = new Size(70, 35);
			buttonRefl.TabIndex = 11;
			buttonRefl.Text = "Отразить";
			buttonRefl.UseVisualStyleBackColor = true;
			buttonRefl.Click += buttonRefl_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(915, 647);
			Controls.Add(buttonRefl);
			Controls.Add(buttonDod);
			Controls.Add(buttonIco);
			Controls.Add(buttonOct);
			Controls.Add(buttonGex);
			Controls.Add(buttonTetr);
			Controls.Add(buttonTrans);
			Controls.Add(btnSwitchProjection);
			Controls.Add(btnResetView);
			Controls.Add(btnZoomOut);
			Controls.Add(btnZoomIn);
			Controls.Add(pictureBox1);
			Margin = new Padding(4, 3, 4, 3);
			Name = "Form1";
			Text = "3D Viewer - Лабораторная работа 6";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
	}
}