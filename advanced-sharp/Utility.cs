using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_sharp
{
    public static class Utility
    {
        public static char FindDuplicate(char[] chars)
        {
            HashSet<char> charCount = new();
            
            for (int i = 0; i < chars.Length; i++)
            {
                if (charCount.Contains(chars[i]))
                {
                    return chars[i];
                }
                else
                {
                    charCount.Add(chars[i]);
                }
            }

            return default;
        }

        public static char[] FindDuplicates(char[] chars)
        {
            List<char> duplicates = new();
            Dictionary<char, int> charCount = new();
            
            for (int i = 0; i < chars.Length; i++)
            {
                charCount[chars[i]] = 1 + (charCount.ContainsKey(chars[i]) ? charCount[chars[i]] : 0);
            }

            foreach (var pair in charCount)
            {
                if (pair.Value > 1)
                {
                    duplicates.Add(pair.Key);
                }
            }

            return duplicates.ToArray();
        }

        public static char[] SortCharArray(char[] chars)
        {

            return chars;
        }
    }
}
