
using Qwitter.Core.Application.Exceptions;
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

        if (request.UserId != bankAccount.UserId)
        {
            _logger.LogWarning("Attempted unauthorized access to bank account crypt wallet by UserId {userId} to BankAccountId: {bankAccountId} - Currency: {currency}", request.UserId, request.BankAccountId, request.Currency);
            throw new NotFoundApiException($"BankAccountId: {request.BankAccountId} not found");
        }

        var existingWallet = await _bankAccountCryptoWalletRepository.GetByBankAccountIdAndCurrency(request.BankAccountId, request.Currency);

        if (existingWallet is not null)
        {
            return new BankCryptoWalletResponse
            {
                Address = existingWallet.Address,
                Currency = existingWallet.Currency
            };
        }

        _logger.LogInformation("Creating new wallet for UserId: {UserId} - BankAccountId: {BankAccountId} - Currency: {Currency}", request.UserId, request.BankAccountId, request.Currency);

        var newWallet = await _walletController.CreateWallet(new CreateWalletRequest
        {
            Currency = request.Currency,
            SubTopic = App.Name
        });

        var entity = new BankAccountCryptoWalletEntity
        {
            Id = Guid.NewGuid(),
            WalletId = newWallet.Id,
            UserId = request.UserId,
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
