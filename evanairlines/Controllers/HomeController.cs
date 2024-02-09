using evanairlines.Models;
using evanairlines.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;

namespace evanairlines.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<UserModel> userManager;
        private readonly SignInManager<UserModel> signInManager;
        private readonly IEmailService emailService;

        public HomeController(ILogger<HomeController> _logger, UserManager<UserModel> _userManager, SignInManager<UserModel> _signInManager, IEmailService _emailService)
        {
            logger = _logger;
            userManager = _userManager;
            signInManager = _signInManager;
            emailService = _emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel _user, string password)
        {
            var user = await userManager.FindByEmailAsync(_user.Email);

            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, password);

                if (passwordCheck)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid Credentials. Please try again";
                    return View();
                }
            }
            else
            {
                // Handle user not found
                TempData["Error"] = "User not found";
                return View();
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            // Optional: You can also sign the user out of any external authentication providers if they were used.
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Redirect to home or any other page after logout
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel _user, string password, string confirm_password)
        {
            if (ModelState.IsValid)
            {
                if (userManager.Users.Any(u => u.Email == _user.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email is already in use.");
                    return View();
                }

                if (password != confirm_password)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View();
                }

                if (string.IsNullOrEmpty(_user.UserName) || _user.UserName.Contains(" "))
                {
                    ModelState.AddModelError(string.Empty, "Invalid username. Please enter a valid username.");
                    return View();
                }

                var user = new UserModel
                {
                    UserName = _user.UserName, // Assuming email is used as the username
                    Email = _user.Email,
                    PasswordHash = password
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    //Store their Role
                    if (user.Email == "johnsonjevane@hotmail.com")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }

                    // Sign in the user
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var model = new PasswordResetRequestModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(PasswordResetRequestModel _model)
        {

            // Create a model to pass the email
            var model = new PasswordResetRequestModel { email = _model.email };

            // Call the method to request password reset
            await RequestPasswordReset(model);

            // Redirect or return a view as needed
            return View(model); // Replace "Index" with the appropriate action
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestModel model)
        {
            var user = await userManager.FindByEmailAsync(model.email);
            if (user == null)
            {
                // User not found
                return BadRequest("Invalid email address");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetLink = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/resetpassword?token={encodedToken}&email={model.email}";

            // Send the reset link via email
            emailService.SendPasswordResetEmail(user.Email, resetLink);
            model.emailSent = true;

            return View(model);
        }

        [HttpGet("resetpassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new PasswordResetModel
            {
                token = token,
                email = email
            };
            if (model.email == null)
            {
                // User not found
                return BadRequest("Invalid email address");
            }
            return View(model);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetModel model)
        {
            var user = await userManager.FindByEmailAsync(model.email);
            var decodedToken = WebEncoders.Base64UrlDecode(model.token);
            var token = Encoding.UTF8.GetString(decodedToken);

            var result = await userManager.ResetPasswordAsync(user, token, model.new_password);
            if (result.Succeeded)
            {
                return Ok("Password reset successfully");
            }

            return BadRequest("Password reset failed");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}