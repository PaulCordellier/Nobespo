using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("user_data")]    // PostgreSQL doesn't allow a table named "user" to be created so I chose the table name user_data
public class User
{
    [Key]
    [Column("user_id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(maximumLength: 25, MinimumLength = 3)]
    public required string Username { get; set; }

    [Column("hashed_password")]
    public required byte[] HashedPassword { get; set; }

    public ICollection<SerieComment> SeriesComments { get; } = [];
    public ICollection<FilmComment> FilmsComments { get; } = [];
    public ICollection<WatchlistComment> WatchlistsComments { get; } = [];

    public ICollection<Watchlist> Watchlists { get; } = [];

    public ICollection<User> Followers { get; } = [];
    public ICollection<User> UsersFollowed { get; } = [];

    public ICollection<FilmRating> FilmRatings { get; } = [];
    public ICollection<SerieRating> SerieRatings { get; } = [];
    public ICollection<WatchlistRating> WatchlistRatings { get; } = [];
}
