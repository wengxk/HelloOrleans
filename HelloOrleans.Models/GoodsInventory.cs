namespace HelloOrleans.Models
{
    using System;

    [Serializable]
    public class GoodsInventory
    {
        public string Goods { get; set; }

        public uint Inventory { get; set; }
    }
}
