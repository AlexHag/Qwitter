using Microsoft.AspNetCore.Mvc;
using Qwitter.Funds.Contract.Events;

namespace Qwitter.Funds.Contract.FundsCallback;

public interface IFundsCallbackService
{
    [HttpPost("callback/funds-allocated")]
    Task OnFundsAllocated(FundsAllocatedEvent evt);

    [HttpPost("callback/funds-settled-from")]
    Task OnFundsSettledFrom(AllocationSettledEvent evt);

    [HttpPost("callback/funds-settled-into")]
    Task OnFundsSettledInto(AllocationSettledEvent evt);
}
