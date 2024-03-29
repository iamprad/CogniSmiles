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
    [Migration("20221023205110_UpdateWithDoctorsModel")]
    partial class UpdateWithDoctorsModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CogniSmiles.Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BillingAddressId")
                        .HasColumnType("int");

                    b.Property<int?>("DeliveryAddressId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PracticeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("DeliveryAddressId");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("CogniSmiles.Models.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Login");
                });

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

            modelBuilder.Entity("CogniSmiles.Models.PostalAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AddressLine1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PostalAddress");
                });

            modelBuilder.Entity("CogniSmiles.Models.Doctor", b =>
                {
                    b.HasOne("CogniSmiles.Models.PostalAddress", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId");

                    b.HasOne("CogniSmiles.Models.PostalAddress", "DeliveryAddress")
                        .WithMany()
                        .HasForeignKey("DeliveryAddressId");

                    b.Navigation("BillingAddress");

                    b.Navigation("DeliveryAddress");
                });
#pragma warning restore 612, 618
        }
    }
}
