namespace AspStudio.Models.DTOs
{
    public class SubMenuDto
    {
        public string SubMenuName { get; set; }
        public string ControllerName { get; set; }
        public string IActionName { get; set; }
        public string MainMenuName { get; set; }
        public string MenuIcon { get; set; }
        public int MainMenuId { get; set; }
        public int ShowOrder { get; set; }
        public string RoleId { get; set; }

        public int Status { get; set; }
        public int Is_Active { get; set; }
        // public MainMenu MainMenu { get; set; }
        // public volunteerUser vUsers { get; set; }
        //public IdentityRole Role { get; set; }
    }
}
