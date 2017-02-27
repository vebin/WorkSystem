using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YmtSystem.Infrastructure.Container.AutofacMvc.Controllers
{
    public interface IContainerTest
    {
        void S();
    }

    public class ContainerTest : IContainerTest
    {

        public void S()
        {

        }
    }
}