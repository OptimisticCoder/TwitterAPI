using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    /// <summary>
    /// A sample of how to implement the Tweet class.
    /// </summary>
    /// <remarks>
    /// Twitter API 1.1 requires you to create API keys on the developer site.
    /// 1. Login to Twitter Dev site with the account you want to use. https://dev.twitter.com/
    /// 2. Go to "My Applications" https://dev.twitter.com/apps
    /// 3. Create a new Twitter app. Fill in all fields, but leave Callback_Url blank.
    /// 4. After the app is created, go to its detail page and create an access token for it.
    /// 5. You need Consumer Key, Consumer Secret, Access Token, and Access Token Secret. (In that order)
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            // Pass in all the OAuth keys to the constructor
            var twitter = new Twitter.Api.Tweet("[YourConsumerKey]",
                                                "[YourConsumerSecret]",
                                                "[YourAccessToken]",
                                                "[YourAccessTokenSecret]");

            // API url for a list
            // https://api.twitter.com/1.1/lists/statuses.json?slug=[LISTNAME]&owner_screen_name=[ACCOUNTNAME]&count=[COUNT]

            // API url for a user timeline
            // https://api.twitter.com/1.1/statuses/user_timeline.json?owner_screen_name=[ACCOUNTNAME]&count=[COUNT]

            // Call a Twitter API
            var tweets = twitter.GetTimeline("[YourAPICallUrl]");

            // Loop through the returned Tweets.
            foreach (var t in tweets)
            {
                Console.WriteLine("UserName: " + t.UserName);
                Console.WriteLine("DisplayName: " + t.DisplayName);
                Console.WriteLine("ProfileImage: " + t.ProfileImage);
                Console.WriteLine("StatusDate: " + t.StatusDate.ToString("dddd, dd MMMM yyyy - hh:mmtt"));
                Console.WriteLine("Status: " + t.Status);
                Console.WriteLine();
            }

            // Wait
            Console.ReadKey();
        }
    }
}
