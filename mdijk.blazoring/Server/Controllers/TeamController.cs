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

using TeamDto = mdijk.f1.domain.Data.Team;
using TeamModel = mdijk.blazoring.Shared.Models.Team;
using mdijk.f1.domain.Data;
using mdijk.blazoring.Shared.Response;
using mdijk.blazoring.Shared.Statistics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TeamController : ControllerBase
	{
		private readonly ILogger<TeamController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly ITeamQueryService _teamQueryService;
		private readonly IDriverQueryService _driverQueryService;
		private readonly IRaceEventQueryService _raceEventQueryService;
		private readonly ISeasonQueryService _seasonQueryService;
		private readonly IMapper _mapper;

		public TeamController(ILogger<TeamController> logger,
		IAllInOneCommandService commandService,
		ITeamQueryService teamQueryService,
		IDriverQueryService driverQueryService,
		IRaceEventQueryService raceEventQueryService,
		ISeasonQueryService seasonQueryService,
		IMapper mapper)
		{
			this.logger = logger;
			_commandService = commandService;
			_teamQueryService = teamQueryService;
			_driverQueryService = driverQueryService;
			_raceEventQueryService = raceEventQueryService;
			_seasonQueryService = seasonQueryService;
			_mapper = mapper;
		}

		[HttpGet("getall")]
		public async Task<IEnumerable<TeamModel>> GetAllTeams()
		{
			var results = await _teamQueryService.GetTeams();

			return results.Select(x => _mapper.Map<TeamModel>(x)).ToList();

		}

		[HttpPost("addTeam")]
		public async Task<AddItemResponse> AddTeam([FromBody] TeamModel Team)
		{
			//validate Team
			if (Team == null) return AddItemResponse.CreateError("Team was null");
			if (string.IsNullOrEmpty(Team.TeamName)) return AddItemResponse.CreateError("Name was null");
			if (string.IsNullOrEmpty(Team.Country)) return AddItemResponse.CreateError("Country was null");

			try
			{
				var dto = _mapper.Map<TeamDto>(Team);
				var newTeamId = await _commandService.AddOrUpdateTeam(dto);

				return new AddItemResponse
				{
					NewItemId = newTeamId
				};
			}
			catch (Exception ex)
			{
				return AddItemResponse.CreateError(ex.Message);
			}

		}
		[HttpGet("stats/{teamId}")]
		public async Task<TeamStatistics> GetTeamStatistics(int teamId)
		{
			var allMetaData = await _teamQueryService.GetAllEventResultMetaDataForTeam(teamId);
			var basicData = await _teamQueryService.GetTeamById(teamId);
			var numberOfRaces = await _teamQueryService.GetEventCountForTeam(teamId);

			var pfpoiTask = ProcessForPoints(allMetaData);
			var pfwTask = ProcessForWins(allMetaData);
			var pfpodTask = ProcessForPodiums(allMetaData);
			var pfpolTask = ProcessForPoles(allMetaData);

			await Task.WhenAll(new Task[] { pfpoiTask, pfwTask, pfpodTask, pfpolTask });

			var ts = new TeamStatistics
			{
				TotalPoints = pfpoiTask.Result,
				TeamId = teamId,
				TeamName = basicData.TeamName,
				NumberOfPodiums = pfpodTask.Result,
				NumberOfPoles = pfpolTask.Result,
				NumberOfRaces = numberOfRaces,
				NumberOfWins = pfwTask.Result
			};

			return ts;
		}

		private async Task<float> ProcessForPoints(IList<EventResultMetaData> meta)
		{
			return await Task.Run<float>(() =>
			{
				return meta.Where(x => x.Key == "Points")
				.Select(x => Convert.ToSingle(x.Value))
				.Sum();
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


		[HttpGet("participation/{teamId}")]
		public async Task<IList<TeamSeasonParticipation>> GetParticipation(int teamId)
		{
			var seasons = await _teamQueryService.GetAllSeasonsForTeam(teamId);

			var result = new List<TeamSeasonParticipation>();
			
			foreach(var s in seasons)
			{
				result.Add(new TeamSeasonParticipation
				{
					SeasonId = s.SeasonId,
					SeasonYear = s.SeasonYear,
					NumberOfDrivers = await _teamQueryService.GetNumberOfDriversForTeamInSeason(teamId, s.SeasonId)
				});
			}

			return result;
		}

		[HttpGet("maxraces/{teamId}")]
		public async Task<int> GetMaxRacesInSeasonForTeam(int teamId)
		{
			return await _teamQueryService.GetMaxRacesInSeasonForTeam(teamId);
		}

		[HttpGet("getseasonresults/{teamId}/{seasonId}")]
		public async Task<TeamSeason> GetSeasonResults(int teamId, int seasonId)
		{

			var eventsInSeason = await _seasonQueryService.GetRaceEventsForSeason(seasonId); //<-- die hebben we, zonder teamdid is oged
			var ts = new TeamSeason
			{
				RacesInSeason = eventsInSeason.Count,
				SeasonId = seasonId,
				SeasonYear = -1,
				EventResults = new List<TeamSeasonEventResult>(),
				DriversIds = new List<int>()
			};

			foreach (var re in eventsInSeason)
			{
				var driversForTeam = await _raceEventQueryService.GetDriversForEvent(teamId, re.RaceEventId);
				var driverResults = new Dictionary<int, TeamSeasonEventDriverResult>();
				
				var results = new TeamSeasonEventResult
				{
					EventId = re.RaceEventId,
					EventName = re.RaceEventName,
					EventSequenceNr = re.SequenceNumber,
				};

				foreach (var driver in driversForTeam)
				{
					var metadata = await _driverQueryService.GetMetaDataForDriverByEventId(driver.DriverId, re.RaceEventId);
					driverResults.Add(driver.DriverId, new TeamSeasonEventDriverResult
					{
						DriverId = driver.DriverId
					});
					if(!ts.DriversIds.Any(x => x == driver.DriverId))
					{
						ts.DriversIds.Add(driver.DriverId);
					}	

					foreach(var item in metadata)
					{
						switch(item.Key)
						{
							case "Status":
								driverResults[driver.DriverId].Finished = item.Value.Equals("Finished") || item.Value.EndsWith("Lap") || item.Value.EndsWith("Laps");
								driverResults[driver.DriverId].Started = (item.Value != "Did not qualify" && item.Value != "107% rule");
								break;
							case "Position":
								driverResults[driver.DriverId].FinishingPosition = Convert.ToInt32(item.Value);
								break;
						}
					}
				}

				results.DriverResults = driverResults.Values.ToList();
				ts.EventResults.Add(results);
			}
			 
			return ts;
		}
		
	}
}
