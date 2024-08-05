using MassTransit;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Contract.Wallets.Events;
using Qwitter.Ledger.FundAllocations.Models;
using Qwitter.Ledger.FundAllocations.Models.Enums;
using Qwitter.Ledger.FundAllocations.Repositories;
using Qwitter.Ledger.Transactions.Services;

namespace Qwitter.Ledger.Crypto.Consumers;

[MessageSuffix(FundDestination.BankAccount)]
public class BankAccountCryptoDepositConsumer : IConsumer<CryptoDepositEvent>
{
    private readonly IFundAllocationRepository _fundAllocationRepository;
    private readonly ITransactionService _transactionService;

    public BankAccountCryptoDepositConsumer(
        IFundAllocationRepository fundAllocationRepository,
        ITransactionService transactionService)
    {
        _fundAllocationRepository = fundAllocationRepository;
        _transactionService = transactionService;
    }

    public async Task Consume(ConsumeContext<CryptoDepositEvent> context)
    {
        var allocation = new FundAllocationEntity
        {
            Id = Guid.NewGuid(),
            SourceId = context.Message.WalletId,
            SourceDomain = FundSource.Crypto,
            SourceAmount = context.Message.Amount,
            SourceCurrency = context.Message.Currency,
            DestinationId = context.Message.DestinationId,
            DestinationDomain = FundDestination.BankAccount,
            Status = FundAllocationStatus.Pending,
        };

        await _fundAllocationRepository.Insert(allocation);
    }
}