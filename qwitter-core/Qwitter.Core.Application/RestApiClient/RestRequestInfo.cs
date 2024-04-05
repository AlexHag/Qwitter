using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Qwitter.Core.Application.RestApiClient;

public class RestRequestInfo
{
    public required HttpMethod HttpMethod { get; set; }
    public IDictionary<string, string>? UrlParams { get; set; }
    public object? Body { get; set; }
    public string Template { get; set; } = string.Empty;

    public string CreateUrl()
    {
        if (UrlParams is null) return Template;

        var baseUrl = Template;
        var queryParams = new List<string>();

        foreach (var param in UrlParams)
        {
            var pattern = $"\\{{({param.Key})\\}}";

            if (Regex.IsMatch(Template, pattern))
            {
                baseUrl = Regex.Replace(baseUrl, pattern, param.Value);
            }
            else
            {
                queryParams.Add($"{param.Key}={param.Value}");
            }
        }

        if (queryParams.Any())
        {
            baseUrl += "?" + string.Join("&", queryParams);
        }

        return baseUrl;
    }

    // TOOD: Use interface and factory method instead of static
    public static RestRequestInfo Create(ILogger logger, string httpMethod, string template, params ParamInfo?[]? parameters)
    {
        var restRequestInfo = new RestRequestInfo
        {
            Template = template,
            HttpMethod = new HttpMethod(httpMethod)
        };

        if (parameters is null) return restRequestInfo;

        foreach (var parameter in parameters)
        {
            if (parameter is null)
            {
                logger.LogInformation("Warning one parameter is null but not all of them may be null. Method: {httpMethod}, Template: {template}", httpMethod, template);
                continue;
            }

            if (parameter.Value.GetType().IsClass && parameter.Value is not string)
            {
                if (restRequestInfo.Body is not null)
                    throw new ArgumentException("Body parameter already set, only one body can be set per request");

                restRequestInfo.Body = parameter.Value;
                continue;
            }

            if (parameter.Value is not string)
                throw new ArgumentException($"Parameter {parameter.Info?.Name} must be a string");
            
            restRequestInfo.UrlParams ??= new Dictionary<string, string>();

            if (parameter.Info is null || parameter.Info.Name is null)
            {
                throw new ArgumentException("Could not get parameter name from parameter info object");
            }

            restRequestInfo.UrlParams.Add(parameter.Info.Name, parameter.Value.ToString()!);
        }

        return restRequestInfo;
    }
}