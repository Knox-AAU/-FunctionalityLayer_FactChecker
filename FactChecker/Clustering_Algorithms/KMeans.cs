using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
namespace FactChecker.Clustering{
public class KMeans
{
    public int numberOfClusters { get; set; } = 5;
    public ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters> Model { get; set; }
    public MLContext mlContext { get; set; }
    public void Train(List<DataPoint> traningData = default)
    {
        mlContext = new MLContext(seed: 0);
        if (traningData == null) {
            traningData = GenerateRandomDataPoints(10000, 16).ToList();
        }
        var dataPoints = traningData;
        IDataView trainingData = mlContext.Data.LoadFromEnumerable(dataPoints);
        var pipeline = mlContext.Clustering.Trainers.KMeans(
            numberOfClusters: numberOfClusters);

        Model = pipeline.Fit(trainingData);

    }
        public List<Prediction> Prediction(IEnumerable<DataPoint> dataPoints = default, bool printMetrics = true){
            if (dataPoints == null) {
                Console.WriteLine("is null");
                dataPoints = GenerateRandomDataPoints(5000, seed: 312);
            }
            // Create testing data. Use a different random seed to make it different
            // from the training data.
            var testData = mlContext.Data.LoadFromEnumerable(dataPoints);

            // Run the model on test data set.
            var transformedTestData = Model.Transform(testData);
            // Convert IDataView object to a list.
            var predictions = mlContext.Data.CreateEnumerable<Prediction>(
                transformedTestData, reuseRowObject: false).ToList();
            if (printMetrics) {
                var metrics = mlContext.Clustering.Evaluate(
                    transformedTestData, "PredictedLabel", scoreColumnName: "Score", "Features");

                PrintMetrics(metrics);
            }

           predictions = predictions.OrderBy(p => p.PredictedClusterId).ToList();
            foreach (var p in predictions)
            {
                Console.WriteLine(
                    $"The first 3 coordinates of the {p.PredictedClusterId} centroid are: " +
                    string.Join(", ", p.Distances.ToArray().Take(3)));
            }

            return predictions;
        }
        private IEnumerable<DataPoint> GenerateRandomDataPoints(int count, int seed = 0)
        {
            var random = new Random(seed);

            float randomFloat() => (float)random.NextDouble();
            for (int i = 0; i < count; i++)
            {
                
                yield return new DataPoint
                {
                
                // Create random features with two clusters.
                // The first half has feature values centered around 0.6, while
                // the second half has values centered around 0.4.
                Features = Enumerable.Repeat(randomFloat(),150).ToArray(),
                    
                };
            
            }
       
        }

        
        private void PrintMetrics(ClusteringMetrics metrics)
        {
            Console.WriteLine($"Normalized Mutual Information: " +
                $"{metrics.NormalizedMutualInformation:F2}");

            Console.WriteLine($"Average Distance: " +
                $"{metrics.AverageDistance:F2}");

            Console.WriteLine($"Davies Bouldin Index: " +
                $"{metrics.DaviesBouldinIndex:F2}");
        }
}
}

