﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using GrowthTracking.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GrowthTracking.Repository.DBContext;

public partial class GrowthTracking_PRN231Context : DbContext
{
    public GrowthTracking_PRN231Context()
    {
    }

    public GrowthTracking_PRN231Context(DbContextOptions<GrowthTracking_PRN231Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Consultations_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Consultations).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.RoleCodeId, "IX_Users_RoleCodeId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.FullName).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired();
            entity.Property(e => e.VerifyOtp).HasColumnName("VerifyOTP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}