using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                //var inputNormalizedFeatureVector = NormalizeFeatureVector(inputFeatureVector);
                var inputNormalizedFeatureVector = ZScoreNormalizeFeatureVector(inputFeatureVector);

                txtBlackPixelCount.Text = "";
                txtBlackPixelCount.AppendText($"   Result   {Environment.NewLine}");
                string folderPath = @"C:\Users\G_I\OneDrive\Рабочий стол\photo";
                var allFeatureVectors = new Dictionary<string, List<float>>();

                double maxCosineSimilarity = double.MinValue;
                double maxManhattanSimilarity = double.MinValue;
                string mostSimilarImageCosine = "";
                string mostSimilarImageManhattan = "";


                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath, "*.bmp");
                    foreach (var file in files)
                    {
                        using (Bitmap bmp = new Bitmap(file))
                        {
                            var featureVector = GetFeatureVector(bmp);
                            var normalizedFeatureVector = ZScoreNormalizeFeatureVector(featureVector);
                            string fileName = Path.GetFileName(file);

                            // Додаємо нормалізований вектор ознак до словника
                            allFeatureVectors[fileName] = normalizedFeatureVector;


                            txtBlackPixelCount.AppendText($"Image: {fileName}{Environment.NewLine}");

                            // Косинусна подібність
                            double cosineSimilarity = ComputeSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            txtBlackPixelCount.AppendText($"Cosine similarity: {cosineSimilarity:F2} {Environment.NewLine}");
                            if (cosineSimilarity > maxCosineSimilarity)
                            {
                                maxCosineSimilarity = cosineSimilarity;
                                mostSimilarImageCosine = file;
                            }

                            // Манхетенська норма
                            double manhattanSimilarity = ComputeManhattanSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            txtBlackPixelCount.AppendText($"Manhattan norm (L1-norm): {manhattanSimilarity:F2} {Environment.NewLine}");
                            if (manhattanSimilarity > maxManhattanSimilarity)
                            {
                                maxManhattanSimilarity = manhattanSimilarity;
                                mostSimilarImageManhattan = file;
                            }


                        }
                    }
                    // Серіалізуємо всі вектори ознак в один JSON
                    string json = JsonSerializer.Serialize(allFeatureVectors, new JsonSerializerOptions { WriteIndented = true });

                    // Зберігаємо JSON у файл
                    string jsonFilePath = Path.Combine(folderPath, "all_features.json");
                    File.WriteAllText(jsonFilePath, json);

                    if (mostSimilarImageManhattan != null)
                    {

                        string fileNamemostSimilarImageCosine = Path.GetFileName(mostSimilarImageCosine);
                        string filemostSimilarImageManhattan = Path.GetFileName(mostSimilarImageManhattan);


                        txtBlackPixelCount.AppendText($"{Environment.NewLine}Most Similar Images by Metric:{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Cosine Similarity: {Path.GetFileName(fileNamemostSimilarImageCosine.IndexOf('.') >= 0 ? fileNamemostSimilarImageCosine.Substring(0, fileNamemostSimilarImageCosine.IndexOf('.')) : fileNamemostSimilarImageCosine)} - Similarity: {maxCosineSimilarity:F2}{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Manhattan Similarity: {Path.GetFileName(filemostSimilarImageManhattan.IndexOf('.') >= 0 ? filemostSimilarImageManhattan.Substring(0, filemostSimilarImageManhattan.IndexOf('.')) : filemostSimilarImageManhattan)} - Similarity: {maxManhattanSimilarity:F2}{Environment.NewLine}");

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

            int blockHeight = bmp.Height / 10;
            int blockWidth = bmp.Width / 8;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    int blackPixelCount = 0;
                    for (int i = y * blockHeight; i < (y + 1) * blockHeight; i++)
                    {
                        for (int j = x * blockWidth; j < (x + 1) * blockWidth; j++)
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

        public List<float> ZScoreNormalizeFeatureVector(List<int> featureVector)
        {
            double mean = featureVector.Average();
            double stdDev = Math.Sqrt(featureVector.Sum(v => Math.Pow(v - mean, 2)) / featureVector.Count);

            return featureVector.Select(v => stdDev > 0 ? (float)((v - mean) / stdDev) : 0).ToList();
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

        //---- Манхетенська норма (L1-норма) ----
        public double ComputeManhattanSimilarity(List<float> vector1, List<float> vector2)
        {
            if (vector1.Count != vector2.Count)
            {
                throw new ArgumentException("Feature vectors must have the same length.");
            }

            double sumOfDifferences = 0;

            for (int i = 0; i < vector1.Count; i++)
            {
                sumOfDifferences += Math.Abs(vector1[i] - vector2[i]);
            }

            // Нормалізація результату
            double maxPossibleDifference = vector1.Count; // Максимальна можлива різниця при нормалізованих значеннях
            double normalizedDifference = sumOfDifferences / maxPossibleDifference;

            return 1 / (1 + normalizedDifference); // Додаємо 1, щоб уникнути ділення на нуль
        }

    }
}


