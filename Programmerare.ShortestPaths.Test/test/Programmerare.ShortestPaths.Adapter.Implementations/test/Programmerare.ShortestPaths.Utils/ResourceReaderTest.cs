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
using System.Collections.Generic;

namespace Programmerare.ShortestPaths.Utils
{

    public class ResourceReaderTest {

	    private ResourceReader resourceReader;

	    public ResourceReaderTest() {
		    resourceReader = new ResourceReader();
	    }

        // TODO: document (more detailed than here) the two assumptions for this test ...
        //      1. Assumptions about the hardcoded base path to "resources" directory (relative to the root folder)
        //      2. Visual Studio: "Copy to output directory" for each of the files
	    [Fact]
	    public void GetNameOfFilesInResourcesFolder() {
		    IList<string> fileNames = resourceReader.GetNameOfFilesInResourcesFolder("directory_for_resource_reader_test");
		    Assert.Equal(4, fileNames.Count);
		    Assert.Equal("txtFile1.txt", fileNames[0]);
		    Assert.Equal("txtFile2.txt", fileNames[1]);
		    Assert.Equal("xmlFile1.xml", fileNames[2]);
		    Assert.Equal("xmlFile2.xml", fileNames[3]);
	    }
    }
}