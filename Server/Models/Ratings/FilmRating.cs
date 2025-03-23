using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("film_rating")]
[PrimaryKey(nameof(UserId), nameof(FilmId))]
public sealed class FilmRating
{
    [Column("user_id")]
    public required int UserId { get; set; }

    [Column("film_id")]
    public required int FilmId { get; set; }

    [Column("film_rating")]
    public required int Rating { get; set; }

    public User User { get; set; } = null!;
}
