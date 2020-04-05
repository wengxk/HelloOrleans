namespace HelloOrleans.DomainModels
{
    using System;

    [Serializable]
    public class BasicGoods
    {
        public long Id { get; set; }

        public string GoodsName { get; set; }
    }
}