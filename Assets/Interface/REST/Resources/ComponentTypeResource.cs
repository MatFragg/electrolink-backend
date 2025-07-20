    namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

    /// <summary>
    ///    Representa un tipo de componente como un recurso de la API.
    ///    Este es el DTO que se enviará a los clientes.
    /// </summary>
    /// <param name="Id">El identificador único del tipo.</param>
    /// <param name="Name">El nombre del tipo.</param>
    public record ComponentTypeResource(int Id, string Name, string Description);