﻿using System.Linq.Expressions;
using Application.Features.Items;
using Application.Features.Items.Mapper;
using Application.Pagination;
using Application.Specification;
using Domain.Bidding;
using Domain.Items;
using Infrastructure.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Features.Items;

public sealed class ItemRepository(DatabaseContext context) : IItemRepository
{
    public async Task<Page<ItemSummary>> ListAllAsync(
        Pageable pageable,
        ISpecification<Item> specification,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<Item, bool>> expression = specification.AsExpression();
        
        IEnumerable<ItemSummary> itemSummaries = await context.Items
            .OrderBy(i => i.Name)
            .Where(expression)
            .Select(item => new ItemSummary(item.Id, item.Name, item.InitialPrice, item.Images.First()))
            .Skip(pageable.Skip)
            .Take(pageable.Size)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        int totalItems = await context.Items.Where(expression).CountAsync(cancellationToken);

        return new Page<ItemSummary>(ref itemSummaries, pageable, totalItems);
    }
    
    public async Task<Page<Item>> ListAllItemsFullDetailsAsync(
        Pageable pageable,
        ISpecification<Item> specification,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<Item, bool>> expression = specification.AsExpression();
        
        IEnumerable<Item> items = await context.Items
            .OrderBy(i => i.Name)
            .Where(expression)
            .Skip(pageable.Skip)
            .Take(pageable.Size)
            .Include(i => i.Category)
            .Include(i => i.Images)
            .Include(i => i.Bids)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        int totalItems = await context.Items.Where(expression).CountAsync(cancellationToken);
        
        return new Page<Item>(ref items, pageable, totalItems);
    }

    public async Task<Option<Item>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Option<Item> item = await context.Items
            .Include(i => i.Category)
            .Include(i => i.Images)
            .Include(i => i.Bids)
            .Where(i => i.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        return item;
    }

    public async Task<List<Item>> ListAllItemsBySpecificationAsync(Specification<Item> specification, CancellationToken cancellationToken = default)
    {
        IIncludableQueryable<Item, List<Bid>> query = context.Items
            .Where(specification.SpecificationExpression)
            .Include(i => i.Category)
            .Include(i => i.Images)
            .Include(i => i.Bids);

        IOrderedQueryable<Item> orderedQuery = specification.OrderByExpression.Match(
            Left: l => query.OrderBy(l),
            Right: r => query.OrderBy(r)
        );

        List<Item> items = await orderedQuery
            .Take(specification.TakeExpression)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return items;
    }


    public async Task<bool> CreateAsync(Item item, CancellationToken cancellationToken = default)
    {
        await context.Items.AddAsync(item, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}