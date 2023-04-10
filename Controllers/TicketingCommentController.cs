using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using Microsoft.AspNetCore.Mvc;
using TicketCommentCreateDto = ERP.Ticketing.HttpApi.Features.Ticketing.TicketCommentCreateDto;

namespace ERP.Ticketing.HttpApi.Controllers;

[Route("api/ticketing/{ticketId:guid}/comments")]
public class TicketingCommentController: AuthApiControllerBase
{
    private readonly TicketingService _ticketingService;
    private readonly TicketingCommentService _commentService;

    public TicketingCommentController(TicketingService ticketingService, 
        TicketingCommentService commentService)
    {
        _ticketingService = ticketingService;
        _commentService = commentService;
    }

    [HttpPost(Name = "create_ticketing_comment")]
    public async Task<ActionResult<TicketCommentDto>> CreateComment(Guid ticketId, [FromForm] TicketCommentCreateDto request)
    {
        var comment = await _commentService.CreateComment(ticketId, request);

        return CreatedAtRoute("create_ticketing_comment", comment);
    }
    
    [HttpDelete("{commentId:guid}", Name = "delete_ticketing_comment")]
    public async Task<IActionResult> DeleteComment(Guid ticketId, Guid commentId)
    {
        await _commentService.DeleteComment(ticketId, commentId);
        return NoContent();
    }
    
    [HttpGet("{commentId:guid}/viewers", Name = "get_ticket_comment_viewers")]
    public async Task<IActionResult> CommentViewers(Guid ticketId, Guid commentId)
    {
        var viewers = await _commentService.CommentViewers(ticketId, commentId);

        return Ok(viewers);
    }
}