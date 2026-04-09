﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;

/// <summary>
/// Query service interface for retrieving service execution data.
/// </summary>
public interface IServiceExecutionQueryService
{
    /// <summary>Handles GetServiceExecutionByIdQuery</summary>
    Task<ServiceExecution?> Handle(GetServiceExecutionByIdQuery query);

    /// <summary>Handles GetAssignedServicesByTechnicianQuery</summary>
    Task<IEnumerable<ServiceExecution>> Handle(GetAssignedServicesByTechnicianQuery query);

    /// <summary>Handles GetActiveServiceByHomeownerQuery</summary>
    Task<ServiceExecution?> Handle(GetActiveServiceByHomeownerQuery query);

    /// <summary>Handles GetServiceHistoryByHomeownerQuery</summary>
    Task<IEnumerable<ServiceExecution>> Handle(GetServiceHistoryByHomeownerQuery query);

    /// <summary>Handles GetServiceHistoryByTechnicianQuery</summary>
    Task<IEnumerable<ServiceExecution>> Handle(GetServiceHistoryByTechnicianQuery query);

    /// <summary>Handles GetWorkLogQuery</summary>
    Task<ServiceExecution?> Handle(GetWorkLogQuery query);

    /// <summary>Handles GetNoShowAlertQuery</summary>
    Task<IEnumerable<ServiceExecution>> Handle(GetNoShowAlertQuery query);
}

