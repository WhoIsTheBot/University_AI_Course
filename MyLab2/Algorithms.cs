using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Linq;

namespace SimpleWinFormsApp
{
    public class Algorithms
    {
        private TextBox txtBlackPixelCount;

        // Конструктор для передачі залежностей
        public Algorithms(TextBox txtBox)
        {
            txtBlackPixelCount = txtBox;
        }
        public void definitionPhoto(Bitmap bmpOne)
        {
            try
            {
                Bitmap inputImage = new Bitmap(bmpOne);
                var inputFeatureVector = GetFeatureVector(inputImage);
                var inputNormalizedFeatureVector = NormalizeFeatureVector(inputFeatureVector);

                txtBlackPixelCount.Text = "";
                txtBlackPixelCount.AppendText($"   Result   {Environment.NewLine}");
                string folderPath = @"D:\code\.NET\AI\MyLab2\photo"; // Змініть на шлях до вашої папки з зображеннями

                double maxSimilarity = double.MinValue;
                string mostSimilarImage = "";

                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath, "*.bmp");
                    foreach (var file in files)
                    {
                        using (Bitmap bmp = new Bitmap(file))
                        {
                            var featureVector = GetFeatureVector(bmp);
                            var normalizedFeatureVector = NormalizeFeatureVector(featureVector);

                            double similarity = ComputeSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            string fileName = Path.GetFileName(file);
                            if (similarity > maxSimilarity)
                            {
                                maxSimilarity = similarity;
                                mostSimilarImage = file;
                            }
                            txtBlackPixelCount.AppendText($"Image: {fileName} - Similarity: {similarity:F2} {Environment.NewLine}");
                        }
                    }
                    if (mostSimilarImage != null)
                    {
                        string fileName = Path.GetFileName(mostSimilarImage);

                        // Знаходимо позицію першої крапки
                        int dotIndex = fileName.IndexOf('.');

                        // Якщо крапка знайдена, беремо частину до неї
                        string fileNameBeforeDot = dotIndex >= 0 ? fileName.Substring(0, dotIndex) : fileName;

                        // Виведення імені файлу та схожості
                        txtBlackPixelCount.AppendText($"Most similar image: {fileNameBeforeDot} - Similarity: {maxSimilarity:F2}");
                    }
                    else
                    {
                        txtBlackPixelCount.AppendText($"Error");
                    }
                }

                else
                {
                    MessageBox.Show($"Directory does not exist.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public List<int> GetFeatureVector(Bitmap bmp)
        {
            List<int> featureVector = new List<int>();

            int numberSecors = 25;
            int sideLength = bmp.Height / numberSecors;

            for (int y = 0; y < numberSecors; y++)
            {
                for (int x = 0; x < numberSecors; x++)
                {
                    int blackPixelCount = 0;
                    for (int i = y * sideLength; i < (y + 1) * sideLength; i++)
                    {
                        for (int j = x * sideLength; j < (x + 1) * sideLength; j++)
                        {
                            Color pixelColor = bmp.GetPixel(j, i);
                            if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
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

        public List<float> NormalizeFeatureVector(List<int> featureVector)
        {
            int maxValue = featureVector.Max();
            return featureVector.Select(v => maxValue > 0 ? (float)v / maxValue : 0).ToList();
        }

        public double ComputeSimilarity(List<float> vector1, List<float> vector2)
        {
            if (vector1.Count != vector2.Count)
            {
                throw new ArgumentException("Feature vectors must have the same length.");
            }

            double dotProduct = vector1.Zip(vector2, (v1, v2) => v1 * v2).Sum();
            double magnitude1 = Math.Sqrt(vector1.Sum(v => v * v));
            double magnitude2 = Math.Sqrt(vector2.Sum(v => v * v));

            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0; // One of the vectors is zero, so similarity is zero
            }

            return dotProduct / (magnitude1 * magnitude2); // Cosine similarity
        }



        public void DrawGrid(Bitmap bmpImage)
        {
            using (Graphics g = Graphics.FromImage(bmpImage))
            {
                Pen gridPen = new Pen(Color.Red, 1);
                int cell = 25;

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


