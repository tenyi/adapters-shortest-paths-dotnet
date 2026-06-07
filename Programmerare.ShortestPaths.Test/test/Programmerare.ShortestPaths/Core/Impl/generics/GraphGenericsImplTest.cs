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
using Xunit;

using static Programmerare.ShortestPaths.Core.Impl.GraphImplTest; // createEdgeGenerics
using static Programmerare.ShortestPaths.Core.Impl.WeightImpl; // SMALL_DELTA_VALUE_FOR_WEIGHT_COMPARISONS
using static Programmerare.ShortestPaths.Core.Impl.VertexImpl; // createVertex
using System.Collections.Generic;
using Programmerare.ShortestPaths.Core.Api.Generics;

namespace Programmerare.ShortestPaths.Core.Impl.Generics
{
    public class GraphGenericsImplTest {

	    private EdgeGenerics<Vertex,Weight> edge1, edge2;
	
	    public GraphGenericsImplTest() {
		    edge1 = createEdgeGenerics(CreateVertex("A"), CreateVertex("B"), CreateWeight(123));
		    edge2 = createEdgeGenerics(CreateVertex("B"), CreateVertex("C"), CreateWeight(456));		
	    }

	    [Fact]
	    public void testGetAllEdges() {
		    IList<EdgeGenerics<Vertex,Weight>> edges = new List<EdgeGenerics<Vertex,Weight>>();
		    edges.Add(edge1);
		    edges.Add(edge2);
		    // refactor the above three rows (duplicated)

		    GraphGenerics<EdgeGenerics<Vertex,Weight>, Vertex,Weight> graph = createGraphGenerics(edges);
		
		    IList<EdgeGenerics<Vertex, Weight>> allEdges = graph.Edges;
		
		    Assert.Equal(2, allEdges.Count);
		    Assert.Same(edge1, allEdges[0]);
		    Assert.Same(edge2, allEdges[1]);
	    }

        private GraphGenerics<EdgeGenerics<Vertex, Weight>, Vertex, Weight> createGraphGenerics(IList<EdgeGenerics<Vertex, Weight>> edges)
        {
            return GraphGenericsImpl<EdgeGenerics<Vertex, Weight>, Vertex, Weight>.CreateGraphGenerics<EdgeGenerics<Vertex, Weight>, Vertex, Weight>(edges);
        }

        [Fact]
	    public void testGetVertices() {
		    IList<EdgeGenerics<Vertex,Weight>> edges = new List<EdgeGenerics<Vertex,Weight>>();
		    edges.Add(createEdgeGenerics(CreateVertex("A"), CreateVertex("B"), CreateWeight(1)));
		    edges.Add(createEdgeGenerics(CreateVertex("A"), CreateVertex("C"), CreateWeight(2)));
		    edges.Add(createEdgeGenerics(CreateVertex("A"), CreateVertex("D"), CreateWeight(3)));
		    edges.Add(createEdgeGenerics(CreateVertex("B"), CreateVertex("C"), CreateWeight(4)));
		    edges.Add(createEdgeGenerics(CreateVertex("B"), CreateVertex("D"), CreateWeight(5)));
		    edges.Add(createEdgeGenerics(CreateVertex("C"), CreateVertex("D"), CreateWeight(6)));
		
		    GraphGenerics<EdgeGenerics<Vertex,Weight>, Vertex,Weight> graph = createGraphGenerics(edges);
		
		    IList<Vertex> vertices = graph.Vertices;
		
		    IList<string> expectedVerticesIds = new List<string>{"A", "B", "C", "D" };
		
		    Assert.Equal(expectedVerticesIds.Count, vertices.Count);
		
		    // verify that all vertices in all edges is one of the four above
		    foreach (EdgeGenerics<Vertex,Weight> edge in edges) {
                // Java version used hamcrest as below:
			    //assertThat(expectedVerticesIds, hasItem(edge.getStartVertex().getVertexId()));
			    //assertThat(expectedVerticesIds, hasItem(edge.getEndVertex().getVertexId()));
                Assert.True(expectedVerticesIds.Contains(edge.StartVertex.VertexId));
                Assert.True(expectedVerticesIds.Contains(edge.EndVertex.VertexId));
                //Assert.Fail("fix the test");
		    }		
	    }
	
	    [Fact]
	    public void testContainsVertex() {
		    List<EdgeGenerics<Vertex,Weight>> edges = new List<EdgeGenerics<Vertex,Weight>>();
		    edges.Add(edge1);
		    edges.Add(edge2);
		    // refactor the above three rows (duplicated)
		
		    GraphGenerics<EdgeGenerics<Vertex,Weight>, Vertex,Weight> graph = createGraphGenerics(edges);

		    Assert.True(graph.ContainsVertex(edge1.StartVertex));
		    Assert.True(graph.ContainsVertex(edge1.EndVertex));
		    Assert.True(graph.ContainsVertex(edge2.StartVertex));
		    Assert.True(graph.ContainsVertex(edge2.EndVertex));
		
		    Vertex vertex = CreateVertex("QWERTY");
		    Assert.False(graph.ContainsVertex(vertex));
	    }
	    // TODO: refactor some code duplicated above and below i.e. put some code in setup method
	    [Fact]
	    public void testContainsEdge() {
		    List<EdgeGenerics<Vertex,Weight>> edges = new List<EdgeGenerics<Vertex,Weight>>();
		    edges.Add(edge1);
		    edges.Add(edge2);
		    // refactor the above three rows (duplicated)
		
		    GraphGenerics<EdgeGenerics<Vertex,Weight>, Vertex,Weight> graph = createGraphGenerics(edges);

		    Assert.True(graph.ContainsEdge(edge1));
		    Assert.True(graph.ContainsEdge(edge2));
		
		    EdgeGenerics<Vertex,Weight> edgeNotInTheGraph = createEdgeGenerics(CreateVertex("XYZ"), CreateVertex("QWERTY"), CreateWeight(987));
		    Assert.False(graph.ContainsEdge(edgeNotInTheGraph));
	    }	
    }

}