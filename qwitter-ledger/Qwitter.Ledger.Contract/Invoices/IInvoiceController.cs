using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.Invoices.Models;

namespace Qwitter.Ledger.Contract.Invoices;

[ApiHost(Host.Port, "invoice")]
public interface IInvoiceController
{
    [HttpPost]
    Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request);
    [HttpPut("pay")]
    Task<InvoiceResponse> PayInvoice(PayInvoiceRequest request);
    [HttpGet("{invoiceId}")]
    Task<InvoiceResponse> GetInvoice(Guid invoiceId);
    [HttpPost("user/{userId}")]
    Task<IEnumerable<InvoiceResponse>> GetUserInvoices(Guid userId, PaginationRequest request);
    [HttpPost("{incoiveId}/payments")]
    Task<IEnumerable<InvoicePaymentResponse>> GetInvoicePayments(Guid invoiceId, PaginationRequest request);
}