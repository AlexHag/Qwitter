using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    public dataContext _context;
    public PostsController(dataContext context)
    {
        _context = context;
    }

    [Route("/GetAllPosts")]
    [HttpGet]
    public IEnumerable<ClientSendPostModel> Get()
    {
        var posts = from p in _context.Posts select new ClientSendPostModel{PostId = p.PostId, Author=p.Author, Content=p.Content, PublicUserId = p.PublicUserId, TimePosted = p.TimePosted, Likes = p.Likes, Dislikes = p.Dislikes, IsPremium = p.IsPremium};
        return posts.OrderByDescending(p => p.TimePosted);
    }

    [Route("/GetOnePost/{id}")]
    [HttpGet]
    public ActionResult<ClientSendPostModel> GetOnePost(Guid id)
    {
        var post = from p in _context.Posts where p.PostId == id select new ClientSendPostModel{PostId = p.PostId, Author=p.Author, Content=p.Content, PublicUserId = p.PublicUserId, TimePosted = p.TimePosted, Likes = p.Likes, Dislikes = p.Dislikes, IsPremium = p.IsPremium};
        if(post.FirstOrDefault() is null) return NotFound();
        return post.FirstOrDefault();
    }

    [Route("/LikePost/{id}")]
    [HttpPut]
    public IActionResult LikePost(Guid id)
    {
        var entity = _context.Posts.Find(id);
        if(entity is null) return NotFound();
        entity.Likes += 1;
        _context.SaveChanges();
        return Ok();
    }

    [Route("/DislikePost/{id}")]
    [HttpPut]
    public IActionResult DislikePost(Guid id)
    {
        var entity = _context.Posts.Find(id);
        if(entity is null) return NotFound();
        entity.Dislikes += 1;
        _context.SaveChanges();
        return Ok();
    }

    [Route("/GetAllPostsAndUser")]
    [HttpGet]
    public IEnumerable<PostsAndUser> GetAndUser()
    {
        var join = from u in _context.Users 
                   join p in _context.Posts on u.Id equals p.UserId 
                   select new PostsAndUser
                   {
                        PostId = p.PostId,
                        Author = p.Author,
                        Content = p.Content,
                        UserId = p.UserId,
                        PublicUserId = p.PublicUserId,
                        TimePosted = p.TimePosted,
                        Likes = p.Likes,
                        Dislikes = p.Dislikes,
                        IsPremium = p.IsPremium,
                        Id = u.Id,
                        Username = u.Username,
                        Password = u.Password,
                        PublicId = u.PublicId
                   };
        return join.OrderByDescending(p => p.TimePosted);
    }

    [Route("/CreatePost")]
    [HttpPost]
    public IActionResult Create(ClientPostModel postInput)
    {
        // Add check for publicId. Make user send it as well for more security idk or less idk.
        var username = from u in _context.Users where u.Id == postInput.UserId select u.Username;
        if (username.FirstOrDefault() is null ) return BadRequest("Unknown userID");

        var pubUserId = from u in _context.Users where u.Id == postInput.UserId select u.PublicId;
        //if (pubUserId.FirstOrDefault() is null) return StatusCode(500);
        var isPremium = from u in _context.Users where u.Id == postInput.UserId select u.IsPremium;
        var newPost = new Posts{PostId = Guid.NewGuid(), 
                                Author = username.FirstOrDefault(), 
                                Content = postInput.Content, 
                                UserId = postInput.UserId,
                                PublicUserId = pubUserId.FirstOrDefault(),
                                TimePosted = DateTimeOffset.Now.ToUnixTimeSeconds(),
                                Likes = 0,
                                Dislikes = 0,
                                IsPremium = isPremium.FirstOrDefault()};
        
        _context.Posts.Add(newPost);
        _context.SaveChanges();

        return StatusCode(201);
    }
    
}