﻿namespace Enqueuer.Queueing.Domain.Exceptions;

/// <summary>
/// Base class for all domain exceptions.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException()
    {
    }

    protected DomainException(string? message)
        : base(message)
    {
    }

    protected DomainException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
