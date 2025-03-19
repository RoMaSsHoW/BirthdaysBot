using BirthdaysBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BirthdaysBot.DAL.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Birthday> Birthdays { get; set; }

    public virtual DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pk");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.UserChatId).HasColumnName("user_chat_id");
            entity.Property(e => e.UserName).HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
