namespace TaskManagement.Application.Exceptions
{
    public interface IStatusCodeException
    {
        public int StatusCode { get; }
    }
}
