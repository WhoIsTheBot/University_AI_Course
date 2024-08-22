using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace SimpleWinFormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
                bmpImage = new Bitmap(filePath);
                pictureBox.Image = bmpImage;
                // Зміна режиму відображення
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; 

                DrawGrid(bmpImage);

                // Отримання розміру зображення
                Size imageSize = pictureBox.Image.Size;
                MessageBox.Show($"Розмір зображення: {imageSize.Width} x {imageSize.Height} пікселів");

            }
        }

        private void BtnCalculateVector_Click(object sender, EventArgs e)
        {
            if (bmpImage != null)
            {
                CountBlackPixels(bmpImage);
            }
            else
            {
                MessageBox.Show("Будь ласка, завантажте зображення спочатку.");
            }
        }

        private void CountBlackPixels(Bitmap bmp)
        {
            ArrayList howIsVector = new ArrayList();
            int blockHeight = bmp.Height / 4;
            int blockWidth = bmp.Width / 3;

            txtBlackPixelCount.Text = "";
            txtBlackPixelCount.AppendText($" Vector{Environment.NewLine}");
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    int blackPixelCount = 0;
                    for (int i = y * blockHeight; i < (y + 1) * blockHeight; i++)
                    {
                        for (int j = x * blockWidth; j < (x + 1) * blockWidth; j++)
                        {
                            Color pixelColor = bmp.GetPixel(j, i);

                            // Перевірка, чи піксель чорного кольору
                            if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
                            {
                                blackPixelCount++;
                            }
                        }
                    }

                    howIsVector.Add(blackPixelCount);
                    txtBlackPixelCount.AppendText($"{blackPixelCount} ");
                }
            }

            Sum(howIsVector);
        }

        private void Sum(ArrayList howIsVector)
        {
            int sumNumberVector = 0;
            int maxValue = int.MinValue;
            foreach (object obj in howIsVector)
            {
                if (obj is int count)
                {
                    sumNumberVector += count;
                    if (count > maxValue)
                    {
                        maxValue = count;
                    }
                }
            }
            txtBlackPixelCount.AppendText($"{Environment.NewLine}");
            txtBlackPixelCount.AppendText($"  Max - {maxValue}{Environment.NewLine}");
            foreach (object obj in howIsVector)
            {
                if (obj is int count)
                {
                    float result = (float)count / maxValue;
                    txtBlackPixelCount.AppendText($"{result:F2} ");
                }
                else
                {
                    MessageBox.Show("Помилка: Неправильний тип даних у ArrayList");
                    return;
                }
            }
            txtBlackPixelCount.AppendText($"{Environment.NewLine}");
            txtBlackPixelCount.AppendText($"  Sum - {sumNumberVector}{Environment.NewLine}");
            float resultSum = 0;
            foreach (object obj in howIsVector)
            {
                if (obj is int count)
                {
                    float result = (float)count / sumNumberVector;
                    resultSum += result;
                    txtBlackPixelCount.AppendText($"{result:F2} "); 
                }
                else
                {
                    MessageBox.Show("Помилка: Неправильний тип даних у ArrayList");
                    return;
                }
            }

            txtBlackPixelCount.AppendText($"{Environment.NewLine}{resultSum:F2} ");
        }

        private void DrawGrid(Bitmap bmpImage)
        {
            using (Graphics g = Graphics.FromImage(bmpImage))
            {
                Pen gridPen = new Pen(Color.Red, 1); 
                int cellWidth = 100; 
                int cellHeight = 100; 

                for (int x = cellWidth; x < bmpImage.Width; x += cellWidth)
                {
                    g.DrawLine(gridPen, x, 0, x, bmpImage.Height);
                }

                for (int y = cellHeight; y < bmpImage.Height; y += cellHeight)
                {
                    g.DrawLine(gridPen, 0, y, bmpImage.Width, y);
                }
            }
        }


    }
}
