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
                var inputNormalizedFeatureVector = NormalizeFeatureVector(inputFeatureVector);
                //var inputNormalizedFeatureVector = ZScoreNormalizeFeatureVector(inputFeatureVector);

                txtBlackPixelCount.Text = "";
                txtBlackPixelCount.AppendText($"   Result   {Environment.NewLine}");
                string folderPath = @"C:\Users\G_I\OneDrive\Рабочий стол\photo";

                var groupedFeatureVectors = new Dictionary<string, List<List<float>>>();
                var averageFeatureVectors = new Dictionary<string, List<float>>();

                double maxCosineSimilarity = double.MinValue;
                double maxManhattanSimilarity = double.MinValue;
                string mostSimilarImageCosine = "";
                string mostSimilarImageManhattan = "";

                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath, "*.bmp");
                    foreach (var file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string prefix = fileName.Split('.')[0];

                        using (Bitmap bmp = new Bitmap(file))
                        {
                            var featureVector = GetFeatureVector(bmp);
                            //var normalizedFeatureVector = ZScoreNormalizeFeatureVector(featureVector);
                            var normalizedFeatureVector = NormalizeFeatureVector(featureVector);
                            //string fileName = Path.GetFileName(file);

                            // Додаємо нормалізований вектор до відповідної групи
                            if (!groupedFeatureVectors.ContainsKey(prefix))
                            {
                                groupedFeatureVectors[prefix] = new List<List<float>>();
                            }
                            groupedFeatureVectors[prefix].Add(normalizedFeatureVector);
                        }
                    }
                    // Серіалізація кожної групи у окремий JSON файл
                    foreach (var group in groupedFeatureVectors)
                    {
                        string jsonFilePath = Path.Combine(folderPath, $"{group.Key}.json");
                        string json = JsonSerializer.Serialize(group.Value, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(jsonFilePath, json);

                        // Обчислення середнього вектора
                        var averageVector = CalculateAverageVector(group.Value);
                        averageFeatureVectors[group.Key] = averageVector;

                        txtBlackPixelCount.AppendText($"Image: {group.Key}{Environment.NewLine}");

                        // Косинусна подібність
                        double cosineSimilarity = ComputeSimilarity(inputNormalizedFeatureVector, averageVector);
                        txtBlackPixelCount.AppendText($"Cosine similarity: {cosineSimilarity:F2} {Environment.NewLine}");
                        if (cosineSimilarity > maxCosineSimilarity)
                        {
                            maxCosineSimilarity = cosineSimilarity;
                            mostSimilarImageCosine = group.Key;
                        }

                        // Манхетенська норма
                        double manhattanSimilarity = ComputeManhattanSimilarity(inputNormalizedFeatureVector, averageVector);
                        txtBlackPixelCount.AppendText($"Manhattan norm (L1-norm): {manhattanSimilarity:F2} {Environment.NewLine}");
                        if (manhattanSimilarity > maxManhattanSimilarity)
                        {
                            maxManhattanSimilarity = manhattanSimilarity;
                            mostSimilarImageManhattan = group.Key;
                        }
                    }

                    // Серіалізація середніх векторів у окремий JSON файл
                    string averageJsonFilePath = Path.Combine(folderPath, "average_vectors.json");
                    string averageJson = JsonSerializer.Serialize(averageFeatureVectors, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(averageJsonFilePath, averageJson);

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


        public List<float> CalculateAverageVector(List<List<float>> vectors)
        {
            int vectorLength = vectors[0].Count;
            List<float> averageVector = new List<float>(new float[vectorLength]);

            foreach (var vector in vectors)
            {
                for (int i = 0; i < vectorLength; i++)
                {
                    averageVector[i] += vector[i];
                }
            }

            for (int i = 0; i < vectorLength; i++)
            {
                averageVector[i] /= vectors.Count;
            }

            return averageVector;
        }

    }
}


