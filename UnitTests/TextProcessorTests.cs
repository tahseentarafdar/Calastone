using Calastone;
using Calastone.Filters;
using Moq;
using NUnit.Framework;
using System.Diagnostics;
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

        private ITextEmitter CreateTextEmitterMock(StringBuilder sb)
        {
            var mockEmitter = new Mock<ITextEmitter>();
            mockEmitter.Setup(e => e.Emit(It.IsAny<string>())).Callback<string>(text => sb.Append(text));
            return mockEmitter.Object;
        }

        private Mock<IWordFilter> CreateWordFilterMock(string filterWord)
        {
            var mockFilter1 = new Mock<IWordFilter>();
            mockFilter1.Setup(f => f.ShouldExclude(It.IsAny<string>())).Returns<string>(text => text.Contains(filterWord));
            return mockFilter1;
        }

        [Test]
        public void FiltersCalledCorrectly()
        {
            var sb = new StringBuilder();

            var mockEmitter = CreateTextEmitterMock(sb);

            var mockFilter1 = CreateWordFilterMock("box");
            var mockFilter2 = CreateWordFilterMock("hat");
            var mockFilter3 = CreateWordFilterMock("xxx");

            var textProcessor = new TextProcessor(mockEmitter);
            var textReader = CreateStreamReader("hat, mat box.");
            var filters = new List<IWordFilter>() { mockFilter1.Object, mockFilter2.Object, mockFilter3.Object };
            textProcessor.Process(textReader, filters);

            Assert.That(sb.ToString(), Is.EqualTo(", mat ."));

            //Ensure subsequent filters are not called if an earlier filter has deemed the word should be excluded
            mockFilter1.Verify(m => m.ShouldExclude(It.IsAny<string>()), Times.Exactly(3));
            mockFilter2.Verify(m => m.ShouldExclude(It.IsAny<string>()), Times.Exactly(2));
            mockFilter3.Verify(m => m.ShouldExclude(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void PuncuationsHandledCorrectly()
        {
            var sb = new StringBuilder();
            var mockEmitter = CreateTextEmitterMock(sb);

            var mockFilter1 = CreateWordFilterMock("box");
            var mockFilter2 = CreateWordFilterMock("hat");

            var textProcessor = new TextProcessor(mockEmitter);
            var textReader = CreateStreamReader("The hat, scarf and bag - were (placed) in the (box)!");
            var filters = new List<IWordFilter>() { mockFilter1.Object, mockFilter2.Object };
            textProcessor.Process(textReader, filters);

            Assert.That(sb.ToString(), Is.EqualTo("The , scarf and bag - were (placed) in the ()!"));
        }

        [Test]
        public void NoFiltersEmitsAllWords()
        {
            var sb = new StringBuilder();
            var mockEmitter = CreateTextEmitterMock(sb);
            var processor = new TextProcessor(mockEmitter);

            var textReader = CreateStreamReader("one two three");
            processor.Process(textReader, Enumerable.Empty<IWordFilter>());

            Assert.That(sb.ToString(), Is.EqualTo("one two three"));
        }

        [Test]
        public void ThrowsOnInvalidArgumentsInProcess()
        {
            var sb = new StringBuilder();
            var mockEmitter = CreateTextEmitterMock(sb);
            var textProcessor = new TextProcessor(mockEmitter);

            Assert.That(
                () => textProcessor.Process(null, new List<IWordFilter>()), 
                Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("reader"));

            var streamReader = new StreamReader(new MemoryStream());
            Assert.That(
                () => textProcessor.Process(streamReader, null),
                Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("filters"));
        }


        [Test]
        public void ThrowsOnInvalidArgumentsInCreation()
        {
            Assert.That(
                () => new TextProcessor(null),
                Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("textEmitter"));
        }
    }
}
