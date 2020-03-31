namespace HelloOrleans.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orleans;

    public interface IShoppingCart : IGrainWithIntegerKey
    {
        Task Add(string goods);

        Task<IEnumerable<string>> All();

        Task Clear();
    }
}