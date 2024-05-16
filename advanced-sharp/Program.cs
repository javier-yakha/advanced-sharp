// See https://aka.ms/new-console-template for more information
using advanced_sharp;

namespace advanced_sharp
{
    public class Program()
    {
        public static void Main(string[] args)
        {
            NonDB DB = new();

            DBManager.InitializeDB(DB);

            Console.WriteLine(DB);

        }
    }
}
