using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("serie_comment")]
public class SerieComment
{
    [Key]
    [Column("serie_comment_id")]
    public required int Id { get; set; }

    [Column("comment_text")]
    [StringLength(maximumLength: 20000)]
    public required string Text { get; set; }

    [Column("publish_date_and_time")]
    public DateOnly PublishDate { get; set; }

    [Column("tmdb_serie_id")]
    public required int TmdbSerieId { get; set; }

    [Column("user_id")]
    public required int UserId { get; set; }

    [Column("user_of_the_comment")]
    public virtual Account User { get; set; } = null!;
}
