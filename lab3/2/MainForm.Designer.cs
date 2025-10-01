namespace lab2
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
            modeSelector = new ComboBox();
            SuspendLayout();
            // 
            // modeSelector
            // 
            modeSelector.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            modeSelector.FormattingEnabled = true;
            modeSelector.Location = new Point(610, 3);
            modeSelector.Name = "modeSelector";
            modeSelector.Size = new Size(170, 28);
            modeSelector.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(modeSelector);
            DoubleBuffered = true;
            Name = "MainForm";
            Text = "Брезенхам (слева) и Ву (справа)";
            MouseClick += MainForm_MouseClick;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox modeSelector;
    }
}
