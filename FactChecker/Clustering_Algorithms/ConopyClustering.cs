using System;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.Clustering
{
public class ConopyClustering {
    public int T1 { get; set; }
    public int T2 { get; set; }
    public List<Centers> Canopys { get; set; } = new List<Centers>();
    Random random = new Random();
    public ConopyClustering(int loose, int tight)
    {
        T1 = loose;
        T2 = tight;
    }

    public void Cluster(List<DataPoint> points, int NumOfClusters) { 
        int lenght = points.Count;
        int p = random.Next(lenght);
        bool isvaild = false;
        Canopys.Add(new Centers(points[p].Features));
        for (int i = 1; i < NumOfClusters; i++) {
            p = random.Next(lenght);
            foreach (var center in Canopys) {
                float total = 0;
                for (int j = 0; j < points[1].Features.Length; j++)
                {
                    total += MathF.Pow(points[p].Features[j] - center.centerpoint[j], 2);
                }
                if (total > Math.Pow(T2, 2)) {
                    isvaild = true;
                }
            }
            if (isvaild) { 
                Canopys.Add(new Centers(points[p].Features));
            }
        }
        foreach (var center in Canopys)
        {
            for (int i = 0; i < lenght; i++) {
                
                if (!points[i].Features.Any(f => center.centerpoint.Contains(f))) {
                    float total = 0;
                    for (int j = 0; j < points[1].Features.Length; j++)
                    {
                        total += MathF.Pow(points[p].Features[j] - center.centerpoint[j], 2);
                    }
                    if (total < Math.Pow(T2, 2))
                    {
                        center.TightCluster.Add(points[i]);
                    }
                    else if (total >= Math.Pow(T2, 2) && total <= Math.Pow(T1, 2))
                    {
                        center.LoosCluster.Add(points[i]);
                    }
                }
                else{
                    Console.WriteLine("Skipping center point");
                }
            }
        }
    }
    public void printClusters() {
        foreach (var cluster in Canopys)
        {
            Console.WriteLine($"Center point: ({cluster.centerpoint.Take(5)})");
            Console.Write("Tight Cluster: ");
            foreach (var p in cluster.TightCluster)
            {
                Console.Write($"({p.Features.Take(5)}), ");
            }
            Console.WriteLine();
            Console.Write("Loose Cluster: ");
            foreach (var p in cluster.LoosCluster)
            {
                Console.Write($"({p.Features.Take(5)}), ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
}
