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
    public required string Username { get; set; }

    [Column("password" /*, Type = "TODO" */)]
    public required string Password { get; set; }
}
