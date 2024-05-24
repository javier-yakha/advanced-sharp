using System;
using System.IO;
using System.Globalization;
using DataModels.Models;
using CsvHelper;
using DataModels;

namespace ExportLibrary
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

        public static void SaveTableRowsIntoCsv(List<Product> tableRows, string tableName)
        {
            string filePath = CreateFilePath(tableName);

            var writer = new StreamWriter(filePath);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(tableRows);

            csvWriter.Dispose();
            writer.Dispose();
        }
        public static void SaveTableRowsIntoCsv(List<Inventory> tableRows, string tableName)
        {
            string filePath = CreateFilePath(tableName);

            var writer = new StreamWriter(filePath);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(tableRows);

            csvWriter.Dispose();
            writer.Dispose();
        }
        public static void SaveTableRowsIntoCsv(List<ReturnProductForm> tableRows, string tableName)
        {
            string filePath = CreateFilePath(tableName);

            var writer = new StreamWriter(filePath);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(tableRows);

            csvWriter.Dispose();
            writer.Dispose();
        }

        private static string CreateFilePath(string tableName)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\CSVdump\\{tableName}";
            bool exists = Directory.Exists(folderPath);

            if (!exists)
            {
                Directory.CreateDirectory(folderPath);
            }
            DateTime date = DateTime.Now;

            return $"{folderPath}\\{date.Year}_{date.Month}_{date.Day}_{date.Hour}_{date.Minute}_{tableName}.csv";
        }

        public static void SaveIntoCsvInventories()
        {
            throw new NotImplementedException();
        }
        public static void SaveIntoCsvReturnProductForms()
        {
            throw new NotImplementedException();
        }
    }
}