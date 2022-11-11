using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Models;
using Algorand;
using Account = Algorand.Account;

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class AlgoController : ControllerBase
{
    public dataContext _context;
    public AlgoController(dataContext context)
    {
        _context = context;
    }

    private static Task<Stream> getHttpStreamTask(string url)
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      return client.GetStreamAsync(url);
    }

    [Route("/RequestWallet/{id}")]
    [HttpGet]
    public ActionResult<string> RequestWallet(Guid id)
    {
        var entity = _context.Users.Find(id);
        if(entity is null) return NotFound("Invalid Id");
        if(entity.PublicAddress is null)
        {
            Account src = new Account();
            var mnemonic = src.ToMnemonic();
            var address = src.Address.ToString();

            entity.PrivateMnemonic = mnemonic;
            entity.PublicAddress = address;
            _context.SaveChanges();
            return address;
        }

        return entity.PublicAddress;
    }

    [Route("/HaveBought")]
    [HttpPost]
    public async Task<IActionResult> HaveBought(AlgoHaveBoughtModel boughtModel)
    {
        var entity = _context.Users.Find(boughtModel.UserId);
        if(entity is null) return NotFound("Invalid Id");
        if(entity.PublicAddress != boughtModel.Address) return BadRequest("Error: Invalid address match");

        var url = $"https://node.algoexplorerapi.io/v2/accounts/{boughtModel.Address}";
        var res = await JsonSerializer.DeserializeAsync<ApiResponseModel>(await getHttpStreamTask(url));
        
        if(res.amount > 899999)
        {
            entity.IsPremium = true;
            
            var premiumPosts = _context.Posts.Where(p => p.UserId==entity.Id).ToList();
            if(premiumPosts.FirstOrDefault() is not null){
                premiumPosts.ForEach(p => p.IsPremium=true);
            }

            var premiumComments = _context.Comments.Where(p => p.UserId==entity.Id).ToList();
            if(premiumComments.FirstOrDefault() is not null)
            {
                premiumComments.ForEach(p => p.isPremium=true);
            }

            _context.SaveChanges();
            return Ok();
        }
        else 
        {
            return StatusCode(402);
        }
    }
}

public class AlgoHaveBoughtModel
{
    public Guid UserId { get; set; }
    public string Address { get; set; }
}

public class ApiResponseModel
{
    public string address { get; set; }
    public int amount { get; set; }
}













// using Algorand;
// using Algorand.V2;
// using Algorand.Client;
// //using Algorand.V2.Model;
// using Account = Algorand.Account;

// string myMemonic = "giraffe affair narrow smile brush gentle admit quick fit visa broccoli canyon torch age image manage bag oak ocean zone apple useful enforce abandon patient";

// Account src = new Account();

// var mnemonic = src.ToMnemonic();
// var address = src.Address.ToString();

// Console.WriteLine(mnemonic);
// Console.WriteLine(address);




//Console.WriteLine(src.Address.ToString());
//var priv = src.GetClearTextPrivateKey();

//Console.WriteLine(src.GetClearTextPrivateKey());

//string result = System.Text.Encoding.UTF8.GetString(src.GetClearTextPrivateKey());
//Console.WriteLine(result);

//string base64String = Convert.ToBase64String(priv, 0, priv.Length);
//Console.WriteLine(base64String);