namespace TaskManagement.Domain.Interfaces
{
    public interface IRequestValidation<TShared>
    {
        public Guid EntityId { get; set; }
        public TShared? SharedObject { get; set; }
    }
}
