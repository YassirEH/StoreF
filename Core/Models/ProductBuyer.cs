using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ProductBuyer
    {
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public Product Product { get; set; }
        public Buyer Buyer { get; set; }
    }
}
