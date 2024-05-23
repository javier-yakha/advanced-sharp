using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Models
    {
        public enum DesiredSolutions
        {
            CashRefund,
            Replace,
            VoucherCard
        }
        public class Product
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public DateTime DateAdded { get; set; }
            public string Description {  get; set; }
            public decimal DiscountPrice { get; set; }
            public bool Enabled { get; set; }
            public int? TotalStock { get; set; }

            public override string ToString()
            {
                string price = String.Format("{0:0.00}", DiscountPrice < Price ? DiscountPrice : Price);
                string prices = $"${price}";
                string enabled = Enabled ? "available" : "unavailable";
                string stock = TotalStock is not null ? $"\n\tCurrent Stock: {TotalStock}" : "";

                return $"\n{Title}"
                    + $"\n\tPrice: {prices}"
                    + $"\n\tAdded: {DateAdded}"
                    + $"\n\tDescription: {Description}"
                    + $"\n\tStatus: {enabled}"
                    + $"{stock}";
            }
        }

        public class Inventory
        {
            public string Id { get; set; }
            public string ProductId { get; set; }
            public DateTime LastUpdated { get; set; }
            public int TotalStock{ get; set; }
        }

        public class ReturnProductForm
        {
            
            public string Id { get; set; }
            public string ProductId { get; set; }
            public bool? Used { get; set; } = null;
            public bool? DamagedOnArrival { get; set; } = null;
            public bool? Working { get; set; } = null;
            public bool? CausedDamage { get; set; } = null;
            public string Complaint { get; set; }
            public DateTime DateOrdered { get; set; }
            public bool ProductArrived { get; set; }
            public DesiredSolutions DesiredSolution { get; set; }
            public string GetDesiredSolution()
            {
                return DesiredSolution switch
                { 
                    DesiredSolutions.CashRefund => "Cash refund",
                    DesiredSolutions.Replace => "Replacement",
                    DesiredSolutions.VoucherCard => "Voucher card",
                    _ => "Voucher card"
                };
            }
            public DateTime? DateReceived { get; set; } = null;
        }
    }
}
