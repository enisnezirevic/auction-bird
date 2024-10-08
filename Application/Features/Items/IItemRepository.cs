﻿using Application.Features.Items.Mapper;
using Application.Pagination;
using Application.Specification;
using Domain.Items;
using LanguageExt;

namespace Application.Features.Items;

public interface IItemRepository
{
    Task<Page<ItemSummary>> ListAllAsync(
        Pageable pageable,
        ISpecification<Item> specification,
        CancellationToken cancellationToken = default);

    Task<Page<Item>> ListAllItemsFullDetailsAsync(
        Pageable pageable,
        ISpecification<Item> specification,
        CancellationToken cancellationToken = default);
    
    Task<List<Item>> ListAllItemsBySpecificationAsync(
        Specification<Item> specification,
        CancellationToken cancellationToken = default);

    Task<Option<Item>> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<bool> CreateAsync(
        Item item,
        CancellationToken cancellationToken = default);
}