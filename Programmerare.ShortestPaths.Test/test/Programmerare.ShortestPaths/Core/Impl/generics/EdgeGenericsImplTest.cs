/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in the "core" project is licensed with MIT.
* Other projects within this Visual Studio solution may be released with other licenses e.g. Apache.
* Please find more information in the files "License.txt" and "NOTICE.txt" 
* in the project root directory and/or in the solution root directory.
* It should also be possible to find more license information at this URL:
* https://github.com/TomasJohansson/adapters-shortest-paths-dotnet/
*/
using Xunit;

using static Programmerare.ShortestPaths.Core.Impl.WeightImpl; // SMALL_DELTA_VALUE_FOR_WEIGHT_COMPARISONS
using static Programmerare.ShortestPaths.Core.Impl.EdgeImpl; // createEdge
using static Programmerare.ShortestPaths.Core.Impl.VertexImpl; // createVertex
using Programmerare.ShortestPaths.Core.Api;

namespace Programmerare.ShortestPaths.Core.Impl.Generics
{
    /**
     * @author Tomas Johansson
     */
    public class EdgeGenericsImplTest {
	    private Vertex vertexA, vertexB;
	    private Weight weight;
	    private double weightValue;
	    private Edge edgeX, edgeY;
	
	    public EdgeGenericsImplTest()  {
		    vertexA = CreateVertex("A");
		    vertexB = CreateVertex("B");
		    weightValue = 123.45;
		    weight = CreateWeight(weightValue);
		    edgeX = CreateEdge(vertexA, vertexB, weight);
		
		    edgeY = CreateEdge(CreateVertex("A"), CreateVertex("B"), CreateWeight(weightValue));
	    }


	    [Fact]
	    public void testGetStartVertex() {
		    Assert.Equal(vertexA.VertexId, edgeX.StartVertex.VertexId);
		    Assert.Equal(vertexA, edgeX.StartVertex);
	    }
	
	    [Fact]
	    public void testGetEndVertex() {
		    Assert.Equal(vertexB.VertexId, edgeX.EndVertex.VertexId);
		    Assert.Equal(vertexB, edgeX.EndVertex);
	    }	
	
	    [Fact]
	    public void testgetEdgeWeight() {
		    Assert.Equal(weightValue, edgeX.EdgeWeight.WeightValue, 8);
		    Assert.Equal(weight, edgeX.EdgeWeight);		
	    }
	
	    [Fact]
	    public void testEquals() {
		    Assert.Equal(edgeX, edgeY);
		    Assert.True(edgeX.Equals(edgeY));
		    Assert.True(edgeY.Equals(edgeX));
	    }
	
	    [Fact]
	    public void testHashCode() {
		    Assert.Equal(edgeX.GetHashCode(), edgeY.GetHashCode());
	    }	
    }
}