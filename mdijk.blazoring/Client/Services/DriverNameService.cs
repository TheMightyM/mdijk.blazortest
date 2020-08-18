using mdijk.blazoring.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace mdijk.blazoring.Client.Services
{
	public class DriverNameServer
	{
		private HttpClient _client;
		public DriverNameServer(Uri baseAddress)
		{
			_baseAddress = baseAddress;
			_client = new HttpClient
			{
				BaseAddress = baseAddress
			};
		}

		public Uri _baseAddress { get; set; }

		private IDictionary<int, Driver> Drivers = new ConcurrentDictionary<int, Driver>();
		private object _locker = new object();
		public async Task<Driver> GetDriverInfo(int driverId)
		{

			if (!Drivers.ContainsKey(driverId))
			{
				var ds = await _client.GetFromJsonAsync<Driver>($"driver/byId/{driverId}");
				try
				{
					Drivers.TryAdd(driverId, ds);
					return Drivers[driverId];
				}
				catch
				{
					//concurrency error. maakt geen hol uit.
				}
			}
			
			try
			{
				return Drivers[driverId];
			}
			catch
			{
				return new Driver
				{
					DriverId = driverId,
					DriverNumber = 1,
					FirstName = "Firstname",
					LastName = "LastName"
				};
			}
		}
	}
}
