using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("film_comment")]
public class FilmComment
{
    [Key]
    [Column("film_comment_id")]
    public int Id { get; set; }

    [Column("comment_text")]
    [StringLength(maximumLength: 10000)]
    public required string Text { get; set; }

    [Column("publish_date")]
    public DateOnly PublishDate { get; set; }

    [Column("tmdb_film_id")]
    public required int TmdbFilmId { get; set; }

    [Column("user_id")]
    public required int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
