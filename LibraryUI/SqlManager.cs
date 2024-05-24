using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using DataModels.Models;
using DataModels.Enums;
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
        /// <summary>
        /// Retrieves all the <see cref="Product"/> entries from the database.
        /// If the stock value is needed together,
        /// a <see cref="bool"/> value can be given to determine the case.
        /// </summary>
        /// <param name="stock">Determines if the Total Stock value is passed along.
        /// <see langword="true"/> to add stock.
        /// <see langword="false"/> to leave it as it is.</param>
        /// <returns>A <see cref="List{T}"/> of every <see cref="Product"/> in the database, optionally together with it's stock.</returns>
        public async Task<List<Product>> ExecuteRetrieveAllProducts(bool stock)
        {
            List<Product> results = [];

            string cmdText = "RETRIEVE_All_Products" + (stock ? "AndStock" : "");
            using (SqlConnection conn = new(ConnectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    SqlCommand command = new($"{cmdText}", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (!reader.HasRows)
                    {
                        return results;
                    }

                    while (await reader.ReadAsync())
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
                            TotalStock = stock ? reader.GetInt32(7) : null
                        };
                    
                        results.Add(product);
                    }
                    await reader.CloseAsync();
                    await conn.CloseAsync();

                    return results;
                }
                catch (SqlException e)
                {
                    await conn.CloseAsync();
                    Console.WriteLine(e.Message);

                    return results;
                }
                catch (Exception e)
                {
                    await conn.CloseAsync();
                    Console.WriteLine(e.Message);

                    return results;
                }
            }
        }

        public async Task ExecuteAddProduct(Product product)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand insertProductCommand = new("dbo.INSERT_Product", connection);
                    insertProductCommand.CommandType = CommandType.StoredProcedure;

                    insertProductCommand.Parameters.AddWithValue("@Title", product.Title);
                    insertProductCommand.Parameters.AddWithValue("@Price", product.Price);
                    insertProductCommand.Parameters.AddWithValue("@Description", product.Description is null ? DBNull.Value : product.Description);

                    int rowsAffected = await insertProductCommand.ExecuteNonQueryAsync();
                    if (rowsAffected <= 0)
                    {
                        return;
                    }

                    SqlCommand productIdCmd = new("dbo.RETRIEVE_Latest_ProductId", connection);
                    productIdCmd.CommandType = CommandType.StoredProcedure;
                    var latestProduct = productIdCmd.ExecuteScalarAsync();

                    SqlCommand insertInventoryProductCommand = new("dbo.INSERT_InventoryProduct_ByProductId", connection);
                    insertInventoryProductCommand.CommandType = CommandType.StoredProcedure;

                    insertInventoryProductCommand.Parameters.AddWithValue("@ProductId", (await latestProduct)?.ToString());

                    await insertInventoryProductCommand.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ErrorCode + e.Message);
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<List<Inventory>> ExecuteRetrieveAllInventories()
        {
            List<Inventory> inventoryList = new();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new("RETRIEVE_All_Inventories", connection);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Inventory inventory = new();
                        inventory.Id = reader.GetGuid(0).ToString();
                        inventory.ProductId = reader.GetGuid(1).ToString();
                        inventory.LastUpdated = reader.GetDateTime(2);
                        inventory.TotalStock = reader.GetInt32(3);

                        inventoryList.Add(inventory);
                    }

                    await reader.CloseAsync();
                    await connection.CloseAsync();

                    return inventoryList;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();

                    return inventoryList;
                }
            }
        }

        public async Task RetrieveProductStock()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand retrieveAllCommand = new("RETRIEVE_All_ProductStock", connection);
                    retrieveAllCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = await retrieveAllCommand.ExecuteReaderAsync();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No Products found.");
                        return;
                    }
                    while (await reader.ReadAsync())
                    {
                        string productTitle = reader.GetString(0);
                        int totalStock = reader.GetInt32(1);
                        string lastUpdated = reader.GetDateTime(2).ToString();
                        Console.WriteLine($"\t{productTitle} ".PadRight(18)
                            + $"current stock: {totalStock}".PadRight(20)
                            + $"- updated: {lastUpdated}".PadLeft(20));
                    }

                    await connection.CloseAsync();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();
                }
            }
        }

        public async Task ExecuteUpdateProductStock(string title, int stock)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand retrieveIdCommand = new("RETRIEVE_ProductId_ByProductTitle", connection);
                    retrieveIdCommand.CommandType = CommandType.StoredProcedure;
                    retrieveIdCommand.Parameters.AddWithValue("@Title", title);

                    var productIdTask = await retrieveIdCommand.ExecuteScalarAsync();
                    string? productId = productIdTask?.ToString();

                    SqlCommand updateCommand = new("UPDATE_ProductStock_ByProductId", connection);
                    updateCommand.CommandType = CommandType.StoredProcedure;
                    updateCommand.Parameters.AddWithValue("@ProductId", productId);
                    updateCommand.Parameters.AddWithValue("@TotalStock", stock);
                    
                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                    if (rowsAffected <= 0)
                    {
                        Console.WriteLine("Query failed !!!! ABORT !!!");
                    }

                    await connection.CloseAsync();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();
                }
            }
        }
        public async Task RetrieveProductTitles()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand retrieveNameCmd = new("RETRIEVE_All_ProductTitles", connection);
                    retrieveNameCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = await retrieveNameCmd.ExecuteReaderAsync();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Table <Products> is empty.");
                        return;
                    }
                    while (await reader.ReadAsync())
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
        public async Task DeleteProduct(string title)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand deleteProduct = new("DELETE_Product_ByTitle", connection);
                    deleteProduct.CommandType = CommandType.StoredProcedure;
                    deleteProduct.Parameters.AddWithValue("@Title", title);

                    int rowsAffected = await deleteProduct.ExecuteNonQueryAsync();
                    if (rowsAffected <= 0)
                    {
                        Console.WriteLine("Failed operation, /dev/sd* was deleted.");
                    }

                    await connection.CloseAsync();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<string?> ReturnProductIdByTitle(string title)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new("RETRIEVE_ProductId_ByProductTitle", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@title", title);

                    var productIdTask = await cmd.ExecuteScalarAsync();
                    string? productId = productIdTask?.ToString();

                    await connection.CloseAsync();

                    return productId;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();

                    return null;
                }
            }
        }
        
        public async Task ExecuteReturnProduct(ReturnProductForm form)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

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
                    cmd.Parameters.AddWithValue("@DesiredSolution", (int)form.DesiredSolution);
                    cmd.Parameters.AddWithValue("@DateReceived", form.DateReceived is null ? DBNull.Value : form.DateReceived);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Return product form inserted correctly.");
                    }
                    else
                    {
                        Console.WriteLine("Fatal error database dead pls help");
                    }
                    await connection.CloseAsync();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<List<ReturnProductForm>> ExecuteRetrieveAllReturnProductForms()
        {
            List<ReturnProductForm> returnProductForms = new();
            using (SqlConnection connection = new(ConnectionString))
            {
                await connection.OpenAsync();
                try
                {
                    SqlCommand cmd = new("RETRIEVE_All_ReturnProductForms", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        ReturnProductForm returnProductForm = new()
                        {
                            Id = reader.GetGuid(0).ToString(),
                            ProductId = reader.GetGuid(1).ToString(),
                            Used = reader.IsDBNull(2) ? null : reader.GetBoolean(2),
                            DamagedOnArrival = reader.IsDBNull(3) ? null : reader.GetBoolean(3),
                            Working = reader.IsDBNull(4) ? null : reader.GetBoolean(4),
                            CausedDamage = reader.IsDBNull(5) ? null : reader.GetBoolean(5),
                            Complaint = reader.GetString(6),
                            DateOrdered = reader.GetDateTime(7),
                            ProductArrived = reader.GetBoolean(8),
                            DesiredSolution = (DesiredSolutions)reader.GetInt32(9),
                            DateReceived = reader.GetDateTime(10)
                        };
                        returnProductForms.Add(returnProductForm);
                    }

                    await connection.CloseAsync();

                    return returnProductForms;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    await connection.CloseAsync();

                    return returnProductForms;
                }
            }
        }
    }
}
