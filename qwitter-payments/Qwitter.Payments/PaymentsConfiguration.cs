using Qwitter.Core.Application.Configuration;

namespace Qwitter.Payments;

[Configuration("Payments")]
public class PaymentsConfiguration
{
    public required List<string> SupportedCurrencies { get; set; }
}