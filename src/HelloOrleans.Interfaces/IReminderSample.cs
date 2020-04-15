namespace HelloOrleans.Interfaces
{
    using System.Threading.Tasks;
    using Orleans;

    public interface IReminderSample: IGrainWithIntegerKey, IRemindable
    {
        Task Initialize();
    }
}