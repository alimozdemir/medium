using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreTweet;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;

namespace textc
{
    public class TweetTest
    {
        private static OAuth2Token _token;
        static TweetTest() 
        {
            // if you want you can disable this part
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("twittersettings.json", optional: false);

            IConfigurationRoot configuration = builder.Build();
            
            _token = OAuth2.GetToken(configuration["customerKey"], configuration["customerSecret"]);

            File.Delete("tweet.txt");
        }

        public static void UsersTimelineTest (PredictionEngine<SentenceData, PredictionData> engine, string userName, string language, int count = 40) 
        {
            Console.WriteLine($"Getting @{userName}'s recent tweets..");
            var timeline = _token.Statuses.UserTimeline(screen_name: userName, count: count );
            int lan = 0;
            List<SentenceData> testData = new List<SentenceData>();
            foreach(var item in timeline)
            {
                var tweet = new SentenceData() { Sentence = item.Text };
                var prediction = engine.Predict(tweet);
                
                if (prediction.PredictedLabel.Equals(language))
                    lan++;
                tweet.Label = prediction.PredictedLabel;
                testData.Add(tweet);
            }

            File.AppendAllLines("tweet.txt", testData.Select(i => i.Label + '\t' + i.Sentence));
            
            Console.WriteLine($"Last {lan} tweets from {count} are in '{language}' class ");
        }
    }
}