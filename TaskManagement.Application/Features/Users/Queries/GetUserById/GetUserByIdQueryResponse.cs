namespace TaskManagement.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
