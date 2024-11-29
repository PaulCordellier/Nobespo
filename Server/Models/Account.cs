using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("account")]
public sealed class Account
{
    [Key]
    [Column("account_id")]
    public required int Id { get; set; }

    [Column("username")]
    [StringLength(maximumLength: 25, MinimumLength = 3, ErrorMessage = "Für den Benutzernamen darf die Anzahl der Zeichen 25 nicht überschreiten.")]
    public required string Username { get; set; }

    [Column("hashed_password")]
    public required byte[] HashedPassword { get; set; }
}
