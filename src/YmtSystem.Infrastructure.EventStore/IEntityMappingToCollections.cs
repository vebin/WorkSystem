using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Infrastructure.EventStore
{
    public interface IEntityMappingToCollections
    {
        string DBName { get; }
        string TBName { get; }
    }

}
