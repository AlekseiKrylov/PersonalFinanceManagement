using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces;

namespace PersonalFinanceManagement.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Register(UserRegistrationAndRestoration userRegistration)
        {
            await _userService.RegisterUserAsync(userRegistration.Email, userRegistration.Password);
            return Ok("User successfully registered.");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var user = await _userService.GetUserByEmailAsync(userLogin.Email);

            if (user is null)
                return NotFound("User not found");

            if (user.VerifiedAt is null)
                return StatusCode(403, "User not verified");

            var token = await _authService.UserLogin(userLogin.Email, userLogin.Password);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(string verificationToken)
        {
            return await _userService.VerifyUserAsync(verificationToken)
                ? Ok("User verified.")
                : NotFound();
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            return await _userService.ForgotPasswordAsync(email)
                ? Ok("You may reset your password.")
                : NotFound();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserRegistrationAndRestoration userRegistration, string token)
        {
            var result = await _userService.ResetPasswordAsync(userRegistration.Email, userRegistration.Password, token);

            return result is null ? NotFound() : (bool)result ? Ok("Password successfully changed.") : StatusCode(403, "Invalid reset token.");
        }
    }
}
