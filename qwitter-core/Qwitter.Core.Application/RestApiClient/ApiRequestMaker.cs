using System.Text;
using System.Text.Json;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Core.Application.RestApiClient;

public static class ApiRequestMaker
{
    private static readonly JsonSerializerOptions s_readOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task<TReturnType> MakeApiRequest<TReturnType>(HttpClient httpClient, string httpMethod, string prefix, string template, params ParamInfo[] parameters)
    {
        var restRequestInfo = RestRequestInfo.Create(httpMethod, template, parameters);

        var requestUri = string.IsNullOrEmpty(prefix) ? restRequestInfo.CreateUrl() : $"{prefix}/{restRequestInfo.CreateUrl()}";

        var httpRequestMessage = new HttpRequestMessage(restRequestInfo.HttpMethod, requestUri);
        
        if (restRequestInfo.Body is not null)
        {
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(restRequestInfo.Body), Encoding.UTF8, "application/json");
        }

        var response = await httpClient.SendAsync(httpRequestMessage);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TReturnType>(contentString, s_readOptions);

                return result!;
            }
            catch (JsonException)
            {
                throw new InternalServerErrorApiException(await response.Content.ReadAsStringAsync());
            }
        }
        else
        {
            var contentString = await response.Content.ReadAsStringAsync();

            // TODO: Add more information to exceptions
            // TOOD: Implement logging
            throw response.StatusCode switch
            {
                System.Net.HttpStatusCode.NotFound => new NotFoundApiException(contentString),
                System.Net.HttpStatusCode.BadRequest => new BadRequestApiException(contentString),
                System.Net.HttpStatusCode.InternalServerError => new InternalServerErrorApiException(contentString),
                _ => new RestApiException(contentString, response.StatusCode),
            };
        }
    }
}