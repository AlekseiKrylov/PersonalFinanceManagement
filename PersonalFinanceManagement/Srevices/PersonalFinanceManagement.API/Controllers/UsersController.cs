using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.Services;

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
        public async Task<IActionResult> Login(UserLogin userLogin, CancellationToken cancel = default)
        {
            var user = await _userService.GetUserByEmailAsync(userLogin.Email, cancel);

            if (user is null)
                return NotFound("User not found");

            if (user.VerifiedAt is null)
                return StatusCode(403, "User not verified");

            var token = await _authService.UserLogin(userLogin.Email, userLogin.Password, cancel);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(string verificationToken, CancellationToken cancel = default)
        {
            return await _userService.VerifyUserAsync(verificationToken, cancel)
                ? Ok("User verified.")
                : NotFound();
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancel = default)
        {
            return await _userService.ForgotPasswordAsync(email, cancel)
                ? Ok("You may reset your password.")
                : NotFound();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserRegistrationAndRestoration userRegistration, string token, CancellationToken cancel = default)
        {
            var result = await _userService.ResetPasswordAsync(userRegistration.Email, userRegistration.Password, token, cancel);

            return result is null ? NotFound() : (bool)result ? Ok("Password successfully changed.") : StatusCode(403, "Invalid reset token.");
        }
    }
}
