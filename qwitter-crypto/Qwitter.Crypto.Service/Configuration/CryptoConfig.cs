using Qwitter.Core.Application.Configuration;

namespace Qwitter.Crypto.Service.Configuration;

[Configuration("CryptoConfig")]
public class CryptoConfig
{
    public required List<string> SupportedCurrencies { get; set; }
}