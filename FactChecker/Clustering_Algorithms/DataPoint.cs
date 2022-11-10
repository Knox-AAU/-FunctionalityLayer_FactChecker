using Microsoft.ML.Data;
namespace FactChecker.Clustering
{
public class DataPoint {
    
    [VectorType(150)]
    public float[] Features { get; set; }
    
}
public class TextData
{
    public string Text { get; set; }
}
public class Prediction
{
    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId;

    [ColumnName("Score")]
    public float[] Distances;
}
}