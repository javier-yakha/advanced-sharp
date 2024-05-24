using DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
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
