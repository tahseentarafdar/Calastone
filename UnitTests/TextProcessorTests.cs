using Calastone;
using Calastone.Filters;
using NUnit.Framework;
using Moq;
using System.Text;

namespace UnitTests
{
    public class TextProcessorTests
    {
        private StreamReader CreateStreamReader(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var stream = new MemoryStream(bytes);
            return new StreamReader(stream);
        }

        [Test]
        public void FiltersCalledCorrectly()
        {
            var sb = new StringBuilder();
            var mockEmitter = new Mock<ITextEmitter>();
            mockEmitter.Setup(e => e.Emit(It.IsAny<string>())).Callback<string>(text => sb.Append(text));

            var mockFilter1 = new Mock<IWordFilter>();
            mockFilter1.Setup(f => f.ShouldFilter(It.IsAny<string>())).Returns<string>(text => text.Contains("box"));
            var mockFilter2 = new Mock<IWordFilter>();
            mockFilter2.Setup(f => f.ShouldFilter(It.IsAny<string>())).Returns<string>(text => text.Contains("hat"));
            var mockFilter3 = new Mock<IWordFilter>();

            //Run the code to test
            var textProcessor = new TextProcessor(mockEmitter.Object);
            var textReader = CreateStreamReader("hat, mat box.");
            var filters = new List<IWordFilter>() { mockFilter1.Object, mockFilter2.Object, mockFilter3.Object };
            textProcessor.Process(textReader, filters);

            //Ensure we have emitted the expected text
            Assert.That(sb.ToString(), Is.EqualTo(", mat ."));

            //Ensure subsequent filters are not called if an earlier filter has deemed the word should be excluded
            mockFilter1.Verify(m => m.ShouldFilter(It.IsAny<string>()), Times.Exactly(3));
            mockFilter2.Verify(m => m.ShouldFilter(It.IsAny<string>()), Times.Exactly(2));
            mockFilter3.Verify(m => m.ShouldFilter(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void PuncuationsHandledCorrectly()
        {
            var sb = new StringBuilder();
            var mockEmitter = new Mock<ITextEmitter>();
            mockEmitter.Setup(e => e.Emit(It.IsAny<string>())).Callback<string>(text => sb.Append(text));
            var mockFilter1 = new Mock<IWordFilter>();
            mockFilter1.Setup(f => f.ShouldFilter(It.IsAny<string>())).Returns<string>(text => text.Contains("box"));
            var mockFilter2 = new Mock<IWordFilter>();
            mockFilter2.Setup(f => f.ShouldFilter(It.IsAny<string>())).Returns<string>(text => text.Contains("hat"));

            //Run the code to test
            var textProcessor = new TextProcessor(mockEmitter.Object);
            var textReader = CreateStreamReader("The hat, scarf and bag - were (placed) in the (box)!");
            var filters = new List<IWordFilter>() { mockFilter1.Object, mockFilter2.Object };
            textProcessor.Process(textReader, filters);

            //Ensure we have emitted the expected text
            Assert.That(sb.ToString(), Is.EqualTo("The , scarf and bag - were (placed) in the ()!"));
        }
    }
}
