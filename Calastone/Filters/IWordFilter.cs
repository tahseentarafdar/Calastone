using System;
using System.Collections.Generic;
using System.Text;

namespace Calastone.Filters
{
    public interface IWordFilter
    {
        bool ShouldFilter(string word);
    }
}
