using Calastone.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calastone
{
    public interface ITextProcessor
    {
        void Process(StreamReader reader, IEnumerable<IWordFilter> filters);
    }
}
