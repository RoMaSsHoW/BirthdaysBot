using System;
using System.Collections.Generic;
using BirthdaysBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BirthdaysBot.DAL.Data;

public partial class DbContext : DbContext
{
    public DbContext()
    {
    }

    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Birthday> Birthdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=birthdays;Username=roman;Password=fleshka5418");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Birthday>(entity =>
        {
            entity.HasKey(e => e.BirthdayId).HasName("birthday_pk");

            entity.ToTable("birthday");

            entity.Property(e => e.BirthdayId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("birthday_id");
            entity.Property(e => e.BirthdayDate).HasColumnName("birthday_date");
            entity.Property(e => e.BirthdayName).HasColumnName("birthday_name");
            entity.Property(e => e.BirthdayTelegramUsername).HasColumnName("birthday_telegram_username");
            entity.Property(e => e.UserChatId).HasColumnName("user_chat_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
