using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Models
    {
        public class Product
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public DateTime DateAdded { get; set; }
            public string Description {  get; set; }
            public decimal DiscountPrice { get; set; }
            public bool Enabled { get; set; }

            public override string ToString()
            {
                string price = String.Format("{0:0.00}", DiscountPrice < Price ? DiscountPrice : Price);
                string prices = $"${price}";
                string enabled = Enabled ? "available" : "unavailable";

                return $"\n{Title}"
                    + $"\n\tPrice: {prices}"
                    + $"\n\tAdded: {DateAdded}"
                    + $"\n\tDescription: {Description}"
                    + $"\n\tStatus: {enabled}";
            }
        }

        public class Inventory
        {
            public string Id { get; set; }
            public string ProductId { get; set; }
            public DateTime LastUpdated { get; set; }
            public int TotalStock{ get; set; }
        }
    }
}
