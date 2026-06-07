/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in the "core" project is licensed with MIT.
* Other projects within this Visual Studio solution may be released with other licenses e.g. Apache.
* Please find more information in the files "License.txt" and "NOTICE.txt" 
* in the project root directory and/or in the solution root directory.
* It should also be possible to find more license information at this URL:
* https://github.com/TomasJohansson/adapters-shortest-paths-dotnet/
*/

using System;

namespace Programmerare.ShortestPaths.Graphs.Utils {

    [Obsolete] // use xml instead
    public class FileReaderForGraphEdgesTest {
	    //[Fact]
	    //public void TestReadEdgesFromFile() {
		   // const string filePath = "directory_for_filereader_test/two_edges_A_B_7__X_Y_8.txt";
		   // // /src/test/resources/directory_for_filereader_test/file_for_filereader_test.txt
		   // // the above line has two rows like below:
		   // //A B 7
		   // //X Y 8
		
		   // FileReaderForGraphEdges<Edge, Vertex, Weight> fileReaderForGraphTestData = FileReaderForGraphEdges<Edge, Vertex, Weight>.CreateFileReaderForGraphEdges(new EdgeFactoryDefault());
		   // IList<Edge> edges = fileReaderForGraphTestData.ReadEdgesFromFile(filePath);
		   // Assert.NotNull(edges);
		   // Assert.Equal(2, edges.Count);
		
		   // Edge edge1 = edges[0]; // A B 7
		   // Edge edge2 = edges[1]; // X Y 8
		   // assertNonNulls(edge1);
		   // assertNonNulls(edge2);

		   // Assert.Equal("A", edge1.StartVertex.VertexId);
		   // Assert.Equal("B", edge1.EndVertex.VertexId);
		   // Assert.Equal(7, edge1.EdgeWeight.WeightValue, 8);
		
		   // Assert.Equal("X", edge2.StartVertex.VertexId);
		   // Assert.Equal("Y", edge2.EndVertex.VertexId);
		   // Assert.Equal(8, edge2.EdgeWeight.WeightValue, 8);		
		
	    //}

	    //private void assertNonNulls(Edge edge) {
		   // Assert.NotNull(edge);
		   // Assert.NotNull(edge.StartVertex);
		   // Assert.NotNull(edge.EndVertex);
		   // Assert.NotNull(edge.EdgeWeight);
	    //}
    }
}