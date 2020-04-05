namespace HelloOrleans.DomainModels
{
    using System;

    [Serializable]
    public class BasicGoods
    {
        public int Id { get; set; }

        public string GoodsName { get; set; }
    }
}