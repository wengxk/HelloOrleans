namespace HelloOrleans.Grains
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using Orleans;
    using Orleans.Runtime;

    public class ShoppingCartGarin : Grain, IShoppingCart
    {
        private readonly IPersistentState<ShoppingCart> _cart;

        public ShoppingCartGarin([PersistentState("cart", "HelloOrleansStorage")]
            IPersistentState<ShoppingCart> cart)
        {
            _cart = cart;
        }

        #region IShoppingCart Members

        public Task Add(string goods)
        {
            _cart.State.Content.Add(goods);
            _cart.WriteStateAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> All()
        {
            return Task.FromResult(_cart.State.Content.AsEnumerable());
        }

        public Task Clear()
        {
            if (_cart.State.Content.Count == 0)
                return Task.CompletedTask;
            _cart.State.Content.Clear();
            _cart.WriteStateAsync();
            return Task.CompletedTask;
        }

        #endregion

        public override Task OnActivateAsync()
        {
            _cart.State.Id = this.GetPrimaryKeyLong();
            if (_cart.State.Content == null)
                _cart.State.Content = new List<string>();
            return Task.CompletedTask;
        }


        public override Task OnDeactivateAsync()
        {
            _cart.WriteStateAsync();
            return base.OnDeactivateAsync();
        }
    }
}