namespace YmtSystem.Domain.Shard
{
    using System;

    public interface IEntity : IEntity<string>
    {
    }
    /// <summary>
    /// 继承此接口的对象表示是领域“实体”
    /// </summary>
    public interface IEntity<TKey> 
    {
        TKey Id { get; }
    }
}
