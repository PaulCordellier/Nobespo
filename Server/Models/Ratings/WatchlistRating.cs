using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("watchlist_rating")]
[PrimaryKey(nameof(UserId), nameof(WatchlistId))]
public sealed class WatchlistRating
{
    [Column("user_id")]
    public required int UserId { get; set; }

    [Column("watchlist_id")]
    public required int WatchlistId { get; set; }

    [Column("watchlist_rating")]
    public required int Rating { get; set; }

    public User User { get; set; } = null!;
    public Watchlist Watchlist { get; set; } = null!;
}
