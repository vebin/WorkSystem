using System;

namespace YmtSystem.CrossCutting
{
    /// <summary>
    /// 实体或者值类型“标识”接口
    /// </summary>
    public interface IIdentity
    {
        //string Id { get; }
    }
    /// <summary>
    /// “标识”基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AbstractIdentity<TKey> : IIdentity 
    {
        public abstract TKey Id { get; protected set; }
        /// <summary>
        /// 标识类型检查
        /// </summary>
        static AbstractIdentity()
        {
            CheckIdentityType();
        }
       
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as AbstractIdentity<TKey>;

            if (identity != null)
            {
                return Equals(identity);
            }

            return false;
        }
        
        public override int GetHashCode()
        {
            return (Id.GetHashCode());
        }
        
        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + Id + "]";
        
        }
       
        public static bool operator ==(AbstractIdentity<TKey> left, AbstractIdentity<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AbstractIdentity<TKey> left, AbstractIdentity<TKey> right)
        {
            return !Equals(left, right);
        } 
        
        private static void CheckIdentityType()
        {
            var type = typeof(TKey);
            if (type == typeof(int) || type == typeof(long) || type == typeof(uint) || type == typeof(ulong))
                return;
            if (type == typeof(Guid) || type == typeof(string))
                return;
            throw new InvalidOperationException("Abstract identity inheritors must provide stable hash. It is not supported for:  " + type);
        }
    }
}
