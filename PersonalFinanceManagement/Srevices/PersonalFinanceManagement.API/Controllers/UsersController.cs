using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using System.Net;

namespace PersonalFinanceManagement.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUsersService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(UserRegistrationAndRestoration userRegistration, CancellationToken cancel = default)
        {
            await _userService.RegisterUserAsync(userRegistration.Email, userRegistration.Password, cancel);
            return Ok("User successfully registered.");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(UserLogin userLogin, CancellationToken cancel = default)
        {
            var user = await _userService.GetUserByEmailAsync(userLogin.Email, cancel);

            if (user is null)
                return StatusCode(404, new ApiError { Message = "User not found", StatusCode = (int)HttpStatusCode.NotFound });

            if (user.VerifiedAt is null)
                return StatusCode(403, new ApiError { Message = "User not verified", StatusCode = (int)HttpStatusCode.Forbidden });

            var token = await _authService.UserLogin(userLogin.Email, userLogin.Password, cancel);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyUser(string verificationToken, CancellationToken cancel = default)
        {
            return await _userService.VerifyUserAsync(verificationToken, cancel)
                ? Ok("User verified.")
                : StatusCode(404, new ApiError { Message = "User not found", StatusCode = (int)HttpStatusCode.NotFound });
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancel = default)
        {
            return await _userService.ForgotPasswordAsync(email, cancel)
                ? Ok("You may reset your password.")
                : StatusCode(404, new ApiError { Message = "User not found", StatusCode = (int)HttpStatusCode.NotFound });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetPassword(UserRegistrationAndRestoration userRegistration, string token, CancellationToken cancel = default)
        {
            var result = await _userService.ResetPasswordAsync(userRegistration.Email, userRegistration.Password, token, cancel);

            return result is null 
                ? StatusCode(404, new ApiError { Message = "User not found", StatusCode = (int)HttpStatusCode.NotFound })
                : (bool)result
                    ? Ok("Password successfully changed.")
                    : StatusCode(403, new ApiError { Message = "Invalid reset token.", StatusCode = (int)HttpStatusCode.Forbidden });
        }
    }
}
