using DataModels.Builders;
using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUI
{
    public class SqlManagerUI
    {
        private readonly SqlManager SQL = new();
        public async Task RunSql()
        {
            bool exit = false;
            Console.WriteLine("Welcome to the SQL manager.");
            while (!exit)
            {
                Console.WriteLine("Show All(1)"
                    + "\nAdd Product(2)"
                    + "\nUpdate Stock(3)"
                    + "\nDelete Product(4)"
                    + "\nReturn Product Form(5)"
                    + "\nSearch Product(6)"
                    + "\nExit(any)");
                char selection = Console.ReadKey(true).KeyChar;

                switch (selection)
                {
                    case '1':
                        await ShowAllProducts();
                        break;
                    case '2':
                        await AddProduct();
                        break;
                    case '3':
                        await UpdateProductStock();
                        break;
                    case '4':
                        await DeleteProduct();
                        break;
                    case '5':
                        await ReturnProductForm();
                        break;
                    case '6':
                        await SearchProduct();
                        break;
                    default:
                        await PreventClose();
                        exit = true;
                        break;
                }
            }
        }

        public async Task ShowAllProducts()
        {
            List<Product> products = await SQL.ExecuteRetrieveAllProducts(true);

            if (products.Count == 0)
            {
                Console.WriteLine("Empty table <Products>");
            }
            foreach (Product product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }

        public async Task AddProduct()
        {
            Console.WriteLine("Adding a new Product.");

            Product product = new ProductBuilder().CreateProduct();

            await SQL.ExecuteAddProduct(product);

            Console.WriteLine("Product added successfully.");
        }

        public async Task UpdateProductStock()
        {
            Console.WriteLine("Updating stock for a product.");
            Console.WriteLine("What product would you like to update?");

            await SQL.RetrieveProductStock();

            string? title = null;
            while (title is null)
            {
                Console.Write("Product title to update: ");
                title = Console.ReadLine();
                if (title is not null && title.Length < 90)
                {
                    break;
                }
            }

            int stock = 0;
            bool stockStatus = false;
            while (!stockStatus)
            {
                Console.Write("New total stock: ");
                bool stockIntStatus = Int32.TryParse(Console.ReadLine(), out stock);
                stockStatus = stockIntStatus && stock >= 0;
            }

            await SQL.ExecuteUpdateProductStock(title, stock);
        }

        public async Task DeleteProduct()
        {
            Console.WriteLine("Deleting a product from the database.");
            Console.WriteLine("What product would you like to delete?");

            await SQL.RetrieveProductTitles();

            string? title = null;
            while (title is null)
            {
                Console.Write("Product to delete: ");
                
                title = Console.ReadLine();
                if (title is not null && title.Length < 90)
                {
                    break;
                }
                Console.WriteLine("Invalid title, try again.");
            }

            await SQL.DeleteProduct(title);
        }

        public async Task ReturnProductForm()
        {
            Console.WriteLine("Returning a product.");

            await SQL.RetrieveProductTitles();

            Console.WriteLine("Please choose the product to return.");

            string? title = null;
            while (title is null)
            {
                title = Console.ReadLine();
            }
            
            string? productId = await SQL.ReturnProductIdByTitle(title);

            if (productId is null)
            {
                Console.WriteLine($"No product matched by the name {title}.");
                return;
            }

            Console.WriteLine("Fill the return product form.");

            ReturnProductFormBuilder formBuilder = new(productId);

            ReturnProductForm form = formBuilder.CreateReturnProductForm();

            await SQL.ExecuteReturnProduct(form);
        }

        public async Task SearchProduct()
        {
            throw new NotImplementedException();
            // TODO - implement search product
            /*
            Console.WriteLine("Searching a product.");
            Console.WriteLine("Enter the product name to search.");
            string? title = null;
            while (title is null)
            {
                title = Console.ReadLine();
            }
            var productList = SQL.ExecuteRetrieveAllProductsByName();
            */
        }
        public Task PreventClose()
        {
            Console.WriteLine("Prevent close");
            return Task.CompletedTask;
        }
    }
}
