using System;
using System.Collections.Generic;
using System.Text;

namespace Calastone.Filters
{
    public class TFilter : IWordFilter
    {
        public bool ShouldExclude(string word)
        {
            if(string.IsNullOrEmpty(word))
            {
                return false;
            }

            return word.Contains('t') || word.Contains('T');
        }
    }
}
