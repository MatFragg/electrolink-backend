using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RequestLimitReachedException(HomeownerId homeownerId)
    : Exception($"Request limit reached for homeowner with ID {homeownerId.Value}.");

