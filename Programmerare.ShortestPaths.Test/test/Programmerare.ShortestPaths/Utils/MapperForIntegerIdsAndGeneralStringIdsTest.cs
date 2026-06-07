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


namespace Programmerare.ShortestPaths.Utils
{

public class MapperForIntegerIdsAndGeneralStringIdsTest {

//	@Before
//	public void setUp() throws Exception {
//	}

	// TODO: improve the testing below. Each method is currently doing too much.

	[Fact]
	public void testStartIndexZero() {
		MapperForIntegerIdsAndGeneralStringIds idMapper = MapperForIntegerIdsAndGeneralStringIds.CreateIdMapper(0);
		Assert.Equal(0, idMapper.CreateOrRetrieveIntegerId("A"));
		Assert.Equal(1, idMapper.CreateOrRetrieveIntegerId("B"));
		Assert.Equal(0, idMapper.CreateOrRetrieveIntegerId("A"));
		Assert.Equal(2, idMapper.CreateOrRetrieveIntegerId("C"));
		Assert.Equal(0, idMapper.CreateOrRetrieveIntegerId("A"));
		Assert.Equal(3, idMapper.CreateOrRetrieveIntegerId("D"));

		Assert.Equal(4, idMapper.GetNumberOfVertices());
		
		Assert.Equal("A", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(0));
		Assert.Equal("B", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(1));
		Assert.Equal("C", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(2));
		Assert.Equal("D", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(3));
	}
	
	[Fact]
	public void testStartIndexOne() {
		MapperForIntegerIdsAndGeneralStringIds idMapper = MapperForIntegerIdsAndGeneralStringIds.CreateIdMapper(1);
		Assert.Equal(1, idMapper.CreateOrRetrieveIntegerId("ABC"));
		Assert.Equal(2, idMapper.CreateOrRetrieveIntegerId("DEF"));
		Assert.Equal(3, idMapper.CreateOrRetrieveIntegerId("GHI"));
		Assert.Equal(2, idMapper.CreateOrRetrieveIntegerId("DEF"));
		
		Assert.Equal(3, idMapper.GetNumberOfVertices());
		
		Assert.Equal("ABC", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(1));
		Assert.Equal("DEF", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(2));
		Assert.Equal("GHI", idMapper.GetBackThePreviouslyStoredGeneralStringIdForInteger(3));
	}	
	
//	@Test
//	public void createOrRetrieveIntegerId() {
//		fail("Not yet implemented");
//	}
//
//	@Test
//	public void testGetNumberOfVertices() {
//		fail("Not yet implemented");
//	}
//
//	@Test
//	public void testGetBackThePreviouslyStoredGeneralStringIdForInteger() {
//		fail("Not yet implemented");
//	}
}
}