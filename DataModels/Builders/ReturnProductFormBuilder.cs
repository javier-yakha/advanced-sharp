using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Builders
{
    public class ReturnProductFormBuilder
    {
        private Models.ReturnProductForm form = new();
        public ReturnProductFormBuilder(string productId)
        {
            form.ProductId = productId;
        }

        public Models.ReturnProductForm CreateReturnProductForm()
        {
            CreateUsed();
            CreateDamagedOnArrival();
            CreateWorking();
            CreateCausedDamage();
            CreateComplaint();
            CreateDateOrdered();
            CreateProductArrived();
            CreateDesiredSolution();
            CreateDateReceived();

            return form;
        }

        private void CreateUsed()
        {
            Console.WriteLine("Was it used? (Y)es (N)o skip(any)");
            char key = Console.ReadKey(true).KeyChar;
            if (char.ToLower(key) == 'y') form.Used = true;
            else if (char.ToLower(key) == 'n') form.Used = false;
        }

        private void CreateDamagedOnArrival()
        {
            Console.WriteLine("Was it damaged on arrival? (Y)es (N)o skip(any)");
            char key = Console.ReadKey(true).KeyChar;
            if (char.ToLower(key) == 'y') form.DamagedOnArrival = true;
            else if (char.ToLower(key) == 'n') form.DamagedOnArrival = false;
        }

        private void CreateWorking()
        {
            Console.WriteLine("Is it working? (Y)es (N)o skip(any)");
            char key = Console.ReadKey(true).KeyChar;
            if (char.ToLower(key) == 'y') form.Working = true;
            else if (char.ToLower(key) == 'n') form.Working = false;
        }

        private void CreateCausedDamage()
        {
            Console.WriteLine("Did it cause damage? (Y)es (N)o skip(any)");
            char key = Console.ReadKey(true).KeyChar;
            if (char.ToLower(key) == 'y') form.CausedDamage = true;
            else if (char.ToLower(key) == 'n') form.CausedDamage = false;
        }

        private void CreateComplaint()
        {
            Console.WriteLine("Formal complaint: ");
            string complaint = string.Empty;
            bool complaintStatus = false;
            while (!complaintStatus)
            {
                complaint = Console.ReadLine();
                if (complaint is not null && complaint.Length > 10)
                {
                    complaintStatus = true;
                }
            }
            form.Complaint = complaint;
        }
        private void CreateDateOrdered()
        {
            Console.WriteLine("When was it ordered?");
            Console.WriteLine("Yesterday(1) - Last week(2) - Last month(3) - Longer(4)");
            char key = Console.ReadKey(true).KeyChar;
            DateTime dateOrdered = DateTime.Now;
            form.DateOrdered = key switch
            {
                '1' => dateOrdered.AddDays(-1),
                '2' => dateOrdered.AddDays(-7),
                '3' => dateOrdered.AddMonths(-1),
                '4' => dateOrdered.AddMonths(-6),
                _ => dateOrdered
            };
        }

        private void CreateProductArrived()
        {
            Console.WriteLine("Did the product arrive? (Y)es No(any)");
            char key = Console.ReadKey(true).KeyChar;
            if (char.ToLower(key) == 'y') form.ProductArrived = true;
            else form.ProductArrived = false;
        }

        private void CreateDesiredSolution()
        {
            Console.WriteLine("What is the desired solution after return?");
            Console.WriteLine("(1)Cash refund - (2)Replacement - (any)Voucher card");
            char key = Console.ReadKey(true).KeyChar;
            form.DesiredSolution = key switch
            {
                '1' => Enums.DesiredSolutions.CashRefund,
                '2' => Enums.DesiredSolutions.Replace,
                _ => Enums.DesiredSolutions.VoucherCard
            };
        }
        private void CreateDateReceived()
        {
            Console.WriteLine("When was it received?");
            Console.WriteLine("(1)Today - (2)Yesterday - (3)Last Week - (4)Last Month - (Any)It was never received");
            char key = Console.ReadKey(true).KeyChar;
            DateTime dateReceived = DateTime.Now;
            form.DateReceived = key switch
            {
                '1' => dateReceived,
                '2' => dateReceived.AddDays(-1),
                '3' => dateReceived.AddDays(-7),
                '4' => dateReceived.AddMonths(-1),
                _ => null,
            };
        }
    }
}
