using MassTransit;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Contract.CryptoTransfer.Events;
using Qwitter.Crypto.Contract.CryptoTransfer.Models;
using Qwitter.Crypto.Currency.Contract.Transfers;
using Qwitter.Crypto.Service.CryptoTransfer.Models;
using Qwitter.Crypto.Service.CryptoTransfer.Repositories;
using Qwitter.Crypto.Service.Wallet.Repositories;

namespace Qwitter.Crypto.Service.CryptoTransfer.Consumers;

public class ProcessCryptoTransferEventConsumer : IConsumer<ProcessCryptoTransferEvent>
{
    private readonly ICryptoTransferRepository _cryptoTransferRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventProducer _eventProducer;
    private readonly ILogger<ProcessCryptoTransferEventConsumer> _logger;

    public ProcessCryptoTransferEventConsumer(
        ICryptoTransferRepository cryptoTransferRepository,
        IWalletRepository walletRepository,
        IServiceProvider serviceProvider,
        IEventProducer eventProducer,
        ILogger<ProcessCryptoTransferEventConsumer> logger)
    {
        _cryptoTransferRepository = cryptoTransferRepository;
        _walletRepository = walletRepository;
        _serviceProvider = serviceProvider;
        _eventProducer = eventProducer;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessCryptoTransferEvent> context)
    {
        var transaction = await _cryptoTransferRepository.GetById(context.Message.TransactionId);

        if (transaction.Status == CryptoTransferStatus.Pending)
        {
            await InitiateTransaction(transaction);
            return;
        }

        if (transaction.Status == CryptoTransferStatus.Initiated)
        {
            await ProcessTransaction(transaction);
            return;
        }

        _logger.LogWarning("Transaction {TransactionId} is in an invalid state {Status}", transaction.TransactionId, transaction.Status);
    }

    public async Task InitiateTransaction(CryptoTransferEntity transaction)
    {
        var wallet = await _walletRepository.GetByAddress(transaction.SourceAddress);
        var transferService = _serviceProvider.GetRequiredKeyedService<ITransferService>(transaction.Currency);

        try
        {
            var transactionHash = await transferService.Transfer(wallet!.PrivateKey!, transaction.DestinationAddress, transaction.Amount);
            transaction.TransactionHash = transactionHash.TransactionHash;
            transaction.Status = CryptoTransferStatus.Initiated;

            await _cryptoTransferRepository.Update(transaction);
            await _eventProducer.Produce(new CryptoTransferStatusUpdatedEvent { TransactionId = transaction.TransactionId }, transaction.SubTopic);

            // TODO: Implement sceduling
            await Task.Delay(5 * 1000);
            await _eventProducer.Produce(new ProcessCryptoTransferEvent { TransactionId = transaction.TransactionId });
        }
        catch
        {
            transaction.Status = CryptoTransferStatus.Failed;
            await _cryptoTransferRepository.Update(transaction);

            await _eventProducer.Produce(new CryptoTransferStatusUpdatedEvent { TransactionId = transaction.TransactionId }, transaction.SubTopic);
        }
    }

    public async Task ProcessTransaction(CryptoTransferEntity transaction)
    {
        var transferService = _serviceProvider.GetRequiredKeyedService<ITransferService>(transaction.Currency);
        var transactionResponse = await transferService.GetTransactionByHash(transaction.TransactionHash!);

        if (transactionResponse == null)
        {
            _logger.LogWarning("Transaction {TransactionId} not found", transaction.TransactionId);

            transaction.Status = CryptoTransferStatus.Failed;
            await _cryptoTransferRepository.Update(transaction);

            return;
        }

        if (transactionResponse.BlockNumber != null || transactionResponse.BlockHash != null)
        {
            transaction.BlockNumber = transactionResponse.BlockNumber;
            transaction.BlockHash = transactionResponse.BlockHash;
            transaction.Status = CryptoTransferStatus.Completed;
            transaction.Fee = transactionResponse.Fee;
            await _cryptoTransferRepository.Update(transaction);

            await _eventProducer.Produce(new CryptoTransferStatusUpdatedEvent { TransactionId = transaction.TransactionId }, transaction.SubTopic);
            return;
        }

        await Task.Delay(5 * 1000);
        await _eventProducer.Produce(new ProcessCryptoTransferEvent { TransactionId = transaction.TransactionId });
    }
}