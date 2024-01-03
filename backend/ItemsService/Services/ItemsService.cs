﻿using ItemsService.Context;
using ItemsService.Core;
using ItemsService.Models;

namespace ItemsService.Services;

public class ItemsService(ItemsServiceContext context) : IItemsService
{
    private readonly ItemsServiceContext _context = context;


    public async Task<Item> CreateItem(Item item)
    {
        item.Id = 0;

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public IAsyncEnumerable<Item> GetItems() => _context.Items.AsAsyncEnumerable();
}
