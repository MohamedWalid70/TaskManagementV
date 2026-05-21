namespace TaskManagement.Application.Features.Auth.Queries.Common
{
    public class AuthQueryResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
