using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces;

namespace PersonalFinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> Register(UserRegistrationAndRestoration userRegistration)
        {
            try
            {
                await _userService.RegisterUserAsync(userRegistration.Email, userRegistration.Password);
                return Ok("User successfully registered.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(userLogin.Email);

                if (user is null)
                    return NotFound("User not found");

                if (user.VerifiedAt is null)
                    return StatusCode(403, "User not verified");

                var token = await _authService.UserLogin(userLogin.Email, userLogin.Password);
                return Ok(token);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, "Wrong email or password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(string verificationToken)
        {
            try
            {
                return await _userService.VerifyUserAsync(verificationToken).ConfigureAwait(false)
                    ? Ok("User verified.")
                    : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                return await _userService.ForgotPasswordAsync(email).ConfigureAwait(false)
                    ? Ok("You may reset your password.")
                    : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserRegistrationAndRestoration userRegistration, string token)
        {
            try
            {
                var result = await _userService.ResetPasswordAsync(userRegistration.Email, userRegistration.Password, token).ConfigureAwait(false);

                return result is null ? NotFound() : (bool)result ? Ok("Password successfully changed.") : StatusCode(403, "Invalid reset token.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
