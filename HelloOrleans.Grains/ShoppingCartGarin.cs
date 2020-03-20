namespace HelloOrleans.Grains
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HelloOrleans.Interfaces;
    using Orleans;

    /// <summary>
    /// Defines the <see cref="ShoppingCartGarin" />
    /// </summary>
    public class ShoppingCartGarin : Grain, IShoppingCart
    {
        /// <summary>
        /// Defines the _cart
        /// </summary>
        private readonly IList<string> _cart = new List<string>();

        /// <summary>
        /// The Add
        /// </summary>
        /// <param name="goods">The goods<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task Add(string goods)
        {
            _cart.Add(goods);
            return Task.CompletedTask;
        }

        /// <summary>
        /// The All
        /// </summary>
        /// <returns>The <see cref="Task{IList{string}}"/></returns>
        public Task<IEnumerable<string>> All()
        {
            return Task.FromResult(_cart.AsEnumerable());
        }

        /// <summary>
        /// The Clear
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public Task Clear()
        {
            _cart.Clear();
            return Task.CompletedTask;
        }
    }
}
