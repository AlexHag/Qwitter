
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;
using Qwitter.Crypto.Contract.Wallets;
using Qwitter.Crypto.Contract.Wallets.Models;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.Contract.Crypto.Models;
using Qwitter.Ledger.Crypto.Models;
using Qwitter.Ledger.Crypto.Repositories;

namespace Qwitter.Ledger.Crypto.Services;

public interface ICryptoService
{
    Task<BankCryptoWalletResponse> GetBankAccountCryptoWallet(GetBankAccountCryptoWalletRequest request);
}

public class CryptoService : ICryptoService
{
    private readonly ILogger<CryptoService> _logger;
    private readonly IBankAccountCryptoWalletRepository _bankAccountCryptoWalletRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IWalletController _walletController;

    public CryptoService(
        ILogger<CryptoService> logger,
        IBankAccountRepository bankAccountRepository,
        IBankAccountCryptoWalletRepository bankAccountCryptoWalletRepository,
        IWalletController walletController)
    {
        _logger = logger;
        _bankAccountRepository = bankAccountRepository;
        _bankAccountCryptoWalletRepository = bankAccountCryptoWalletRepository;
        _walletController = walletController;
    }

    public async Task<BankCryptoWalletResponse> GetBankAccountCryptoWallet(GetBankAccountCryptoWalletRequest request)
    {
        var bankAccount = await _bankAccountRepository.GetById(request.BankAccountId) ?? throw new NotFoundApiException($"BankAccountId: {request.BankAccountId} not found");

        var existingWallet = await _bankAccountCryptoWalletRepository.GetByBankAccountIdAndCurrency(request.BankAccountId, request.Currency);

        if (existingWallet is not null)
        {
            return new BankCryptoWalletResponse
            {
                Address = existingWallet.Address,
                Currency = existingWallet.Currency
            };
        }

        var newWallet = await _walletController.CreateWallet(new CreateWalletRequest
        {
            Currency = request.Currency,
            DestinationDomain = FundDomain.BankAccount,
            DestinationId = bankAccount.Id
        });

        var entity = new BankAccountCryptoWalletEntity
        {
            Id = Guid.NewGuid(),
            WalletId = newWallet.Id,
            BankAccountId = request.BankAccountId,
            Address = newWallet.Address,
            Currency = newWallet.Currency
        };

        await _bankAccountCryptoWalletRepository.Insert(entity);

        return new BankCryptoWalletResponse
        {
            Address = entity.Address,
            Currency = entity.Currency
        };
    }
}
