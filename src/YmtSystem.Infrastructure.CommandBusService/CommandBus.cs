namespace YmtSystem.Infrastructure.CommandBusService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Local Memory CommandBus
    /// </summary>
    public class CommandBus
    {
        [ThreadStatic]
        private static CommandBus bus;
        public static CommandBus Instance { get { if (bus == null)bus = new CommandBus(); return bus; } }
        private static readonly Dictionary<Type, HashSet<dynamic>> commandHandles = new Dictionary<Type, HashSet<dynamic>>();

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var type = typeof(TCommand);
            HashSet<dynamic> hash;
            if (!commandHandles.TryGetValue(type, out hash))
            {
                throw new KeyNotFoundException("未找到 Command 对应的处理器");
            }
            foreach (var handle in hash)
            {
                handle.Handle(command);
            }
        }

        public void Subscribe<TCommand>(ICommandHandle<TCommand> commandHandle) where TCommand : ICommand
        {
            var type = typeof(TCommand);
            if (!commandHandles.ContainsKey(type))
            {
                var hash = new HashSet<dynamic>();
                hash.Add(commandHandle);
                commandHandles.Add(type, hash);
            }
            else
            {
                var hash = commandHandles[type];
                if (!hash.Contains(commandHandle))
                    hash.Add(commandHandle);
            }
        }
    }
}
