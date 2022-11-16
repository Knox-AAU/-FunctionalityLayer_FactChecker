
using System.Collections.Generic;

namespace FactChecker.Clustering
{
public class Centers {

    public float[] centerpoint { get; set; }
    public List<DataPoint> TightCluster {get; set;} = new List<DataPoint>();
    public List<DataPoint> LoosCluster {get; set; } = new List<DataPoint>();
    public Centers(float[] point)
    {
       centerpoint = point;
    }
}
}