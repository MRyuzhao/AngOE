using System;

namespace AngOE.Common
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException() : base()
        {
        }

        public OperationFailedException(string message, object parameterBlob = null) : base(message)
        {
            if (parameterBlob != null)
            {
                Data.Add("ExceptionParameters", parameterBlob);
            }
        }
    }
}