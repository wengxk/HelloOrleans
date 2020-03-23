namespace HelloOrleans.Grains
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HelloOrleans.Interfaces;
    using HelloOrleans.Models;
    using Orleans;
    using Orleans.Runtime;

    /// <summary>
    /// Defines the <see cref="ShoppingCartGarin" />.
    /// </summary>
    public class ShoppingCartGarin : Grain, IShoppingCart
    {
        #region Fields

        /// <summary>
        /// Defines the _cart.
        /// </summary>
        private readonly IPersistentState<ShoppingCart> _cart;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartGarin"/> class.
        /// </summary>
        /// <param name="cart">The cart<see cref="IPersistentState{ShoppingCart}"/>.</param>
        public ShoppingCartGarin([PersistentState("cart", "HelloOrleansStorage")] IPersistentState<ShoppingCart> cart)
        {
            _cart = cart;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="goods">The goods<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task Add(string goods)
        {
            _cart.State.Content.Add(goods);
            _cart.WriteStateAsync();
            return Task.CompletedTask;
        }

        /// <summary>
        /// The All.
        /// </summary>
        /// <returns>The <see cref="Task{IList{string}}"/>.</returns>
        public Task<IEnumerable<string>> All()
        {
            return Task.FromResult(_cart.State.Content.AsEnumerable());
        }

        /// <summary>
        /// The Clear.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task Clear()
        {
            if (_cart.State.Content.Count == 0)
                return Task.CompletedTask;
            _cart.State.Content.Clear();
            _cart.WriteStateAsync();
            return Task.CompletedTask;
        }

        /// <summary>
        /// The OnActivateAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public override Task OnActivateAsync()
        {
            _cart.State.Id = this.GetPrimaryKeyLong();
            if (_cart.State.Content == null)
                _cart.State.Content = new List<string>();
            return Task.CompletedTask;
        }

        #endregion
    }
}
