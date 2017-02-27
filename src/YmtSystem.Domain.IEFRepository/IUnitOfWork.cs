namespace YmtSystem.Domain.IEFRepository
{
    using System;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// 工作单元模式；
    ///<para><seealso cref="http://martinfowler.com/eaaCatalog/unitOfWork.html"/> </para> 
    ///<para><seealso cref="http://msdn.microsoft.com/en-us/magazine/dd882510.aspx"/> </para> 
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 同步提交当前的Unit Of Work事务。
        /// </summary>
        /// <param name="retry">异常情况下重试次数</param>
        /// <param name="lockd">是否独占方式提交</param>
        /// <returns>返回受影响的</returns>
        ExecuteResult<int> Commit(int retry = 1, bool lockd = false);
        /// <summary>
        /// 异步提交当前Unit of Work
        /// </summary>
        /// <param name="retry">异常情况下重试次数</param>
        /// <returns></returns>
        Task<ExecuteResult<int>> AsyncCommit(int retry = 1);
        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        void RollbackChanges();
    }
}
