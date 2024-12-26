using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public sealed partial class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<FilmComment> FilmsComments { get; set; } = null!;
    public DbSet<SerieComment> SeriesComments { get; set; } = null!;
    public DbSet<Watchlist> Watchlists { get; set; } = null!;

    public ApiDbContext() : base()
    {
    }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(x => x.Username)
                  .IsUnique();

            entity.HasMany(x => x.FilmsComments)
                  .WithOne(x => x.User)
                  .HasForeignKey(x => x.UserId)
                  .HasConstraintName("fk_user_film_comment");

            entity.HasMany(x => x.SeriesComments)
                  .WithOne(x => x.User)
                  .HasForeignKey(x => x.UserId)
                  .HasConstraintName("fk_user_serie_comment");

            entity.HasMany(x => x.Watchlists)
                  .WithOne(x => x.User)
                  .HasForeignKey(x => x.UserId)
                  .HasConstraintName("fk_user_watchlist")
                  .IsRequired();
        });

        modelBuilder.Entity<FilmComment>(entity =>
        {
            entity.Property(x => x.PublishDate).HasDefaultValueSql("CURRENT_DATE");
            entity.HasIndex(x => x.TmdbFilmId);
        });

        modelBuilder.Entity<SerieComment>(entity => 
        {
            entity.Property(x => x.PublishDate).HasDefaultValueSql("CURRENT_DATE");
            entity.HasIndex(x => x.TmdbSerieId);
        });

        modelBuilder.Entity<Watchlist>().Property(x => x.PublishDate).HasDefaultValueSql("CURRENT_DATE");
    }
}
