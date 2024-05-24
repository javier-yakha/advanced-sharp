using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;
using ExportLibrary;

namespace LibraryUI
{
    public class CsvManagerUI
    {
        private readonly SqlManager SQL = new();
        // private readonly CsvManager CSV = new();

        public async Task RunCsv()
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
                        await SaveIntoCsvProducts();
                        break;
                    case '2':
                        await SaveIntoCsvInventories();
                        break;
                    case '3':
                        await SaveIntoCsvReturnProductForms();
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
        }

        private async Task SaveIntoCsvProducts()
        {
            string name = "Products";
            Console.WriteLine($"Saving {name} into CSV file.");

            List<Product> products = await SQL.ExecuteRetrieveAllProducts(false);

            await CsvManager.SaveTableRowsIntoCsv(products, name);
        }

        private async Task SaveIntoCsvInventories()
        {
            string name = "Inventories";
            Console.WriteLine($"Saving {name} into CSV file.");

            List<Inventory> inventories = await SQL.ExecuteRetrieveAllInventories();

            await CsvManager.SaveTableRowsIntoCsv(inventories, name);
        }

        private async Task SaveIntoCsvReturnProductForms()
        {
            string name = "ReturnProductForms";
            Console.WriteLine($"Saving {name} into CSV file.");

            List<ReturnProductForm> returnProductForms = await SQL.ExecuteRetrieveAllReturnProductForms();

            await CsvManager.SaveTableRowsIntoCsv(returnProductForms, name);
        }
    }
}
