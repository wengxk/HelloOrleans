namespace HelloOrleans.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ShoppingCart
    {
        public long Id { get; set; }

        public IList<string> Content { get; set; }

    }
}
