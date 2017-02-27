using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Infrastructure.CommandBusService;
using YmtSystem.CrossCutting;


namespace YmtSystem.Infrastructure.Test
{
    [TestFixture]
    public class CommandBusTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            CommandBus.Instance.Subscribe(new CreateOrderCommandHandle());
            CommandBus.Instance.Subscribe(new CreateOrderCommand2Handle());
        }
        [Test]
        public void Test1()
        {

            CommandBus.Instance.Send(new CreateOrderCommand());


            CommandBus.Instance.Send(new CreateOrderCommand2());
        }
    }

    public class CreateOrderCommandHandle : ICommandHandle<CreateOrderCommand>
    {

        public void Handle(CreateOrderCommand command)
        {
            Console.WriteLine(command.CreateTime);
        }
    }

    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand()
        {
            this.CreateTime = DateTime.Now;
        }

        public DateTime CreateTime { get; private set; }
    }
    public class CreateOrderCommand2Handle : ICommandHandle<CreateOrderCommand2>
    {

        public void Handle(CreateOrderCommand2 command)
        {
            Console.WriteLine(command.Id.Id);
        }
    }
    public class CreateOrderCommand2 : ICommand<I>
    {
        public CreateOrderCommand2()
        {
            this.CreateTime = DateTime.Now;
            Id = new I();
        }
        public I Id { get; private set; }
        public DateTime CreateTime { get; private set; }

    }

    public class I : AbstractIdentity<string>
    {
        public override string Id
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }
    }
}
