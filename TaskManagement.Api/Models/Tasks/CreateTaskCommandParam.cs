using System.ComponentModel.DataAnnotations;
using TaskManagement.Common.CustomTypes;

namespace TaskManagement.Api.Models.Tasks
{
    public class CreateTaskCommandParam
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }
        public string Description { get; set; }
        public CustomTaskStatus Status { get; set; }
        public byte Priority { get; set; }
    }
}
