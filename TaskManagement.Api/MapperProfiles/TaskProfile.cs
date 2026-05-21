using AutoMapper;
using TaskManagement.Api.Models.Tasks;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Application.Features.Tasks.Queries.GetTaskById;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.MapperProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskEntity, GetTaskByIdQueryResponse>()
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.ToString()));

            CreateMap<CreateTaskCommandParam, CreateTaskCommand>();
            CreateMap<UpdateTaskStatusCommandParam, UpdateTaskStatusCommand>()
                .ForMember(d => d.EntityId, m => m.MapFrom(s => s.Id));

            CreateMap<UpdateTaskStatusCommand, TaskEntity>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.EntityId));

        }
    }
}
