using System.Net;

namespace Atelie.Domain.Exceptions; 

public abstract class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected AppException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound) { }
}

public class ValidationException : AppException
{
    public ValidationException(string message)
        : base(message, HttpStatusCode.BadRequest) { }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base(message, HttpStatusCode.Unauthorized) { }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base(message, HttpStatusCode.Forbidden) { }
}

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, HttpStatusCode.Conflict) { }
}

public class BusinessRuleException : AppException
{
    public BusinessRuleException(string message)
        : base(message, HttpStatusCode.BadRequest) { }
}

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message)
        : base(message, HttpStatusCode.InternalServerError) { }
}

public class ServiceUnavailableException : AppException
{
    public ServiceUnavailableException(string message)
        : base(message, HttpStatusCode.ServiceUnavailable) { }
}