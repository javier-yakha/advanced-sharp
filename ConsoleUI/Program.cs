using LibraryUI;
using System.ComponentModel.Design;
using System.Text;

namespace UIconsole
{
    public class Program()
    {
        public static async Task Main(string[] args)
        {
            UserInterface ui = new();
            await ui.Run();
        }
    }
}
