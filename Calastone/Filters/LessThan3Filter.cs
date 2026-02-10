using System;
using System.Collections.Generic;
using System.Text;

namespace Calastone.Filters
{
    public class LessThan3Filter : IWordFilter
    {
        public bool ShouldExclude(string word)
        {
            if(string.IsNullOrEmpty(word))
            {
                return true;
            }

            return word.Length < 3;
        }
    }
}
