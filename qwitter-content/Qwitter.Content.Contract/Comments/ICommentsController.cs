
using Microsoft.AspNetCore.Mvc;
using Qwitter.Content.Contract.Comments.Models;
using Qwitter.Core.Application.RestApiClient;

namespace Qwitter.Content.Contract.Comments;

[ApiHost("5003", "comments")]
public interface ICommentsController
{
    [HttpPost]
    Task<CreateCommentResponse> CreateComment(CreateCommentRequest request);
}