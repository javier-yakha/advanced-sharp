using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUI
{
    public class ClassForTaskMethods
    {
        public async void Main()
        {
            await FirstTaskMethod();
            string result = await SecondTaskMethod();
        }
        public Task FirstTaskMethod()
        {

            return Task.CompletedTask;
        }
        public Task<string> SecondTaskMethod()
        {

            return Task.FromResult("result");
        }
    }
}
