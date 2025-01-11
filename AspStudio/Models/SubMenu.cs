using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AspStudio.Models;

public partial class SubMenu
{
    [Key]
    public int Id { get; set; }

    public string SubMenuName { get; set; } = null!;

    [Column("IActionName")]
    public string IactionName { get; set; } = null!;

    public string ControllerName { get; set; } = null!;

    public int MainMenuId { get; set; }

    [StringLength(450)]
    public string RoleId { get; set; } = null!;

    public int Status { get; set; }

    [Column("Is_Active")]
    public int IsActive { get; set; }

    [ForeignKey("MainMenuId")]
    [InverseProperty("SubMenu")]
    public virtual MainMenu MainMenu { get; set; } = null!;
}
