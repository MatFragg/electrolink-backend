﻿namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record ComponentUsageItemResource(
    string ComponentTypeId,
    string ComponentTypeName,
    int QuantityUsed,
    int QuantityReserved
);