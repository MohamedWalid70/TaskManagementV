using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models.Tasks;
using TaskManagement.Application.Features.Common;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Application.Features.Tasks.Queries.GetAllPaginatedTasks;
using TaskManagement.Application.Features.Tasks.Queries.GetPersonalizedPaginatedTasks;
using TaskManagement.Application.Features.Tasks.Queries.GetTaskById;
using TaskManagement.Common.GenericResponses;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("api/v1/tasks")]
    [ApiController]
    public class TaskController(ISender sender, IMapper mapper) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly IMapper _mapper = mapper;

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IAsyncEnumerable<GetTaskByIdQueryResponse> GetPersonalizedPaginatedTasks(int pageNumber = 1, int pageSize = 5)
        {
            return _sender.CreateStream(new GetPersonalizedPaginatedTasksQuery(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IAsyncEnumerable<GetTaskByIdQueryResponse> GetAllPaginatedTasks(int pageNumber = 1, int pageSize = 5)
        {
            return _sender.CreateStream(new GetAllPaginatedTasksQuery(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GenericResult<GetTaskByIdQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenericResult<GetTaskByIdQueryResponse>>> GetTaskDataById(Guid id)
        {
            var taskData = await _sender.Send(new GetTaskByIdQuery(id));

            var result = new GenericResult<GetTaskByIdQueryResponse> { StatusCode = StatusCodes.Status200OK, Data = taskData };

            return Ok(result);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<Guid>), StatusCodes.Status201Created)]
        public async Task<ActionResult<IdResponse<Guid>>> CreateTask([FromBody] CreateTaskCommandParam createTaskCommandParam)
        {
            var createTaskCommand = _mapper.Map<CreateTaskCommand>(createTaskCommandParam);

            var newlyCreatedId = await _sender.Send(createTaskCommand);

            return CreatedAtAction(nameof(GetTaskDataById), new {id = newlyCreatedId.Id}, newlyCreatedId);
        
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusCommandParam updateTaskStatusCommandParam)
        {
            var updateTaskStatusCommand = _mapper.Map<UpdateTaskStatusCommand>(updateTaskStatusCommandParam);

            await _sender.Send(updateTaskStatusCommand); 

            return Ok();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            await _sender.Send(new DeleteTaskCommand { EntityId = id });
            return NoContent();

        }
    }
}
