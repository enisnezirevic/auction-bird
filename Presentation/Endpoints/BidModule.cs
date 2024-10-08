﻿using System.IdentityModel.Tokens.Jwt;
using Application.Features.Bidding.Commands;
using Application.Features.Bidding.Mapper;
using Application.Features.Bidding.Queries.FindHighestBidder;
using Carter;
using MediatR;

namespace Presentation.Endpoints;

public sealed class BidModule() : CarterModule(Versioning.Version)
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("bids", MakeBid);
        app.MapGet("bids/{itemId:guid}", FindHighestBidder);
    }

    private static async Task<bool> MakeBid(ISender sender, HttpContext context)
    {
        string idToken = context.Request.Cookies["idToken"];
        if (string.IsNullOrEmpty(idToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return false;
        }

        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken? token = handler.ReadJwtToken(idToken);

        string userId = token.Claims.First(claim => claim.Type == "sub").Value;
        if (string.IsNullOrEmpty(userId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return false;
        }

        IFormCollection form = await context.Request.ReadFormAsync();
        MakeBidDto bid = form.MapToMakeBidDto(userId);

        return await sender.Send(new MakeBidCommand(bid));
    }

    private static async Task<string> FindHighestBidder(ISender sender, Guid itemId)
    {
        string highestBidderId = await sender.Send(new FindHighestBidderQuery(itemId));

        return highestBidderId;
    }
}