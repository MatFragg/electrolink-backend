namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;


public partial class ServiceOperation : IEntityWithCreatedUpdatedDate
{
    [Column("CreatedAt")]
    public DateTimeOffset? CreatedDate { get; set; }

    [Column("UpdatedAt")]
    public DateTimeOffset? UpdatedDate { get; set; }
}
