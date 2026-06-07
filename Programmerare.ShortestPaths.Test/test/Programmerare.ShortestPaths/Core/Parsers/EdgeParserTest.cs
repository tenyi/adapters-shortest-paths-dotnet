/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in the "core" project is licensed with MIT.
* Other projects within this Visual Studio solution may be released with other licenses e.g. Apache.
* Please find more information in the files "License.txt" and "NOTICE.txt" 
* in the project root directory and/or in the solution root directory.
* It should also be possible to find more license information at this URL:
* https://github.com/TomasJohansson/adapters-shortest-paths-dotnet/
*/

using Programmerare.ShortestPaths.Core.Api;
using Programmerare.ShortestPaths.Core.Api.Generics;
using Xunit;

using static Programmerare.ShortestPaths.Core.Impl.VertexImpl; // createVertex
using static Programmerare.ShortestPaths.Core.Impl.EdgeImpl;
using static Programmerare.ShortestPaths.Core.Impl.WeightImpl; // createWeight
using System.Collections.Generic;
using System;

namespace Programmerare.ShortestPaths.Core.Parsers
{

    public class EdgeParserTest
    {
        private EdgeParser<Edge, Vertex, Weight> edgeParser;

        public EdgeParserTest()
        {
            edgeParser = EdgeParser<Edge,Vertex,Weight>.CreateEdgeParserDefault();
        }

        [Fact]
        public void testFromStringToEdge()
        {
            Edge edge = edgeParser.FromStringToEdge("A B 3.7");
            Assert.NotNull(edge);
            Assert.NotNull(edge.StartVertex);
            Assert.NotNull(edge.EndVertex);
            Assert.NotNull(edge.EdgeWeight);
            Assert.Equal("A", edge.StartVertex.VertexId);
            Assert.Equal("B", edge.EndVertex.VertexId);
            Assert.Equal(3.7, edge.EdgeWeight.WeightValue, 8);
        }
        // TODO: refactor away duplication from above and below methods
        [Fact]
        public void testFromStringToEdgeGenerics()
        {
            Edge edge = edgeParser.FromStringToEdge("A B 3.7");
            Assert.NotNull(edge);
            Assert.NotNull(edge.StartVertex);
            Assert.NotNull(edge.EndVertex);
            Assert.NotNull(edge.EdgeWeight);
            Assert.Equal("A", edge.StartVertex.VertexId);
            Assert.Equal("B", edge.EndVertex.VertexId);
            Assert.Equal(3.7, edge.EdgeWeight.WeightValue, 8);
        }

        [Fact]
        public void testFromEdgeParserGenericsToString()
        {
            Vertex startVertex = CreateVertex("A");
            Vertex endVertex = CreateVertex("B");
            Weight weight = CreateWeight(3.7);
            Edge edge = CreateEdge(startVertex, endVertex, weight);
            Assert.Equal("A B 3.7", edgeParser.FromEdgeToString(edge));
        }
        // TODO: refactor away duplication from above and below methods	
        [Fact]
        public void testFromEdgeToString()
        {
            Vertex startVertex = CreateVertex("A");
            Vertex endVertex = CreateVertex("B");
            Weight weight = CreateWeight(3.7);
            Edge edge = CreateEdge(startVertex, endVertex, weight);
            Assert.Equal("A B 3.7", edgeParser.FromEdgeToString(edge));
        }

        [Fact]
        public void testFromMultiLineStringToListOfEdgesGenerics()
        {
            //	    <graphDefinition>
            //	    A B 5
            //	    A C 6
            //	    B C 7
            //	    B D 8
            //	    C D 9    
            //	    </graphDefinition>
            const String multiLinedString = "A B 5\r\n" +
                    "A C 6\r\n" +
                    "B C 7\r\n" +
                    "B D 8\r\n" +
                    "C D 9";
            IList<Edge> edges = edgeParser.FromMultiLinedStringToListOfEdges(multiLinedString);
            Assert.NotNull(edges);
            Assert.Equal(5, edges.Count);
            EdgeGenerics<Vertex, Weight> firstEdge = edges[0];
            EdgeGenerics<Vertex, Weight> lastEdge = edges[4];
            assertNotNulls(firstEdge);
            assertNotNulls(lastEdge);

            Assert.Equal("A", firstEdge.StartVertex.VertexId);
            Assert.Equal("B", firstEdge.EndVertex.VertexId);
            Assert.Equal(5, firstEdge.EdgeWeight.WeightValue, 8);

            Assert.Equal("C", lastEdge.StartVertex.VertexId);
            Assert.Equal("D", lastEdge.EndVertex.VertexId);
            Assert.Equal(9, lastEdge.EdgeWeight.WeightValue, 8);
        }
        // TODO: refactor away duplication from above and below methods	
        [Fact]
        public void testFromMultiLineStringToListOfEdges()
        {
            //	    <graphDefinition>
            //	    A B 5
            //	    A C 6
            //	    B C 7
            //	    B D 8
            //	    C D 9    
            //	    </graphDefinition>
            const String multiLinedString = "A B 5\r\n" +
                    "A C 6\r\n" +
                    "B C 7\r\n" +
                    "B D 8\r\n" +
                    "C D 9";
            IList<Edge> edges = edgeParser.FromMultiLinedStringToListOfEdges(multiLinedString);
            Assert.NotNull(edges);
            Assert.Equal(5, edges.Count);
            EdgeGenerics<Vertex, Weight> firstEdge = edges[0];
            EdgeGenerics<Vertex, Weight> lastEdge = edges[4];
            assertNotNulls(firstEdge);
            assertNotNulls(lastEdge);

            Assert.Equal("A", firstEdge.StartVertex.VertexId);
            Assert.Equal("B", firstEdge.EndVertex.VertexId);
            Assert.Equal(5, firstEdge.EdgeWeight.WeightValue, 8);

            Assert.Equal("C", lastEdge.StartVertex.VertexId);
            Assert.Equal("D", lastEdge.EndVertex.VertexId);
            Assert.Equal(9, lastEdge.EdgeWeight.WeightValue, 8);
        }

        private void assertNotNulls(EdgeGenerics<Vertex, Weight> edge)
        {
            Assert.NotNull(edge);
            Assert.NotNull(edge.StartVertex);
            Assert.NotNull(edge.EndVertex);
            Assert.NotNull(edge.EdgeWeight);
        }

    }
}