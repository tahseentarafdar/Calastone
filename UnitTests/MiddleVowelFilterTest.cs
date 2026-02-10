using Calastone;
using Calastone.Filters;
using NUnit.Framework;
using Moq;
using System.Text;

namespace UnitTests
{
    public class MiddleVowelFilterTest
    {
        [TestCase("rabbit", false)]
        [TestCase("water", false)]
        [TestCase("bridge", true)]
        [TestCase("heath", true)]
        public void FiltersCorrectly(string word, bool expectedResult)
        {
            var filter = new MiddleVowelFilter();

            var result = filter.ShouldExclude(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
