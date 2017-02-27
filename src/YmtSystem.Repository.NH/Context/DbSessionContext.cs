namespace YmtSystem.Repository.NH.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using System.IO;
    using NHibernate.Cfg;
    using NHibernate;
    using FluentNHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;
    using System.Data;
    using System.Reflection;

    /// <summary>
    /// 数据库会话上下文基类；
    /// </summary>
    public abstract class DbSessionContext
    {
        private static readonly string _defaultNhXmlConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\NH.xml");
        private Configuration _configuration;
        private ISessionFactory _sessionFactory;
        private IsolationLevel? _level;
        private ISession _session;
        private EntityClassMap _map;
        private bool _autoExecuteSchema;
        private bool _autoUpdateSchema;

        /// <summary>
        ///   初始化DbSessionContext；不自动自动执行映射生成的初始化DB脚本
        /// </summary>
        /// <param name="configPath"></param>
        public DbSessionContext(string configPath)
            : this(null, configPath, false, false)
        {

        }

        /// <summary>
        /// 初始化DbSessionContext
        /// </summary>
        /// <param name="level"></param>
        /// <param name="autoExecuteSchema">是否自动执行映射生成的初始化DB脚本（适合测试环境，总是创建新表）</param>
        /// <param name="autoUpdateSchema">是否更新数据库架构（适合测试环境）</param> 
        public DbSessionContext(IsolationLevel? level, bool autoExecuteSchema, bool autoUpdateSchema)
            : this(level, _defaultNhXmlConfigPath, autoExecuteSchema, autoUpdateSchema)
        {
        }

        /// <summary>
        ///  初始化DbSessionContext
        /// </summary>
        /// <param name="level"></param>
        /// <param name="nhxmlPath"></param>
        /// <param name="autoExecuteSchema">是否自动执行映射生成的初始化DB脚本（适合测试环境，总是创建新表）</param>
        /// <param name="autoUpdateSchema">是否更新数据库架构（适合测试环境）</param>
        public DbSessionContext(IsolationLevel? level, string nhxmlPath, bool autoExecuteSchema, bool autoUpdateSchema)
        {
            Interlocked.CompareExchange(ref _configuration, new Configuration(), null);
            _configuration.Configure(nhxmlPath ?? _defaultNhXmlConfigPath);
            _level = level;
            _map = new EntityClassMap();
            _autoExecuteSchema = autoExecuteSchema;
            _autoUpdateSchema = autoExecuteSchema;
        }

        /// <summary>
        /// 创建实体映射
        /// </summary>
        /// <param name="map"></param>
        protected abstract void OnEntityMap(EntityClassMap map);

        /// <summary>
        /// 获取当前Session
        /// </summary>
        public ISession Current
        {
            get
            {
              
                if (_sessionFactory == null)
                {
                    BuildSession();
                    return Open();
                }
                else
                {
                    return Open();
                }
            }
        }

        /// <summary>
        /// 是否自动执行初始化DB脚本
        /// </summary>
        public bool IsAutoExecuteInitDbScript
        {
            get { return _autoExecuteSchema; }
            private set { _autoExecuteSchema = value; }
        }

        /// <summary>
        /// 关闭Session
        /// </summary>
        public void Close()
        {
            _session.Close();
        }

        /// <summary>
        /// 清除Session
        /// </summary>
        public void Clear()
        {
            _session.Clear();
            _map.Clear();
        }

        /// <summary>
        /// 获取事务锁定行为
        /// </summary>
        public IsolationLevel _IsolationLevel
        {
            get
            {
                return this._level.HasValue ? this._level.Value : IsolationLevel.Unspecified;
            }
        }

        /// <summary>
        /// all build
        /// </summary>
        /// <param name="mappingAssembly">实体映射所在的程序集</param>
        /// <returns></returns>
        [Obsolete("使用 BuildSession()", false)]
        protected virtual DbSessionContext BuildSession(Assembly mappingAssembly)
        {
            Interlocked.CompareExchange(ref _sessionFactory, BuildSessionFactory(mappingAssembly), null);
            return this;
        }

        /// <summary>
        /// 绑定会话
        /// </summary>
        /// <returns></returns>
        protected virtual DbSessionContext BuildSession()
        {
            Interlocked.CompareExchange(ref _sessionFactory,BuildSessionFactory(), null);
            return this;
        }

        /// <summary>
        /// 初始化数据库对象 （测试环境使用）
        /// </summary>
        /// <param name="writefile">输出执行语句到指定的文件</param>
        protected virtual void InitDatabaseSchema(Configuration configuration, string writefile = null)
        {
            if (!IsAutoExecuteInitDbScript) return;
            var nhSchem = new SchemaExport(configuration);
            nhSchem.Execute(false, true, false);
            if (!string.IsNullOrEmpty(writefile))
                nhSchem.SetOutputFile(writefile);
        }

        /// <summary>
        /// 更新数据架构（测试环境使用）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="writefile"></param>
        protected virtual void UpdateDatabaseSchema(Configuration configuration, string writefile = null)
        {
            if (!IsAutoExecuteInitDbScript) return;
            var nhSchem = new SchemaUpdate(configuration);
            nhSchem.Execute(true, true);
        }

        private ISessionFactory BuildSessionFactory()
        {
            this._map.Clear();
            OnEntityMap(this._map);

            return Fluently
                            .Configure(_configuration)
                            .ExposeConfiguration(c => { InitDatabaseSchema(c); UpdateDatabaseSchema(c); })
                            .Mappings(c => _map.GetMap().Each(e => c.FluentMappings.Add(e.GetType())))
                            .ExposeConfiguration(c => c.SetInterceptor(new SqlCommandInterceptor()))
                            .BuildSessionFactory();
        }

        private ISessionFactory BuildSessionFactory(Assembly mappingAssembly)
        {
            return Fluently
                            .Configure(_configuration)
                            .ExposeConfiguration(c => { InitDatabaseSchema(c); UpdateDatabaseSchema(c); })
                            .Mappings(c => c.FluentMappings.AddFromAssembly(mappingAssembly))
                            .ExposeConfiguration(c => c.SetInterceptor(new SqlCommandInterceptor()))
                            .BuildSessionFactory();
        }

        private ISession Open()
        {
            //如果session 为空，则创建新的session
            if (Interlocked.CompareExchange(ref _session, _sessionFactory.OpenSession(), null) != null)
            {
                //如果session ，被释放，则重新打开session
                if (!_session.IsOpen)
                    _session = _sessionFactory.OpenSession();
            }          
            return _session;
        }
    }
}
