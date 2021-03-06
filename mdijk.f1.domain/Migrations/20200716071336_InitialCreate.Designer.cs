﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domain.Migrations
{
    [DbContext(typeof(FormulaContext))]
    [Migration("20200716071336_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<int>("EngineId")
                        .HasColumnType("int");

                    b.Property<int>("FirstDriverId")
                        .HasColumnType("int");

                    b.Property<string>("FullEntryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("SecondDriverId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("EntryId");

                    b.HasIndex("EngineId");

                    b.HasIndex("FirstDriverId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("SecondDriverId");

                    b.HasIndex("TeamId");

                    b.ToTable("Entries");
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

            modelBuilder.Entity("mdijk.f1.domain.Data.Entry", b =>
                {
                    b.HasOne("mdijk.f1.domain.Data.Engine", "Engine")
                        .WithMany()
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Driver", "FirstDriver")
                        .WithMany()
                        .HasForeignKey("FirstDriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Season", "Season")
                        .WithMany("Entries")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Driver", "SecondDriver")
                        .WithMany()
                        .HasForeignKey("SecondDriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("mdijk.f1.domain.Data.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
