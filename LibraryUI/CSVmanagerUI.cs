using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUI
{
    public class CSVmanagerUI
    {
        private readonly SqlManager SQL = new();

        public void RunCSV()
        {
            bool exit = false;
            Console.WriteLine("Welcome to the CSV manager.");
            Console.WriteLine("What records would you like to save in CSV?");
            while (!exit)
            {
                Console.WriteLine("Save Products(1)"
                    + "\nSave Inventories(2)"
                    + "\nSave return product forms(3)"
                    + "\nExit(any)");
                char selection = Console.ReadKey(true).KeyChar;

                switch (selection)
                {
                    case '1':
                        SaveIntoCsvProducts();
                        break;
                    case '2':
                        SaveIntoCsvInventories();
                        break;
                    case '3':
                        SaveIntoCsvReturnProductForms();
                        break;
                    default:
                    exit = true;
                        break;
                }
            }
        }

        private void SaveIntoCsvProducts()
        {
            Console.WriteLine("Saving Products into CSV file.");

            List<Product> products = SQL.ExecuteRetrieveAllProducts();

            CsvManager.SaveProductsIntoCsv(products);
        }

        private void SaveIntoCsvInventories()
        {
            Console.WriteLine("Saving inventories into CSV file.");

            List<Inventory> inventories = SQL.ExecuteRetrieveAllInventories();

            CsvManager.SaveInventoriesIntoCsv(inventories);
        }

        private void SaveIntoCsvReturnProductForms()
        {
            Console.WriteLine("Saving return product forms into CSV file.");

            List<ReturnProductForm> returnProductForms = SQL.ExecuteRetrieveAllReturnProductForms();

            CsvManager.SaveReturnProductFormsIntoCsv(returnProductForms);
        }
    }
}
