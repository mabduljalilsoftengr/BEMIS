using AspStudio.Data;
using Microsoft.AspNetCore.Identity;

namespace AspStudio.Models.ViewModels.UserVM
{
    public class UserRoleViewModel
    {
        public List<BEMISUser> users { get; set; }
        public List<IdentityRole> roles { get; set; }
       
    }

}
