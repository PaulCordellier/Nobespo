using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public sealed partial class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<FilmComment> FilmsComments { get; set; } = null!;
    public DbSet<SerieComment> SeriesComments { get; set; } = null!;
    public DbSet<Watchlist> Watchlists { get; set; } = null!;
    public DbSet<WatchlistComment> WatchlistComments { get; set; } = null!;

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

        modelBuilder.Entity<Watchlist>(entity =>
        {
            entity.Property(x => x.PublishDate).HasDefaultValueSql("CURRENT_DATE");
            entity.HasMany(x => x.Comments)
                  .WithOne(x => x.Watchlist)
                  .HasForeignKey(x => x.WatchlistId)
                  .HasConstraintName("fk_watchlist_comment");
        });

        modelBuilder.Entity<WatchlistComment>().Property(x => x.PublishDate).HasDefaultValueSql("CURRENT_DATE");

        modelBuilder.HasDbFunction(typeof(ApiDbContext).GetMethod(nameof(SearchWatchlists), [typeof(string)])!)
                    .HasName("search_watchlists");

        modelBuilder.HasDbFunction(typeof(ApiDbContext).GetMethod(nameof(SearchUsernames), [typeof(string)])!)
                    .HasName("search_username");
    }

    public IQueryable<Watchlist> SearchWatchlists(string searchText) => FromExpression(() => SearchWatchlists(searchText));
    public IQueryable<User> SearchUsernames(string searchText) => FromExpression(() => SearchUsernames(searchText));
}
