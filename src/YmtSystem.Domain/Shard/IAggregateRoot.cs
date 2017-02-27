namespace YmtSystem.Domain.Shard
{
    /// <summary>
    /// 继承此接口的类表示是领域“聚合根对象”；   
    /// </summary>
    public interface IAggregateRoot:IEntity
    {
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>
    {
    }
}
