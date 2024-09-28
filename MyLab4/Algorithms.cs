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
                var inputNormalizedFeatureVector = ZScoreNormalizeFeatureVector(inputFeatureVector);

                txtBlackPixelCount.Text = "";
                txtBlackPixelCount.AppendText($"   Result   {Environment.NewLine}");
                string folderPath = @"D:\code\.NET\AI\MyLab2\photo"; 

                double maxCosineSimilarity = double.MinValue;
                string mostSimilarImageCosine = "";

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
                        }
                    }
                    if (mostSimilarImageCosine != null)
                    {
                        string fileNamemostSimilarImageCosine = Path.GetFileName(mostSimilarImageCosine);

                        txtBlackPixelCount.AppendText($"{Environment.NewLine}Most Similar Images by Metric:{Environment.NewLine}");
                        txtBlackPixelCount.AppendText($"Cosine Similarity: {Path.GetFileName(fileNamemostSimilarImageCosine.IndexOf('.') >= 0 ? fileNamemostSimilarImageCosine.Substring(0, fileNamemostSimilarImageCosine.IndexOf('.')) : fileNamemostSimilarImageCosine)} - Similarity: {maxCosineSimilarity:F2}{Environment.NewLine}");
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
       
    }
}


