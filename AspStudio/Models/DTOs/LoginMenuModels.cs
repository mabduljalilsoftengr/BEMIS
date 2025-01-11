namespace AspStudio.Models.DTOs
{
    public class LoginMenuModels
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string UserRoleId { get; set; }
        public string RoleName { get; set; }

        public List<SubMenuDto> MenuList { get; set; } = new List<SubMenuDto>();
    }
}
