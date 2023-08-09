using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markov;

namespace YodaIpsum.Shared
{
    public class QuoteGenerator : IQuoteGenerator
    {
        public static string LoremIpsum = "Ipsum lorem, sit amet dolor, ";

        public MarkovChain<string> Load()
        {
            var chain = new MarkovChain<string>(1);
            var allCorpus = Resources.CoreText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sentence in allCorpus)
            {
                chain.Add(sentence.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            }
            return chain;
        }

        public string Generate(MarkovChain<string> chain, GenerationMode generationMode, OutputMode outputMode, int numText, bool startLorem, int? headerSizes)
        {
            var rand = new Random();

            var result = new List<string>();
            for (int i = 0; i < numText; i++)
            {
                if (generationMode == GenerationMode.Paragraphs)
                {
                    if (outputMode == OutputMode.Html && headerSizes.HasValue)
                    {
                        // create header
                        result.Add($"<h{headerSizes.Value}></h{headerSizes.Value}>");
                    }

                    var para = new List<string>();
                    var sentenceRand = new Random();
                    var numSentences = sentenceRand.Next(3, 8);
                    for (int j = 0; j < numSentences; j++)
                    {
                        var wordRand = new Random();
                        var sentence = string.Join(" ", chain.Chain(wordRand.Next(4, 9)));
                        para.Add(sentence);
                    }

                    

                    // result.Add(outputMode == OutputMode.Html ? $"<p>{sentence}.</p>" : sentence);
                }
                else
                {

                }
            }
            return string.Join("\r\n", result);
        }

        public string GenerateSentence(MarkovChain<string> chain, OutputMode outputMode, int numText, bool startLorem)
        {
            var wordRand = new Random();
            var sentence = string.Empty;
            var chainResult = string.Join(" ", chain.Chain(wordRand.Next(4, numText)));
            sentence = (startLorem)
                ? LoremIpsum + char.ToLower(chainResult[0]) + chainResult.Substring(1)
                : chainResult;
            return outputMode == OutputMode.Html ? $"<p>{sentence}</p>" : sentence;
        }

        public string GenerateParagraph(MarkovChain<string> chain, OutputMode outputMode, int numText, bool startLorem, int? headerSizes)
        {
            var sb = new StringBuilder();
            for (int p = 0; p < numText; p++)
            {
                if (headerSizes.HasValue)
                {
                    var headerText = GenerateSentence(chain, OutputMode.PlainText, new Random().Next(5, 7), false);
                    var header = $"<h{headerSizes}>{headerText}</h{headerSizes}>";
                    sb.Append(header);
                }

                if (outputMode == OutputMode.Html) sb.Append("<p>");
                for (int i = 0; i < new Random().Next(4, 12); i++)
                {
                    sb.Append(GenerateSentence(chain, OutputMode.PlainText, new Random().Next(4, Int32.MaxValue), p == 0 && i == 0 && startLorem) + " ");
                }
                sb.Append(outputMode == OutputMode.Html ? "</p>" : Environment.NewLine + Environment.NewLine);
            }

            return sb.ToString();
            //var sb = new StringBuilder();
            //var paras = new List<List<string>>();
            //for (int i = 0; i < numText; i++)
            //{
            //    // generate a paragraph
            //    var paragraph = new List<string>();
            //    var sentenceRand = new Random();
            //    for (int j = 0; j < sentenceRand.Next(3, 6); j++)
            //    {
            //        // generate a sentence in the paragraph
            //        var wordRand = new Random();
            //        paragraph.Add(GenerateSentence(chain, OutputMode.PlainText, wordRand.Next(4, 12), (j == 0 && i == 0 && startLorem)));
            //    }
            //    paras.Add(paragraph);
            //}

            //// chain it all together
            //foreach (var para in paras)
            //{
            //    var paraString = string.Join(Environment.NewLine, para);
            //    if (outputMode == OutputMode.Html) paraString = $"<p>{paraString}</p>";
            //    if (headerSizes.HasValue)
            //    {
            //        var headerText = GenerateSentence(chain, OutputMode.PlainText, new Random().Next(5, 7), false);
            //        paraString = $"<h{headerSizes}>{paraString}</h{headerSizes}>";
            //    }
            //    sb.AppendLine(paraString);
            //}

        }
    }

    public interface IQuoteGenerator
    {
        string Generate(MarkovChain<string> chain, GenerationMode generationMode, OutputMode outputMode, int numText, bool startLorem, int? headerSizes);

        MarkovChain<string> Load();

        string GenerateSentence(MarkovChain<string> chain, OutputMode outputMode, int numText, bool startLorem);

        string GenerateParagraph(MarkovChain<string> chain, OutputMode outputMode, int numText, bool startLorem, int? headerSizes);

    }
}
