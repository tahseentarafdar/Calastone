using Calastone.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calastone
{

    public class TextProcessor : ITextProcessor
    {
        private ITextEmitter _textEmitter;

        public TextProcessor(ITextEmitter textEmitter)
        {
            _textEmitter = textEmitter;
        }

        public void Process(StreamReader reader, IEnumerable<IWordFilter> filters)
        {
            var wordBuffer = new StringBuilder();
            int charInt; 
            while ((charInt = reader.Read()) != -1)
            { 
                char c = (char)charInt; 
                if(char.IsPunctuation(c) || char.IsWhiteSpace(c))
                {
                    if(wordBuffer.Length > 0)
                    {
                        var currentWord = wordBuffer.ToString();
                        if (!filters.Any(f => f.ShouldFilter(currentWord)))
                        {
                            _textEmitter.Emit(currentWord);
                        }
                        wordBuffer.Clear();
                    }
                    _textEmitter.Emit(c.ToString());
                }
                else
                {
                    wordBuffer.Append(c);
                }
            }

            if (wordBuffer.Length > 0)
            {
                var currentWord = wordBuffer.ToString();
                if (!filters.Any(f => f.ShouldFilter(currentWord)))
                {
                    _textEmitter.Emit(currentWord);
                }
            }
        }
    }
}
