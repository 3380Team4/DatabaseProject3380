using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Transactions
    {
        public string TransactionId { get; set; }
        public DateTime DateOfSale { get; set; }
        public string MerchId { get; set; }
        public decimal SaleAmount { get; set; }
        public string SellerEmployeeId { get; set; }
        public int GuestId { get; set; }
        public int Quantity { get; set; }

        public Customers Guest { get; set; }
        public Merchandise Merch { get; set; }
        public Employees SellerEmployee { get; set; }
    }
}
