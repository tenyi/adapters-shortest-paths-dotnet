namespace Programmerare.ShortestPaths.Example.Roadrouting {
    public sealed class CityRoadServiceFactory {

	    public static CityRoadService CreateCityRoadService() {
		    return new CityRoadServiceHardcoded();
	    }
    }
}
