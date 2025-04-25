using Microsoft.EntityFrameworkCore;
using System;

namespace WhatsTodo.Models;

public class PaxDbContext : DbContext 
{
    public PaxDbContext(DbContextOptions<PaxDbContext> options) :
        base(options) { }

    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> User { get; set; }

    public DbSet<SysLogs> SysLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured is false)
            optionsBuilder.UseNpgsql(AppSettings.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>()
            .HasIndex(t => t.NotificationDate);
        modelBuilder.Entity<Todo>()
            .HasIndex(t => t.UserPhone);
        modelBuilder.Entity<WhatsappBotLog>()
            .HasIndex(l => l.Timestamp);
        modelBuilder.Entity<WhatsappBotLog>()
            .HasIndex(l => l.UserPhone);
    }
}
