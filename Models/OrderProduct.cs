using System;
using System.Collections.Generic;

namespace WriteErase.Models;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string ProductArticleNumber { get; set; } = null!;

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductArticleNumberNavigation { get; set; } = null!;
}
