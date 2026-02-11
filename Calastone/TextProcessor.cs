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
            if (textEmitter == null) throw new ArgumentNullException(nameof(textEmitter));

            _textEmitter = textEmitter;
        }

        public void Process(StreamReader reader, IEnumerable<IWordFilter> filters)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (filters == null) throw new ArgumentNullException(nameof(filters));

            var wordBuffer = new StringBuilder();
            int charInt; 
            while ((charInt = reader.Read()) != -1)
            {
                var c = (char)charInt; 
                if(char.IsPunctuation(c) || char.IsWhiteSpace(c))
                {
                    if(wordBuffer.Length > 0)
                    {
                        var currentWord = wordBuffer.ToString();
                        if (!filters.Any(f => f.ShouldExclude(currentWord)))
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
                if (!filters.Any(f => f.ShouldExclude(currentWord)))
                {
                    _textEmitter.Emit(currentWord);
                }
            }
        }
    }
}
