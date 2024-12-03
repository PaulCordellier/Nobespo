using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public sealed partial class ApiDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<FilmComment> FilmsComments { get; set; } = null!;
    public DbSet<SerieComment> SeriesComments { get; set; } = null!;

    public ApiDbContext() : base()
    {
    }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(x => x.Username)
                  .IsUnique();

            entity.HasMany(x => x.FilmsComments)
                  .WithOne(x => x.User)
                  .HasForeignKey(x => x.UserId)
                  .HasConstraintName("fk_user_film_comment")
                  .IsRequired();

            entity.HasMany(x => x.SeriesComments)
                  .WithOne(x => x.User)
                  .HasForeignKey(x => x.UserId)
                  .HasConstraintName("fk_user_serie_comment")
                  .IsRequired();
        });

        // The Comment class is the base class of every comment in the app
        // Entity framework can't adapt to Postgresql's inheritence, so this code will bind the Comment classes to their tables,
        // using the same TPC mapping and the same id name comment_id

        modelBuilder.Entity<FilmComment>(entity =>
        {
            entity.Property(x => x.PublishDateAndTime).HasDefaultValueSql("CURRENT_DATE");
            entity.HasIndex(x => x.TmdbFilmId);
        });

        modelBuilder.Entity<SerieComment>(entity => 
        {
            entity.Property(x => x.PublishDateAndTime).HasDefaultValueSql("CURRENT_DATE");
            entity.HasIndex(x => x.TmdbSerieId);
        });
    }
}
