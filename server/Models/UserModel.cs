using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid PublicId { get; set; }
}

public class ClientUserModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}