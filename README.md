# Grammophone.IPLocation.Caching
This libary provides a cache for querying locations by IP for implementations following the interfaces defined in the [Grammophone.IPLocation](https://github.com/grammophone/Grammophone.IPLocation) library.

To create an instance of the cache, call the contructor of the `LocationCache` class with the cache options and the `ILocationProviderFactory` implementation to use for opening sessions to query IP's upon cache miss.
Call the `Dispose` method when the cache is no longer needed.

The library depends on [Grammophone.IPLocation](https://github.com/grammophone/Grammophone.IPLocation) being in a sibling directory.
