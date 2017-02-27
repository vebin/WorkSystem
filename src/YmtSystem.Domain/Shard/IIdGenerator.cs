namespace YmtSystem.Domain.Shard
{
    using System;

    /// <summary>
    /// 实体ID生成器
    /// </summary>
    public interface IIdGenerator
    {
        string Generator();
    }
}
