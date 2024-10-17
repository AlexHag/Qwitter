
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Exchange.Contract.FxRate;
using Qwitter.Exchange.Contract.FxRate.Models;
using Qwitter.Exchange.Service.Rate.Models;
using Qwitter.Exchange.Service.Rate.Repositories;

namespace Qwitter.Exchange.Service.FxRate;

[ApiController]
[Route("exchange-rate")]
public class FxRateService : ControllerBase, IFxRateService
{
    private readonly IFxRateRepository _fxRateRepository;
    private readonly IMapper _mapper;

    public FxRateService(
        IFxRateRepository fxRateRepository,
        IMapper mapper)
    {
        _fxRateRepository = fxRateRepository;
        _mapper = mapper;
    }

    [HttpGet("{sourceCurrency}/{destinationCurrency}")]
    public async Task<FxRateResponse> GetFxRate(string sourceCurrency, string destinationCurrency)
    {
        var fxRate = await _fxRateRepository.GetLatestByCurrencyPair(sourceCurrency, destinationCurrency);
        return _mapper.Map<FxRateResponse>(fxRate);
    }

    [HttpPut("update")]
    public async Task<FxRateResponse> UpdateFxRate(UpdateFxRateRequest request)
    {
        var fxRate = new FxRateEntity
        {
            FxRateId = Guid.NewGuid(),
            SourceCurrency = request.SourceCurrency,
            DestinationCurrency = request.DestinationCurrency,
            Rate = request.Rate,
            Created = DateTime.UtcNow
        };

        await _fxRateRepository.Insert(fxRate);
        return _mapper.Map<FxRateResponse>(fxRate);
    }
}