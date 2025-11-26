using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab6
{
    public partial class TextureSettingsForm : Form
    {
        public Texture SelectedTexture { get; private set; }

        public TextureSettingsForm()
        {
            CreateCustomComponents();
            CreateStripedTexture();
        }

        private void CreateCustomComponents()
        {
            this.SuspendLayout();

            this.Text = "Настройки текстуры";
            this.Size = new Size(300, 250); 
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var btnLoad = new Button();
            btnLoad.Text = "Загрузить текстуру";
            btnLoad.Location = new Point(20, 20);
            btnLoad.Size = new Size(150, 40);
            btnLoad.Click += BtnLoad_Click;

            var btnStripes = new Button();
            btnStripes.Text = "Текстура в полоску";
            btnStripes.Location = new Point(20, 70);
            btnStripes.Size = new Size(150, 40);
            btnStripes.Click += BtnStripes_Click;

            var btnRemoveTexture = new Button();
            btnRemoveTexture.Text = "Убрать текстуру";
            btnRemoveTexture.Location = new Point(20, 120);
            btnRemoveTexture.Size = new Size(150, 40);
            btnRemoveTexture.Click += BtnRemoveTexture_Click;
            btnRemoveTexture.BackColor = Color.LightCoral;

            var previewBox = new PictureBox();
            previewBox.Location = new Point(180, 20);
            previewBox.Size = new Size(80, 80);
            previewBox.BorderStyle = BorderStyle.FixedSingle;
            previewBox.SizeMode = PictureBoxSizeMode.StretchImage;
            previewBox.Name = "previewBox";

            var btnApply = new Button();
            btnApply.Text = "Применить";
            btnApply.Location = new Point(80, 170);
            btnApply.Size = new Size(80, 30);
            btnApply.Click += BtnApply_Click;
            btnApply.BackColor = Color.LightGreen;

            var btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Location = new Point(170, 170);
            btnCancel.Size = new Size(80, 30);
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] {
                btnLoad, btnStripes, btnRemoveTexture, previewBox, btnApply, btnCancel
            });

            this.ResumeLayout();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png;*.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";
            openFileDialog.Title = "Выберите файл текстуры";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var tempImage = Image.FromFile(openFileDialog.FileName))
                    {
                        var bitmap = new Bitmap(tempImage);
                        SelectedTexture = new Texture(bitmap);
                        UpdatePreview(bitmap);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке текстуры: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnStripes_Click(object sender, EventArgs e)
        {
            CreateStripedTexture();
        }

        private void BtnRemoveTexture_Click(object sender, EventArgs e)
        {
            SelectedTexture = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (SelectedTexture != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Сначала выберите текстуру",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CreateStripedTexture()
        {
            int size = 256;
            var bitmap = new Bitmap(size, size);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                int stripeWidth = size / 8;
                for (int i = 0; i < size; i += stripeWidth)
                {
                    Color color = (i / stripeWidth) % 2 == 0 ? Color.Blue : Color.White;
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillRectangle(brush, i, 0, stripeWidth, size);
                    }
                }
            }

            SelectedTexture = new Texture(bitmap);
            UpdatePreview(bitmap);
        }

        private void UpdatePreview(Bitmap bitmap)
        {
            var previewBox = this.Controls.Find("previewBox", true)[0] as PictureBox;
            if (previewBox != null)
            {
                previewBox.Image = new Bitmap(bitmap, previewBox.Size);
            }
        }
    }
}