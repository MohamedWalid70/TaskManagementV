namespace TaskManagement.Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryResponse 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public byte Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
