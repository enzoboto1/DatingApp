using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace API.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<MemberLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // Suppress the pending model changes warning for development
        // This warning can occur due to static model configuration differences
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Photo>().HasQueryFilter(x => x.IsApproved);

        // Configure Message Id to auto-generate GUID when not provided
        modelBuilder.Entity<Message>()
            .Property(m => m.Id)
            .ValueGeneratedNever();

        // Set default values for Member timestamps in the database
        modelBuilder.Entity<Member>()
            .Property(m => m.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Member>()
            .Property(m => m.LastActive)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Set default values for Message timestamps in the database
        modelBuilder.Entity<Message>()
            .Property(m => m.MessageSent)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Apply value converters to specific DateTime properties
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : null,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
        );

        // Apply to Member entity
        modelBuilder.Entity<Member>()
            .Property(m => m.Created)
            .HasConversion(dateTimeConverter);

        modelBuilder.Entity<Member>()
            .Property(m => m.LastActive)
            .HasConversion(dateTimeConverter);

        // Apply to Message entity
        modelBuilder.Entity<Message>()
            .Property(m => m.MessageSent)
            .HasConversion(dateTimeConverter);

        modelBuilder.Entity<Message>()
            .Property(m => m.DateRead)
            .HasConversion(nullableDateTimeConverter);

        modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Id = "member-id", Name = "Member", NormalizedName = "MEMBER" },
                new IdentityRole { Id = "moderator-id", Name = "Moderator", NormalizedName = "MODERATOR" },
                new IdentityRole { Id = "admin-id", Name = "Admin", NormalizedName = "ADMIN" }
            );

        modelBuilder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MemberLike>()
            .HasKey(x => new { x.SourceMemberId, x.TargetMemberId });

        modelBuilder.Entity<MemberLike>()
            .HasOne(s => s.SourceMember)
            .WithMany(t => t.LikedMembers)
            .HasForeignKey(s => s.SourceMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MemberLike>()
            .HasOne(s => s.TargetMember)
            .WithMany(t => t.LikedByMembers)
            .HasForeignKey(s => s.TargetMemberId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}