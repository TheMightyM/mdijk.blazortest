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
using System.Diagnostics;
using mdijk.f1.domainservices.QueryServices.Impl;
using Microsoft.EntityFrameworkCore;

namespace mdijk.blazoring.Server.Controllers
{
	public class SpeedTest
	{
		private readonly ILogger<TeamController> logger;
		private readonly IAllInOneCommandService _commandService;
		private readonly ITeamQueryService _teamQueryService;
		private readonly IDriverQueryService _driverQueryService;
		private readonly IRaceEventQueryService _raceEventQueryService;
		private readonly ISeasonQueryService _seasonQueryService;
		private readonly IMapper _mapper;

		public SpeedTest(ILogger<TeamController> logger,
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

		[HttpGet("test1")]
		public async Task<string> TestGetRaceEventsForSeasonWithResults()
		{
			return await Task.FromResult("a");
			/*var taskMillsTrack = new List<long>();
			var taskMillsNoTrack = new List<long>();

			for (int j = 0; j < 200; j++)
			{
				for (int i = 0; i < 60; i++)
				{
					taskMillsTrack.Add(await GetRaceEventsForSeasonWithResultsTask(i, true));
					taskMillsNoTrack.Add(await GetRaceEventsForSeasonWithResultsTask(i, false));
				}
			}

			return @$"Tracking:
{Environment.NewLine}MIN:{taskMillsTrack.Min()};
{Environment.NewLine}MAX:{taskMillsTrack.Max()};
{Environment.NewLine}AVG:{taskMillsTrack.Average()}
{Environment.NewLine}NoTracking:
{Environment.NewLine}MIN:{taskMillsNoTrack.Min()};
{Environment.NewLine}MAX:{taskMillsNoTrack.Max()};
{Environment.NewLine}AVG:{taskMillsNoTrack.Average()}
";*/
		}

		private async Task<long> GetRaceEventsForSeasonWithResultsTask(int i, bool tacking)
		{
			var constr = "Server=OVSPC435\\SQLEXPRESS; Database=formula;User Id=formula; Password=formula;";

			var optionsBuilder = new DbContextOptionsBuilder<FormulaContext>();
			optionsBuilder.UseSqlServer(constr);

			var sqs = new SeasonQueryService(new FormulaContext(optionsBuilder.Options));
			Stopwatch sw = new Stopwatch();
			sw.Start();
			var throwAway = await sqs.GetRaceEventsForSeasonWithResults(i, false, tacking);
			sw.Stop();
			
			return sw.ElapsedMilliseconds;
		}

	}
}
