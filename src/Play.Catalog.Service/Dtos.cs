using System;
using System.ComponentModel.DataAnnotations;

// DTO - data transfer object
// used for agreeing the data used with both parts communicating through the API
namespace Play.Catalog.Service.Dto
{
  public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

  public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

  public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
}
