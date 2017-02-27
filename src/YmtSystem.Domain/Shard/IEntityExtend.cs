namespace YmtSystem.Domain.Shard
{
    using System;

    /// <summary>
    /// 实体附加属性
    /// </summary>
    public interface IEntityExtend
    {
        DateTime CreateTime { get; set; }
        DateTime? ModifyTime { get; set; }       
    }
}
