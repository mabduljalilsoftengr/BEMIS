using AspStudio.Data;
using AspStudio.Models.DTOs.UserD;
using Microsoft.AspNetCore.Identity;

namespace AspStudio.Repositories.UserRepo
{
    public class User_Repo : IUser
    {
        private readonly UserManager<BEMISUser> _userManager;

        public User_Repo(UserManager<BEMISUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterUser(UsersDTO model)
        {
            try
            {
                var user = new BEMISUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    Gender = model.Gender,
                    Isverified = "1"//model.Isverified // Assuming Isverified is part of UsersDTO
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Optionally, you can assign roles or do additional logic
                }
                else
                {
                    // Handle errors if user creation fails
                    string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception("User registration failed: " + errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                string message = ex.Message + ex.InnerException;
            }
        }
    }

}
