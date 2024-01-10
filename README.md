# Grammophone.IPLocation.Caching
This libary provides a cache for querying locations by IP for implementations following the interfaces
defined in the [Grammophone.IPLocation](https://github.com/grammophone/Grammophone.IPLocation) library.

To create an instance of the cache, call the contructor of the `LocationCache` class with the cache options
and the `ILocationProviderFactory` implementation to use for opening sessions to query IP's upon cache miss.
Call the `Dispose` method when the cache is no longer needed.

## Example usage
The following example uses location providers depending on MaxMind API's defined in the
[Grammophone.IPLocation.MaxMind](https://github.com/grammophone/Grammophone.IPLocation.MaxMind) library,
which should also be present in a sibling directory along the [Grammophone.IPLocation](https://github.com/grammophone/Grammophone.IPLocation) library.

The cache uses aggregation of location providers to first try web API querying then falling back to database query if the former fails.
```CS
string databaseFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeoLite2-City.mmdb");

var locationProviderFactories = new ILocationProviderFactory[]
{
  new WebServiceMaxMindLocationProviderFactory(123456, "[your account key]"),
  new DatabaseMaxMindLocationProviderFactory(databaseFilename)
};

var aggregateLocationProviderFactory =
  new AggregateLocationProviderFactory(locationProviderFactories);

var memoryCacheOptions = new MemoryCacheOptions
{
  SizeLimit = 1024
};

using (var locationCache = new LocationCache(memoryCacheOptions, aggregateLocationProviderFactory))
{
  var firstLocation = await locationCache.GetLocationAsync(IPAddress.Parse("[some IP address]"));

  var secondLocation = await locationCache.GetLocationAsync(IPAddress.Parse("[same IP address]"));

  Assert.AreEqual(firstLocation, secondLocation);
}
```

The library depends on [Grammophone.IPLocation](https://github.com/grammophone/Grammophone.IPLocation) being in a sibling directory.
