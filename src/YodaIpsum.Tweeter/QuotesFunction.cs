using System;
using BlazorApp.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Models;
using YodaIpsum.Shared;

namespace YodaIpsum.Tweeter
{
    public class QuotesFunction
    {
        private readonly IConfiguration _configuration;
        private readonly IQuoteGenerator _quotesGen;
        private readonly ITwitterClient _twitterClient;

        public QuotesFunction(IConfiguration configuration, IQuoteGenerator quoteGenerator, ITwitterClient twitterClient)
        {
            _configuration = configuration;
            _quotesGen = _quotesGen;
            _twitterClient = twitterClient;
        }

        [FunctionName("twitter-tweet")]
        public void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var rand = new Random();
            var tweetContent = _quotesGen.Generate(_quotesGen.Load(), GenerationMode.Sentences, OutputMode.PlainText,
                rand.Next(1, 4), false, null);
            // calculate Yoda quote
            _twitterClient.Tweets.PublishTweetAsync(tweetContent);

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
