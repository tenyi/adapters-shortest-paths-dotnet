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
using static Programmerare.ShortestPaths.Core.Impl.WeightImpl; // SMALL_DELTA_VALUE_FOR_WEIGHT_COMPARISONS

namespace Programmerare.ShortestPaths.Core.Impl
{
    /**
     * @author Tomas Johansson
     */

    public class WeightImplTest {

	    private Weight weightA;
	    private Weight weightB;
	    private double weightValueA;
	    private double weightValueB;
	
	    public WeightImplTest()  {
		    weightValueA = 12345.6789;
		    weightValueB = 12345.6789;
		    weightA = CreateWeight(weightValueA);
		    weightB = CreateWeight(weightValueB);
	    }
	
	    [Fact]
	    public void testGetWeightValue() {
		    Assert.Equal(weightValueA, weightA.WeightValue, 8);
	    }
	
	    [Fact]
	    public void testEquals() {
		    Assert.Equal(weightA, weightB);

		    Assert.True(weightA.Equals(weightB));
		    Assert.True(weightB.Equals(weightA));
	    }
	
	    [Fact]
	    public void testHashCode() {
		    Assert.Equal(weightA.GetHashCode(), weightB.GetHashCode());
	    }	
    }
}