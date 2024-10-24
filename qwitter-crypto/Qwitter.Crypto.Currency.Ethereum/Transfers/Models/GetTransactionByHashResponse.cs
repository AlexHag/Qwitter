using System.Text.Json.Serialization;

namespace Qwitter.Crypto.Currency.Ethereum.Transfers.Models;

public class GetTransactionByHashResponse
{
    [JsonPropertyName("blockHash")]
    public string? BlockHash { get; set; }

    [JsonPropertyName("blockNumber")]
    public string? BlockNumber { get; set; }

    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("to")]
    public string? To { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("gas")]
    public string? Gas { get; set; }

    [JsonPropertyName("gasPrice")]
    public string? GasPrice { get; set; }
}
