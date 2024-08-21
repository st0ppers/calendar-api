using CalendarApi.Contracts.Requests;
using CSharpFunctionalExtensions;
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

    public static Result<RegisterRequest, Exception> Validate(this RegisterRequest request) =>
        Validator<RegisterRequest>.Instance(request)
            .Validate(x => string.IsNullOrWhiteSpace(x.Username), "Username should not be empty or null")
            .Validate(x => x.Password.Length < 8, "Password should be at least 8 characters long")
            .Validate(x => x.Color.Length < 3, "Color should be at least 3 characters long")
            .ToResult();

    public static Result<UpdateFreeTimeRequest, Exception> Validate(this UpdateFreeTimeRequest request) =>
        Validator<UpdateFreeTimeRequest>.Instance(request)
            .Validate(x => x.From > x.To, "From should be less than To")
            .ToResult();

    public static Result<GroupIdRequest, Exception> Validate(this GroupIdRequest request) =>
        Validator<GroupIdRequest>.Instance(request)
            .Validate(x => x.GroupId <= 0, "GroupId should be greater than 0")
            .ToResult();
}