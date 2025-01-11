using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AspStudio.Models;

public partial class MainMenu
{
    [Key]
    public int Id { get; set; }

    public string MainMenuName { get; set; } = null!;

    public string MenuIcon { get; set; } = null!;

    public int ShowOrder { get; set; }

    [InverseProperty("MainMenu")]
    public virtual ICollection<SubMenu> SubMenu { get; set; } = new List<SubMenu>();
}
