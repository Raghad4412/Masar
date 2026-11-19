using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Admin")]
public class Admin
{
    [Key]
    [Column("Admin_ID")]
    public int AdminId { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("Permission")]
    public string Permission { get; set; } = string.Empty;

    public User? User { get; set; }
}
