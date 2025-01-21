using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("watchlist")]
public sealed class Watchlist
{
    [Key]
    [Column("watchlist_id")]
    public int Id { get; set; }

    [Column("watchlist_title")]
    [StringLength(maximumLength: 200)]
    public required string Title { get; set; }

    [Column("watchlist_description")]
    [StringLength(maximumLength: 20000)]
    public required string Description { get; set; }

    [Column("publish_date")]
    public DateOnly PublishDate { get; set; }

    [Column("poster_paths")]
    public ICollection<string> PosterPaths { get; set; } = [];

    [Column("watchlist_films_ids")]
    public required ICollection<int> FilmsIds { get; init; }

    [Column("watchlist_series_ids")]
    public required ICollection<int> SeriesIds { get; init; }

    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<WatchlistComment> Comments { get; } = [];
}
