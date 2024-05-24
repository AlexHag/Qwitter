using MassTransit;
using Qwitter.Ledger.Contract.Invoices.Models;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.Invoices.Repositories;
using Qwitter.Ledger.Transactions.Repositories;
using Qwitter.Ledger.Transactions.Services;

namespace Qwitter.Ledger.Invoices.Consumers;

public class InvoiceOverpayedConsumer : IConsumer<InvoiceOverpayedEvent>
{
    private readonly ILogger<InvoiceOverpayedConsumer> _logger;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ITransactionService _transactionService;
    private readonly ITransactionRepository _transactionRepository;

    public InvoiceOverpayedConsumer(
        ILogger<InvoiceOverpayedConsumer> logger,
        IInvoiceRepository invoiceRepository,
        ITransactionService transactionService,
        ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _invoiceRepository = invoiceRepository;
        _transactionService = transactionService;
        _transactionRepository = transactionRepository;
    }

    public async Task Consume(ConsumeContext<InvoiceOverpayedEvent> context)
    {
        var invoice = await _invoiceRepository.GetById(context.Message.InvoiceId);

        if (invoice is null)
        {
            _logger.LogWarning("Invoice with id {InvoiceId} not found", context.Message.InvoiceId);
            throw new Exception($"Invoice with id {context.Message.InvoiceId} not found");
        }

        var transaction = await _transactionRepository.GetById(context.Message.TransactionId);

        if (transaction is null)
        {
            _logger.LogWarning("Transaction with id {TransactionId} not found", context.Message.TransactionId);
            throw new Exception($"Transaction with id {context.Message.TransactionId} not found");
        }

        var amountOverPayed = invoice.AmountPayed - invoice.Amount;

        var creditFundsRequest = new CreditFundsRequest
        {
            UserId = context.Message.UserId,
            BankAccountId = transaction.BankAccountId,
            Amount = amountOverPayed,
            Currency = invoice.Currency,
            Message = "Overpayed invoice refund"
        };

        await _transactionService.CreditFunds(creditFundsRequest);
    }
}