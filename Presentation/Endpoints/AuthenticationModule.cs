﻿using Application.Features.Authentication.Commands.SignIn;
using Application.Features.Authentication.Commands.SignInRefreshToken;
using Application.Features.Authentication.Commands.SignOut;
using Application.Features.Authentication.Commands.SignUp;
using Application.Features.Authentication.Mapper;
using Carter;
using MediatR;

namespace Presentation.Endpoints;

public sealed class AuthenticationModule() : CarterModule(Versioning.Version)
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("signin", SignIn);
        app.MapPost("signin/refresh", SignInWithRefreshToken);
        app.MapPost("signup", SignUp);
        app.MapPost("signout", SignOut);
    }

    private static async Task<bool> SignIn(ISender sender, HttpRequest httpRequest)
    {
        IFormCollection form = await httpRequest.ReadFormAsync();
        SignInDto signInDto = form.MapToSignInDto();

        return await sender.Send(new SignInCommand(signInDto));
    }

    private static async Task<bool> SignInWithRefreshToken(ISender sender, HttpRequest httpRequest)
    {
        IFormCollection form = await httpRequest.ReadFormAsync();
        SignInRefreshTokenDto signInRefreshTokenDto = form.MapToSignInRefreshTokenDto();

        return await sender.Send(new SignInRefreshTokenCommand(signInRefreshTokenDto));
    }

    private static async Task SignUp(ISender sender, HttpRequest httpRequest)
    {
        IFormCollection form = await httpRequest.ReadFormAsync();
        SignUpDto signUpDto = form.MapToSignUpDto();

        await sender.Send(new SignUpCommand(signUpDto));
    }

    private static async Task SignOut(ISender sender, HttpRequest httpRequest)
    {
        IFormCollection form = await httpRequest.ReadFormAsync();
        SignOutDto signOutDto = form.MapToSignOutDto();

        await sender.Send(new SignOutCommand(signOutDto));
    }
}