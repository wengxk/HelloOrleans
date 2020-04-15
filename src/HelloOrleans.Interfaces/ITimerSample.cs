namespace HelloOrleans.Interfaces
{
    using System.Threading.Tasks;
    using Orleans;

    public interface ITimerSample: IGrainWithIntegerKey
    {
        Task Initialize();
    }
}