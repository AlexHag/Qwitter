using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Contract.Invoices.Models;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.Invoices.Models;
using Qwitter.Ledger.Invoices.Repositories;
using Qwitter.Ledger.Transactions.Services;
using Qwitter.Ledger.User.Repositories;

namespace Qwitter.Ledger.Invoices.Services;

public interface IInvoiceService
{
    Task<InvoiceEntity> CreateInvoice(CreateInvoiceRequest request);
    Task<InvoiceEntity> PayInvoice(PayInvoiceRequest request);
    Task<InvoiceEntity> GetInvoice(Guid invoiceId);
    Task<IEnumerable<InvoiceEntity>> GetUserInvoices(Guid userId, PaginationRequest request);
    Task<IEnumerable<InvoicePaymentEntity>> GetInvoicePayments(Guid invoiceId, PaginationRequest request);
}

public class InvoiceService : IInvoiceService
{
    private readonly IUserRepository _userRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInvoicePaymentRepository _invoicePaymentRepository;
    private readonly ITransactionService _transactionService;
    private readonly IEventProducer _eventProducer;

    public InvoiceService(
        IUserRepository userRepository,
        IInvoiceRepository invoiceRepository,
        IInvoicePaymentRepository invoicePaymentRepository,
        ITransactionService transactionService,
        IEventProducer eventProducer)
    {
        _userRepository = userRepository;
        _invoiceRepository = invoiceRepository;
        _invoicePaymentRepository = invoicePaymentRepository;
        _transactionService = transactionService;
        _eventProducer = eventProducer;
    }

    public async Task<InvoiceEntity> CreateInvoice(CreateInvoiceRequest request)
    {
        var user = await _userRepository.GetById(request.UserId) ?? throw new NotFoundApiException("User not found");

        if (user.UserState == UserState.Canceled)
        {
            throw new BadRequestApiException($"Cannot create invoice for canceled user {user.UserId}");
        }

        var invoice = new InvoiceEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Amount = request.Amount,
            AmountPayed = 0,
            Status = InvoiceStatus.Pending,
            Currency = request.Currency,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
        };

        await _invoiceRepository.Insert(invoice);

        return invoice;
    }

    public Task<InvoiceEntity> GetInvoice(Guid invoiceId)
    {
        var invoice = _invoiceRepository.GetById(invoiceId) ?? throw new NotFoundApiException("Invoice not found");

        return invoice!;
    }

    public Task<IEnumerable<InvoicePaymentEntity>> GetInvoicePayments(Guid invoiceId, PaginationRequest request)
    {
        var response = _invoicePaymentRepository.GetByInvoiceId(invoiceId, request);
        return response;
    }

    public Task<IEnumerable<InvoiceEntity>> GetUserInvoices(Guid userId, PaginationRequest request)
    {
        var invoices = _invoiceRepository.GetByUserId(userId, request);
        return invoices;
    }

    public async Task<InvoiceEntity> PayInvoice(PayInvoiceRequest request)
    {
        throw new NotImplementedException();
        // var user = await _userRepository.GetById(request.UserId) ?? throw new NotFoundApiException("User not found");

        // if (user.UserState != UserState.Verified)
        // {
        //     throw new BadRequestApiException($"Cannot pay invoice for unverified user {user.UserId}");
        // }

        // var invoice = await _invoiceRepository.GetById(request.InvoiceId) ?? throw new NotFoundApiException("Invoice not found");

        // if (invoice.Status == InvoiceStatus.Paid)
        // {
        //     throw new BadRequestApiException($"Invoice {invoice.Id} is already paid");
        // }

        // if (invoice.Status == InvoiceStatus.Cancelled)
        // {
        //     throw new BadRequestApiException($"Cannot pay canceled invoice {invoice.Id}");
        // }

        // var debitFundsRequest = new DebitFundsRequest
        // {
        //     // UserId = request.UserId,
        //     BankAccountId = request.BankAccountId.Value,
        //     Amount = request.Amount,
        //     // Currency = invoice.Currency,
        //     // Message = request.Message,
        // };

        // var transaction = await _transactionService.DebitFunds(debitFundsRequest);

        // var invoicePayment = new InvoicePaymentEntity
        // {
        //     Id = Guid.NewGuid(),
        //     InvoiceId = invoice.Id,
        //     TransactionId = transaction.Id,
        //     Amount = transaction.DestinationAmount.Value,
        //     Currency = transaction.DestinationCurrency,
        //     CreatedAt = DateTime.UtcNow
        // };

        // await _invoicePaymentRepository.Insert(invoicePayment);
        // invoice.AmountPayed += transaction.DestinationAmount.Value;

        // if (invoice.AmountPayed >= invoice.Amount)
        // {
        //     invoice.Status = InvoiceStatus.Paid;
        // }

        // await _invoiceRepository.Update(invoice);

        // if (invoice.AmountPayed > invoice.Amount)
        // {
        //     var overpayedEvent = new InvoiceOverpayedEvent
        //     {
        //         UserId = request.UserId,
        //         InvoiceId = invoice.Id,
        //         TransactionId = transaction.Id,
        //         InvoicePaymentId = invoicePayment.Id
        //     };

        //     await _eventProducer.Produce(overpayedEvent);
        // }

        // return invoice;
    }
}