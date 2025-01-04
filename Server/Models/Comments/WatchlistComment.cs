using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("watchlist_comment")]
public class WatchlistComment
{
    [Key]
    [Column("watchlist_comment_id")]
    public int Id { get; set; }

    [Column("comment_text")]
    [StringLength(maximumLength: 10000)]
    public required string Text { get; set; }

    [Column("publish_date")]
    public DateOnly PublishDate { get; set; }

    [Column("watchlist_id")]
    public required int WatchlistId { get; set; }

    [Column("user_id")]
    public required int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Watchlist Watchlist { get; set; } = null!;
}
