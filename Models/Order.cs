using System;
using System.Collections.Generic;

namespace WriteErase.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? DateOrder { get; set; }

    public DateOnly? DateDelivery { get; set; }

    public int? PickUp { get; set; }

    public int? Code { get; set; }

    public int Status { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PickUpPoint? PickUpNavigation { get; set; }

    public virtual Status StatusNavigation { get; set; } = null!;
}
