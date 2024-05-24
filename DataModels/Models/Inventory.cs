using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Inventory
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public DateTime LastUpdated { get; set; }
        public int TotalStock { get; set; }
    }
}
