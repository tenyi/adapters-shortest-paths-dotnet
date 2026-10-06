using Xunit;

namespace Programmerare.ShortestPaths.Example.Roadrouting {
    /**
     * A smoke test that runs the example main method end-to-end.
     * (Retained from the original NUnit version, migrated to xUnit.)
     */
    public class RoadRoutingMainTest {

        [Fact]
        public void TestMain() {
            RoadRoutingMain.MainMethod();
        }
    }
}
