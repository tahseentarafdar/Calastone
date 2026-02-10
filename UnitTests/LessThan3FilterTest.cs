using Calastone;
using Calastone.Filters;
using NUnit.Framework;
using Moq;
using System.Text;

namespace UnitTests
{
    public class LessThan3FilterTest
    {
        [TestCase("Ken", false)]
        [TestCase("is", true)]
        [TestCase("a", true)]
        [TestCase("Human", false)]
        public void FiltersCorrectly(string word, bool expectedResult)
        {
            var filter = new LessThan3Filter();

            var result = filter.ShouldFilter(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
