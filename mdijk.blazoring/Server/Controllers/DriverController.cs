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

using DriverDto = mdijk.f1.domain.Data.Driver;
using DriverModel = mdijk.blazoring.Shared.Models.Driver;
using mdijk.f1.domain.Data;
using mdijk.blazoring.Shared.Response;
using mdijk.blazoring.Shared.Statistics;
using System.Reflection;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DriverController : ControllerBase
	{
		private readonly ILogger<DriverController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly IDriverQueryService _driverQueryService;
		private readonly ISeasonQueryService _seasonQueryService;

		private readonly IMapper _mapper;

		public DriverController(ILogger<DriverController> logger,
			IAllInOneCommandService commandService,
			IDriverQueryService driverQueryService,
			ISeasonQueryService seasonQueryService,
			IMapper mapper)
		{
			this.logger = logger;
			_commandService = commandService;
			_driverQueryService = driverQueryService;
			_seasonQueryService = seasonQueryService;
			_mapper = mapper;
		}

		[HttpGet("getall")]
		public async Task<IEnumerable<DriverModel>> GetAllDrivers()
		{
			var results = await _driverQueryService.GetDrivers();

			return results.Select(x => _mapper.Map<DriverModel>(x)).ToList();

		}

		[HttpPost("adddriver")]
		public async Task<AddItemResponse> AddDriver([FromBody] DriverModel driver)
		{
			//validate driver
			if (driver == null) return AddItemResponse.CreateError("Driver was null");
			if(string.IsNullOrEmpty(driver.FirstName)) return AddItemResponse.CreateError("Firstname was null");
			if(string.IsNullOrEmpty(driver.LastName)) return AddItemResponse.CreateError("LastName was null");
			if(string.IsNullOrEmpty(driver.Country)) return AddItemResponse.CreateError("Country was null");
			//if(driver.DriverNumber < 1) return AddItemResponse.CreateError("DriverNumber invalid");
			// niet waar, want drivernummer kan leeg zijn! of -1 tijdens import!

			try
			{
				var dto = _mapper.Map<DriverDto>(driver);
				var newDriverId = await _commandService.AddOrUpdateDriver(dto);

				return new AddItemResponse
				{
					NewItemId = newDriverId
				};
			}
			catch(Exception ex)
			{
				return AddItemResponse.CreateError(ex.Message);
			}

		}

		[HttpGet("stats/{driverId}")]
		public async Task<DriverStatistics> GetDriverStatistics(int driverId)
		{
			var allMetaData = await _driverQueryService.GetAllEventResultMetaDataForDriver(driverId);
			var basicData = await _driverQueryService.GetDriverById(driverId);

			/*
			 * ProcessForPoints
			 * ProcessForFinishes
			 * ProcessForWins
			 * ProcessForPodiums
			 * ProcessForPoles
			 * ProcessForDNF
			 * ProcessForDNQ
			 */

			var pfpoiTask = ProcessForPoints(allMetaData);
			var pffTask = ProcessForFinishes(allMetaData);
			var pfwTask = ProcessForWins(allMetaData);
			var pfpodTask = ProcessForPodiums(allMetaData);
			var pfpolTask = ProcessForPoles(allMetaData);
			var pfdnfTask = ProcessForDNF(allMetaData);
			var pfdnqTask = ProcessForDNQ(allMetaData);

			await Task.WhenAll(new Task[] { pfpoiTask, pffTask, pfwTask, pfpodTask, pfpolTask, pfdnfTask, pfdnqTask });

			var ds = new DriverStatistics
			{
				CareerPoints = pfpoiTask.Result,
				DriverId = driverId,
				DriverName = $"{basicData.FirstName} {basicData.LastName}",
				NumberOfDNF = pfdnfTask.Result,
				NumberOfDNQ = pfdnqTask.Result,
				NumberOfPodiums = pfpodTask.Result,
				NumberOfPoles = pfpolTask.Result,
				NumberOfRaces = pffTask.Result,
				NumberOfWins = pfwTask.Result
			};

			return ds;
		}

		private async Task<float> ProcessForPoints(IList<EventResultMetaData> meta)
		{
			return await Task.Run<float>(()=>
			{
				return meta.Where(x => x.Key == "Points")
				.Select(x => Convert.ToSingle(x.Value))
				.Sum();				
			});
		}

		private async Task<int> ProcessForFinishes(IList<EventResultMetaData> meta)
		{
			return await Task.Run<int>(() =>
			{
				return meta.Where(x => x.Key == "Position")
				.Count();
			});
		}

		private async Task<int> ProcessForWins(IList<EventResultMetaData> meta)
		{
			return await Task.Run<int>(() =>
			{
				return meta.Where(x => x.Key == "Position")
				.Select(x => Convert.ToInt32(x.Value))
				.Count(x => x == 1);
			});
		}

		private async Task<int> ProcessForPodiums(IList<EventResultMetaData> meta)
		{
			return await Task.Run<int>(() =>
			{
				return meta.Where(x => x.Key == "Position")
				.Select(x => Convert.ToInt32(x.Value))
				.Count(x => x <= 3 && x >= 1);
			});
		}

		private async Task<int> ProcessForPoles(IList<EventResultMetaData> meta)
		{
			return await Task.Run<int>(() =>
			{
				return meta.Where(x => x.Key == "Grid")
				.Select(x => Convert.ToInt32(x.Value))
				.Count(x => x == 1);
			});
		}

		private async Task<int> ProcessForDNF(IList<EventResultMetaData> meta)
		{
			return await Task.Run<int>(() =>
			{
				return meta.Where(x => x.Key == "Status")
				.Select(x => x.Value)
				.Count(x => !x.Equals("Finished") && ! x.EndsWith("Lap") && !x.EndsWith("Laps"));
			});
		}

		private async Task<int> ProcessForDNQ(IList<EventResultMetaData> meta)
		{
			return await Task.Run<int>(() =>
			{
				return meta.Count(x => x.Key == "Status"
				&& (x.Value == "Did not qualify" || x.Value == "107% rule"));
			});
		}

		[HttpGet("fullseasonstats/{driverId}")]
		public async Task<DriverSeasonStatistics> GetDriverSeasonStats(int driverId)
		{
			var allMetaData = await _driverQueryService.GetAllEventResultMetaDataForDriver2(driverId);
			var numberOfRacesPerSeason = await _seasonQueryService.GetNumberOfRacesPerSeason();

			
			// eerst zooi per eventId verzamelen
			var driversResults = new Dictionary<int, DriverResult>();
			var events = new Dictionary<int, RaceEvent>();
			foreach(var item in allMetaData)
			{
				DriverResult dr;
				if (driversResults.ContainsKey(item.EventResult.RaceEvent.RaceEventId))
				{
					dr = driversResults[item.EventResult.RaceEvent.RaceEventId];
				}
				else
				{
					dr = new DriverResult
					{
						EventId = item.EventResult.RaceEvent.RaceEventId,
						EventName = item.EventResult.RaceEvent.RaceEventName,
						EventSequenceNr = item.EventResult.RaceEvent.SequenceNumber
					};
					driversResults.Add(dr.EventId, dr);
					events.Add(item.EventResult.RaceEvent.RaceEventId, item.EventResult.RaceEvent);
				}

				switch(item.Key)
				{
					case "Status":
						dr.Finished = item.Value.Equals("Finished") || item.Value.EndsWith("Lap") || item.Value.EndsWith("Laps");
						dr.Started = (item.Value != "Did not qualify" && item.Value != "107% rule");
						break;
					case "Position":
						dr.FinishingPosition = Convert.ToInt32(item.Value);
						break;
				}
			}

			var driverSeasons = new Dictionary<int, DriverSeason>();
			foreach(var e in events.Values.ToList())
			{
				DriverSeason cs;
				if(driverSeasons.ContainsKey(e.SeasonId))
				{
					cs = driverSeasons[e.SeasonId];
				}
				else
				{
					cs = new DriverSeason
					{
						EventsInSeason = numberOfRacesPerSeason[e.Season.SeasonYear],
						SeasonYear = e.Season.SeasonYear,
						SeasonId = e.Season.SeasonId,
						Results = new List<DriverResult>()
					};
					driverSeasons.Add(e.SeasonId, cs);
				}

				cs.Results.Add(driversResults[e.RaceEventId]);
			}

			var dss = new DriverSeasonStatistics
			{
				DriverId = driverId,
				Seasons = driverSeasons.Values.OrderByDescending(x=>x.SeasonYear).ToList()
			};



			return dss;
		}


		[HttpGet("byId/{driverId}")]
		public async Task<DriverModel> GetDriver(int driverId)
		{
			var driverDTO = await _driverQueryService.GetDriverById(driverId);

			var mapped = _mapper.Map<DriverModel>(driverDTO);
			return mapped;
		}

		[HttpGet("pagedrivers/{pagenr}/{pagesize}")]
		public async Task<ICollection<DriverModel>> PageDrivers(int pagenr, int pagesize)
		{
			var results = await _driverQueryService.GetDrivers(pagenr, pagesize);
			return results.Select(x => _mapper.Map<DriverModel>(x)).ToList();
		}

		[HttpGet("numberofdrivers")]
		public async Task<int> GetNumberOfDrivers()
		{
			return await _driverQueryService.GetTotalNumberOfDrivers(); 
		}
	}
}
