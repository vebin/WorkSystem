namespace YmtSystem.Infrastructure.EventBusService
{
    using System;
    using System.Collections.Generic;

    public interface IBus<TMessage>
    {
        void Publish(TMessage message, bool asyncPub = false, TimeSpan timeOut = default(TimeSpan), Action<Exception> pubErrorHandler = null);
        void Publist(IEnumerable<TMessage> message, bool asyncPub = false, TimeSpan timeOut = default(TimeSpan), Action<Exception> pubErrorHandler = null);
        void Clear();
    }
}
