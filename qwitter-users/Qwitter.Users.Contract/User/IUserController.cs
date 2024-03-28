using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.Contract.User;

[ApiHost(Host.Port, "user")]
public interface IUserController
{
    [HttpGet("me")]
    Task<UserProfile> GetUser();

    [HttpGet("get/{foo}/and/{bar}")]
    Task<TestResponse> Get(string foo, string bar, string baz, string bom);

    [HttpPost("posting/{foo}/path")]
    Task<TestResponse> Post(string foo, string bar, string baz, TestRequest body);
}
