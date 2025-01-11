using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspStudio.Data
{
    public class BEMISUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(20)")]
        public string Mobile { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(20)")]
        public string Gender { get; set; }

        public string Isverified { get; set; }
    }
}
