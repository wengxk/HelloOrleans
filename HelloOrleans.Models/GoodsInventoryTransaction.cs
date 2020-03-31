namespace HelloOrleans.Models
{
    using System;

    [Serializable]
    public class GoodsInventoryTransaction
    {
        public int Id { get; set; }

        public string TransactionType { get; set; }

        public uint Amount { get; set; }
    }
}