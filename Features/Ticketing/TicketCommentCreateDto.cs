using FluentValidation;
using ERP.Shared.Ticketing;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketCommentCreateDto
{
    public string Message { get; set; }
    public Guid? ReplayId { get; set; }
    public IEnumerable<IFormFile>? Files { get; set; }
}

public class TicketCommentCreateDtoValidator : AbstractValidator<TicketCommentCreateDto>
{
    public TicketCommentCreateDtoValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(TicketingCommentConstants.MinimumMessageLength)
            .NotNull();
    }
}