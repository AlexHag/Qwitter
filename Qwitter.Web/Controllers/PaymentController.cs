using Qwitter.Domain.Api;

using Microsoft.AspNetCore.Mvc;
using Qwitter.Web.Services;

namespace Qwitter.Web.Controllers;

[ApiController]
[Route("payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentClient _paymentClient;

    public PaymentController(IPaymentClient paymentClient)
    {
        _paymentClient = paymentClient;
    }

    [HttpPost]
    public async Task<IActionResult> BuyPremium()
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            await _paymentClient.BuyPremium(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("transactions")]
    public async Task<IActionResult> GetTransactionHistory()
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            var transactions = await _paymentClient.GetTransactionHistory(userId);
            return Ok(transactions);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
