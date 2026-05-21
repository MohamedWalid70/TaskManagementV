using System.ComponentModel.DataAnnotations;
using TaskManagement.Common.CustomTypes;

namespace TaskManagement.Api.Models.Tasks
{
    public class UpdateTaskStatusCommandParam
    {
        [Required]
        public Guid Id { get; set; }
        public CustomTaskStatus Status { get; set; }
    }

}
