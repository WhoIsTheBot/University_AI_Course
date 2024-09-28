using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic; // Для List


namespace SimpleWinFormsApp
{
    public class Perceptron
    {
        private TextBox txtBlackPixelCount;
        private List<float> weights;  // Вектор ваг
        private float bias;           // Зсув
        private float learningRate;   // Швидкість навчання

        // Конструктор для передачі залежностей і ініціалізації
        public Perceptron(TextBox txtBox, int inputSize, float learningRate = 0.01f)
        {
            txtBlackPixelCount = txtBox;
            this.learningRate = learningRate;
            weights = new List<float>(new float[inputSize]);  // Ініціалізація ваг нулями
            bias = 0;  // Ініціалізація зсуву
        }

        // Метод для навчання перцептрону
        public void Train(List<List<int>> featureVectors, List<int> labels, int epochs)
        {
            txtBlackPixelCount.AppendText($"Кількість векторів ознак: {featureVectors.Count}, Кількість міток: {labels.Count}{Environment.NewLine}");

            if (featureVectors.Count != labels.Count)
            {
                throw new ArgumentException($"Кількість векторів ознак повинна відповідати кількості міток.{Environment.NewLine}");
            }

            // Для кожної епохи навчання
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                for (int i = 0; i < featureVectors.Count; i++)
                {
                    var vector = featureVectors[i];
                    var label = labels[i];

                    // Нормалізація вектора ознак
                    var normalizedVector = ZScoreNormalizeFeatureVector(vector);

                    // Передбачення класу
                    int prediction = Predict(normalizedVector);

                    // Налаштування ваг
                    if (prediction != label)
                    {
                        for (int j = 0; j < normalizedVector.Count; j++)
                        {
                            weights[j] += learningRate * (label - prediction) * normalizedVector[j];
                        }
                        bias += learningRate * (label - prediction);
                    }
                }
            }
        }



        // Метод для передбачення класу (1 або -1)
        public int Predict(List<float> input)
        {
            float sum = bias;

            for (int i = 0; i < input.Count; i++)
            {
                sum += input[i] * weights[i];
            }

            // Порогова функція активації
            return sum >= 0 ? 1 : -1;
        }

        // Метод для визначення класу зображення
        public void ClassifyImage(Bitmap bmp)
        {

            try
            {
                Bitmap inputImage = new Bitmap(bmp);
                var inputFeatureVector = GetFeatureVector(inputImage);
                var inputNormalizedFeatureVector = ZScoreNormalizeFeatureVector(inputFeatureVector);

                int output = Predict(inputNormalizedFeatureVector);

                txtBlackPixelCount.Text = "";
                txtBlackPixelCount.AppendText($"Результат розпізнавання: {(output == 1 ? "Клас A" : "Клас B")}{Environment.NewLine}");
            }
            catch (Exception ex)
            {


                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Отримання вектору ознак зображення
        public List<int> GetFeatureVector(Bitmap bmp)
        {

            List<int> featureVector = new List<int>();

            int sectorSize = 100; // Розмір сектора
            int numberSectorsX = bmp.Width / sectorSize; // Кількість секторів по ширині
            int numberSectorsY = bmp.Height / sectorSize; // Кількість секторів по висоті

            for (int y = 0; y < numberSectorsY; y++)
            {
                for (int x = 0; x < numberSectorsX; x++)
                {
                    int blackPixelCount = 0;

                    // Визначення меж для обчислення координат
                    int startY = y * sectorSize;
                    int endY = Math.Min((y + 1) * sectorSize, bmp.Height);
                    int startX = x * sectorSize;
                    int endX = Math.Min((x + 1) * sectorSize, bmp.Width);

                    for (int i = startY; i < endY; i++)
                    {
                        for (int j = startX; j < endX; j++)
                        {
                            Color pixelColor = bmp.GetPixel(j, i);
                            if (IsBlackPixel(pixelColor))
                            {
                                blackPixelCount++;
                            }
                        }
                    }
                    featureVector.Add(blackPixelCount);
                }
            }

            return featureVector;
        }


        // Метод для перевірки чорного пікселя
        private bool IsBlackPixel(Color pixelColor)
        {
            return pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0;
        }

        // Нормалізація вектору ознак за методом Z-score
        public List<float> ZScoreNormalizeFeatureVector(List<int> featureVector)
        {

            double mean = featureVector.Average();
            double stdDev = Math.Sqrt(featureVector.Sum(v => Math.Pow(v - mean, 2)) / featureVector.Count);

            return featureVector.Select(v => stdDev > 0 ? (float)((v - mean) / stdDev) : 0).ToList();
        }

        private Bitmap ResizeImage(Bitmap bmp, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
            return resizedImage;
        }

    }
}
