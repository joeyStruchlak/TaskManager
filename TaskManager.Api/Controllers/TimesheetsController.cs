using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Commands.Timesheets;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TimesheetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Roles = "Worker")]
        public async Task<IActionResult> Submit([FromBody] SubmitTimesheetCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Submit), new {id}, null);
        }
    }
}
