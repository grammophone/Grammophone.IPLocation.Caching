using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Grammophone.IPLocation.Caching
{
	/// <summary>
	/// Abstract base for a cache resolving IP addresses to location information.
	/// </summary>
	public class LocationCache : IDisposable
	{
		#region Private fields

		private readonly MemoryCache memoryCache;

		private readonly ILocationProviderFactory locationProviderFactory;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="cacheOptions">The options for the underlying memory cache.</param>
		/// <param name="locationProviderFactory">The factory used for creation <see cref="ILocationProvider"/> instances.</param>
		public LocationCache(MemoryCacheOptions cacheOptions, ILocationProviderFactory locationProviderFactory)
		{
			if (cacheOptions == null) throw new ArgumentNullException(nameof(cacheOptions));
			if (locationProviderFactory == null) throw new ArgumentNullException(nameof(locationProviderFactory));

			this.memoryCache = new MemoryCache(cacheOptions);
			this.locationProviderFactory = locationProviderFactory;
		}

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="options">The options for the underlying memory cache.</param>
		/// <param name="locationProviderFactory">The factory used for creation <see cref="ILocationProvider"/> instances.</param>
		public LocationCache(IOptions<MemoryCacheOptions> options, ILocationProviderFactory locationProviderFactory)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));
			if (locationProviderFactory == null) throw new ArgumentNullException(nameof(locationProviderFactory));

			this.memoryCache = new MemoryCache(options);
			this.locationProviderFactory = locationProviderFactory;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Dispose the cache.
		/// </summary>
		public void Dispose() => memoryCache.Dispose();

		/// <summary>
		/// Get the estimated location for the given IP address.
		/// </summary>
		/// <param name="ipAddress">The IP address.</param>
		public async Task<Models.Location> GetLocationAsync(System.Net.IPAddress ipAddress)
		{
			if (ipAddress == null) throw new ArgumentNullException(nameof(ipAddress));

			return await memoryCache.GetOrCreateAsync<Models.Location>(ipAddress.ToString(), async cacheEntry =>
			{
				using (var locationProvider = locationProviderFactory.CreateLocationProvider())
				{
					return await locationProvider.GetLocationAsync(ipAddress);
				}
			});
		}

		#endregion
	}
}
