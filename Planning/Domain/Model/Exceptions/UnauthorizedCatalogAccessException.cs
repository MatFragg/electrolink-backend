using System;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class UnauthorizedCatalogAccessException : Exception
{
    public UnauthorizedCatalogAccessException() 
        : base("You are not authorized to modify this catalog.")
    { }

    public UnauthorizedCatalogAccessException(string message) 
        : base(message)
    { }
}

