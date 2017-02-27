namespace YmtSystem.Infrastructure.CommandBusService
{
    using System;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// 标记为消息
    /// </summary>
    public interface IMessage
    {
    }
    /// <summary>
    /// 标记为命令
    /// </summary>
    public interface ICommand : IMessage
    {
    }
    //注：out 关键字指定该类型参数是协变的。
    //通过协变，可以使用与泛型参数指定的派生类型相比，派生程度更大的类型。
    //这样可以对委托类型和实现变体接口的类进行隐式转换。 
    //引用类型支持协变和逆变，但值类型不支持。
    public interface ICommand<out TIIdentity> : ICommand
        where TIIdentity : IIdentity
    {
        TIIdentity Id { get; }
    }

    public interface ICommandBus
    {
        void Send(ICommand command);
    }

    public interface ICommandHandle<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
