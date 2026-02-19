using System.ComponentModel.DataAnnotations;

public class Information
{
    [Key]
    public int id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string password { get; set; }

    [Required(ErrorMessage = "Contact number is required")]
    [Phone(ErrorMessage = "Enter a valid contact number")]
    public string contact { get; set; }

    [Required(ErrorMessage = "Image is required")]
    public string image { get; set; }

    [Required(ErrorMessage = "Type is required")]
    public string type { get; set; }

    public int CreateBy { get; set;}
}