using System.Collections.Generic;
using mdijk.f1.domain.Migrations;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domain.Data
{
	public class FormulaContext : DbContext
	{
		public DbSet<Driver> Drivers { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Engine> Engines { get; set; }
		public DbSet<Entry> Entries { get; set; }
		public DbSet<Season> Seasons { get; set; }
		public DbSet<Circuit> Circuits { get; set; }
		public DbSet<RaceEvent> RaceEvents { get; set; }
		public DbSet<DriverEntry> DriverEntries { get; set; }

		public DbSet<EventResult> EventResults { get; set; }
		public DbSet<EventResultMetaData> EventResultMetaDatas { get; set; }

		public DbSet<RaceEventMetaData> RaceEventMetaData { get; set; }

		public DbSet<Laptime> Laptimes { get; set; }
		public DbSet<Pitstop> Pitstops { get; set; }

		public FormulaContext(DbContextOptions<FormulaContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Entry>()
				.HasOne(e => e.Team)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Entry>()
				.HasOne(e => e.Engine)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<EventResult>()
				.HasOne(e => e.RaceEvent)
				.WithMany(e => e.Results)
				.HasForeignKey(e => e.RaceEventId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Laptime>()
				.HasOne(e => e.RaceEvent)
				.WithMany(e => e.Laptimes)
				.HasForeignKey(e => e.RaceEventId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Pitstop>()
				.HasOne(e => e.RaceEvent)
				.WithMany(e => e.Pitstops)
				.HasForeignKey(e => e.RaceEventId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<EventResultMetaData>()
				.HasIndex(ermd => new { 
					ermd.EventResultId,
					ermd.EventResultMetaDataId
				})
				.IncludeProperties(ermd => new { 
					ermd.Key,
					ermd.Value
				});

			modelBuilder.Entity<EventResult>()
				.HasIndex(er => new {
					er.RaceEventId,
					er.EventResultId,
					er.DriverEntryId
				})
				.IncludeProperties(er => new {
					er.FinishingPosition,
					er.FastestLap
				})
				.HasName("IX_EventResults_WithIncludes");

			modelBuilder.Entity<DriverEntry>()
				.HasIndex(de => new
				{
					de.EntryId,
					de.DriverId
				})
				.IncludeProperties(de => new
				{
					de.DriverEntryId
				});

			modelBuilder.Entity<RaceEvent>()
				.HasIndex(re => new {
					re.SeasonId,
					re.CircuitId
				})
				.IncludeProperties(re => new { 
					re.RaceEventId,
					re.SequenceNumber,
					re.RaceEventName
				});
		}
	}
}
