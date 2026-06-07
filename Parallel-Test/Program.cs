using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Programmerare.ShortestPaths.Core.Api;
using Programmerare.ShortestPaths.Adapter.YanQi;
using Programmerare.ShortestPaths.Adapter.Bsmock;
using Programmerare.ShortestPaths.Adapter.QuikGraph;
using static Programmerare.ShortestPaths.Core.Impl.VertexImpl;
using static Programmerare.ShortestPaths.Core.Impl.WeightImpl;
using static Programmerare.ShortestPaths.Core.Impl.EdgeImpl;
using static Programmerare.ShortestPaths.Core.Impl.GraphImpl;

namespace Parallel_Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("  Yen's Algorithm Parallel Comparison (.NET 10)   ");
            Console.WriteLine("==================================================");

            // 1. 初始化一個稍微複雜的圖（100 個頂點，約 300 條邊）
            Console.WriteLine("Generating test graph...");
            var vertices = new List<Vertex>();
            for (int i = 0; i < 100; i++)
            {
                vertices.Add(CreateVertex($"V{i}"));
            }

            var edges = new List<Edge>();
            var rand = new Random(42); // 固定種子確保每次運行圖形結構一致

            var generatedPairs = new HashSet<string>();
            // 確保主通道連通 V0 -> V99
            for (int i = 0; i < 99; i++)
            {
                edges.Add(CreateEdge(vertices[i], vertices[i + 1], CreateWeight(rand.Next(2, 10))));
                generatedPairs.Add($"{i}_{i + 1}");
            }

            // 加入隨機跳躍邊以豐富最短路徑的組合
            int addedEdges = 0;
            while (addedEdges < 200)
            {
                int u = rand.Next(0, 95);
                int v = rand.Next(u + 2, 100); // 確保是前向圖，避免產生無窮負環或自環
                string key = $"{u}_{v}";
                if (!generatedPairs.Contains(key))
                {
                    edges.Add(CreateEdge(vertices[u], vertices[v], CreateWeight(rand.Next(5, 25))));
                    generatedPairs.Add(key);
                    addedEdges++;
                }
            }

            var graph = CreateGraph(edges);
            var startVertex = vertices[0];
            var endVertex = vertices[99];
            const int maxK = 15; // 尋找前 15 條最短路徑

            Console.WriteLine($"Graph generated with {vertices.Count} vertices and {edges.Count} edges.");
            Console.WriteLine($"Finding K shortest paths (K={maxK}) from {startVertex.VertexId} to {endVertex.VertexId}.\n");

            // 2. 準備三種演算法的 Factory
            var factories = new Dictionary<string, PathFinderFactory>
            {
                { "YanQi", new PathFinderFactoryYanQi() },
                { "Bsmock", new PathFinderFactoryBsmock() },
                { "QuikGraph", new PathFinderFactoryQuikGraph() }
            };

            // 3. 多執行緒平行運算比較 (Task.Run)
            Console.WriteLine("Running pathfinders in parallel...");
            var tasks = factories.Select(pair => Task.Run(() =>
            {
                var stopwatch = Stopwatch.StartNew();
                
                // 每個執行緒在自己的 context 下建立 PathFinder
                var pathFinder = pair.Value.CreatePathFinder(graph);
                var paths = pathFinder.FindShortestPaths(startVertex, endVertex, maxK);
                
                stopwatch.Stop();
                return new PathFinderResult(pair.Key, paths, stopwatch.Elapsed.TotalMilliseconds);
            })).ToArray();

            // 等待所有執行緒完成
            var results = await Task.WhenAll(tasks);

            // 4. 印出效能結果與驗證一致性
            Console.WriteLine("\n=================== Results ======================");
            Console.WriteLine($"{"Implementation",-15} | {"Time (ms)",-12} | {"Paths Found",-12} | {"Best Weight",-12}");
            Console.WriteLine("--------------------------------------------------");

            foreach (var res in results)
            {
                var bestWeight = res.Paths.Count > 0 
                    ? res.Paths[0].TotalWeightForPath.WeightValue.ToString("F2") 
                    : "N/A";
                Console.WriteLine($"{res.Name,-15} | {res.ElapsedMs,-12:F3} | {res.Paths.Count,-12} | {bestWeight,-12}");
            }
            Console.WriteLine("==================================================");

            // 5. 結果正確性交叉比對
            var reference = results[0];
            bool allMatch = true;
            for (int i = 1; i < results.Length; i++)
            {
                var current = results[i];
                if (current.Paths.Count != reference.Paths.Count)
                {
                    Console.WriteLine($"[Warning] Path count mismatch: {reference.Name} found {reference.Paths.Count} paths, but {current.Name} found {current.Paths.Count}.");
                    allMatch = false;
                    continue;
                }

                for (int j = 0; j < reference.Paths.Count; j++)
                {
                    double wRef = reference.Paths[j].TotalWeightForPath.WeightValue;
                    double wCur = current.Paths[j].TotalWeightForPath.WeightValue;
                    if (Math.Abs(wRef - wCur) > 1e-6)
                    {
                        Console.WriteLine($"[Warning] Weight mismatch at path index {j}: {reference.Name} weight = {wRef:F4}, but {current.Name} weight = {wCur:F4}.");
                        allMatch = false;
                    }
                }
            }

            if (allMatch)
            {
                Console.WriteLine("\n[Success] All implementations returned identical path weight results!");
            }
            else
            {
                Console.WriteLine("\n[Alert] Divergent results detected between implementations. Please check adapter mapping logic.");
            }
        }
    }

    class PathFinderResult
    {
        public string Name { get; }
        public IList<Path> Paths { get; }
        public double ElapsedMs { get; }

        public PathFinderResult(string name, IList<Path> paths, double elapsedMs)
        {
            Name = name;
            Paths = paths;
            ElapsedMs = elapsedMs;
        }
    }
}
