﻿using And9.Service.Election.Abstractions.Interfaces;
using And9.Service.Election.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace And9.Service.Election.Database;

public class ElectionDataContext : DbContext
{
    public ElectionDataContext(DbContextOptions<ElectionDataContext> options) : base(options) { }

    public DbSet<Abstractions.Models.Election> Elections { get; set; } = null!;
    public DbSet<ElectionVote> ElectionVotes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Election");

        modelBuilder.Entity<Abstractions.Models.Election>(entity =>
        {
            entity.HasKey(x => new
            {
                x.ElectionId,
                x.Direction,
            });
            entity.Property(x => x.ElectionId);
            entity.HasIndex(x => x.ElectionId).IsUnique(false);
            entity.Property(x => x.Direction);
            entity.HasIndex(x => x.Direction).IsUnique(false);

            entity.Property(x => x.AdvisorsStartDate);
            entity.Property(x => x.Status);
            entity.Property(x => x.AgainstAllVotes);

            entity.Property(x => x.ConcurrencyToken).IsConcurrencyToken();
            entity.Property(x => x.LastChanged).IsRowVersion();
        });

        modelBuilder.Entity<ElectionVote>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ElectionId);
            entity.HasIndex(x => x.ElectionId).IsUnique(false);
            entity.Property(x => x.Direction);
            entity.HasIndex(x => x.Direction).IsUnique(false);
            entity.HasIndex(x => new
            {
                x.ElectionId,
                x.Direction,
            }).IsUnique(false);

            entity.Property(x => x.MemberId);
            entity.Property(x => x.Voted).IsRequired(false);
            entity.Property(x => x.Votes);

            entity.Property(x => x.ConcurrencyToken).IsConcurrencyToken();
            entity.Property(x => x.LastChanged).IsRowVersion();
        });
    }
}