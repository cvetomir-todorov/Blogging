﻿// <auto-generated />
using System;
using CloudDeploy.Management.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudDeploy.Management.Data.Migrations
{
    [DbContext(typeof(ManagementDbContext))]
    [Migration("20200729095510_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CloudDeploy.Management.Data.Entities.DeploymentEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PackageVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("ProjectID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Deployment");
                });

            modelBuilder.Entity("CloudDeploy.Management.Data.Entities.ProjectEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PackageSource")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("CloudDeploy.Management.Data.Entities.ServerEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<int>("Cloud")
                        .HasColumnType("int");

                    b.Property<int?>("DeploymentID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DeploymentID");

                    b.ToTable("Server");
                });

            modelBuilder.Entity("CloudDeploy.Management.Data.Entities.DeploymentEntity", b =>
                {
                    b.HasOne("CloudDeploy.Management.Data.Entities.ProjectEntity", "Project")
                        .WithMany("Deployments")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CloudDeploy.Management.Data.Entities.ServerEntity", b =>
                {
                    b.HasOne("CloudDeploy.Management.Data.Entities.DeploymentEntity", null)
                        .WithMany("Servers")
                        .HasForeignKey("DeploymentID");
                });
#pragma warning restore 612, 618
        }
    }
}
