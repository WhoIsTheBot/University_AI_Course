using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic; // Для List


namespace SimpleWinFormsApp
{
    public partial class MainForm : Form
    {
        private Perceptron perceptron;
        private Bitmap? bmpImage;
        private int inputSize = 25 * 25; // Розмір вхідного вектора ознак (кількість секторів)
        private bool isGridVisible = false; // Змінна для контролю видимості сітки


        public MainForm()
        {
            InitializeComponent();
            perceptron = new Perceptron(txtBlackPixelCount, inputSize); // Ініціалізуємо перцептрон
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


                // Отримання розміру зображення
                // Size imageSize = pictureBox.Image.Size;
                // MessageBox.Show($"Розмір зображення: {imageSize.Width} x {imageSize.Height} пікселів");
            }
        }

        private void BtnCalculateVector_Click(object sender, EventArgs e)
        {
            if (bmpImage != null)
            {

                perceptron.ClassifyImage(bmpImage); // Класифікуємо зображення за допомогою перцептрону
            }
            else
            {
                MessageBox.Show("Будь ласка, завантажте зображення спочатку.");
            }
        }

        private void BtnDrawing_Click(object sender, EventArgs e)
        {
            if (bmpImage == null) // Check if the image is null
            {
                MessageBox.Show("Будь ласка, завантажте зображення спочатку."); // Show a message
                return; // Exit the method early
            }

            isGridVisible = !isGridVisible; // Toggle the grid visibility state

            if (isGridVisible)
            {
                DrawGrid(bmpImage); // Draw the grid if it's visible
            }
            else
            {
                pictureBox.Image = bmpImage; // Show the original image without the grid
            }
        }



        private void BtnTrain_Click(object sender, EventArgs e)
        {
            // Отримуємо кореневу директорію проєкту
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            // Список векторів ознак для навчальних зображень
            var trainingData = new List<List<int>>();
            // Список відповідних міток (1 для класу A, -1 для класу B)
            var labels = new List<int>();

            // Створюємо відносний шлях до папки з фото класу A
            string classAFolder = @"D:\code\.NET\AI\MyLab4\photo\A";
            if (Directory.Exists(classAFolder))
            {
                var classAFiles = Directory.GetFiles(classAFolder, "*.bmp");
                foreach (var file in classAFiles)
                {
                    using (Bitmap bmp = new Bitmap(file))
                    {
                        var featureVector = perceptron.GetFeatureVector(bmp); // Обчислення вектора ознак
                        trainingData.Add(featureVector); // Додавання вектора до trainingData
                        labels.Add(1); // Додавання мітки для класу A
                    }
                }
            }
            txtBlackPixelCount.AppendText($"Training Data Count: {trainingData.Count}, Labels Count: {labels.Count}{Environment.NewLine}");


            // Завантаження навчальних зображень для класу B
            string classBFolder = @"D:\code\.NET\AI\MyLab4\photo\B";
            if (Directory.Exists(classBFolder))
            {
                var classBFiles = Directory.GetFiles(classBFolder, "*.bmp");
                foreach (var file in classBFiles)
                {
                    using (Bitmap bmp = new Bitmap(file))
                    {
                        var featureVector = perceptron.GetFeatureVector(bmp); // Обчислення вектора ознак
                        trainingData.Add(featureVector); // Додавання вектора до trainingData
                        labels.Add(-1); // Додавання мітки для класу B
                    }
                }
            }
            txtBlackPixelCount.AppendText($"Training Data Count: {trainingData.Count}, Labels Count: {labels.Count}{Environment.NewLine}");


            // Навчання перцептрону
            int epochs = 1000; // Кількість епох для навчання
            if (trainingData.Count != labels.Count)
            {
                MessageBox.Show("Кількість векторів ознак не відповідає кількості міток.");
                return;
            }

            perceptron.Train(trainingData, labels, epochs);
            txtBlackPixelCount.AppendText($"{Environment.NewLine}Навчання завершено.{Environment.NewLine}");
    
        }


        public void DrawGrid(Bitmap bmpImage)
        {
            Bitmap gridImage = new Bitmap(bmpImage); // Клон зображення для малювання сітки

            using (Graphics g = Graphics.FromImage(gridImage))
            {
                Pen gridPen = new Pen(Color.Red, 1);
                int cell = 100;

                for (int x = cell; x < bmpImage.Width; x += cell)
                {
                    g.DrawLine(gridPen, x, 0, x, bmpImage.Height);
                }

                for (int y = cell; y < bmpImage.Height; y += cell)
                {
                    g.DrawLine(gridPen, 0, y, bmpImage.Width, y);
                }
            }

            pictureBox.Image = gridImage; // Відображаємо зображення з сіткою
        }
    }
}
