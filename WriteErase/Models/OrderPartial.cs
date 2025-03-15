using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteErase.Models
{
    public partial class Order
    {
        public string Background 
        {
            get
            {
                if (!OrderProducts.Any(it => it.ProductArticleNumberNavigation.QuantityInStock <= 3)) return "#20b2aa";
                return "#ff8c00";
            }
        }

        public double AllCost
        {
            get
            {
                double allCost = 0;
                foreach (var it in OrderProducts)
                {
                    allCost += (double)(it.Count * ((it.ProductArticleNumberNavigation.Cost / 100) * (100 - it.ProductArticleNumberNavigation.CurrentDiscount)));
                }
                return allCost;
            }
        }

        public double AllDiscount
        {
            get
            {
                double _fullCost = 0;
                double _fullDiscount = 0;
                double cost = 0;
                foreach (var it in OrderProducts)
                {
                    _fullCost += (double)(it.Count * ((it.ProductArticleNumberNavigation.Cost / 100) * (100 - it.ProductArticleNumberNavigation.CurrentDiscount)));
                    cost += (double)(it.Count * it.ProductArticleNumberNavigation.Cost);
                }
                if (cost > _fullCost) _fullDiscount = (1.0 - (_fullCost / cost)) * 100;
                return _fullDiscount;
            }
        }

        public bool IsVisFIO
        {
            get => IdUserNavigation != null;
        }
    }
}
