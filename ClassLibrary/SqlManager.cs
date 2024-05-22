using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace ClassLibrary
{
    public class SqlManager
    {
        private readonly string ConnectionString;

        public SqlManager()
        {
            ConnectionString = "Data Source=DESKTOP-SIE9983;Initial Catalog=db_taurius;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Command Timeout=0";
        }

        public void RunSql()
        {
            bool exit = false;
            Console.WriteLine("Welcome to the SQL manager.");
            while (!exit)
            {
                Console.WriteLine("Show All(1)" + "Update Stock(2)".PadLeft(16) + "Add Product(3)".PadLeft(15) + "Delete(4)".PadLeft(10) + "Exit(5)".PadLeft(8));
                char selection = Console.ReadKey(true).KeyChar;

                switch (selection)
                {
                    case '1':
                        RetrieveAllProducts();
                        break;
                    case '2':
                        UpdateProductStock();
                        break;
                    case '3':
                        AddProduct();
                        break;
                    case '4':
                        break;
                    case '5':
                        exit = true;
                        break;
                }
            }
        }
        public List<Models.Product> RetrieveAllProducts()
        {
            List<Models.Product> results = [];

            using (SqlConnection conn = new(ConnectionString))
            {
                conn.Open();
                try
                {
                    SqlCommand command = new SqlCommand("RETRIEVE_AllProducts", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Empty table <Products>");
                        return results;
                    }

                    while (reader.Read()) 
                    {
                        Models.Product product = new();
                        product.Id = reader.GetGuid(0).ToString();
                        product.Title = reader.GetString(1);
                        product.Price = reader.GetDecimal(2);
                        product.DateAdded = reader.GetDateTime(3);
                        product.Description = reader.GetString(4);
                        product.DiscountPrice = reader.GetDecimal(5);
                        product.Enabled = reader.GetBoolean(6);

                        results.Add(product);
                    }

                    foreach (var result in results)
                    {
                        Console.WriteLine(result);
                    }
                    Console.WriteLine();

                    return results;
                }
                catch (SqlException e)
                {
                    conn.Close();
                    Console.WriteLine(e.Message);

                    return results;
                }
            }
        }

        public void AddProduct()
        {
            bool titleStatus = false;
            bool descriptionStatus = false;
            bool priceStatus = false;

            string title = string.Empty;
            decimal parsedPrice = default;
            string description = string.Empty;

            Console.WriteLine("Adding a new Product.");
            while (true)
            {
                if (!titleStatus)
                {
                    Console.Write("Title: ");
                    title = Console.ReadLine();
                    if (title is not null && title.Length < 50)
                    {
                        titleStatus = true;
                    }
                    continue;
                }
                if (!priceStatus)
                {
                    Console.Write("Price: ");
                    string price = Console.ReadLine();
                    priceStatus = decimal.TryParse(price, out parsedPrice) && parsedPrice >= 0;
                    continue;
                }
                if (!descriptionStatus)
                {
                    Console.Write("Description: ");
                    description = Console.ReadLine();
                    if (description is not null && description.Length < 90)
                    {
                        descriptionStatus = true;
                    }
                    continue;
                }
                break;
            }
            bool result = ExecuteAddProduct(title, parsedPrice, description);
            Console.WriteLine(result);
        }

        private bool ExecuteAddProduct(string title, decimal price, string description)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                connection.Open();
                try
                {

                    SqlCommand insertProductCommand = new("dbo.INSERT_Product", connection);
                    insertProductCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    insertProductCommand.Parameters.AddWithValue("@Title", title);
                    insertProductCommand.Parameters.AddWithValue("@Price", price);
                    insertProductCommand.Parameters.AddWithValue("@Description", description);

                    int rowsAffected = insertProductCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        return false;
                    }

                    SqlCommand productIdCmd = new("dbo.RETRIEVE_LatestProductId", connection);
                    productIdCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var latestProduct = productIdCmd.ExecuteScalar().ToString();

                    SqlCommand insertInventoryProductCommand = new("dbo.INSERT_InventoryProduct", connection);
                    insertInventoryProductCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    insertInventoryProductCommand.Parameters.AddWithValue("@ProductId", latestProduct);

                    int rowsAffect = insertInventoryProductCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        return false;
                    }

                    return true;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ErrorCode + e.Message);
                    connection.Close();
                    return false;
                }
            }
        }

        public void UpdateProductStock()
        {
            Console.WriteLine("Updating stock for a product.");
            Console.WriteLine("What product would you like to update?");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand retrieveAllCommand = new("RETRIEVE_AllProductStock", connection);
                    retrieveAllCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = retrieveAllCommand.ExecuteReader();


                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No Products found.");
                        return;
                    }
                    while (reader.Read())
                    {
                        string productTitle = reader.GetString(0);
                        int totalSTock = reader.GetInt32(1);
                        string lastUpdated = reader.GetDateTime(2).ToString();
                        Console.WriteLine($"\t{productTitle} " 
                            + $"current stock: {totalSTock}"
                            + $"- updated: {lastUpdated}");
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();
                }
            }

            string title = string.Empty;
            int stock = 0;
            bool titleStatus = false;
            bool stockStatus = false;

            while (true)
            {
                if (!titleStatus)
                {
                    Console.Write("Product title to update: ");
                    title = Console.ReadLine();
                    if (title is not null && title.Length < 90) 
                    { 
                        titleStatus = true;
                    }
                    continue;
                }
                if (!stockStatus)
                {
                    Console.Write("New total stock: ");
                    stockStatus = Int32.TryParse(Console.ReadLine(), out stock);
                    continue;
                }
                break;
            }
            ExecuteUpdateProductStock(title, stock);
        }

        private void ExecuteUpdateProductStock(string title, int stock)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand retrieveIdCommand = new("RETRIEVE_ProductIdByName", connection);
                    retrieveIdCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    retrieveIdCommand.Parameters.AddWithValue("@Title", title);
                    var productId = retrieveIdCommand.ExecuteScalar().ToString();

                    SqlCommand updateCommand = new("UPDATE_ProductStockById", connection);
                    updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    updateCommand.Parameters.AddWithValue("@ProductId", productId);
                    updateCommand.Parameters.AddWithValue("@TotalStock", stock);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        Console.WriteLine("Query failed !!!! ABORT !!!");
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
