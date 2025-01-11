// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AspStudio.Models.DTOs;
using static AspStudio.Helper.Utility;
using AspStudio.Data;
using AspStudio.Helper;
using AspStudio.Models;

namespace AspStudio.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        
       // private readonly IdentityUser _User;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly BEMISDbContext _dbContext;
        //private readonly Utility _Utility;
        // IdentityUser user, 
        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager,
             //Utility utility, 
             ILogger<LoginModel> logger)
        {
            //_User = user;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _dbContext = dbContext;
            // _Utility = utility;
        }
        BEMISDbContext dbContext = new BEMISDbContext();
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    var authenticatedUser = await _userManager.FindByEmailAsync(Input.Email);
                    if (authenticatedUser != null //&& authenticatedUser.Isverified == "Verified"
                        )
                   
                    {
                        #region Check verified user
                        //// Get the user by username
                        var loggedInUser = await _userManager.FindByEmailAsync(Input.Email);
                        var loggedInUserRole = new IdentityRole();

                        if (loggedInUser != null)
                        {
                            // Get the roles for the user
                            var roles = await _userManager.GetRolesAsync(loggedInUser);

                            foreach (var role in roles)
                            {
                                loggedInUserRole = await _roleManager.FindByNameAsync(role);
                            }


                            // Now you have the user and role information
                            // You can access properties of the loggedInUser object and roles list

                            List<SubMenuDto> subMenuDtos = (from sm in dbContext.SubMenu
                                                            join mm in dbContext.MainMenu on sm.MainMenuId equals mm.Id
                                                            where sm.RoleId == loggedInUserRole.Id
                                                            && sm.IsActive == 1 && sm.Status == 1
                                                            select new SubMenuDto
                                                            {
                                                                SubMenuName = sm.SubMenuName,
                                                                ControllerName = sm.ControllerName,
                                                                IActionName = sm.IactionName,
                                                                MainMenuId = sm.MainMenuId,
                                                                RoleId = sm.RoleId,
                                                                Status = sm.Status,
                                                                Is_Active = sm.IsActive,
                                                                MainMenuName = mm.MainMenuName,
                                                                MenuIcon = mm.MenuIcon,
                                                                ShowOrder = mm.ShowOrder,
                                                            })
                                                            .ToList();

                            var lmm = new LoginMenuModels
                            {
                                //Id = _User.Id,
                                RoleId = loggedInUserRole.Id,
                                UserId = loggedInUser.Id,
                                UserName = loggedInUser.UserName,
                                Password = loggedInUser.PasswordHash,
                                UserRoleId = loggedInUserRole.Id,
                                RoleName = loggedInUserRole.Name,
                                MenuList = subMenuDtos
                            };

                                //_Utility.AddSession(UserSession.BEMISLogin, lmm);
                            //    //return RedirectToAction("Index", "Dashboard");
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);

                        }
                       
                        #endregion

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                        //return RedirectToAction("Login", "Auth");
                    }
                    //return RedirectToAction("Login", "Auth");

                   
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

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
