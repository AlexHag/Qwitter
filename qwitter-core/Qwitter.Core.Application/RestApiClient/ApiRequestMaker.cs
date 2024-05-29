using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Core.Application.RestApiClient;

public static class ApiRequestMaker
{
    private static readonly JsonSerializerOptions s_readOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task MakeApiRequestVoid(ILogger logger, HttpClient httpClient, string httpMethod, string prefix, string template, params ParamInfo[] parameters)
    {
        var restRequestInfo = RestRequestInfo.Create(logger, httpMethod, template, parameters);

        var requestUri = string.IsNullOrEmpty(prefix) ? restRequestInfo.CreateUrl() : $"{prefix}/{restRequestInfo.CreateUrl()}";

        var httpRequestMessage = new HttpRequestMessage(restRequestInfo.HttpMethod, requestUri);

        if (restRequestInfo.Body is not null)
        {
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(restRequestInfo.Body), Encoding.UTF8, "application/json");
        }

        logger.LogInformation("Making {httpMethod} request to {requestUri}", restRequestInfo.HttpMethod, requestUri);

        var response = await httpClient.SendAsync(httpRequestMessage);

        await HandleUnsuccessfulResponse(logger, response, requestUri);
    }

    public static async Task<TReturnType> MakeApiRequest<TReturnType>(ILogger logger, HttpClient httpClient, string httpMethod, string prefix, string template, params ParamInfo[] parameters)
    {
        var restRequestInfo = RestRequestInfo.Create(logger, httpMethod, template, parameters);

        var requestUri = string.IsNullOrEmpty(prefix) ? restRequestInfo.CreateUrl() : $"{prefix}/{restRequestInfo.CreateUrl()}";

        var httpRequestMessage = new HttpRequestMessage(restRequestInfo.HttpMethod, requestUri);

        if (restRequestInfo.Body is not null)
        {
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(restRequestInfo.Body), Encoding.UTF8, "application/json");
        }

        logger.LogInformation("Making {httpMethod} request to {requestUri}", restRequestInfo.HttpMethod, requestUri);

        var response = await httpClient.SendAsync(httpRequestMessage);
        
        await HandleUnsuccessfulResponse(logger, response, requestUri);

        var contentString = await response.Content.ReadAsStringAsync();

        if (typeof(TReturnType).IsValueType || typeof(TReturnType) == typeof(string))
        {
            return (TReturnType)Convert.ChangeType(contentString, typeof(TReturnType));
        }
        else
        {
            try
            {
                var result = JsonSerializer.Deserialize<TReturnType>(contentString, s_readOptions);
                return result!;
            }
            catch (JsonException)
            {
                logger.LogError("Failed to deserialize response from {requestUri}", requestUri);
                throw new InternalServerErrorApiException(await response.Content.ReadAsStringAsync());
            }
        }
    }

    private static async Task HandleUnsuccessfulResponse(ILogger logger, HttpResponseMessage response, string requestUri)
    {
        if (response.IsSuccessStatusCode) return;
        var contentString = await response.Content.ReadAsStringAsync();

        logger.LogError("Failed to make request to {requestUri}. Response: {response}", requestUri, contentString);

        throw response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => new NotFoundApiException(contentString),
            System.Net.HttpStatusCode.BadRequest => new BadRequestApiException(contentString),
            System.Net.HttpStatusCode.InternalServerError => new InternalServerErrorApiException(contentString),
            _ => new RestApiException(contentString, response.StatusCode),
        };
    }
}