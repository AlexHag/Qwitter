using Qwitter.Core.Application.Configuration;

namespace Qwitter.Crypto.Currency.Ethereum;

[Configuration("AlchemyConfiguration")]
public class AlchemyConfiguration
{
    public required string Token { get; set; }
    public required string BaseUrl { get; set; }
}