using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Qwitter.Core.Application.RestApiClient;

public static class RestClientFactory
{
    public static TController CreateRestClient<TController>() where TController : class
    {
        var assemblyName = new AssemblyName("RestClientAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("RestClientModule");
        var typeBuilder = moduleBuilder.DefineType($"{typeof(TController).Name}-TheRestClientClass");

        var apiHostAttribute = typeof(TController).GetCustomAttribute<ApiHostAttribute>();
        
        if (apiHostAttribute == null)
        {
            throw new Exception("ApiHostAttribute is required for the controller interface");
        }

        var port = apiHostAttribute.Port;

        typeBuilder.AddInterfaceImplementation(typeof(TController));

        foreach (var method in typeof(TController).GetMethods())
        {
            var httpMethodAttribute = method.GetCustomAttribute<HttpMethodAttribute>();

            if (httpMethodAttribute == null)
            {
                throw new Exception("HttpMethodAttribute is required for the method");
            }

            var parameters = method.GetParameters();
            var methodBuilder = typeBuilder.DefineMethod(method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.CallingConvention,
                method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            
            var il = methodBuilder.GetILGenerator();

            if (method.ReturnType.GetGenericTypeDefinition() != typeof(Task<>))
            {
                throw new InvalidOperationException($"Return type for method {method.Name} must be Task<>");
            }

            if (method.ReturnType.GenericTypeArguments[0].GetGenericTypeDefinition() != typeof(ActionResult<>))
            {
                throw new InvalidOperationException($"Return type for method {method.Name} must be ActionResult<>");
            }

            var apiReturnType = method.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0];

            MethodInfo makeApiRequestMethod = typeof(ApiRequestMaker)
                .GetMethod(nameof(ApiRequestMaker.MakeApiRequest), BindingFlags.Public | BindingFlags.Static)!
                .MakeGenericMethod(apiReturnType);

            il.Emit(OpCodes.Ldstr, httpMethodAttribute.HttpMethods.First());
            il.Emit(OpCodes.Ldstr, port);
            il.Emit(OpCodes.Ldstr, httpMethodAttribute.Template);

            // Create an array of objects to pass as parameters to the MakeApiRequest method
            il.Emit(OpCodes.Ldc_I4, parameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldarg, i + 1); // here is where the parameters are actualy places on the stack
                if (parameters[i].ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Box, parameters[i].ParameterType);
                }
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Call, makeApiRequestMethod);

            if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                il.Emit(OpCodes.Pop);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, method.ReturnType);
            }

            il.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, method);
        }

        var type = typeBuilder.CreateType();
        var clientInstance = Activator.CreateInstance(type);
        return clientInstance as TController;
    }
}