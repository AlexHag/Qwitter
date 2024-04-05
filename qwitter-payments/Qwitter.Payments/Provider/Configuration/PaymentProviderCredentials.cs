
using Qwitter.Core.Application.Configuration;

namespace Qwitter.Payments.Provider.Configuration;

[Configuration("ProviderCredentials")]
public class PaymentProviderCredentials
{
    public required string Token { get; set; }
    public required string BaseAddress { get; set; }
}