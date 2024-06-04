
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Content.Comments.Repositories;
using Qwitter.Content.Contract.Comments;
using Qwitter.Content.Contract.Comments.Models;
using Qwitter.Core.Application.Authentication;

namespace Qwitter.Content.Comments;

[ApiController]
[Route("comments")]
public class CommentsController : ControllerBase, ICommentsController
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;

    public CommentsController(
        IMapper mapper,
        ICommentRepository commentRepository)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<CreateCommentResponse> CreateComment(CreateCommentRequest request)
    {
        var comment = await _commentRepository.InsertComment(User.GetUserId(), request.PostId, request.Content);
        return _mapper.Map<CreateCommentResponse>(comment);
    }
}