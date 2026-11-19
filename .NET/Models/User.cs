using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Masar.Support;

namespace Masar.Models;

[Table("Users")]
public class User
{
    [Key]
    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("Full_Name")]
    public string FullName { get; set; } = string.Empty;

    [Column("Email")]
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    [Column("Password")]
    public string Password { get; set; } = string.Empty;

    [Column("Phone")]
    public string Phone { get; set; } = string.Empty;

    [Column("Role")]
    public string Role { get; set; } = string.Empty;

    [Column("Status")]
    public string Status { get; set; } = "Active";

    [Column("City")]
    public string? City { get; set; }

    [Column("Photo_URL")]
    public string? PhotoUrl { get; set; }

    public Client? Client { get; set; }

    public Expert? Expert { get; set; }

    public Admin? Admin { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public string GetAuthPassword()
    {
        return Password;
    }

    public string? GetRememberToken()
    {
        return null;
    }

    public void SetRememberToken(string? value)
    {
    }

    public string? GetRememberTokenName()
    {
        return null;
    }

    public bool IsClient()
    {
        return Role == "Client";
    }

    public bool IsExpert()
    {
        return Role == "Expert";
    }

    public bool IsAdmin()
    {
        return Role == "Admin";
    }

    public bool IsBlocked()
    {
        return Status == "Blocked";
    }

    [NotMapped]
    public string ShortName
    {
        get
        {
            var parts = Regex.Split(FullName.Trim(), @"\s+");
            if (parts.Length < 2)
            {
                return FullName;
            }
            return parts[0] + " " + parts[^1].Substring(0, 1).ToUpperInvariant() + ".";
        }
    }

    [NotMapped]
    public string FirstName => Regex.Split(FullName.Trim(), @"\s+")[0];

    [NotMapped]
    public string LastName
    {
        get
        {
            var parts = Regex.Split(FullName.Trim(), @"\s+");
            return parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : string.Empty;
        }
    }

    [NotMapped]
    public string Initials
    {
        get
        {
            var parts = Regex.Split(FullName.Trim(), @"\s+");
            var first = parts.Length > 0 && parts[0].Length > 0 ? parts[0].Substring(0, 1) : string.Empty;
            var last = parts.Length > 1 ? parts[^1].Substring(0, 1) : string.Empty;
            return (first + last).ToUpperInvariant();
        }
    }

    [NotMapped]
    public string PhoneDisplay
    {
        get
        {
            var d = Regex.Replace(Phone ?? string.Empty, @"\D", string.Empty);
            if (d.Length != 12)
            {
                return Phone ?? string.Empty;
            }
            return "+966 " + d.Substring(3, 2) + " " + d.Substring(5, 3) + " " + d.Substring(8);
        }
    }

    [NotMapped]
    public string? Photo => Img.Url(PhotoUrl);
}
