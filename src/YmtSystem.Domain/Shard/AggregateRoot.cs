namespace YmtSystem.Domain.Shard
{
    /// <summary>
    /// 继承此类的领域对象表示是“聚合根”
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
    }

    /// <summary>
    /// 继承此类的领域对象表示是“聚合根”
    /// </summary>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
    }
}
