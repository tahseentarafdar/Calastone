using Calastone;
using Calastone.Filters;
using NUnit.Framework;
using Moq;
using System.Text;

namespace UnitTests
{
    public class TFilterTest
    {
        [TestCase("test", true)]
        [TestCase("bat", true)]
        [TestCase("they", true)]
        [TestCase("park", false)]
        public void FiltersCorrectly(string word, bool expectedResult)
        {
            var filter = new TFilter();

            var result = filter.ShouldFilter(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
