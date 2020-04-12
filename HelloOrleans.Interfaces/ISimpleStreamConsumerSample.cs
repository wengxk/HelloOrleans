namespace HelloOrleans.Interfaces
{
    using System.Threading.Tasks;
    using Orleans;

    public interface ISimpleStreamConsumerSample : IGrainWithGuidKey
    {
        Task Initialize();
    }
}