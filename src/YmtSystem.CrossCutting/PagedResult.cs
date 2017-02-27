namespace YmtSystem.CrossCutting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 表示包含了分页信息的集合类型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> : List<T>
    {
        #region Public Fields
        /// <summary>
        /// 获取一个当前类型的空值。
        /// </summary>
        public static readonly PagedResult<T> Empty = new PagedResult<T>(0, 0, 0, 0, null);
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>PagedResult{T}</c>类型的实例。
        /// </summary>
        public PagedResult()
        {
            Data = new List<T>();
        }
        /// <summary>
        /// 初始化一个新的<c>PagedResult{T}</c>类型的实例。
        /// </summary>
        /// <param name="totalRecords">总记录数。</param>
        /// <param name="totalPages">页数。</param>
        /// <param name="pageSize">页面大小。</param>
        /// <param name="pageNumber">页码。</param>
        /// <param name="data">当前页面的数据。</param>
        public PagedResult(int totalRecords, int totalPages, int pageSize, int pageNumber, IEnumerable<T> data)
        {
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.Data = data;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置总记录数。
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// 获取或设置页数。
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 获取或设置页面大小。
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取或设置页码。
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 获取或设置当前页面的数据。
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// 确定指定的Object是否等于当前的Object。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>如果指定的Object与当前Object相等，则返回true，否则返回false。</returns>
        /// <remarks>有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.equals。
        /// </remarks>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == (object)null)
                return false;
            var other = obj as PagedResult<T>;
            if (other == (object)null)
                return false;
            return this.TotalPages == other.TotalPages &&
                this.TotalRecords == other.TotalRecords &&
                this.PageNumber == other.PageNumber &&
                this.PageSize == other.PageSize &&
                this.Data == other.Data;
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前Object的哈希代码。</returns>
        /// <remarks>有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.gethashcode。
        /// </remarks>
        public override int GetHashCode()
        {
            return this.TotalPages.GetHashCode() ^
                this.TotalRecords.GetHashCode() ^
                this.PageNumber.GetHashCode() ^
                this.PageSize.GetHashCode();
        }

        /// <summary>
        /// 确定两个对象是否相等。
        /// </summary>
        /// <param name="a">待确定的第一个对象。</param>
        /// <param name="b">待确定的另一个对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public static bool operator ==(PagedResult<T> a, PagedResult<T> b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            return a.Equals(b);
        }

        /// <summary>
        /// 确定两个对象是否不相等。
        /// </summary>
        /// <param name="a">待确定的第一个对象。</param>
        /// <param name="b">待确定的另一个对象。</param>
        /// <returns>如果两者不相等，则返回true，否则返回false。</returns>
        public static bool operator !=(PagedResult<T> a, PagedResult<T> b)
        {
            return !(a == b);
        }
        #endregion
    }
}
