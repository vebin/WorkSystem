namespace YmtSystem.Infrastructure.Serialization
{
    using ServiceStack.Text;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class SerializationExpand
    {
        public static string Serialization<T>(this T val)
        {
            return TypeSerializer.SerializeToString<T>(val);
        }
    }
}
