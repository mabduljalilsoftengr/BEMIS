using AspStudio.Data;
using AspStudio.Helper;
using AspStudio.Models;
using AspStudio.Models.DTOs;
using AspStudio.Models.DTOs.UserD;
using AspStudio.Models.ViewModels.UserVM;
using AspStudio.Repositories.UserRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static AspStudio.Helper.Utility;
using static System.Net.Mime.MediaTypeNames;

namespace AspStudio.Controllers
{
    public class AuthController : Controller
    {
        //private readonly IUser _User;
        private readonly UserManager<BEMISUser> _userManager;
        private readonly SignInManager<BEMISUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private readonly ApplicationDbContext _IdentityContext;
        private readonly BEMISDbContext _bemisContext;
        private readonly Utility _Utility;
        //private readonly IAuth _auth;
        //IUser user,
        public AuthController( UserManager<BEMISUser> userManager,
           SignInManager<BEMISUser> signInManager, RoleManager<IdentityRole> roleManager,
           ApplicationDbContext appContext, Utility utility, IConfiguration configuration
            , BEMISDbContext becontext)
        {
            //_User = user;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _IdentityContext = appContext;
            _bemisContext = becontext;
            _Utility = utility;
            _configuration = configuration;
        }

      

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UsersDTO user)
        {
            try
            {

                var email = user.Email;
                var usr = _IdentityContext.Users.Where(x => x.Email == email
                && x.Isverified == "Verified").FirstOrDefault();

                if (usr != null)
                {
                    TempData["ErrorMessage"] = "Email already used, please try another.";
                    return RedirectToAction("Register", "Auth");
                }

                //user.Password = "Abcd@1234kbjdbkjsd98746589475bnv485";
                // Create a new user
                var user1 = new BEMISUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    Gender = user.Gender,
                    UserName = user.Email,
                    NormalizedUserName = user.UserName,
                    Email = user.Email,
                    NormalizedEmail = user.Email, 
                    PasswordHash = user.Password, 
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = user.Mobile,
                    Isverified = "Verified"//"Pending"
                };

                var result = await _userManager.CreateAsync(user1, user.Password); // Set the user's password

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Record added successfully.";
                    // User creation succeeded
                    // You can optionally sign in the user here if needed
                    return RedirectToAction("Login", "Auth"); // Redirect to the login page
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred.";
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        TempData["ErrorMessage"] += " " + error.Description; // Concatenate error descriptions
                    }
                    return RedirectToAction("Register", "Auth");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred.";
                string message = ex.Message;
                return RedirectToAction("Register", "Auth");
            }

            //await _User.RegisterUser(user);
            //return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsersDTO user)
        {
            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var authenticatedUser = await _userManager.FindByEmailAsync(user.Email);
                if (authenticatedUser != null && authenticatedUser.Isverified == "Verified")
                {
                    #region Check verified user
                    // Get the user by username
                    var loggedInUser = await _userManager.FindByEmailAsync(user.Email);
                    var loggedInUserRole = new IdentityRole();

                    if (loggedInUser != null)
                    {
                        // Get the roles for the logged-in user
                        var roles = await _userManager.GetRolesAsync(loggedInUser);

                        foreach (var role in roles)
                        {
                            loggedInUserRole = await _roleManager.FindByNameAsync(role);
                        }


                        // Now you have the user and role information
                        // You can access properties of the loggedInUser object and roles list

                        List<SubMenuDto> subMenuDtos = (from sm in _bemisContext.SubMenu
                                                        join mm in _bemisContext.MainMenu on sm.MainMenuId equals mm.Id
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
                            //Id = user.Id,
                            RoleId = loggedInUserRole.Id,
                            UserId = loggedInUser.Id,
                            UserName = loggedInUser.UserName,
                            Password = loggedInUser.PasswordHash,
                            UserRoleId = loggedInUserRole.Id,
                            RoleName = loggedInUserRole.Name,
                            MenuList = subMenuDtos
                        };

                        _Utility.AddSession(UserSession.BEMISLogin, lmm);
                        return RedirectToAction("Index", "Home");

                    }
                    #endregion

                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
                return RedirectToAction("Login", "Auth");

            }
            else
            {
                // Authentication failed, handle errors
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return RedirectToAction("Login", "Auth");// Return to the login view with errors
            }

        }

        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                TempData["ErrorMessage"] = "Please enter role name.";
                // Handle role creation failure, e.g., display error messages
                return RedirectToAction("AddRole", "Auth");
            }
            // Check if the role already exists
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                // If the role doesn't exist, create it
                //var role = new IdentityRole(roleName);
                var role = new IdentityRole
                {
                    Name = roleName,
                    // You can generate a new ConcurrencyStamp using UserManager
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Record added successfully.";
                    // Role was created successfully
                    return RedirectToAction("AddRole", "Auth"); // You can redirect to a page showing all roles or wherever you want
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred.";
                    // Handle role creation failure, e.g., display error messages
                    return RedirectToAction("AddRole", "Auth");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Role with same name already exist.";
                // Role already exists, handle accordingly
                return RedirectToAction("AddRole", "Auth");
            }


        }

        [HttpGet]
        public IActionResult AssignRoleToUser()
        {
            // Retrieve all users and roles from the database using your context
            var users = _IdentityContext.Users.ToList();
            var roles = _IdentityContext.Roles.ToList();

            // You can pass the users and roles to the view as needed, for example, in a ViewModel
            var viewModel = new UserRoleViewModel
            {
                users = users,
                roles = roles
            };

            // Send the data to the view
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> AssignRole_To_User(string RoleID, string UserID)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            var role = await _roleManager.FindByIdAsync(RoleID);

            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    // Role assignment was successful
                    return Json(new { status = "success", message = "Role assigned successfully" });
                }
                else
                {
                    // Role assignment failed, handle the error
                    return Json(new { status = "error", message = "Failed to assign role" });
                }
            }
            else
            {
                // User or role not found, handle the error
                return Json(new { status = "error", message = "User or role not found" });
            }


        }

    }
}
