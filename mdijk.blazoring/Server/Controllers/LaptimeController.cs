using mdijk.blazoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mdijk.f1.domainservices.CommandServices;
using mdijk.f1.domainservices.QueryServices;
using AutoMapper;

using LaptimeDto = mdijk.f1.domain.Data.Laptime;
using LaptimeModel = mdijk.blazoring.Shared.Models.Laptime;
using DriverDto = mdijk.f1.domain.Data.Driver;
using DriverModel = mdijk.blazoring.Shared.Models.Driver;
using mdijk.f1.domain.Data;
using mdijk.blazoring.Shared.Response;
using mdijk.blazoring.Shared.Submit;
using mdijk.blazoring.Shared.Models.CircuitEvents;
using mdijk.blazoring.Shared.Models;

namespace mdijk.blazoring.Server.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class LaptimeController : ControllerBase
	{
		private readonly ILogger<LaptimeController> logger;
		private readonly ILaptimeQueryService _laptimeQueryService;
		private readonly IMapper _mapper;

		public LaptimeController(ILogger<LaptimeController> logger,
		ILaptimeQueryService laptimeQueryService,
		IMapper mapper)
		{
			this.logger = logger;
			_laptimeQueryService = laptimeQueryService;
			_mapper = mapper;
		}

		[HttpGet("laptimesfordriver/{eventId}/{driverId}")]
		public async Task<DriverLaptimes> LaptimesForDriver(int eventId, int driverId)
		{
			var times = await _laptimeQueryService.GetAllLaptimesForEventAndDriver(eventId, driverId);

			return new DriverLaptimes
			{
				DriverId = driverId,
				Driver = _mapper.Map<DriverModel>(times.First().DriverEntry.Driver),
				Laptimes = times.Select(x => new LaptimeModel
				{
					CurrentPosition = x.CurrentPosition,
					LapNumber = x.LapNumber,
					Milliseconds = x.Milliseconds
				}).ToList()
			};
		}

		[HttpGet("laptimesforevent/{eventId}")]

		public async Task<ICollection<DriverLaptimes>> GetLaptimesForEvent(int eventId)
		{
			var times = await _laptimeQueryService.GetAllLaptimesForEvent(eventId);

			var dict = new Dictionary<int, DriverLaptimes>();

			foreach(var time in times)
			{
				if(!dict.ContainsKey(time.DriverEntry.DriverId))
				{
					dict.Add(time.DriverEntry.DriverId, new DriverLaptimes
					{
						DriverId = time.DriverEntry.DriverId,
						Driver = _mapper.Map<DriverModel>(time.DriverEntry.Driver),
						Laptimes = new List<LaptimeModel>(),
					});
				}
				dict[time.DriverEntry.DriverId].Laptimes.Add(new LaptimeModel
				{
					CurrentPosition = time.CurrentPosition,
					LapNumber = time.LapNumber,
					Milliseconds = time.Milliseconds
				});
			}

			return dict.Values.ToList();
		}


		[HttpGet("everythingfordriver/{driverId}")]
		public async Task<DriverLaptimes> GetEverythingForDriver(int driverId)
		{
			var times = await _laptimeQueryService.AllLapsForDriver(driverId);

			return new DriverLaptimes
			{
				DriverId = driverId,
				Driver = _mapper.Map<DriverModel>(times.First().DriverEntry.Driver),
				Laptimes = times.Select(x => new LaptimeModel
				{
					CurrentPosition = x.CurrentPosition,
					LapNumber = x.LapNumber,
					Milliseconds = x.Milliseconds
				}).ToList()
			};
		}

		[HttpGet("getlaptime/{seasonYear}/{circuitId}/{driverId}/{lapnumber}/{position}")]
		public async Task<ICollection<LaptimeModel>> GetSpecificLaptime(int seasonYear, int circuitId, int driverId, int lapnumber, int position)
		{
			var laptimes = await _laptimeQueryService.GetSpecificLaptime(seasonYear, circuitId, driverId, lapnumber, position);

			if(laptimes == null || !laptimes.Any())
			{
				return new List<LaptimeModel>();
			}

			return laptimes.Select(x => new LaptimeModel
			{
				CurrentPosition = x.CurrentPosition,
				LapNumber = x.LapNumber,
				Milliseconds = x.Milliseconds
			}).ToList();
		}
	}
}
