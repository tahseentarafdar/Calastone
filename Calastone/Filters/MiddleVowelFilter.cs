using System;
using System.Collections.Generic;
using System.Text;

namespace Calastone.Filters
{
    public class MiddleVowelFilter : IWordFilter
    {
        private static readonly char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };

        public bool ShouldFilter(string word)
        {
            if(string.IsNullOrEmpty(word))
            {
                return false;
            }

            var middle = GetMiddle(word);
            return ContainsVowel(middle);
        }

        private bool ContainsVowel(string s)
        {
            return vowels.Any(v => s.Contains(v));
        }


        private string GetMiddle(string word)
        {
            var len = word.Length;

            if (len < 3)
            {
                return word;
            }

            var mid = len / 2;

            var midLen = len % 2 == 0 ? 2 : 1;

            return word.Substring(mid - midLen + 1, midLen);
        }
    }
}
