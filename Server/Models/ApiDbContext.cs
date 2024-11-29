using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public partial class ApiDbContext : DbContext
{
    public virtual DbSet<Account> Accounts { get; set; } = null!;

    public ApiDbContext() : base()
    {
    }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasIndex(x => x.Username)
            .IsUnique();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
