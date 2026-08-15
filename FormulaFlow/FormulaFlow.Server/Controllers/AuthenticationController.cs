using FormulaFlow.Data.Models;
using FormulaFlow.Data.Role;
using FormulaFlow.Server.Dto.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FormulaFlow.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password are required.");

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict("A user with that email already exists.");

            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Ensure default role exists and assign it
            var roleExists = await _roleManager.RoleExistsAsync(RoleNames.User);
            if (!roleExists)
            {
                var roleCreate = await _roleManager.CreateAsync(new IdentityRole(RoleNames.User));
                if (!roleCreate.Succeeded)
                    return StatusCode(500, "Failed to create default role.");
            }

            var addToRole = await _userManager.AddToRoleAsync(user, RoleNames.User);
            if (!addToRole.Succeeded)
                return StatusCode(500, "Failed to assign role to user.");

            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            var roles = await _userManager.GetRolesAsync(user);
            var session = new SessionDto { Email = user.Email, Roles = roles };
            return Ok(session);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password are required.");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized();

            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, isPersistent: false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var session = new SessionDto { Email = user.Email, Roles = roles };

            return Ok(session);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
