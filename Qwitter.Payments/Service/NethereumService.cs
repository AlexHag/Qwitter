using NBitcoin;
using Nethereum.HdWallet;
using Nethereum.Web3;

namespace Qwitter.Payments.Service;

public interface INethereumService
{
    string GeneratePrivateMnemonic();
    string GetAddressFromPrivateMnemonic(string privateMnemonic);
    Task<decimal> CheckBalance(string address);
    Task SendTransaction(string privateMnemonic, string toAddress, decimal amount);
}

public class NethereumService : INethereumService
{
    private readonly string _url;

    public NethereumService(string url)
    {
        _url = url;
    }

    public string GeneratePrivateMnemonic()
    {
        Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.TwentyFour);
        return mnemo.ToString();
    }

    public string GetAddressFromPrivateMnemonic(string privateMnemonic)
    {
        return new Wallet(privateMnemonic, "").GetAccount(0).Address;
    }

    public async Task<decimal> CheckBalance(string address)
    {
        var web3 = new Web3(_url);
        var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
        return Web3.Convert.FromWei(balance.Value);
    }

    public async Task SendTransaction(string privateMnemonic, string toAddress, decimal amount)
    {
        var account = new Wallet(privateMnemonic, "").GetAccount(0);
        var web3 = new Web3(account, _url);

        var transaction = await web3.Eth.GetEtherTransferService()
            .TransferEtherAndWaitForReceiptAsync(toAddress, amount);
    }
}