using System.ComponentModel.DataAnnotations;

public class Data
{
    [Key]
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string contact { get; set; }
    public string password { get; set; }
    public string type { get; set; }
    public string image { get; set; }
}