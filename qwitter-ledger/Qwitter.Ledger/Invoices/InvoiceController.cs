using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Contract.Invoices;
using Qwitter.Ledger.Contract.Invoices.Models;
using Qwitter.Ledger.Invoices.Services;

namespace Qwitter.Ledger.Invoices;

[ApiController]
[Route("invoice")]
public class InvoiceController : ControllerBase, IInvoiceController
{
    private readonly IMapper _mapper;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(
        IMapper mapper,
        IInvoiceService invoiceService)
    {
        _mapper = mapper;
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request)
    {
        var response = await _invoiceService.CreateInvoice(request);
        return _mapper.Map<InvoiceResponse>(response);
    }

    [HttpPut("pay")]
    public async Task<InvoiceResponse> PayInvoice(PayInvoiceRequest request)
    {
        var response = await _invoiceService.PayInvoice(request);
        return _mapper.Map<InvoiceResponse>(response);
    }

    [HttpGet("{invoiceId}")]
    public async Task<InvoiceResponse> GetInvoice(Guid invoiceId)
    {
        var response = await _invoiceService.GetInvoice(invoiceId);
        return _mapper.Map<InvoiceResponse>(response);
    }

    [HttpPost("user/{userId}")]
    public async Task<IEnumerable<InvoiceResponse>> GetUserInvoices(Guid userId, PaginationRequest request)
    {
        var response = await _invoiceService.GetUserInvoices(userId, request);
        return _mapper.Map<IEnumerable<InvoiceResponse>>(response);
    }

    [HttpPost("{invoiceId}/payments")]
    public async Task<IEnumerable<InvoicePaymentResponse>> GetInvoicePayments(Guid invoiceId, PaginationRequest request)
    {
        var response = await _invoiceService.GetInvoicePayments(invoiceId, request);
        return response.Select(_mapper.Map<InvoicePaymentResponse>);
    }
}