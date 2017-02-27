namespace YmtSystem.CrossCutting
{
    using System;

    public class ResponseMessage<TMessage>
    {
        public string Message { get; private set; }
        public string Code { get; private set; }
        public bool Success { get; private set; }
        public TMessage Result { get; private set; }

        private ResponseMessage() { }

        public static ResponseMessage<TMessage> CreateSuccess(TMessage msg, string message = null, string code = null)
        {
            return new ResponseMessage<TMessage>
            {
                Success = true,
                Result = msg,
                Message = message,
                Code = code
            };
        }

        public static ResponseMessage<TMessage> CreateFail(TMessage msg, string errorCode = null, string lastErrorMessage = null)
        {
            return new ResponseMessage<TMessage>
            {
                Success = false,
                Result = msg,
                Code = errorCode,
                Message = lastErrorMessage,

            };
        }

        public static ResponseMessage<TMessage> Create(TMessage msg, bool success, string lastErrorMessage = null, string errorCode = null)
        {
            return new ResponseMessage<TMessage>
            {
                Success = success,
                Result = msg,
                Message = lastErrorMessage,
                Code = errorCode,
            };
        }
    }
}
