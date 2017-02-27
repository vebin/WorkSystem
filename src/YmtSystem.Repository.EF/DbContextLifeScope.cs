namespace YmtSystem.Repository.EF
{
    using System;

    /// <summary>
    ///  DbContex 生命周期
    /// </summary>
    public enum DbContextLifeScope
    {
        /// <summary>
        /// 每次创建新的上下文
        /// </summary>
        New = 0,
        /// <summary>
        /// 相同线程贡献同一个上下文
        /// </summary>
        SameThread = 1,
        /// <summary>
        /// 相同http 请求共享一个上下文
        /// </summary>
        SameHttpContext = 2
    }
}
