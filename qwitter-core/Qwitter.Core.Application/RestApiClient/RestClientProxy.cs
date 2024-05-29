using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace Qwitter.Core.Application.RestApiClient;

public class RestClientProxy<TController> : DispatchProxy
{
    private ILogger _logger = null!;
    private HttpClient _httpClient = null!;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null)
        {
            throw new ArgumentException($"{nameof(targetMethod)} is cannot be null");
        }

        var host = typeof(TController).GetCustomAttribute<ApiHostAttribute>();
        
        if (host is null)
        {
            throw new Exception($"{typeof(ApiHostAttribute).Name} is required for the controller interface {typeof(TController).Name}");
        }

        var httpMethodAttribute = targetMethod?.GetCustomAttribute<HttpMethodAttribute>();

        if (httpMethodAttribute is null)
        {
            throw new Exception($"{typeof(HttpMethodAttribute).Name} is required for the method {targetMethod?.Name}");
        }

        MethodInfo makeApiRequestMethod;

        if (typeof(Task).IsAssignableFrom(targetMethod?.ReturnType) && targetMethod.ReturnType.IsGenericType)
        {
            var apiReturnType = targetMethod.ReturnType.GenericTypeArguments[0];
            makeApiRequestMethod = typeof(ApiRequestMaker)
                .GetMethod(nameof(ApiRequestMaker.MakeApiRequest), BindingFlags.Public | BindingFlags.Static)!
                .MakeGenericMethod(apiReturnType);
        }
        else if (targetMethod?.ReturnType == typeof(Task))
        {
            makeApiRequestMethod = typeof(ApiRequestMaker)
                .GetMethod(nameof(ApiRequestMaker.MakeApiRequestVoid), BindingFlags.Public | BindingFlags.Static)!;
        }
        else
        {
            throw new InvalidOperationException($"Return type for method {targetMethod?.Name} must be {typeof(Task<>).Name}");
        }
        
        var paramArgs = args is null || args.Length == 0 ? null : new ParamInfo[args.Length];
        var parameterInfo = targetMethod.GetParameters();

        if (args is not null && args.Length != 0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                paramArgs![i] = new ParamInfo(args[i]!, parameterInfo[i]);
            }
        }

        var response = makeApiRequestMethod.Invoke(null, [_logger, _httpClient, httpMethodAttribute.HttpMethods.First(), host.Prefix, httpMethodAttribute.Template, paramArgs]);

        return response;
    }

    private void SetParameters(
        ILogger logger,
        HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public TController GetTransparentProxy(
        ILogger logger,
        HttpClient httpClient)
    {
        object proxy = Create<TController, RestClientProxy<TController>>() ?? throw new ArgumentNullException($"Failed to create RestClient proxy for {typeof(TController).Name}");
        ((RestClientProxy<TController>)proxy).SetParameters(logger, httpClient);
        return (TController)proxy;
    }
}