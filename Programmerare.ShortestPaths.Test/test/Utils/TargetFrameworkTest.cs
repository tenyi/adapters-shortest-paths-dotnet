using Xunit;
using Programmerare.ShortestPaths.Test.Utils;

namespace dotnet_adapters_shortest_paths_test.test.Utils {

    public class TargetFrameworkTest {

        [Fact]
        public void IsSupportingFileStreamReaderTest() {
            var netStandard_1_0 = new TargetFramework(TargetFrameworkEnum.NETSTANDARD1_0);
            var netStandard_1_6 = new TargetFramework(TargetFrameworkEnum.NETSTANDARD1_6);
            var netStandard_2_0 = new TargetFramework(TargetFrameworkEnum.NETSTANDARD2_0);
            var netFramework_4_0 = new TargetFramework(TargetFrameworkEnum.NET40);

            Assert.False(netStandard_1_0.IsSupportingFileStreamReader());
            Assert.False(netStandard_1_6.IsSupportingFileStreamReader());
            
            Assert.True(netStandard_2_0.IsSupportingFileStreamReader());
            Assert.True(netFramework_4_0.IsSupportingFileStreamReader());
        }
    }
}
