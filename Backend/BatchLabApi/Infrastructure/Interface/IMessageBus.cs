namespace BatchLabApi.Infrastructure.Interface
{
    //TO-DO: Generic message bus interface
    public interface IMessageBus
    {
        Task<bool> PublishAsync(Dto.JobDto job); //WARNING: Infrastructure can have dependency on Dto?
    }
}