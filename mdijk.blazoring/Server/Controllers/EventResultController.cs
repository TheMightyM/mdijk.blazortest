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

using EventResultDto = mdijk.f1.domain.Data.EventResult;
using EventResultModel = mdijk.blazoring.Shared.Models.EventResult;
using TeamModel = mdijk.blazoring.Shared.Models.Team;
using EngineModel = mdijk.blazoring.Shared.Models.Engine;
using EventResultMetaDataDto = mdijk.f1.domain.Data.EventResultMetaData;
using EventResultMetaDataTyped = mdijk.blazoring.Shared.Models.EventResultMetaDataTyped;

using RaceEventDto = mdijk.f1.domain.Data.RaceEvent;
using RaceEventModel = mdijk.blazoring.Shared.Models.RaceEvent;

using mdijk.f1.domain.Data;
using mdijk.blazoring.Shared.Response;
using mdijk.blazoring.Shared.Submit;
using mdijk.blazoring.Shared.Models.CircuitEvents;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EventResultController : ControllerBase
	{
		private readonly ILogger<EventResultController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly IRaceEventQueryService _raceEventQueryService;
		private readonly IMapper _mapper;

		public EventResultController(ILogger<EventResultController> logger,
		IAllInOneCommandService commandService,
		IRaceEventQueryService raceEventQueryService,
		IMapper mapper)
		{
			this.logger = logger;
			_commandService = commandService;
			_raceEventQueryService = raceEventQueryService;
			_mapper = mapper;
		}


		[HttpGet("getresultsforevent/{eventId}/{eventName}")]
		public async Task<IList<EventResultModel>> GetResultsForEvent(int eventId, string eventName)
		{
			var results = await _raceEventQueryService.GetResultsForEvent(eventId);

			var mappedList = new List<EventResultModel>();
			foreach(var result in results)
			{
				var mapped = _mapper.Map<EventResultModel>(result);
				// some manual/semi-automapping to prevent json crapping out because of reference depth
				// cleary, the automapping mapping needs work, Mark!
				mapped.DriverEntry.EntryFullEntryName = result.DriverEntry.Entry.FullEntryName;
				mapped.DriverEntry.EntryTeam = _mapper.Map<TeamModel>(result.DriverEntry.Entry.Team);
				mapped.DriverEntry.EntryEngine = _mapper.Map<EngineModel>(result.DriverEntry.Entry.Engine);

				mapped.TypedMetaData = ProcessMetaData(result.MetaData);

				mappedList.Add(mapped);
			}
			return mappedList;
		}

		private EventResultMetaDataTyped ProcessMetaData(IList<EventResultMetaDataDto> metaData)
		{
			var er = new EventResultMetaDataTyped();

			foreach(var meta in metaData)
			{
				switch(meta.Key.ToLower())
				{
					case "points":
						er.Points = meta.Value;
						break;
					case "grid":
						er.GridPosition = Convert.ToInt32(meta.Value);
						break;
					case "laps":
						er.Laps = Convert.ToInt32(meta.Value);
						break;
					case "status":
						er.Status = meta.Value;
						break;
					case "time":
						er.Time = meta.Value;
						break;
					case "fastestlap":
						er.FastestLap = meta.Value;
						break;
					case "fastestlaprank":
						er.FastestLapRank = Convert.ToInt32(meta.Value);
						break;
				}
			}

			return er;
		}

		[HttpPost("AddCompleteResult")]
		public async Task<AddItemResponse> AddCompleteResult(AddEventResultModel eventResultModel)
		{
			//validate Season
			if (eventResultModel == null) return AddItemResponse.CreateError("eventResult was null");

			try
			{
				foreach (var result in eventResultModel.Rows)
				{
					// convert the model from strings to int, because blazor is being an annoying piece of crap
					var converted = new EventResultDto
					{
						DriverEntryId = Convert.ToInt32(result.DriverEntryId),
						//FastestLap = result.FastestLap,
						FinishingPosition= result.FinishingPosition,
						RaceEventId = eventResultModel.EventId
					};
					var newId = await _commandService.AddOrUpdateEventResult(converted);

					if(result.MetaDatas == null || !result.MetaDatas.Any())
					{
						result.MetaDatas = new List<MetaData>
						{
							new MetaData { Key = "Points", Value = result.Points},
							new MetaData { Key = "Laps", Value = result.Laps},
							new MetaData { Key = "Status", Value = result.Status},
							new MetaData { Key = "Time", Value = result.Time},
							new MetaData { Key = "Grid", Value = result.GridPosition},
							new MetaData { Key = "Position", Value = result.FinishingPosition.ToString()}
						};
					}

					if (result.MetaDatas != null && result.MetaDatas.Any())
					{
						foreach (var md in result.MetaDatas.Where(x=> !string.IsNullOrEmpty(x.Value)))
						{
							await _commandService.AddEventResultMetaData(new EventResultMetaDataDto
							{
								EventResultId = newId,
								Key = md.Key,
								Value = md.Value
							});
						}
					}
					//geen metadata, dus dan maar types meuk doen
					else
					{
						//public string Points { get; set; }
						//public string GridPosition { get; set; }
						//public string Laps { get; set; }
						//public string Status { get; set; }
						//public string Time { get; set; }

					}
				}
				return new AddItemResponse();
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return AddItemResponse.CreateError(ex.InnerException.Message);

				}
				return AddItemResponse.CreateError(ex.Message);
			}
		}

		[HttpGet("mostrecenteventresult")]
		public async Task<RaceEventModel> GetMostRecentEventResult()
		{
			var dto = await _raceEventQueryService.GetMostRecentCompletedRace();
			var mappMe = _mapper.Map<RaceEventModel>(dto);
			return mappMe;
		}

		[HttpPost("UpdateCompleteResult")]
		public async Task<AddItemResponse> UpdateCompleteResult(AddEventResultModel eventResultModel)
		{
			// delete all results.
			// en daarna de nieuwe zooi toevoegen
			await _commandService.DeleteEventResult(eventResultModel.EventId);

			return await AddCompleteResult(eventResultModel);
		}
	}
}
