namespace YmtSystem.Domain.Shard
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using YmtSystem.CrossCutting;
    using YmtSystem.CrossCutting.Utility;

    /// <summary>
    /// 领域实体。
    /// <remarks>默认主键为string类</remarks>
    /// </summary>
    public abstract class Entity : Entity<string>
    {
    }

    /// <summary>
    /// 领域实体基类。
    /// <remarks>如果子类没有使用重写或者没有使用基类Id，则需要实现CompareComponents比较器。</remarks>
    /// </summary>
    public abstract class Entity<TKey> : IEntity<TKey>, IValidatableObject
    {
        private int? _requestedHashCode;

        /// <summary>
        ///获取或者设计实体ID；子类可以重写ID实现方式
        /// </summary>      
        public virtual TKey Id { get; protected set; }
        //public virtual DateTime CreateTime { get; set; }
        //public virtual DateTime? ModifyTime { get; set; }

        public Entity()
        {

        }

        #region Overrides Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            Entity<TKey> item = (Entity<TKey>)obj;

            //如果子类添加了自定义比较元素，则使用子类指定的元素
            if (this.CompareComponents().Any())
            {
                return this.CompareComponents().SequenceEqual(item.CompareComponents());
            }
            return item.Id.Equals(this.Id);
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            if (this.CompareComponents().Any())
            {
                return HashCodeHelper.CombineHashCodes(this.CompareComponents());
            }

            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }

        #endregion

        protected virtual IEnumerable<object> CompareComponents()
        {
            return Enumerable.Empty<object>();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
