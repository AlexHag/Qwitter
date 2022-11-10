using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    public dataContext _context;
    public ProfileController(dataContext context)
    {
        _context = context;
    }

    [Route("/GetProfile/{username}")]
    [HttpGet]
    public ActionResult<Profile> GetProfile(string username)
    {
        var pubId = from u in _context.Users where u.Username == username select u.PublicId;
        if(pubId.FirstOrDefault() == Guid.Empty) return NotFound();

        var posts = from p in _context.Posts where p.PublicUserId == pubId.FirstOrDefault() select p;

        var profileToSend = new Profile{
            PublicId = pubId.FirstOrDefault(),
            Username = username,
            Posts = posts.ToList()
        };
        return profileToSend;
    }

    [Route("/SearchProfiles/{username}")]
    [HttpGet]
    public ActionResult<IEnumerable<SearchProfileModel>> SearchProfiles(string username)
    {
        if(username == "")
        {
            var allProfiles = from u in _context.Users select new SearchProfileModel{
                PublicId = u.PublicId,
                Username = u.Username
            };
            return allProfiles.ToList();
        }

        var profiles = from u in _context.Users where u.Username.Contains(username) select new SearchProfileModel{
            PublicId = u.PublicId,
            Username = u.Username
        };
        if(profiles.FirstOrDefault() is null) return NotFound();
        return profiles.ToList();
    }

    [Route("/SearchProfiles")]
    [HttpGet]
    public ActionResult<IEnumerable<SearchProfileModel>> SearchAllProfiles()
    {

        var allProfiles = from u in _context.Users select new SearchProfileModel{
            PublicId = u.PublicId,
            Username = u.Username
        };
        return allProfiles.ToList();
    }

}