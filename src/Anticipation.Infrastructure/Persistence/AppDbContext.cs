using Anticipation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anticipation.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AnticipationRequest> AnticipationRequests => Set<AnticipationRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnticipationRequest>(builder =>
        {
            builder.HasKey(request => request.Id);
            builder.Property(request => request.CreatorId)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(request => request.Status)
                .HasConversion<int>();
            builder.Property(request => request.CreatedAtUtc)
                .IsRequired();

            builder.OwnsOne(request => request.Amount, money =>
            {
                money.Property(value => value.Amount)
                    .IsRequired();

                money.Property(value => value.Currency)
                    .HasMaxLength(3)
                    .IsRequired();
            });
        });
    }
}