using System.Text.Json.Serialization;

namespace Qwitter.Crypto.Currency.Ethereum.Wallet.Models;

public class AssetTransfer
{
    [JsonPropertyName("blockNum")]
    public string? BlockNum { get; set; }
    [JsonPropertyName("uniqueId")]
    public string? UniqueId { get; set; }
    [JsonPropertyName("hash")]
    public string? Hash { get; set; }
    [JsonPropertyName("from")]
    public string? From { get; set; }
    [JsonPropertyName("to")]
    public string? To { get; set; }
    [JsonPropertyName("value")]
    public decimal Value { get; set; }
    [JsonPropertyName("asset")]
    public string? Asset { get; set; }
    [JsonPropertyName("category")]
    public string? Category { get; set; }
}
