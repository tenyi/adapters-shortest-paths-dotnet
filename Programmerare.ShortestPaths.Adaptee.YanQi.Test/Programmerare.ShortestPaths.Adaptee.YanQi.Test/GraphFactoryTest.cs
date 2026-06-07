using Xunit;

namespace Programmerare.ShortestPaths.Adaptee.YanQi.Test
{

public class GraphFactoryTest
    {
        [Fact]
        public void TestGetFullPath_ExistingFile()
        {
            Assert.True(System.IO.File.Exists(
                    GraphFactory.GetFullPath("data/test_50")
                ));
        }

        [Fact]
        public void TestGetFullPath_NotExistingFile()
        {
            Assert.False(System.IO.File.Exists(
                    GraphFactory.GetFullPath("data/nameOfNonExistinFile")
                ));
        }
    }
}
