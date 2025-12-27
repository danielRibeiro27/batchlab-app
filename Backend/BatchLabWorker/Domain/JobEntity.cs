namespace BatchLabWorker.Domain
{
    public class JobEntity()
    {
        public Guid Id { get; set; }
        public string? Description { get; set; } //TO-DO: Validate null or empty
        public string? Status { get; set; } //TO-DO: Validate null or empty
        public DateTime CreatedAt { get; set; }
    }
}