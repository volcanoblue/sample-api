using System;

namespace VolcanoBlue.Core.Error
{
    public sealed class ExceptionalError : IError
    {
        public Exception Exception { get; }

        public ExceptionalError(Exception exception)
        {
            Exception = exception;
        }
    }
}
