using ClassLibrary;
using System.ComponentModel.Design;
using System.Text;

namespace advanced_sharp
{
    public class Program()
    {
        public static void Main(string[] args)
        {
            bool auth = Authenticate();
            Menu(auth);
        }
        private static void Menu(bool auth)
        {
            var sql = new SqlManager();

            while (auth)
            {
                Console.WriteLine("SQL(1) - Exit(any)");
                char selection = Console.ReadKey(true).KeyChar;

                if (selection == '1') sql.RunSql();
                else break;
            }
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

            return sb.ToString() == "123";
        }
    }
}
