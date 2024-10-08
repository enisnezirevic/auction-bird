﻿using Application.Features.Items;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Features.Items;

public sealed class ItemsConfig : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(ItemConstraint.MaxNameLength)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(ItemConstraint.MaxDescriptionLength);

        builder
            .Property(x => x.InitialPrice)
            .HasColumnType("decimal(10,2)")
            .HasPrecision(10, 2)
            .IsRequired();

        builder
            .Property(x => x.StartTime)
            .IsRequired();

        builder
            .Property(x => x.EndTime)
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .IsRequired();

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();

        builder
            .HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Bids)
            .WithOne()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.OwnerId)
            .IsRequired();
    }
}