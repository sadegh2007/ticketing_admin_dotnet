using ERP.Shared.Commons;
using ERP.Shared.Ticketing;
using Microsoft.AspNetCore.Mvc;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using TicketCreateDto = ERP.Ticketing.HttpApi.Features.Ticketing.TicketCreateDto;

namespace ERP.Ticketing.HttpApi.Controllers;

public class TicketingController : AuthApiControllerBase
{
    private readonly TicketingService _ticketingService;

    public TicketingController(TicketingService ticketingService)
    {
        _ticketingService = ticketingService;
    }
    
    [HttpPost("list", Name = "ticket_list")]
    public async Task<PagedResult<ListTicketDto>> ListOfTickets([FromBody] TicketListFilter filters)
    {
        return await _ticketingService.ListOfTicket(filters);
    }
    
    [HttpGet("{ticketId:guid}", Name = "get_ticket_by_id")]
    public async Task<IActionResult> GetTicket(Guid ticketId)
    {
        var ticket = await _ticketingService.GetSingleTicket(ticketId);
        return Ok(ticket);
    }

    [HttpPost(Name = "create_ticket")]
    public async Task<IActionResult> CreateTicket([FromForm] TicketCreateDto request)
    {
        var ticket = await _ticketingService.CreateTicket(request);
        return Ok(ticket);
    }
    
    [HttpPut("{ticketId:guid}", Name = "update_list")]
    public async Task<IActionResult> UpdateTicket([FromBody] TicketUpdateDto request, Guid ticketId)
    {
        await _ticketingService.UpdateTicket(request, ticketId);
        return NoContent();
    }
    
    [HttpDelete("{ticketId:guid}", Name = "delete_ticket")]
    public async Task<IActionResult> DeleteTicket(Guid ticketId)
    {
        await _ticketingService.DeleteTicket(ticketId);
        return NoContent();
    }
    
    [HttpPost("{ticketId:guid}/move/{departmentId:guid}",Name = "move_ticket_department")]
    public async Task<IActionResult> MoveToDepartment(Guid ticketId, Guid departmentId)
    {
        await _ticketingService.MoveToDepartment(ticketId, departmentId);
        return NoContent();
    }
    
    [HttpPost("{ticketId:guid}/categories", Name = "assign_category_to_ticket")]
    public async Task<IActionResult> AssignCategoriesToTicket(Guid ticketId, AssignTicketCategoriesDto request)
    {
        await _ticketingService.AssignCategories(ticketId, request);
        return NoContent();
    }
    
    [HttpGet("{ticketId:guid}/histories", Name = "single_ticket_histories")]
    public async Task<IActionResult> Histories(Guid ticketId)
    {
        var histories = await _ticketingService.GetHistories(ticketId);
        return Ok(histories);
    }
    
    [HttpPut("{ticketId:guid}/change_status", Name = "change_ticket_status")]
    public async Task<IActionResult> ChangeStatus(Guid ticketId, TicketChangeStatusRequest request)
    {
        var result = await _ticketingService.ChangeStatus(ticketId, request);
        return Accepted(result);
    }
}