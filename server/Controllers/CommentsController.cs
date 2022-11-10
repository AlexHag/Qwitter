using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    public dataContext _context;
    public CommentsController(dataContext context)
    {
        _context = context;
    }

    
}