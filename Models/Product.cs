using System;
using System.Collections.Generic;

namespace WriteErase.Models;

public partial class Product
{
    public string ArticleNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Unit { get; set; }

    public decimal Cost { get; set; }

    public int? MaxDiscountAmount { get; set; }

    public int Manufacturer { get; set; }

    public int Supplier { get; set; }

    public int Category { get; set; }

    public decimal? CurrentDiscount { get; set; }

    public int QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual Category CategoryNavigation { get; set; } = null!;

    public virtual Manufacturer ManufacturerNavigation { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Supplier SupplierNavigation { get; set; } = null!;

    public virtual Unit UnitNavigation { get; set; } = null!;
}
