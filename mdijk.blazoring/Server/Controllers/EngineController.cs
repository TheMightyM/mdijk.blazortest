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

using EngineDto = mdijk.f1.domain.Data.Engine;
using EngineModel = mdijk.blazoring.Shared.Models.Engine;
using mdijk.f1.domain.Data;
using mdijk.blazoring.Shared.Response;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EngineController : ControllerBase
	{
		private readonly ILogger<EngineController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly IEngineQueryService _queryService;
		private readonly IMapper _mapper;

		public EngineController(ILogger<EngineController> logger,
		IAllInOneCommandService commandService,
		IEngineQueryService queryService,
		IMapper mapper)
		{
			this.logger = logger;
			_commandService = commandService;
			_queryService = queryService;
			_mapper = mapper;
		}

		[HttpGet("getall")]
		public async Task<IEnumerable<EngineModel>> GetAllEngines()
		{
			var results = await _queryService.GetEngines();

			return results.Select(x => _mapper.Map<EngineModel>(x)).ToList();

		}

		[HttpPost("addEngine")]
		public async Task<AddItemResponse> AddEngine([FromBody] EngineModel Engine)
		{
			//validate Engine
			if (Engine == null) return AddItemResponse.CreateError("Engine was null");
			if (string.IsNullOrEmpty(Engine.EngineName)) return AddItemResponse.CreateError("Name was null");
			if (string.IsNullOrEmpty(Engine.Country)) return AddItemResponse.CreateError("Country was null");

			try
			{
				var dto = _mapper.Map<EngineDto>(Engine);
				var newEngineId = await _commandService.AddOrUpdateEngine(dto);

				return new AddItemResponse
				{
					NewItemId = newEngineId
				};
			}
			catch (Exception ex)
			{
				return AddItemResponse.CreateError(ex.Message);
			}

		}

	}
}
