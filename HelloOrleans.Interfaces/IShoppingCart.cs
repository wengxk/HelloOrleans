namespace HelloOrleans.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orleans;

    /// <summary>
    /// Defines the <see cref="IShoppingCart" />
    /// </summary>
    public interface IShoppingCart : IGrainWithIntegerKey
    {
        /// <summary>
        /// The Add
        /// </summary>
        /// <param name="goods">The goods<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task Add(string goods);

        /// <summary>
        /// The All
        /// </summary>
        /// <returns>The <see cref="Task{IList{string}}"/></returns>
        Task<IEnumerable<string>> All();

        /// <summary>
        /// The Clear
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        Task Clear();
    }
}
