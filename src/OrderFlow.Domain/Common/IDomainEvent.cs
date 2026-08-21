namespace OrderFlow.Domain.Common
{
    public interface IDomainEvent
    {
        DateTime CreatedOn { get; }
    }
}
