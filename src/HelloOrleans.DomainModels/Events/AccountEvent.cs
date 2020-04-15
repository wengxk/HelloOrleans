namespace HelloOrleans.DomainModels.Events
{
    using System;

    public abstract class AccountEvent
    {
        public static readonly int NEW_ETag = -1;

        public int ETag { get; set; } = NEW_ETag;

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        
        public long AccountId { get; set; }
    }
}