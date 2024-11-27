using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("account")]
public sealed class Account
{
    [Key]
    public required int Id { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }
}
