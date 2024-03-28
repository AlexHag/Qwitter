using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Core.Application.RestApiClient;

public static class ApiRequestMaker
{
    public static async Task<TReturnType> MakeApiRequest<TReturnType>(string httpMethod, string port, string template, params ParamInfo[] parameters)
    {
        var restRequestInfo = RestRequestInfo.Create(httpMethod, template, parameters);

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"http://localhost:{port}")
        };

        var httpRequestMessage = new HttpRequestMessage(restRequestInfo.HttpMethod, restRequestInfo.CreateUrl());
        
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
                var result = JsonSerializer.Deserialize<TReturnType>(contentString);

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