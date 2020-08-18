using AutoMapper;

using DriverDTO = mdijk.f1.domain.Data.Driver;
using DriverModel = mdijk.blazoring.Shared.Models.Driver;

using EngineDTO = mdijk.f1.domain.Data.Engine;
using EngineModel = mdijk.blazoring.Shared.Models.Engine;

using TeamDTO = mdijk.f1.domain.Data.Team;
using TeamModel = mdijk.blazoring.Shared.Models.Team;

using SeasonDTO = mdijk.f1.domain.Data.Season;
using SeasonModel = mdijk.blazoring.Shared.Models.Season;


using EntryDTO = mdijk.f1.domain.Data.Entry;
using EntryModel = mdijk.blazoring.Shared.Models.Entry;

using CircuitDTO = mdijk.f1.domain.Data.Circuit;
using CircuitModel = mdijk.blazoring.Shared.Models.Circuit;

using RaceEventDTO = mdijk.f1.domain.Data.RaceEvent;
using RaceEventModel = mdijk.blazoring.Shared.Models.RaceEvent;

using DriverEntryDTO = mdijk.f1.domain.Data.DriverEntry;
using DriverEntryModel = mdijk.blazoring.Shared.Models.DriverEntry;

using EventResultDTO = mdijk.f1.domain.Data.EventResult;
using EventResultModel = mdijk.blazoring.Shared.Models.EventResult;

using EventResultMetaDataDTO = mdijk.f1.domain.Data.EventResultMetaData;
using EventResultMetaDataModel = mdijk.blazoring.Shared.Models.EventResultMetaData;

using SeasonRaceEvent = mdijk.blazoring.Shared.Models.SeasonResults.SeasonRaceEvent;
using mdijk.blazoring.Shared.Models.SeasonResults;

namespace mdijk.blazoring.Server.Mapping
{
	public class DataToModelDriverProfile : Profile
	{
		public DataToModelDriverProfile()
		{
			CreateMap<DriverDTO, DriverModel>();
			CreateMap<DriverModel, DriverDTO>();

			CreateMap<TeamDTO, TeamModel>();
			CreateMap<TeamModel, TeamDTO>();

			CreateMap<EngineDTO, EngineModel>();
			CreateMap<EngineModel, EngineDTO>();

			CreateMap<SeasonDTO, SeasonModel>();
			CreateMap<SeasonModel, SeasonDTO>();

			CreateMap<EntryDTO, EntryModel>();
			CreateMap<EntryModel, EntryDTO>();

			CreateMap<CircuitDTO, CircuitModel>();
			CreateMap<CircuitModel, CircuitDTO>();

			CreateMap<RaceEventDTO, RaceEventModel>();
			CreateMap<RaceEventModel, RaceEventDTO>();

			CreateMap<DriverEntryDTO, DriverEntryModel>();
			CreateMap<DriverEntryModel, DriverEntryDTO>();

			CreateMap<EventResultDTO, EventResultModel>();
			CreateMap<EventResultModel, EventResultDTO>();

			CreateMap<EventResultMetaDataDTO, EventResultMetaDataModel>();
			CreateMap<EventResultMetaDataModel, EventResultMetaDataDTO>();

			CreateMap<RaceEventDTO, SeasonRaceEvent>();
			CreateMap<EventResultDTO, SeasonRaceEventResult>();
		}
	}
}
