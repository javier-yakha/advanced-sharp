using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using ClassLibrary;
using DataModels.Models;
using System.Numerics;

namespace LibraryUI
{
    public class SqlManager
    {
        private readonly string ConnectionString;

        public SqlManager()
        {
            ConnectionString = "Data Source=DESKTOP-SIE9983;Initial Catalog=db_taurius;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Command Timeout=0";
        }
        
        public List<Product> ExecuteRetrieveAllProducts()
        {
            List<Product> results = [];

            using SqlConnection conn = new(ConnectionString);
            conn.Open();
            try
            {
                SqlCommand command = new("RETRIEVE_AllProducts", conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return results;
                }

                while (reader.Read())
                {
                    Product product = new()
                    {
                        Id = reader.GetGuid(0).ToString(),
                        Title = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        DateAdded = reader.GetDateTime(3),
                        Description = reader.GetString(4),
                        DiscountPrice = reader.GetDecimal(5),
                        Enabled = reader.GetBoolean(6),
                        TotalStock = reader.GetInt32(7)
                    };

                    results.Add(product);
                }
                conn.Close();

                return results;
            }
            catch (SqlException e)
            {
                conn.Close();
                Console.WriteLine(e.Message);

                return results;
            }
        }

        public bool ExecuteAddProduct(Product product)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                connection.Open();
                try
                {

                    SqlCommand insertProductCommand = new("dbo.INSERT_Product", connection);
                    insertProductCommand.CommandType = CommandType.StoredProcedure;

                    insertProductCommand.Parameters.AddWithValue("@Title", product.Title);
                    insertProductCommand.Parameters.AddWithValue("@Price", product.Price);
                    insertProductCommand.Parameters.AddWithValue("@Description", product.Description is null ? DBNull.Value : product.Description);

                    int rowsAffected = insertProductCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        return false;
                    }

                    SqlCommand productIdCmd = new("dbo.RETRIEVE_LatestProductId", connection);
                    productIdCmd.CommandType = CommandType.StoredProcedure;
                    var latestProduct = productIdCmd.ExecuteScalar().ToString();

                    SqlCommand insertInventoryProductCommand = new("dbo.INSERT_InventoryProduct", connection);
                    insertInventoryProductCommand.CommandType = CommandType.StoredProcedure;

                    insertInventoryProductCommand.Parameters.AddWithValue("@ProductId", latestProduct);

                    int rowsAffect = insertInventoryProductCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        connection.Close();
                        return false;
                    }

                    connection.Close();
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

        public void RetrieveProductStock()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand retrieveAllCommand = new("RETRIEVE_AllProductStock", connection);
                    retrieveAllCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = retrieveAllCommand.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No Products found.");
                        return;
                    }
                    while (reader.Read())
                    {
                        string productTitle = reader.GetString(0);
                        int totalStock = reader.GetInt32(1);
                        string lastUpdated = reader.GetDateTime(2).ToString();
                        Console.WriteLine($"\t{productTitle} ".PadRight(18)
                            + $"current stock: {totalStock}".PadRight(20)
                            + $"- updated: {lastUpdated}".PadLeft(20));
                    }

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();
                }
            }
        }

        public void ExecuteUpdateProductStock(string title, int stock)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand retrieveIdCommand = new("RETRIEVE_ProductIdByTitle", connection);
                    retrieveIdCommand.CommandType = CommandType.StoredProcedure;
                    retrieveIdCommand.Parameters.AddWithValue("@Title", title);
                    var productId = retrieveIdCommand.ExecuteScalar().ToString();

                    SqlCommand updateCommand = new("UPDATE_ProductStockById", connection);
                    updateCommand.CommandType = CommandType.StoredProcedure;
                    updateCommand.Parameters.AddWithValue("@ProductId", productId);
                    updateCommand.Parameters.AddWithValue("@TotalStock", stock);
                    
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        Console.WriteLine("Query failed !!!! ABORT !!!");
                    }

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();
                }
            }
        }
        public void RetrieveProductTitles()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand retrieveNameCmd = new("RETRIEVE_AllProductTitles", connection);
                    retrieveNameCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = retrieveNameCmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Table <Products> is empty.");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void DeleteProduct(string title)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand deleteProduct = new("DELETE_Product", connection);
                    deleteProduct.CommandType = CommandType.StoredProcedure;
                    deleteProduct.Parameters.AddWithValue("@Title", title);

                    int rowsAffected = deleteProduct.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        Console.WriteLine("Failed operation, /dev/sd* was deleted.");
                    }

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();
                }
            }
        }

        public string? ReturnProductIdByTitle(string title)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new("RETRIEVE_ProductIdByTitle", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@title", title);

                    string? productId = cmd.ExecuteScalar().ToString();

                    connection.Close();

                    return productId;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();

                    return null;
                }
            }
        }
        
        public void ExecuteReturnProduct(ReturnProductForm form)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT_ReturnProductForm", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", form.ProductId is null ? DBNull.Value : form.ProductId);
                    cmd.Parameters.AddWithValue("@Used", form.Used is null ? DBNull.Value : form.Used);
                    cmd.Parameters.AddWithValue("@DamagedOnArrival", form.DamagedOnArrival is null ? DBNull.Value : form.DamagedOnArrival);
                    cmd.Parameters.AddWithValue("@Working", form.Working is null ? DBNull.Value : form.Working);
                    cmd.Parameters.AddWithValue("@CausedDamage", form.CausedDamage is null ? DBNull.Value : form.CausedDamage);
                    cmd.Parameters.AddWithValue("@Complaint", form.Complaint);
                    cmd.Parameters.AddWithValue("@DateOrdered", form.DateOrdered);
                    cmd.Parameters.AddWithValue("@ProductArrived", form.ProductArrived);
                    cmd.Parameters.AddWithValue("@DesiredSolution", form.GetDesiredSolution());
                    cmd.Parameters.AddWithValue("@DateReceived", form.DateReceived is null ? DBNull.Value : form.DateReceived);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Return product form inserted correctly.");
                    }
                    else
                    {
                        Console.WriteLine("Fatal error database dead pls help");
                    }
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();
                }
            }
        }

        public void RetrieveProductForms()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("RETRIEVE_AllReturnProductForms", connection);
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close();
                }
            }
        }
    }
}
