using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Portfolio")]
public class Portfolio
{
    [Key]
    [Column("Portfolio_ID")]
    public int PortfolioId { get; set; }

    [Column("Expert_ID")]
    public int ExpertId { get; set; }

    [Column("Title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("Image_URL")]
    public string ImageUrl { get; set; } = string.Empty;

    public Expert? Expert { get; set; }

    [NotMapped]
    public string? Image => Img.Url(ImageUrl);
}
