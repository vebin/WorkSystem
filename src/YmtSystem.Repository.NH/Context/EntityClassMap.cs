namespace YmtSystem.Repository.NH.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Repository.NH.ModelMapping;
   
    /// <summary>
    /// 实体映射
    /// </summary>
    public class EntityClassMap
    {
        private readonly List<object> _list;
        /// <summary>
        /// 
        /// </summary>
        public EntityClassMap()
        {
            _list = new List<object>();
        }
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="map"></param>
        public void AddMap<TEntity>(ModelMappingBase<TEntity> map)
        {
            _list.Add(map);
        }
        /// <summary>
        /// 获取映射（转为强类型）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<ModelMappingBase<TEntity>> GetMap<TEntity>()
        {
            if (_list.IsEmpty()) throw new InvalidOperationException(" map list is empty");
            return _list.ConvertAll(e => e as ModelMappingBase<TEntity>);
        }
        /// <summary>
        /// 获取映射
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetMap()
        {
            if (_list.IsEmpty()) throw new InvalidOperationException(" map list is empty");
            return _list;
        }
        /// <summary>
        /// 清除所有映射
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }
    }
}
