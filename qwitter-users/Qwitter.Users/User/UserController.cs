using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Auth.Services;
using Qwitter.Users.Contract.User;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase, IUserController
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserController(
        IMapper mapper,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    [HttpGet("get/{foo}/and/{bar}")]
    public Task<TestResponse> Get(string foo, string bar, string baz, string bom)
    {
        return Task.FromResult(new TestResponse
        {
            Message = $"Foo: {foo}, Bar: {bar}, Baz: {baz}, Bom: {bom}"
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<UserProfile> GetUser()
    {
        var user = await _userRepository.GetUserById(User.GetUserId());
        return _mapper.Map<UserProfile>(user!);
    }

    [HttpPost("posting/{foo}/path")]
    public Task<TestResponse> Post(string foo, string bar, string baz, TestRequest body)
    {
        return Task.FromResult(new TestResponse
        {
            Message = $"Foo: {foo}, Bar: {bar}, Baz: {baz}, Message: {body.Message}, Number: {body.Number}"
        });
    }
}