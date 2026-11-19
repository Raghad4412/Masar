using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Masar.Models;

[Table("Service_Type")]
public class ServiceType
{
    [Key]
    [Column("Service_Type_ID")]
    public int ServiceTypeId { get; set; }

    [Column("Type_Name")]
    public string TypeName { get; set; } = string.Empty;

    [Column("Description")]
    public string Description { get; set; } = string.Empty;

    [Column("Min_Price")]
    public decimal MinPrice { get; set; }

    [Column("Service_Packge")]
    public string ServicePackge { get; set; } = string.Empty;

    public ICollection<ServiceRequest> Requests { get; set; } = new List<ServiceRequest>();

    [NotMapped]
    public List<string> Bullets =>
        Regex.Split(Description ?? string.Empty, @"\r\n|\r|\n")
            .Select(l => l.Trim())
            .Where(l => l != string.Empty)
            .ToList();

    [NotMapped]
    public int RequestsCount { get; set; }
}
