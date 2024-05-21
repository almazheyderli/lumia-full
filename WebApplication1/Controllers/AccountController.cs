using Lumbia.Core.DTOs.AccountDto;
using Lumbia.Core.Models;
using Lumbia.Helpers.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lumbia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
       
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Surname = registerDto.Surname,
                Name = registerDto.Name,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto,string? ReturnUrl=null)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UsernameorEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UsernameorEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "email ve ya password yanlisdir");
                    return View();
                }
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "birazdan yeniden cehd edin");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password is wrong");
                return View();
            }
                await _signInManager.SignInAsync(user, loginDto.isRemember);
            if (ReturnUrl != null)
            {
                RedirectToAction(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");

            }
        

        public async Task <IActionResult> CreateRole()
        {
            foreach(var item in Enum.GetValues(typeof(UserRole)))
                {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = item.ToString(),
                });
            }
            return Ok();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

}