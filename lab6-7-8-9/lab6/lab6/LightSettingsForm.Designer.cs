namespace lab6
{
	partial class LightSettingsForm
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
			buttonOK = new Button();
			buttonCancel = new Button();
			numericX = new NumericUpDown();
			label1 = new Label();
			numericY = new NumericUpDown();
			numericZ = new NumericUpDown();
			((System.ComponentModel.ISupportInitialize)numericX).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericY).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericZ).BeginInit();
			SuspendLayout();
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			buttonOK.DialogResult = DialogResult.OK;
			buttonOK.Location = new Point(47, 109);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(75, 23);
			buttonOK.TabIndex = 0;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.DialogResult = DialogResult.Cancel;
			buttonCancel.Location = new Point(167, 109);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(75, 23);
			buttonCancel.TabIndex = 1;
			buttonCancel.Text = "Отмена";
			buttonCancel.UseVisualStyleBackColor = true;
			// 
			// numericX
			// 
			numericX.Location = new Point(34, 65);
			numericX.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
			numericX.Name = "numericX";
			numericX.Size = new Size(54, 23);
			numericX.TabIndex = 3;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BackColor = SystemColors.InactiveCaption;
			label1.Location = new Point(59, 26);
			label1.Name = "label1";
			label1.Size = new Size(163, 15);
			label1.TabIndex = 4;
			label1.Text = "Позиция источника (X, Y, Z):";
			// 
			// numericY
			// 
			numericY.Location = new Point(119, 65);
			numericY.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
			numericY.Name = "numericY";
			numericY.Size = new Size(54, 23);
			numericY.TabIndex = 5;
			// 
			// numericZ
			// 
			numericZ.Location = new Point(206, 65);
			numericZ.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
			numericZ.Name = "numericZ";
			numericZ.Size = new Size(54, 23);
			numericZ.TabIndex = 6;
			// 
			// LightSettingsForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(284, 148);
			Controls.Add(numericZ);
			Controls.Add(numericY);
			Controls.Add(label1);
			Controls.Add(numericX);
			Controls.Add(buttonCancel);
			Controls.Add(buttonOK);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Name = "LightSettingsForm";
			Text = "Настройки освещения";
			((System.ComponentModel.ISupportInitialize)numericX).EndInit();
			((System.ComponentModel.ISupportInitialize)numericY).EndInit();
			((System.ComponentModel.ISupportInitialize)numericZ).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button buttonOK;
		private Button buttonCancel;
		private NumericUpDown numericX;
		private Label label1;
		private NumericUpDown numericY;
		private NumericUpDown numericZ;
	}
}