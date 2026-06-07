using Xunit;
using edu.ufl.cise.bsmock.graph;
using edu.ufl.cise.bsmock.graph.ksp;
using edu.ufl.cise.bsmock.graph.util;
using System;
using System.Collections.Generic;

namespace Programmerare.ShortestPaths.Adaptee.Bsmock.Test
{
    /// <summary>
    /// Tomas Johansson is the author of this test class.
    /// </summary>
    public class YenTest {

        [Fact]
        public void YenKShortestPathsTest()
        {
            const double deltaValue = 0.0000001;

            var graph = new Graph();
            graph.AddEdge("A", "B", 5);
            graph.AddEdge("A", "C", 6);
            graph.AddEdge("B", "C", 7);
            graph.AddEdge("B", "D", 8);
            graph.AddEdge("C", "D", 9);

            Yen yenAlgorithm = new Yen();
            IList<Path> paths = yenAlgorithm.Ksp(graph, "A", "D", 5);
            Assert.Equal(3, paths.Count);

            Path path1 = paths[0];
            Path path2 = paths[1];
            Path path3 = paths[2];
            Assert.Equal(13, path1.GetTotalCost(), 8);
            Assert.Equal(15, path2.GetTotalCost(), 8);
            Assert.Equal(21, path3.GetTotalCost(), 8);

            List<String> nodes1 = path1.GetNodes();
            Assert.Equal(3, nodes1.Count);
            Assert.Equal("A", nodes1[0]);
            Assert.Equal("B", nodes1[1]);
            Assert.Equal("D", nodes1[2]);
        
            List<String> nodes2 = path2.GetNodes();
            Assert.Equal(3, nodes2.Count);
            Assert.Equal("A", nodes2[0]);
            Assert.Equal("C", nodes2[1]);
            Assert.Equal("D", nodes2[2]);

            List<String> nodes3 = path3.GetNodes();
            Assert.Equal(4, nodes3.Count);
            Assert.Equal("A", nodes3[0]);
            Assert.Equal("B", nodes3[1]);
            Assert.Equal("C", nodes3[2]);
            Assert.Equal("D", nodes3[3]);
        }
    }
}