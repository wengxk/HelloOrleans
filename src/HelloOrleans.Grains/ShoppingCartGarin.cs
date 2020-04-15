namespace HelloOrleans.Grains
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DomainModels;
    using Interfaces;
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

        public async Task Add(string goods)
        {
            _cart.State.Content.Add(goods);
            await _cart.WriteStateAsync();
        }

        public async Task<IEnumerable<string>> All()
        {
            return await Task.FromResult(_cart.State.Content.AsEnumerable());
        }

        public async Task Clear()
        {
            if (_cart.State.Content.Count == 0)
                return;
            _cart.State.Content.Clear();
            await _cart.WriteStateAsync();
        }

        #endregion

        public override Task OnActivateAsync()
        {
            _cart.State.Id = this.GetPrimaryKeyLong();
            if (_cart.State.Content == null)
                _cart.State.Content = new List<string>();
            return Task.CompletedTask;
        }


        public override async Task OnDeactivateAsync()
        {
            await _cart.WriteStateAsync();
        }
    }
}