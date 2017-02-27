namespace YmtSystem.Domain.Shard
{
    /// <summary>
    /// 并发检查类型
    /// </summary>
    public interface IConcurrencyCheck
    {
        byte[] Version { get; set; }
    }
}
