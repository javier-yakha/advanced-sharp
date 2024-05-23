using System;
using System.IO;
using System.Globalization;
using ClassLibrary;
using ClassLibrary.Models;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CsvManagement
{
    public class CsvManager
    {
        public static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSVTraining";
            bool exists = Directory.Exists(folderPath);

            if (!exists)
            {
                Directory.CreateDirectory(folderPath);
            }

            var reader = new StreamReader($"{folderPath}\\file.csv");
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var output = csvReader.GetRecords<TestDataModel>().ToList();
        }
        public class TestDataModel
        {
            public int Id { get; set;}
        }

        public void SaveProductIntoCsv()
        {
            string folderPath = CreateDirectoryIfNotExists();
            List<Models.Product> productList = GetProducts();

            DateTime date = DateTime.Now;
            string filePath = $"{folderPath}\\{date.Year}_{date.Month}_{date.Day}_Products.csv";

            var writer = new StreamWriter(filePath);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(productList);

            csvWriter.Dispose();
            writer.Dispose();
        }
        public string CreateDirectoryIfNotExists()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSVdump";
            bool exists = Directory.Exists(folderPath);

            if (!exists)
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
        public List<Models.Product> GetProducts()
        {
            SqlManager sql = new();
            return sql.ExecuteRetrieveAllProducts();
        } 
    }
}