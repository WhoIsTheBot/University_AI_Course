using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleWinFormsApp
{
    public partial class MainForm : Form
    {
        private Algorithms algorithms;
        private Bitmap? bmpImage; // Nullable тип для уникнення помилки CS8618

        public MainForm()
        {
            InitializeComponent();
            algorithms = new Algorithms(txtBlackPixelCount);
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            // Створення діалогу для вибору файлу
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BMP files (*.bmp)|*.bmp";
            openFileDialog.Title = "Виберіть BMP зображення";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Завантаження та відображення зображення
                string filePath = openFileDialog.FileName;
                bmpImage = new Bitmap(filePath); // Використання поля класу
                pictureBox.Image = bmpImage;
                // Зміна режиму відображення
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; 
                DrawGrid(bmpImage);

                // Отримання розміру зображення
                // Size imageSize = pictureBox.Image.Size;
                // MessageBox.Show($"Розмір зображення: {imageSize.Width} x {imageSize.Height} пікселів");
            }
        }

        private void BtnCalculateVector_Click(object sender, EventArgs e)
        {
            if (bmpImage != null)
            {
                algorithms.definitionPhoto(bmpImage); // Використання поля класу
                //algorithms.definitionPhoto(); // Використання поля класу
            }
            else
            {
                MessageBox.Show("Будь ласка, завантажте зображення спочатку.");
            }
        }

        public void DrawGrid(Bitmap bmpImage)
        {
            using (Graphics g = Graphics.FromImage(bmpImage))
            {
                Pen gridPen = new Pen(Color.Red, 1);
                int cell = 50;

                for (int x = cell; x < bmpImage.Width; x += cell)
                {
                    g.DrawLine(gridPen, x, 0, x, bmpImage.Height);
                }

                for (int y = cell; y < bmpImage.Height; y += cell)
                {
                    g.DrawLine(gridPen, 0, y, bmpImage.Width, y);
                }
            }
        }
    }
}
