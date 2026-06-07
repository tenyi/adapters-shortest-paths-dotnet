using System.Collections.Generic;
using System.Linq;
using Xunit;
using QuikGraph;
using QuikGraph.Algorithms.ShortestPath;

namespace Programmerare.ShortestPaths.Adapter.QuikGraph.Test
{

    public class QuikGraphTest
    {

        // This test method for QuikGraph 1.0.0 is based on the code example in the following issue for QuickGraph 3.7.3 (which failed):
        // https://github.com/YaccConstructor/QuickGraph/issues/178
        [Fact]
        public void Test()
        {
            var graph = new AdjacencyGraph<string, EquatableTaggedEdge<string, double>>(false);
            graph.AddVertexRange(new List<string> { "A", "B", "C", "D" });
            graph.AddEdge(new EquatableTaggedEdge<string, double>("A", "B", 5));
            graph.AddEdge(new EquatableTaggedEdge<string, double>("A", "C", 6));
            graph.AddEdge(new EquatableTaggedEdge<string, double>("B", "C", 7));
            graph.AddEdge(new EquatableTaggedEdge<string, double>("B", "D", 8));
            graph.AddEdge(new EquatableTaggedEdge<string, double>("C", "D", 9));
            var yen = new YenShortestPathsAlgorithm<string>(graph, "A", "D", 5);
            // The three paths *should* be:
            // A -> B -> D (weight: 13 = 5 + 8)
            // A -> C -> D (weight: 15 = 6 + 9)
            // A -> B -> C -> D (weight: 21 = 5 + 7 + 9)
            var actualPaths = yen.Execute().ToList();
            //foreach(var path in actualPaths)
            //{
            //    var edges = path.ToList();
            //    Console.WriteLine();
            //    Console.Write(edges[0].Source);
            //    for(int i=0; i<edges.Count; i++) {
            //        Console.Write(" -> " +edges[i].Target);
            //    }
            //}
            // Output from above loops:
            //  A -> B -> D
            //  A -> C -> D
            // The last path was missing (i.e. A -> B -> C -> D) for QuickGraph 3.7.3
            Assert.Equal(3, actualPaths.Count); // Failure for QuickGraph 3.7.3 : Expected: 3 But was:  2
            // but when running the code with the fork for QuikGraph 1.0.0, then it works

            var path1 = actualPaths[0];
            var path2 = actualPaths[1];
            var path3 = actualPaths[2];

            Assert.Equal("A", path1.ElementAt(0).Source);
            Assert.Equal("B", path1.ElementAt(0).Target);
            Assert.Equal("B", path1.ElementAt(1).Source);
            Assert.Equal("D", path1.ElementAt(1).Target);

            Assert.Equal("A", path2.ElementAt(0).Source);
            Assert.Equal("C", path2.ElementAt(0).Target);
            Assert.Equal("C", path2.ElementAt(1).Source);
            Assert.Equal("D", path2.ElementAt(1).Target);

            Assert.Equal("A", path3.ElementAt(0).Source);
            Assert.Equal("B", path3.ElementAt(0).Target);
            Assert.Equal("B", path3.ElementAt(1).Source);
            Assert.Equal("C", path3.ElementAt(1).Target);
            Assert.Equal("C", path3.ElementAt(2).Source);
            Assert.Equal("D", path3.ElementAt(2).Target);
        }
    }
}