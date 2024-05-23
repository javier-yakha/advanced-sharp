using System;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace CsvManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSVTraining";
            bool exists = System.IO.Directory.Exists(folderPath);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            List<TestDataModel> testList = new();

            for (int i = 0; i < 12; i++)
            {
                testList.Add(new TestDataModel() { id = 1, name = $"Named: {i}"} );
            }

            var writer = new StreamWriter($"{folderPath}\\file.csv");
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(testList);

            csvWriter.Dispose();
            writer.Dispose();

            var reader = new StreamReader($"{folderPath}\\file.csv");
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var output = csvReader.GetRecords<TestDataModel>().ToList();
        }

        public class TestDataModel
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}