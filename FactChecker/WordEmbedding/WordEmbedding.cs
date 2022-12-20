using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace FactChecker.WordEmbedding
{
    public class WordEmbedding
    {
        public double GetEvidence(string _1, string _2)
        {
            var context = new MLContext();
            var embeddingsPipline = context.Transforms.Text.NormalizeText("Text", null, keepDiacritics: false, keepPunctuations: false, keepNumbers: false)
                .Append(context.Transforms.Text.TokenizeIntoWords("Tokens", "Text"))
                .Append(context.Transforms.Text.ApplyWordEmbedding("Features", "Tokens", WordEmbeddingEstimator.PretrainedModelKind.SentimentSpecificWordEmbedding));
            var predictionEngine = context.Model.CreatePredictionEngine<TextInput, TextFeatures>(embeddingsPipline.Fit(context.Data.LoadFromEnumerable(new List<TextInput>())));
            double[] Prediction1 = predictionEngine.Predict(new TextInput { Text = _1 }).Features.Select(p => (double)p).ToArray();
            double[] Prediction2 = predictionEngine.Predict(new TextInput { Text = _2 }).Features.Select(p => (double)p).ToArray();
            return new AForge.Math.Metrics.CosineSimilarity().GetSimilarityScore(Prediction1, Prediction2);
        }
    }
public class TextInput
        {
            public string Text { get; set; }
        }

        public class TextFeatures
        {
            public float[] Features { get; set; }
        }
}
