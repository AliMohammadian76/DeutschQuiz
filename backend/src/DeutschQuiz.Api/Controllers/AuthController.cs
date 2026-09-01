using DeutschQuiz.Application;
using Microsoft.AspNetCore.Mvc;

namespace DeutschQuiz.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResult>> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await authService.RegisterAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (NotSupportedException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { message = exception.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await authService.LoginAsync(request, cancellationToken);
            return result is null
                ? Unauthorized(new { message = "Invalid email or password." })
                : Ok(result);
        }
        catch (NotSupportedException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { message = exception.Message });
        }
    }
}
