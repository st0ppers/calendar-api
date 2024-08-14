﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Internal;

public static class Extensions
{
    public static ActionResult ToActonResult(this Exception e)
    {
        return e switch
        {
            UserAlreadyExists uae => new ConflictObjectResult(uae.Message),
            DatabaseException de => new StatusCodeResult(StatusCodes.Status500InternalServerError),
            InvalidCredentialException ice => new BadRequestObjectResult(ice.Message),
            ValidationException ve => new BadRequestObjectResult(ve.Message),
            _ => new BadRequestObjectResult(e.Message)
        };
    }

    public static Result<LoginRequest, Exception> Validate(this LoginRequest request) =>
        Validator<LoginRequest>.Instance(request)
            .Validate(x => string.IsNullOrWhiteSpace(x.Username), "Username should not be empty or null")
            .Validate(x => x.Password.Length < 8, "Password should be at least 8 characters long")
            .ToResult();
}