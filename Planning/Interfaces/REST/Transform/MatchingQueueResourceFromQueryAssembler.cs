using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class MatchingQueueResourceFromQueryResultAssembler
{
    /// <summary>
    /// Construye el MatchingQueueResource a partir de las listas de solicitudes en cola
    /// tal como las retorna ServiceDesignQueryService.Handle(GetMatchingQueueQuery).
    ///
    /// Parámetros directos en lugar de DTO: el query service retorna las colecciones
    /// descompuestas en sus campos primitivos para no generar acoplamiento por DTO.
    /// </summary>
    public static MatchingQueueResource ToResource(
        IEnumerable<(string RequestId, string HomeownerId, bool IsPriority, DateTime CreatedAt)> priorityItems,
        IEnumerable<(string RequestId, string HomeownerId, bool IsPriority, DateTime CreatedAt)> normalItems,
        int totalPending)
    {
        var priorityQueue = priorityItems
            .Select(item => new QueuedRequestResource(
                item.RequestId,
                item.HomeownerId,
                item.IsPriority,
                item.CreatedAt.ToString()))
            .ToList();
 
        var normalQueue = normalItems
            .Select(item => new QueuedRequestResource(
                item.RequestId,
                item.HomeownerId,
                item.IsPriority,
                item.CreatedAt.ToString()))
            .ToList();
 
        return new MatchingQueueResource(priorityQueue, normalQueue, totalPending);
    }
}