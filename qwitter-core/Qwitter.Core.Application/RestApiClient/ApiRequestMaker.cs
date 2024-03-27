using System.Text.Json;
using System.Text.RegularExpressions;

namespace Qwitter.Core.Application.RestApiClient;

public static class ApiRequestMaker
{
    public static async Task<object> MakeApiRequest(string httpMethod, string port, string template, Type returnType, params object[] parameters)
    {
        Console.WriteLine($"HttpMethod: {httpMethod}, Port: {port}, Template: {template}, ReturnType: {returnType.Name}, Parameters: {string.Join(", ", parameters)}");

        if (httpMethod == "GET")
        {
            return await MakeGetRequest(port, template, returnType, parameters);
        }
        else if (httpMethod == "POST")
        {
            return await MakePostRequest(port, template, returnType, parameters);
        }
        else
        {
            throw new NotImplementedException("Only GET and POST methods are supported.");
        }
    }

    public static async Task<object> MakeGetRequest(string port, string template, Type returnType, params object[] parameters)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"http://localhost:{port}")
        };

        if (!parameters.All(p => p.GetType() == typeof(string)))
        {
            throw new ArgumentException("All parameters must be strings when making a GET request");
        }

        var query = CreateGetQuery(template, parameters.Select(p => p.ToString()!).ToArray());

        using var response = await httpClient.GetAsync(query);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var result = JsonSerializer.Deserialize(content, returnType);

            return result;
        }
        else
        {
            Console.WriteLine($"Failed to make GET request: {response.StatusCode}");
            throw new Exception($"Failed to make GET request, status code: {response.StatusCode}");
        }
    }

    public static string CreateGetQuery(string template, params string[] args)
    {
        string pattern = @"\{[^}]+\}";
        int argIndex = 0;
        string url = Regex.Replace(template, pattern, match =>
        {
            if (argIndex >= args.Length) return match.Value;
            string replacement = args[argIndex++];
            return replacement;
        });

        return url;
    }

    public static async Task<object> MakePostRequest(string port, string template, Type returnType, params object[] parameters)
    {
        throw new NotImplementedException();
    }
}