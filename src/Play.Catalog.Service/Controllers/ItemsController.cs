using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dto;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
  [ApiController]
  [Route("items")] // /items
  public class ItemsController : ControllerBase
  {
    private readonly ItemsRepository itemsRepository = new();

    [HttpGet] // GET /items
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
      var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
      return items;
    }

    [HttpGet("{id}")] // GET /items/:id
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
      var item = await itemsRepository.GetAsync(id);

      if (item == null)
      {
        return NotFound();
      }

      return item.AsDto();
    }

    [HttpPost] // POST /items
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
      var item = new Item
      {
        Name = createItemDto.Name,
        Description = createItemDto.Description,
        Price = createItemDto.Price,
        CreatedAt = DateTimeOffset.Now
      };

      await itemsRepository.CreateAsync(item);

      return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")] // PUT /items/:id
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
      var existingItem = await itemsRepository.GetAsync(id);

      if (existingItem == null)
      {
        return NotFound();
      }

      existingItem.Name = updateItemDto.Name;
      existingItem.Description = updateItemDto.Description;
      existingItem.Price = updateItemDto.Price;

      await itemsRepository.UpdateAsync(existingItem);

      return NoContent();
    }

    [HttpDelete("{id}")] // Delete /items/:id
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      var item = await itemsRepository.GetAsync(id);

      if (item == null)
      {
        return NotFound();
      }

      await itemsRepository.RemoveAsync(id);

      return NoContent();
    }
  }

}
