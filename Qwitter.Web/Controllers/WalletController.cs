using Microsoft.AspNetCore.Mvc;
using Qwitter.Domain.Api;
using Qwitter.Domain.DTO;
using Qwitter.Web.Services;

namespace Qwitter.Web.Controllers;

[ApiController]
[Route("wallet")]
public class WalletController : ControllerBase
{
    private readonly ILogger<WalletController> _logger;
    private readonly IPaymentClient _paymentClient;

    public WalletController(
        ILogger<WalletController> logger,
        IPaymentClient paymentClient)
    {
        _logger = logger;
        _paymentClient = paymentClient;   
    }

    [HttpGet]
    public async Task<IActionResult> GetWallet()
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            var walletDTO = await _paymentClient.GetUserWallet(userId);
            return Ok(walletDTO);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}