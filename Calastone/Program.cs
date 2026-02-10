using Microsoft.Extensions.DependencyInjection;
using Calastone.Filters;

namespace Calastone
{
    internal class Program
    {
        private static void RegisterServices(IServiceCollection container)
        {
            container.AddSingleton<ITextEmitter, TextEmitter>();
            container.AddSingleton<ITextProcessor, TextProcessor>();
        }

        static void Main(string[] args)
        {
            IServiceCollection container = new ServiceCollection();
            RegisterServices(container);
            IServiceProvider provider = container.BuildServiceProvider();

            var textProcessor = provider.GetService<ITextProcessor>();

            var filters = new List<IWordFilter>() { 
                new LessThan3Filter(), 
                new TFilter(), 
                new MiddleVowelFilter() };

            using (StreamReader reader = new StreamReader(@"Data\Text.txt"))
            {
                textProcessor.Process(reader, filters);
            }
        }
    }
}
