using System;
using System.Threading.Tasks;
using Lab1_new.Data;
using Lab1_new.Models;
using Lab1_new.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Lab1_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost]
        [Route("register")]  //api/authentication/register
        public async Task<ActionResult> RegisterUser(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if(result.Succeeded)
            {
                return Ok(new RegisterResponse { ConfirmationToken = user.SecurityStamp });
            }

            return BadRequest(result.Errors);
        }


        [HttpPost]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmUser(ConfirmUserRequest confirmUserRequest)
        {
            var user = new ApplicationUser
            {
                Email = confirmUserRequest.Email,
                UserName = confirmUserRequest.Email,
                
            };

            var toConfirm = _context.ApplicationUsers
                .Where(u => u.Email == confirmUserRequest.Email && u.SecurityStamp == confirmUserRequest.ConfirmationToken)
                .FirstOrDefault();

            if (toConfirm != null)
            {
                toConfirm.EmailConfirmed = true;
                _context.Entry(toConfirm).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }


    }
}
