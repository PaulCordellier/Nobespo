using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Design; // TODO Supprimer ce package nuget

namespace Server.Models;

[Table("media")]
public class Media
{
    [Key]
    [StringLength(maximumLength: 9, MinimumLength = 9, ErrorMessage = "La longueur de l'identifiant ImdbID doit être de 9 caractères pas vrai ?")]
    [Column("imdb_id", TypeName = "char(9)")]
    [JsonPropertyName("imdbID")]
    public required string ImdbID { get; set; }

    public required string Title { get; set; }

    //[JsonConverter(typeof(string))]
    public required string Year { get; set; }

    [JsonPropertyName("Poster")]
    public required string PosterUrl { get; set; }
}
