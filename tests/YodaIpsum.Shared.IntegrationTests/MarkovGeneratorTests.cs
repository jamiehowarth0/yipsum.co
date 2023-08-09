using Markov;
using Moq;

namespace YodaIpsum.Shared.IntegrationTests
{
    public class MarkovGeneratorTests
    {
        private IQuoteGenerator _quotesGenerator;

        [SetUp]
        public void Setup()
        {
            _quotesGenerator = new QuoteGenerator();
        }

        [Test]
        public void Test_GenerateSentence_PlainText()
        {
            var chain = _quotesGenerator.Load();
            var sentence = _quotesGenerator.GenerateSentence(chain, OutputMode.PlainText, 10, true);
            Assert.IsNotEmpty(sentence);
            Assert.That(sentence.Contains(QuoteGenerator.LoremIpsum));
        }

        [Test]
        public void Test_GenerateSentence_RichText()
        {
            var chain = _quotesGenerator.Load();
            var sentence = _quotesGenerator.GenerateSentence(chain, OutputMode.Html, 10, true);
            Assert.IsNotEmpty(sentence);
            Assert.That(sentence.Contains($"<p>"));
            Assert.That(sentence.Contains($"</p>"));
            Assert.That(sentence.Contains(QuoteGenerator.LoremIpsum));
        }

        [Test]
        public void Test_GenerateParagraph_PlainText()
        {
            var chain = _quotesGenerator.Load();
            var sentence = _quotesGenerator.GenerateParagraph(chain, OutputMode.PlainText, 1, false, null);
            Assert.IsNotEmpty(sentence);
        }

        [Test]
        public void Test_GenerateParagraph_RichText_NoHeading()
        {
            var chain = _quotesGenerator.Load();
            var sentence = _quotesGenerator.GenerateParagraph(chain, OutputMode.Html, 1, false, null);
            Assert.That(sentence.Contains($"<p>"));
            Assert.That(sentence.Contains($"</p>"));
        }
        
        [Test]
        public void Test_GenerateParagraph_RichText_WithHeading()
        {
            int headerSize = 3;
            var chain = _quotesGenerator.Load();
            var sentence = _quotesGenerator.GenerateParagraph(chain, OutputMode.Html, 1, false, headerSize);
            Assert.That(sentence.Contains($"<h{headerSize}>"));
            Assert.That(sentence.Contains($"</h{headerSize}>"));
            Assert.That(sentence.Contains($"<p>"));
            Assert.That(sentence.Contains($"</p>"));
        }

        [Test]
        public void Test_GenerateMultipleParagraphs_PlainText_NoHeaders()
        {
            var chain = _quotesGenerator.Load();
            var numText = 5;
            var text = _quotesGenerator.GenerateParagraph(chain, OutputMode.PlainText, numText,false, null);
            Assert.That(text.Contains(Environment.NewLine));
        }

        [Test]
        public void Test_GenerateMultipleParagraphs_RichText_NoHeaders()
        {
            var chain = _quotesGenerator.Load();
            var numText = new Random().Next(1, 10);
            var text = _quotesGenerator.GenerateParagraph(chain, OutputMode.Html, numText, true, null);
            Assert.That(text.Contains(QuoteGenerator.LoremIpsum));
            
            Assert.GreaterOrEqual(text.AllIndexesOf($"<p>").Count(), numText);
            Assert.GreaterOrEqual(text.AllIndexesOf($"</p>").Count(), numText);
        }

        [Test]
        public void Test_GenerateMultipleParagraphs_RichText_WithHeaders()
        {
            var chain = _quotesGenerator.Load();
            var numText = new Random().Next(1, 10);
            var headerSize = 2;
            var text = _quotesGenerator.GenerateParagraph(chain, OutputMode.Html, numText, true, headerSize);
            Assert.That(text.Contains(QuoteGenerator.LoremIpsum));
            Assert.GreaterOrEqual(text.AllIndexesOf($"<h{headerSize}>").Count(), numText);
            Assert.GreaterOrEqual(text.AllIndexesOf($"</h{headerSize}>").Count(), numText);
            Assert.GreaterOrEqual(text.AllIndexesOf($"<p>").Count(), numText);
            Assert.GreaterOrEqual(text.AllIndexesOf($"</p>").Count(), numText);
        }
    }
}