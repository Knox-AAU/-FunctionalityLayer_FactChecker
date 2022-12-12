using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.WordEmbedding
{
    public class WordEmbedding
    {
        public double GetEvidence(string _1, string _2)
        {
            var context = new MLContext();

            var emptyData = context.Data.LoadFromEnumerable(new List<TextInput>());

            var embeddingsPipline = context.Transforms.Text.NormalizeText("Text", null, keepDiacritics: false, keepPunctuations: false, keepNumbers: false)
                .Append(context.Transforms.Text.TokenizeIntoWords("Tokens", "Text"))
                .Append(context.Transforms.Text.ApplyWordEmbedding("Features", "Tokens", WordEmbeddingEstimator.PretrainedModelKind.SentimentSpecificWordEmbedding));

            var predictionEngine = context.Model.CreatePredictionEngine<TextInput, TextFeatures>(embeddingsPipline.Fit(emptyData));

            var dogData = new TextInput { Text = _1 };
            var catData = new TextInput { Text = _2};

            double[] Prediction1 = predictionEngine.Predict(dogData).Features.Select(p => (double)p).ToArray();
            double[] Prediction2 = predictionEngine.Predict(catData).Features.Select(p => (double)p).ToArray();

            var sim = new AForge.Math.Metrics.CosineSimilarity();
            var res = sim.GetSimilarityScore(Prediction1, Prediction2);
            return res;
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
