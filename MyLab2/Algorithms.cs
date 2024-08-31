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
                //var inputNormalizedFeatureVector = NormalizeFeatureVector(inputFeatureVector);
                var inputNormalizedFeatureVector = ZScoreNormalizeFeatureVector(inputFeatureVector);

                txtBlackPixelCount.Text = "";
                txtBlackPixelCount.AppendText($"   Result   {Environment.NewLine}");
                string folderPath = @"D:\code\.NET\AI\MyLab2\photo"; // Змініть на шлях до вашої папки з зображеннями

                double maxCosineSimilarity = double.MinValue;
                double maxChebyshevSimilarity = double.MinValue;
                double maxManhattanSimilarity = double.MinValue;
                double maxEuclideanSimilarity = double.MinValue;

                string mostSimilarImageCosine = "";
                string mostSimilarImageChebyshev = "";
                string mostSimilarImageManhattan = "";
                string mostSimilarImageEuclidean = "";

                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath, "*.bmp");
                    foreach (var file in files)
                    {
                        using (Bitmap bmp = new Bitmap(file))
                        {
                            var featureVector = GetFeatureVector(bmp);
                            var normalizedFeatureVector = ZScoreNormalizeFeatureVector(featureVector);
                            //var normalizedFeatureVector = NormalizeFeatureVector(featureVector);
                            string fileName = Path.GetFileName(file);
                            txtBlackPixelCount.AppendText($"Image: {fileName}{Environment.NewLine}");

                            // Косинусна подібність
                            double cosineSimilarity = ComputeSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            txtBlackPixelCount.AppendText($"Cosine similarity: {cosineSimilarity:F2} {Environment.NewLine}");
                            if (cosineSimilarity > maxCosineSimilarity)
                            {
                                maxCosineSimilarity = cosineSimilarity;
                                mostSimilarImageCosine = file;
                            }

                            // Норма Чебишева
                            double chebyshevSimilarity = ComputeChebyshevSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            txtBlackPixelCount.AppendText($"Chebyshev norm (L∞-norm): {chebyshevSimilarity:F2} {Environment.NewLine}");
                            if (chebyshevSimilarity > maxChebyshevSimilarity)
                            {
                                maxChebyshevSimilarity = chebyshevSimilarity;
                                mostSimilarImageChebyshev = file;
                            }

                            // Манхетенська норма
                            double manhattanSimilarity = ComputeManhattanSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            txtBlackPixelCount.AppendText($"Manhattan norm (L1-norm): {manhattanSimilarity:F2} {Environment.NewLine}");
                            if (manhattanSimilarity > maxManhattanSimilarity)
                            {
                                maxManhattanSimilarity = manhattanSimilarity;
                                mostSimilarImageManhattan = file;
                            }

                            // Евклідова норма
                            double euclideanSimilarity = ComputeEuclideanSimilarity(inputNormalizedFeatureVector, normalizedFeatureVector);
                            txtBlackPixelCount.AppendText($"Euclidean norm (L2-norm): {euclideanSimilarity:F2} {Environment.NewLine}");
                            if (euclideanSimilarity > maxEuclideanSimilarity)
                            {
                                maxEuclideanSimilarity = euclideanSimilarity;
                                mostSimilarImageEuclidean = file;
                            }
                        }
                    }
                    if (mostSimilarImageChebyshev != null)
                    {
                        string fileNameSimilarImageChebyshev = Path.GetFileName(mostSimilarImageChebyshev);
                        string fileNamemostSimilarImageCosine = Path.GetFileName(mostSimilarImageCosine);
                        string filemostSimilarImageManhattan = Path.GetFileName(mostSimilarImageManhattan);
                        string fileNamemostSimilarImageEuclidean = Path.GetFileName(mostSimilarImageEuclidean);

                        txtBlackPixelCount.AppendText($"{Environment.NewLine}Most Similar Images by Metric:{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Cosine Similarity: {Path.GetFileName(fileNamemostSimilarImageCosine.IndexOf('.') >= 0 ? fileNamemostSimilarImageCosine.Substring(0, fileNamemostSimilarImageCosine.IndexOf('.')) : fileNamemostSimilarImageCosine)} - Similarity: {maxCosineSimilarity:F2}{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Chebyshev Similarity: {Path.GetFileName(fileNameSimilarImageChebyshev.IndexOf('.') >= 0 ? fileNameSimilarImageChebyshev.Substring(0, fileNameSimilarImageChebyshev.IndexOf('.')) : fileNameSimilarImageChebyshev)} - Similarity: {maxChebyshevSimilarity:F2}{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Manhattan Similarity: {Path.GetFileName(filemostSimilarImageManhattan.IndexOf('.') >= 0 ? filemostSimilarImageManhattan.Substring(0, filemostSimilarImageManhattan.IndexOf('.')) : filemostSimilarImageManhattan)} - Similarity: {maxManhattanSimilarity:F2}{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Euclidean Similarity: {Path.GetFileName(fileNamemostSimilarImageEuclidean.IndexOf('.') >= 0 ? fileNamemostSimilarImageEuclidean.Substring(0, fileNamemostSimilarImageEuclidean.IndexOf('.')) : fileNamemostSimilarImageEuclidean)} - Similarity: {maxEuclideanSimilarity:F2}{Environment.NewLine}");

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
        //---- Норма Чебишева ----
        public double ComputeChebyshevSimilarity(List<float> vector1, List<float> vector2)
        {
            if (vector1.Count != vector2.Count)
            {
                throw new ArgumentException("Feature vectors must have the same length.");
            }

            double maxDifference = 0;

            for (int i = 0; i < vector1.Count; i++)
            {
                double difference = Math.Abs(vector1[i] - vector2[i]);
                if (difference > maxDifference)
                {
                    maxDifference = difference;
                }
            }

            // Норма Чебишева повертає найбільшу відстань, але для схожості краще використовувати зворотне значення
            return 1 / (1 + maxDifference); // Додаємо 1, щоб уникнути ділення на нуль
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


        //---- Евклідова норма (L2-норма) ----
        public double ComputeEuclideanSimilarity(List<float> vector1, List<float> vector2)
        {
            if (vector1.Count != vector2.Count)
            {
                throw new ArgumentException("Feature vectors must have the same length.");
            }

            double sumOfSquaredDifferences = 0;

            for (int i = 0; i < vector1.Count; i++)
            {
                double difference = vector1[i] - vector2[i];
                sumOfSquaredDifferences += difference * difference;
            }

            double euclideanDistance = Math.Sqrt(sumOfSquaredDifferences);

            return 1 / (1 + euclideanDistance); // Додаємо 1, щоб уникнути ділення на нуль
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


