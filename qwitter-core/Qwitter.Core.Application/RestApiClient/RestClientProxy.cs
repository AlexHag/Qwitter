using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Qwitter.Core.Application.RestApiClient;

public class RestClientProxy<TController> : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null)
        {
            throw new ArgumentException($"{nameof(targetMethod)} is cannot be null");
        }

        var host = typeof(TController).GetCustomAttribute<ApiHostAttribute>();
        
        if (host == null)
        {
            throw new Exception($"{typeof(ApiHostAttribute).Name} is required for the controller interface {typeof(TController).Name}");
        }

        var httpMethodAttribute = targetMethod?.GetCustomAttribute<HttpMethodAttribute>();

        if (httpMethodAttribute == null)
        {
            throw new Exception($"{typeof(HttpMethodAttribute).Name} is required for the method {targetMethod?.Name}");
        }

        if (targetMethod!.ReturnType.GetGenericTypeDefinition() != typeof(Task<>))
        {
            throw new InvalidOperationException($"Return type for method {targetMethod.Name} must be {typeof(Task<>).Name}");
        }

        var apiReturnType = targetMethod.ReturnType.GenericTypeArguments[0];

        MethodInfo makeApiRequestMethod = typeof(ApiRequestMaker)
            .GetMethod(nameof(ApiRequestMaker.MakeApiRequest), BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(apiReturnType);
        
        var paramArgs = args is null || args.Length == 0 ? null : new ParamInfo[args.Length];
        var parameterInfo = targetMethod.GetParameters();

        if (args is not null && args.Length != 0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                paramArgs![i] = new ParamInfo(args[i]!, parameterInfo[i]);
            }
        }

        var response = makeApiRequestMethod.Invoke(null, [httpMethodAttribute.HttpMethods.First(), host.Port, host.Prefix, httpMethodAttribute.Template, paramArgs]);

        return response;
    }

    public TController GetTransparentProxy()
    {
        return Create<TController, RestClientProxy<TController>>();
    }
}