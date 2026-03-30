using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CatalogAlreadyExistsException(TechnicianId technicianId) : Exception($"A catalog already exists for TechnicianId: {technicianId}");

