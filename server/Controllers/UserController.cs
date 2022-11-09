using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public dataContext _context;
    public UserController(dataContext context)
    {
        _context = context;
    }

    [Route("/GetUsers")]
    [HttpGet]
    public IEnumerable<User> GetUsers()
    {
        var users = from d in _context.Users select d;
        return users;
    }

    [Route("CreateAccount")]
    [HttpPost]
    public IActionResult CreateAccount(ClientUserModel user)
    {
        var serverUser = new User{ Id = Guid.NewGuid(), Username = user.Username, Password = user.Password};
        try {
            _context.Users.Add(serverUser);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return Conflict("Username already exist");
        }
        return StatusCode(201);
    }

    [Route("/Login")]
    [HttpPost]
    public IActionResult Login(ClientUserModel user)
    {
        Console.WriteLine("user tried to log in");
        var user_ = from u in _context.Users 
                     where u.Username == user.Username 
                     where u.Password == user.Password
                     select u;
        if(user_.FirstOrDefault() is null) return Unauthorized("Wrong Password Or Password");
        return Accepted(user_);
    }
}