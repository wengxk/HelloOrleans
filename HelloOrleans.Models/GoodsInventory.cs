﻿namespace HelloOrleans.Models
{
    using System;

    [Serializable]
    public class GoodsInventory
    {
        public int Id { get; set; }

        public string GoodsName { get; set; }

        public uint Inventory { get; set; }
    }
}
