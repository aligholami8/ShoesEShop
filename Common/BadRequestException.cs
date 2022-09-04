using System;
using System.Net;
using PoolManagement;

namespace Common
{
    public class BadRequestException : AppException
    {
        public BadRequestException()
            : base(ApiResultStatusCode.BadRequest, HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message)
            : base(ApiResultStatusCode.BadRequest, message, HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(object additionalData)
            : base(ApiResultStatusCode.BadRequest, null, HttpStatusCode.BadRequest, additionalData)
        {
        }

        public BadRequestException(string message, object additionalData)
            : base(ApiResultStatusCode.BadRequest, message, HttpStatusCode.BadRequest, additionalData)
        {
        }

        public BadRequestException(string message, Exception exception)
            : base(ApiResultStatusCode.BadRequest, message, exception, HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCode.BadRequest, message, HttpStatusCode.BadRequest, exception, additionalData)
        {
        }
    }
}
