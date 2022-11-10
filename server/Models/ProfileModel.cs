
namespace server.Models;

public class Profile
{
    public Guid PublicId { get; set; }
    public string Username { get; set; }
    public List<Posts> Posts { get; set; }
}

public class SearchProfileModel
{
    public Guid PublicId { get; set; }
    public string Username { get; set; }
}