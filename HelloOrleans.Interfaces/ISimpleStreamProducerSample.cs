namespace HelloOrleans.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Orleans;

    public interface ISimpleStreamProducerSample : IGrainWithGuidKey
    {
        Task Initialize();
    }
}