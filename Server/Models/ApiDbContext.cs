using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public partial class ApiDbContext : DbContext
{
    public virtual DbSet<Media> Medias { get; set; }

    public ApiDbContext() : base()
    {
    }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>();

        base.OnModelCreating(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
