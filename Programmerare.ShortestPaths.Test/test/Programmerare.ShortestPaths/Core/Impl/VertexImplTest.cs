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

using Programmerare.ShortestPaths.Core.Api;
using static Programmerare.ShortestPaths.Core.Impl.VertexImpl; // createVertex

namespace Programmerare.ShortestPaths.Core.Impl
{
    /**
     * @author Tomas Johansson
     */

    public class VertexImplTest {

	    private Vertex vertexA;
	    private Vertex vertexB;
	
	    public VertexImplTest() {
		    vertexA = CreateVertex(357);
		    vertexB = CreateVertex("357");		
	    }
	
	    [Fact]
	    public void testGetVertexId() {
		
		    Assert.Equal(vertexA.VertexId, vertexB.VertexId);
		
		    Assert.Equal(vertexA, vertexB);
		    Assert.Equal(vertexA.GetHashCode(), vertexB.GetHashCode());
	    }
	
	    [Fact]
	    public void testEquals() {
		    Assert.Equal(vertexA, vertexB);

		    Assert.True(vertexA.Equals(vertexB));
		    Assert.True(vertexB.Equals(vertexA));
	    }
	
	    [Fact]
	    public void testHashCode() {
		    Assert.Equal(vertexA.GetHashCode(), vertexB.GetHashCode());
	    }	

    }
}