using System;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace ClassLibrary
{
    public static class CsvManager
    {
        public class TestDataModel
        {
            public int Id { get; set;}
        }
        public static void ReadTest()
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

            foreach ( var record in output )
            {
                Console.WriteLine( record );
            }
        }

        public static void SaveProductsIntoCsv()
        {
            string folderPath = CreateDirectoryIfNotExists("Products");
            List<Models.Product> productList = GetProducts();

            DateTime date = DateTime.Now;
            string filePath = $"{folderPath}\\{date.Year}_{date.Month}_{date.Day}_{date.Hour}_{date.Minute}_Products.csv";

            var writer = new StreamWriter(filePath);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(productList);

            csvWriter.Dispose();
            writer.Dispose();
        }
        private static string CreateDirectoryIfNotExists(string tableName)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\CSVdump_{tableName}";
            bool exists = Directory.Exists(folderPath);

            if (!exists)
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
        public static List<Models.Product> GetProducts()
        {
            SqlManager sql = new();

            return sql.ExecuteRetrieveAllProducts();
        } 
    }
}