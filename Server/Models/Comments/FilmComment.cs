using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("film_comment")]
public class FilmComment
{
    [Key]
    [Column("film_comment_id")]
    public required int Id { get; set; }

    [Column("comment_text")]
    [StringLength(maximumLength: 20000)]
    public required string Text { get; set; }

    [Column("publish_date_and_time")]
    public DateOnly PublishDateAndTime { get; set; }

    [Column("tmdb_film_id")]
    public required int TmdbFilmId { get; set; }

    [Column("user_id")]
    public required int UserId { get; set; }

    [Column("user_of_the_comment")]
    public virtual Account User { get; set; } = null!;
}
