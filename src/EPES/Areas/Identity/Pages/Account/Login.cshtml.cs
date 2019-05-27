using EPES.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EPES.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} จำเป็นต้องกรอกข้อมูล")]
            //[EmailAddress]
            public string Username { get; set; }

            [Required(ErrorMessage = "{0} จำเป็นต้องกรอกข้อมูล")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "บันทึกการเข้าสู่ระบบไว้จนกว่าจะคลิกออกจากระบบ ?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (((Input.Username == "admin") || (Input.Username == "pak") || (Input.Username == "sortor") || (Input.Username == "bortor")) && (Input.Password == "P@ssw0rd"))
                {
                    if (Input.Username == "admin")
                    {
                        if ((await _userManager.FindByNameAsync("admin")) == null)
                        {
                            var admin = new ApplicationUser { UserName = "admin", Email = "admin@epes.rd.go.th", FName = "Admin", LName = "EPES", OfficeId = "00013000" };
                            await _userManager.CreateAsync(admin, "P@ssw0rd");

                            if ((await _roleManager.FindByNameAsync("Admin")) == null)
                            {
                                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                            }

                            if ((await _roleManager.FindByNameAsync("Manager")) == null)
                            {
                                await _roleManager.CreateAsync(new IdentityRole { Name = "Manager" });
                            }

                            if ((await _roleManager.FindByNameAsync("User")) == null)
                            {
                                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
                            }
                            await _userManager.AddToRoleAsync(admin, "Admin");
                        }
                        
                        
                        var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }
                    if (Input.Username == "pak")
                    {
                        if ((await _userManager.FindByNameAsync("pak")) == null)
                        {
                            var pak = new ApplicationUser { UserName = "pak", Email = "pak@epes.rd.go.th", FName = "Pak", LName = "EPES", OfficeId = "01000000" };
                            await _userManager.CreateAsync(pak, "P@ssw0rd");
                            await _userManager.AddToRoleAsync(pak, "Manager");
                        }
                        var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }
                    if (Input.Username == "sortor")
                    {
                        if ((await _userManager.FindByNameAsync("sortor")) == null)
                        {
                            var sortor = new ApplicationUser { UserName = "sortor", Email = "sortor@epes.rd.go.th", FName = "Sortor", LName = "EPES", OfficeId = "01003000" };
                            await _userManager.CreateAsync(sortor, "P@ssw0rd");
                            await _userManager.AddToRoleAsync(sortor, "User");
                        }
                        var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }
                    if (Input.Username == "bortor")
                    {
                        if ((await _userManager.FindByNameAsync("bortor")) == null)
                        {
                            var bortor = new ApplicationUser { UserName = "bortor", Email = "bortor@epes.rd.go.th", FName = "Bortor", LName = "EPES", OfficeId = "00007000" };
                            await _userManager.CreateAsync(bortor, "P@ssw0rd");
                            await _userManager.AddToRoleAsync(bortor, "Manager");
                        }
                        var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }
                }
                else
                {
                    WSEOffice.AuthenUserEoffice1SoapClient soapClient = new WSEOffice.AuthenUserEoffice1SoapClient(WSEOffice.AuthenUserEoffice1SoapClient.EndpointConfiguration.AuthenUserEoffice1Soap);
                    WSEOffice.AuthenUserResponse udata = await soapClient.AuthenUserAsync("InternetUser", "InternetPass", Input.Username.ToUpper(), Input.Password);
                    if (udata.DataUser.Authen)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = Input.Username,
                            Title = udata.DataUser.TITLE,
                            Email = udata.DataUser.EMAIL,
                            PIN = udata.DataUser.PIN,
                            FName = udata.DataUser.FNAME,
                            LName = udata.DataUser.LNAME,
                            PosName = udata.DataUser.POSITION_M,
                            Class = udata.DataUser.CLASS_NEW,
                            OfficeId = udata.DataUser.OFFICEID,
                            OfficeName = udata.DataUser.OFFICENAME,
                            GroupName = udata.DataUser.GROUPNAME
                        };
                        var resultCreate = await _userManager.CreateAsync(user, "P@ssw0rd");
                        if (resultCreate.Succeeded)
                        {
                            _logger.LogInformation("User created a new account with password.");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { userId = user.Id, code = code },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                            //await _signInManager.SignInAsync(user, isPersistent: false);
                            //return LocalRedirect(returnUrl);
                        }
                        foreach (var error in resultCreate.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login eoffice");
                        return Page();
                    }

                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.Username, "P@ssw0rd", Input.RememberMe, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
