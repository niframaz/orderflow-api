namespace OrderFlow.Infrastructure.Persistence.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string EventType { get; private set; } = null!;
        public string Payload { get; private set; } = null!;
        public DateTime OccuredOn { get; private set; }
        public DateTime? ProcessedOn { get; private set; }
        public string? Error { get; private set; }

        private OutboxMessage() { }

        public OutboxMessage(Guid id, string eventType, string payload, DateTime occuredOn)
        {
            Id = id;
            EventType = eventType;
            Payload = payload;
            OccuredOn = occuredOn;
        }

        public void MarkAsProcessed()
        {
            ProcessedOn = DateTime.UtcNow;
            Error = null;
        }

        public void MarkAsFailed(string error)
        {
            ProcessedOn = DateTime.UtcNow;
            Error = error;
        }
    }
}
