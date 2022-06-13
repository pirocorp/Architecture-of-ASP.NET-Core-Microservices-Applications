namespace CommandService.EventProcessing
{
    using System.Threading.Tasks;

    public interface IEventProcessor
    {
        Task ProcessEvent(string messageString);
    }
}
