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

using CircuitDto = mdijk.f1.domain.Data.Circuit;
using CircuitModel = mdijk.blazoring.Shared.Models.Circuit;

using mdijk.blazoring.Shared.Models.CircuitEvents;

using mdijk.blazoring.Shared.Response;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CircuitController : ControllerBase
	{
		private readonly ILogger<CircuitController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly ICircuitQueryService _circuitQueryService;
		private readonly IMapper _mapper;

		public CircuitController(ILogger<CircuitController> logger,
			IAllInOneCommandService commandService,
			ICircuitQueryService circuitQueryService,
			IMapper mapper)
		{
			this.logger = logger;
			_commandService = commandService;
			_circuitQueryService = circuitQueryService;
			_mapper = mapper;
		}

		[HttpGet("getall")]
		public async Task<IEnumerable<CircuitModel>> GetAllCircuits()
		{
			var results = await _circuitQueryService.GetCircuits();

			return results.Select(x => _mapper.Map<CircuitModel>(x)).ToList();

		}

		[HttpPost("addcircuit")]
		public async Task<AddItemResponse> AddCircuit([FromBody] CircuitModel circuit)
		{
			//validate circuit
			if (circuit == null) return AddItemResponse.CreateError("Circuit was null");
			if (string.IsNullOrEmpty(circuit.CircuitName)) return AddItemResponse.CreateError("Firstname was null");
			if (string.IsNullOrEmpty(circuit.Country)) return AddItemResponse.CreateError("Country was null");

			try
			{
				var dto = _mapper.Map<CircuitDto>(circuit);
				var newCircuitId = await _commandService.AddOrUpdateCircuit(dto);

				return new AddItemResponse
				{
					NewItemId = newCircuitId
				};
			}
			catch (Exception ex)
			{
				return AddItemResponse.CreateError(ex.Message);
			}

		}

		[HttpGet("history/{circuitId}")]
		public async Task<ICollection<HistoricalRaceEvent>> GetCircuitHistoryEvent(int circuitId)
		{
			var res = await _circuitQueryService.GetRaceEventsAtCircuit(circuitId);
			return res.Select(re => new HistoricalRaceEvent
			{
				RaceEventId = re.RaceEventId,
				RaceEventName = re.RaceEventName,
				SeasonId = re.SeasonId,
				SequenceNumber = re.SequenceNumber,
				SeasonYear = re.Season.SeasonYear
			}).ToList();
		}
	}
}
