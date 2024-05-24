using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUI
{
    public class SqlManagerUI
    {
        private readonly SqlManager SQL = new();
        public void RunSql()
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
                        RetrieveAllProducts();
                        break;
                    case '2':
                        AddProduct();
                        break;
                    case '3':
                        UpdateProductStock();
                        break;
                    case '4':
                        DeleteProduct();
                        break;
                    case '5':
                        ReturnProductForm();
                        break;
                    case '6':
                        SearchProduct();
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
        }

        public void RetrieveAllProducts()
        {
            List<Product> products = SQL.ExecuteRetrieveAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("Empty table <Products>");

                return;
            }
            foreach (Product product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }

        public void AddProduct()
        {

            Console.WriteLine("Adding a new Product.");

            ProductBuilder productBuilder = new();
            Product product = productBuilder.CreateProduct();

            bool result = SQL.ExecuteAddProduct(product);

            // TODO - Implement check
            Console.WriteLine("Product added successfully.");
        }

        public void UpdateProductStock()
        {
            Console.WriteLine("Updating stock for a product.");
            Console.WriteLine("What product would you like to update?");

            SQL.RetrieveProductStock();

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

            SQL.ExecuteUpdateProductStock(title, stock);
        }

        public void DeleteProduct()
        {
            Console.WriteLine("Deleting a product from the database.");
            Console.WriteLine("What product would you like to delete?");

            SQL.RetrieveProductTitles();

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

            SQL.DeleteProduct(title);
        }

        public void ReturnProductForm()
        {
            Console.WriteLine("Returning a product.");
            Console.WriteLine("Please choose the product to return.");

            SQL.RetrieveProductTitles();

            string? title = null;
            while (title is null)
            {
                title = Console.ReadLine();
            }
            
            string? productId = SQL.ReturnProductIdByTitle(title);

            if (productId is null)
            {
                Console.WriteLine($"No product matched by the name {title}.");
                return;
            }

            Console.WriteLine("Fill the return product form.");

            ReturnProductFormBuilder formBuilder = new(productId);

            ReturnProductForm form = formBuilder.CreateReturnProductForm();

            SQL.ExecuteReturnProduct(form);
        }

        public void SearchProduct()
        {
            // TODO - implement search product
            throw new NotImplementedException();
        }
    }
}
