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
        var isPremium = from u in _context.Users where u.Username == username select u.IsPremium;

        var posts = from p in _context.Posts where p.PublicUserId == pubId.FirstOrDefault() select p;
        if(posts.FirstOrDefault() is null)
        {
            var profileToSendWithoutPost = new Profile{
                PublicId = pubId.FirstOrDefault(),
                Username = username,
                IsPremium = isPremium.FirstOrDefault(),
                Posts = null
            };
            return profileToSendWithoutPost;
        }

        var profileToSend = new Profile{
            PublicId = pubId.FirstOrDefault(),
            Username = username,
            IsPremium = posts.FirstOrDefault().IsPremium,
            Posts = posts.ToList()
        };
        return profileToSend;
    }

    [Route("/SearchProfiles/{username}")]
    [HttpGet]
    public ActionResult<IEnumerable<SearchProfileModel>> SearchProfiles(string username)
    {
        var profiles = from u in _context.Users where u.Username.Contains(username) select new SearchProfileModel{
            PublicId = u.PublicId,
            IsPremium = u.IsPremium,
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
            IsPremium = u.IsPremium,
            Username = u.Username
        };
        return allProfiles.ToList();
    }

}