using System;
using System.Collections.Generic;

namespace WriteErase.Models;

public partial class Unit
{
    public string? Name { get; set; }

    public int IdUnit { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
