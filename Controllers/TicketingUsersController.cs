using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Controllers;

[Route("api/ticketing/{ticketId:guid}/users")]
public class TicketingUsersController : AuthApiControllerBase
{
    private readonly TicketingUserService _ticketingUserService;

    public TicketingUsersController(TicketingUserService ticketingUserService)
    {
        _ticketingUserService = ticketingUserService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUsers(Guid ticketId, [FromBody] TicketUserAddDto request)
    {
        await _ticketingUserService.AddUsers(request, ticketId);
        return Ok();
    }
    
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> RemoveUsers(Guid ticketId, Guid userId)
    {
        await _ticketingUserService.RemoveUser(ticketId, userId);
        return NoContent();
    }
}