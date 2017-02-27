namespace YmtSystem.Domain.Shard
{
    /// <summary>
    /// 实体是否逻辑删除
    /// </summary>
    public interface IEntityLogicDelete
    {
        bool IsDelete { get; set; }
    }
}
