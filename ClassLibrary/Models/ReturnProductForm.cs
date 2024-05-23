using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
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
        public Enums.DesiredSolutions DesiredSolution { get; set; }
        public string GetDesiredSolution()
        {
            return DesiredSolution switch
            {
                Enums.DesiredSolutions.CashRefund => "Cash refund",
                Enums.DesiredSolutions.Replace => "Replacement",
                Enums.DesiredSolutions.VoucherCard => "Voucher card",
                _ => "Voucher card"
            };
        }
        public DateTime? DateReceived { get; set; } = null;
    }
}
