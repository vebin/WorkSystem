namespace YmtSystem.Domain.Shard
{
    using System;
    using System.Collections.Generic;   
    using System.Threading.Tasks;

    public class ValidatorResult
    {
        internal ValidatorResult SetSuccess(bool val)
        {
            this.Success = val;
            return this;
        }
        internal ValidatorResult SetMessage(IEnumerable<String> message)
        {
            this.Message = message;
            return this;
        }
        public bool Success { get; private set; }
        public IEnumerable<String> Message { get; private set; }
    }
}
