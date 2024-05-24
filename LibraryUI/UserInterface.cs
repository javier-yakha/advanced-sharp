using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace LibraryUI
{
    public class UserInterface
    {
        private static bool Auth = false;
        private readonly SqlManagerUI SqlUI = new();
        private readonly CsvManagerUI CsvUI = new();

        public void Run()
        {
            while (!Auth)
            {
                Auth = Authenticate();
            }

            while (Auth)
            {
                Console.WriteLine("SQL(1)" +
                    "\nCSV(2)" +
                    "\nExit(any)");

                char selection = Console.ReadKey(true).KeyChar;
                switch (selection)
                {
                    case '1':
                        SqlUI.RunSql();
                        break;
                    case '2':
                        CsvUI.RunCsv();
                        break;
                    default:
                        Auth = false;
                        break;
                };
            }
            Console.WriteLine("Logged out of the DataBase management Service.");
        }

        private static bool Authenticate()
        {
            int charCount = 0;
            StringBuilder sb = new();
            Console.Write("Enter the password. (hint: 123) ");
            while (charCount < 3)
            {
                char key = Console.ReadKey(true).KeyChar;
                charCount++;
                sb.Append(key);
            }

            Console.WriteLine();

            if (sb.ToString() == "123")
            {
                Console.WriteLine("Logged in successfully.");

                return true;
            }
            Console.WriteLine("Wrong password.");

            return false;
        }
    }
}
