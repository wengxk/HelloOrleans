namespace HelloOrleans.Interfaces
{
    using System.Threading.Tasks;
    using Orleans;

    public interface IGoodsInventory : IGrainWithIntegerKey
    {
        Task StockIn(uint amount);

        Task StockOut(uint amount);
    }

}
