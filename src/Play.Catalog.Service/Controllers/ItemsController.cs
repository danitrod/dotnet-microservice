using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dto;

namespace Play.Catalog.Service.Controllers
{
  [ApiController]
  [Route("items")] // /items
  public class ItemsController : ControllerBase
  {
    private static readonly List<ItemDto> items = new()
    {
      new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.Now),
      new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 3, DateTimeOffset.Now),
      new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 25, DateTimeOffset.Now),
    };

    [HttpGet] // GET /items
    public IEnumerable<ItemDto> Get()
    {
      return items;
    }

    [HttpGet("{id}")] // GET /items/:id
    public ActionResult<ItemDto> GetById(Guid id)
    {
      var item = items.Where(item => item.Id == id).SingleOrDefault();

      if (item == null)
      {
        return NotFound();
      }

      return item;
    }

    [HttpPost] // POST /items
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
      var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.Now);
      items.Add(item);

      return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")] // PUT /items/:id
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
      var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

      if (existingItem == null)
      {
        return NotFound();
      }

      var updatedItem = existingItem with
      {
        Name = updateItemDto.Name,
        Description = updateItemDto.Description,
        Price = updateItemDto.Price
      };

      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items[index] = updatedItem;

      return NoContent();
    }

    [HttpDelete("{id}")] // Delete /items/:id
    public IActionResult Delete(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);

      if (index < 0)
      {
        return NotFound();
      }

      items.RemoveAt(index);

      return NoContent();
    }
  }

}