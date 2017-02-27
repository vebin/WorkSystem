namespace YmtSystem.Domain.Repository.EF.ModelMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DbMappingRepository
    {
        private static readonly HashSet<dynamic> set = new HashSet<dynamic>();

        public static void Register(dynamic dy)
        {
            set.Add(dy);
        }

        public static IEnumerable<dynamic> GetRegisterEntityConfigure
        {
            get { return set; }
        }
    }
}
