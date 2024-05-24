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

        public void RunCsv()
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

        private async void SaveIntoCsvProducts()
        {
            Console.WriteLine("Saving Products into CSV file.");

            List<Product> products = await SQL.ExecuteRetrieveAllProducts(false);
            List<IDataBaseModel> dataBaseModels = new(products);

            CsvManager.SaveTableRowsIntoCsv(dataBaseModels, "Products");
        }

        private void SaveIntoCsvInventories()
        {
            Console.WriteLine("Saving inventories into CSV file.");

            List<Inventory> inventories = SQL.ExecuteRetrieveAllInventories();
            List<IDataBaseModel> dataBaseModels = new(inventories);

            CsvManager.SaveTableRowsIntoCsv(dataBaseModels, "Inventories");
        }

        private async void SaveIntoCsvReturnProductForms()
        {
            Console.WriteLine("Saving return product forms into CSV file.");

            List<ReturnProductForm> returnProductForms = await SQL.ExecuteRetrieveAllReturnProductForms();
            List<IDataBaseModel> dataBaseModels = new(returnProductForms);

            CsvManager.SaveTableRowsIntoCsv(dataBaseModels, "ReturnProductForms");
        }
    }
}
