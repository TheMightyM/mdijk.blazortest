﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domain.Migrations
{
    [DbContext(typeof(FormulaContext))]
    [Migration("20200727061920_eventmetadata")]
    partial class eventmetadata
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("mdijk.f1.domain.Data.Circuit", b =>
                {
                    b.Property<int>("CircuitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CircuitName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CircuitId");

                    b.ToTable("Circuits");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DriverNumber")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DriverId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.DriverEntry", b =>
                {
                    b.Property<int>("DriverEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<int>("EntryId")
                        .HasColumnType("int");

                    b.HasKey("DriverEntryId");

                    b.HasIndex("DriverId");

                    b.HasIndex("EntryId");

                    b.ToTable("DriverEntries");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.Engine", b =>
                {
                    b.Property<int>("EngineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EngineName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EngineId");

                    b.ToTable("Engines");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.Entry", b =>
                {
                    b.Property<int>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EngineId")
                        .HasColumnType("int");

                    b.Property<string>("FullEntryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("EntryId");

                    b.HasIndex("EngineId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("TeamId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.EventResult", b =>
                {
                    b.Property<int>("EventResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DriverEntryId")
                        .HasColumnType("int");

                    b.Property<bool>("FastestLap")
                        .HasColumnType("bit");

                    b.Property<int>("FinishingPosition")
                        .HasColumnType("int");

                    b.Property<int>("RaceEventId")
                        .HasColumnType("int");

                    b.HasKey("EventResultId");

                    b.HasIndex("DriverEntryId");

                    b.HasIndex("RaceEventId");

                    b.ToTable("EventResults");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.EventResultMetaData", b =>
                {
                    b.Property<int>("EventResultMetaDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventResultId")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventResultMetaDataId");

                    b.HasIndex("EventResultId");

                    b.ToTable("EventResultMetaDatas");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.RaceEvent", b =>
                {
                    b.Property<int>("RaceEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CircuitId")
                        .HasColumnType("int");

                    b.Property<string>("RaceEventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.HasKey("RaceEventId");

                    b.HasIndex("CircuitId");

                    b.HasIndex("SeasonId");

                    b.ToTable("RaceEvents");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.RaceEventMetaData", b =>
                {
                    b.Property<int>("RaceEventMetaDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RaceEventId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RaceEventMetaDataId");

                    b.HasIndex("RaceEventId");

                    b.ToTable("RaceEventMetaData");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.Season", b =>
                {
                    b.Property<int>("SeasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SeasonYear")
                        .HasColumnType("int");

                    b.HasKey("SeasonId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.DriverEntry", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Entry", "Entry")
                        .WithMany("Drivers")
                        .HasForeignKey("EntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.Entry", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.Engine", "Engine")
                        .WithMany()
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("mdijk.f1.domain.Data.Season", "Season")
                        .WithMany("Entries")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.EventResult", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.DriverEntry", "DriverEntry")
                        .WithMany()
                        .HasForeignKey("DriverEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.RaceEvent", "RaceEvent")
                        .WithMany("Results")
                        .HasForeignKey("RaceEventId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.EventResultMetaData", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.EventResult", "EventResult")
                        .WithMany("MetaData")
                        .HasForeignKey("EventResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.RaceEvent", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.Circuit", "Circuit")
                        .WithMany()
                        .HasForeignKey("CircuitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Season", "Season")
                        .WithMany("Events")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("mdijk.f1.domain.Data.RaceEventMetaData", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.RaceEvent", "RaceEvent")
                        .WithMany("MetaDatas")
                        .HasForeignKey("RaceEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
