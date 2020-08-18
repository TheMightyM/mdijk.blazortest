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

using SeasonDto = mdijk.f1.domain.Data.Season;
using SeasonModel = mdijk.blazoring.Shared.Models.Season;
using EntryDto = mdijk.f1.domain.Data.Entry;
using EntryModel = mdijk.blazoring.Shared.Models.Entry;
using RaceEventDto = mdijk.f1.domain.Data.RaceEvent;
using RaceEventModel = mdijk.blazoring.Shared.Models.RaceEvent;
using DriverDto = mdijk.f1.domain.Data.Driver;
using DriverModel = mdijk.blazoring.Shared.Models.Driver;

using TeamDto = mdijk.f1.domain.Data.Team;
using TeamModel = mdijk.blazoring.Shared.Models.Team;
using EventResultDto = mdijk.f1.domain.Data.EventResult;
using EventResultModel = mdijk.blazoring.Shared.Models.EventResult;
using mdijk.f1.domain.Data;
using mdijk.blazoring.Shared.Response;
using mdijk.blazoring.Shared.Submit;
using mdijk.blazoring.Shared.Models.SeasonResults;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SeasonController : ControllerBase
	{
		private readonly ILogger<SeasonController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly ISeasonQueryService _seasonQueryService;
		private readonly IRaceEventQueryService _raceEventQueryService;
		private readonly IMapper _mapper;

		public SeasonController(ILogger<SeasonController> logger,
		IAllInOneCommandService commandService,
		ISeasonQueryService seasonQueryService,
		IRaceEventQueryService raceEventQueryService,
		IMapper mapper)
		{
			this.logger = logger;
			_commandService = commandService;
			_seasonQueryService = seasonQueryService;
			_raceEventQueryService = raceEventQueryService;
			_mapper = mapper;
		}

		[HttpGet("getall")]
		public async Task<IEnumerable<SeasonModel>> GetAllSeasons()
		{
			var results = (await _seasonQueryService.GetSeasons()).OrderByDescending(x => x.SeasonYear);

			return results.Select(x => _mapper.Map<SeasonModel>(x)).ToList();

		}

		[HttpPost("addSeason")]
		public async Task<AddItemResponse> AddSeason([FromBody] SeasonModel Season)
		{
			//validate Season
			if (Season == null) return AddItemResponse.CreateError("Season was null");
			if (Season.SeasonYear < 1950) return AddItemResponse.CreateError("Invalid year");

			try
			{
				var dto = _mapper.Map<SeasonDto>(Season);
				var newSeasonId = await _commandService.AddOrUpdateSeason(dto);

				return new AddItemResponse
				{
					NewItemId = newSeasonId
				};
			}
			catch (Exception ex)
			{
				return AddItemResponse.CreateError(ex.Message);
			}

		}

		[HttpPost("register")]
		public async Task<AddItemResponse> Register([FromBody] RegisterModel model)
		{
			if (model == null) return AddItemResponse.CreateError("entry was null");

			// is er al een entry voor dit seizoen dat zo heet?
			var entry = await _seasonQueryService.GetEntryByNameAndSeason(model.EntryName, model.SeasonId);
			int entryId = 0;
			if (entry == null)
			{
				// entry registeren.,
				entryId = await _commandService.AddOrUpdateEntry(new EntryDto
				{
					TeamId = model.TeamId,
					FullEntryName = model.EntryName,
					SeasonId = model.SeasonId
				});
			}
			else
			{
				entryId = entry.EntryId;
			}

			// controlleer of dit driver entry al niet bij deze team hoort
			try
			{

				var dE = await _seasonQueryService.DriverEntryExistsForSeason(model.TeamId, model.DriverId, model.SeasonId);
				if (dE == null)
				{
					var dEiD = await _commandService.AddDriverToEntry(entryId, model.DriverId);
					return new AddItemResponse
					{
						IsError = false,
						NewItemId = dEiD
					};
				}
				// coureur toevoegen aan die entry

				// entryId returnen
				return new AddItemResponse
				{
					IsError = false,
					NewItemId = dE.DriverEntryId
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost("addentry")]
		public async Task<AddItemResponse> AddEntry([FromBody] AddEntryModel entryModel)
		{
			//validate Season
			if (entryModel == null) return AddItemResponse.CreateError("entry was null");			

			try
			{
				// convert the model from strings to int, because blazor is being an annoying piece of crap
				var converted = new EntryDto
				{
					EngineId = Convert.ToInt32(entryModel.EngineId),
				//	FirstDriverId = Convert.ToInt32(entryModel.FirstDriverId),
					//SecondDriverId = Convert.ToInt32(entryModel.SecondDriverId),
					TeamId = Convert.ToInt32(entryModel.TeamId),
				//	todo: seasonid moet er nog bij!
					SeasonId = entryModel.SeasonId,
					FullEntryName = entryModel.FullEntryName
				};
				//var dto = _mapper.Map<EntryDto>(converted);
				var newId= await _commandService.AddOrUpdateEntry(converted);

				await _commandService.AddDriverToEntry(newId, Convert.ToInt32(entryModel.FirstDriverId));
				await _commandService.AddDriverToEntry(newId, Convert.ToInt32(entryModel.SecondDriverId));

				return new AddItemResponse
				{
					NewItemId = newId
				};
			}
			catch (Exception ex)
			{
				if(ex.InnerException != null)
				{
					return AddItemResponse.CreateError(ex.InnerException.Message);

				}
				return AddItemResponse.CreateError(ex.Message);
			}
		}

		[HttpGet("getseason/{seasonId}/{seasonYear}")]
		public async Task<SeasonModel> GetSeason(int seasonId, int seasonYear)
		{
			try
			{
				var currentSeason = await _seasonQueryService.GetSeasonById(seasonId);
				var mapped = _mapper.Map<SeasonModel>(currentSeason);

				return mapped;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
			
		}

		[HttpGet("getseasoncalendar/{seasonId}/{seasonYear}")]
		public async Task<IList<RaceEventModel>> GetSeasonCalendar(int seasonId, int seasonYear)
		{
			var events = await _seasonQueryService.GetRaceEventsForSeason(seasonId);
			IList<RaceEventModel> mdl = new List<RaceEventModel>();
			foreach(var x in events.OrderBy(x => x.SequenceNumber))
			{
				mdl.Add(_mapper.Map<RaceEventModel>(x));
			}

			return mdl;
		}


		[HttpPost("addraceevent")]
		public async Task<AddItemResponse> AddRaceEvent([FromBody] AddRaceEventModel raceEventModel)
		{
			//validate Season
			if (raceEventModel == null) return AddItemResponse.CreateError("raceEvent was null");			

			try
			{
				// convert the model from strings to int, because blazor is being an annoying piece of crap
				var converted = new RaceEventDto
				{
					RaceEventId = Convert.ToInt32(raceEventModel.RaceEventId),
					CircuitId = Convert.ToInt32(raceEventModel.CircuitId),
					SequenceNumber = Convert.ToInt32(raceEventModel.SequenceNr),
					SeasonId = raceEventModel.SeasonId,
					RaceEventName = raceEventModel.RaceEventName
				};
				//var dto = _mapper.Map<RaceEventDto>(converted);
				var newId= await _commandService.AddOrUpdateRaceEvent(converted);

				return new AddItemResponse
				{
					NewItemId = newId
				};
			}
			catch (Exception ex)
			{
				if(ex.InnerException != null)
				{
					return AddItemResponse.CreateError(ex.InnerException.Message);

				}
				return AddItemResponse.CreateError(ex.Message);
			}
		}
		 

		[HttpGet("getentriesforevent/{eventId}")]
		public async Task<IList<EntryModel>> GetEntriesForEvent(int eventId)
		{
			try
			{
				var results = await _raceEventQueryService.GetEntriesForEvent(eventId);
				var mapped = _mapper.Map<IList<EntryModel>>(results);
				return mapped;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("currentseasonresults")]
		public async Task<SeasonResultsModel> CurrentSeasonResults()
		{
			// wat we willen is een lijst met alle uitslagen.
			//--driver
			//-- team
			//-- results
			//-- - positions
			//-- - fastestlap

			var currentSeason = await _seasonQueryService.GetMostReasonSeason();

			var eventResults = await _seasonQueryService.GetRaceEventsForSeasonWithResults(currentSeason.SeasonId);

			var result = new SeasonResultsModel
			{
				SeasonId = currentSeason.SeasonId,
				SeasonYear = currentSeason.SeasonYear,
				RaceEvents = _mapper.Map<IList<SeasonRaceEvent>>(eventResults)
			};			

			//foreach(var eR in eventResults)
			//{
			//	foreach(var rr in eR.Results)
			//	{
			//		var srmr = result.DriverResults.FirstOrDefault(x => x.Driver.DriverId == rr.DriverEntry.DriverId
			//						&& x.Team.TeamId == rr.DriverEntry.Entry.TeamId);
			//		if (srmr == null)
			//		{
			//			srmr = new SeasonResultsModelRow
			//			{
			//				Driver = _mapper.Map<DriverModel>(rr.DriverEntry.Driver),
			//				Team = _mapper.Map<TeamModel>(rr.DriverEntry.Entry.Team),
			//				TeamEntryName = rr.DriverEntry.Entry.FullEntryName,
			//				Results = new List<EventResultModel>()
			//			};

			//			result.DriverResults.Add(srmr);
			//		}

			//		// deze niet automappen, want we hebben veel minder gegevens nodig in deze.
			//		srmr.Results.Add(new EventResultModel
			//		{
			//			FinishingPosition = rr.FinishingPosition,
			//			FastestLap = rr.FastestLap
			//		});
			//	}	
			//}


			return result;
			
		}

		[HttpGet("seasonresults/{seasonId}")]

		public async Task<SeasonResultsModel> GetSeasonResults(int seasonId)
		{
			var season = await _seasonQueryService.GetSeasonById(seasonId);
			var eventResults = await _seasonQueryService.GetRaceEventsForSeasonWithResults(seasonId);

			return new SeasonResultsModel
			{
				SeasonId = seasonId,
				SeasonYear = season.SeasonYear,
				RaceEvents = _mapper.Map<IList<SeasonRaceEvent>>(eventResults)
			};
		}

	}
}
