using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Exceptions
{
    public class CQRSDomainException : Exception
    {
        public CQRSDomainException()
        { }

        public CQRSDomainException(string message) : base(message)
        { }

        public CQRSDomainException(string message, Exception innerException) : base(message, innerException)
        { }

    }
}
