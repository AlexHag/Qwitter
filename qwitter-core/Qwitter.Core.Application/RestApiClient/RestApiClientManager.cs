using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Qwitter.Core.Application.RestApiClient;

public interface IRestApiClientManager
{
    TController GetApiClientFor<TController>(string hostPath);
}

public class RestApiClientManager : IRestApiClientManager
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public RestApiClientManager(
        IConfiguration configuration,
        ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public TController GetApiClientFor<TController>(string hostPath)
    {
        var httpClientHandler = new HttpClientHandler().ConfigureMtls(_configuration);

        var httpClient = new HttpClient(httpClientHandler)
        {
            BaseAddress = new Uri(hostPath),
        };

        var client = new RestClientProxy<TController>().GetTransparentProxy(_logger, httpClient);

        if (client is null)
        {
            throw new Exception($"Failed to create proxy for {typeof(TController).Name}");
        }

        return client;
    }
}