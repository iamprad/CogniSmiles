﻿// <auto-generated />
using System;
using CogniSmiles.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CogniSmiles.Migrations
{
    [DbContext(typeof(CogniSmilesContext))]
    [Migration("20221021215522_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CogniSmiles.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DicomFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ImplantDiameter")
                        .HasColumnType("int");

                    b.Property<string>("ImplantSite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImplantSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InmplantLength")
                        .HasColumnType("int");

                    b.Property<string>("PatientCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientStatus")
                        .HasColumnType("int");

                    b.Property<string>("StlIosFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SurgicalGuideWrittenDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });
#pragma warning restore 612, 618
        }
    }
}
