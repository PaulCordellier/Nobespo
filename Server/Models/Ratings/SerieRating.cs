using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("serie_rating")]
[PrimaryKey(nameof(UserId), nameof(SerieId))]
public sealed class SerieRating
{
    [Column("user_id")]
    public required int UserId { get; set; }

    [Column("serie_id")]
    public required int SerieId { get; set; }

    [Column("serie_rating")]
    public required int Rating { get; set; }

    public User User { get; set; } = null!;
}
