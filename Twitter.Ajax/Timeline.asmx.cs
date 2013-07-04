using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Twitter.Api;

namespace Twitter.Ajax
{
    /// <summary>
    /// A web service to provide ajax Twitter functionality.
    /// </summary>
    [WebService(Namespace = "http://uape.co.uk/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Timeline : System.Web.Services.WebService
    {
        [WebMethod]
        public Tweet[] GetTweets(Int32 count)
        {
            String apiUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json?owner_screen_name={0}&count={1}";

            TwitterSection config = 
                (TwitterSection)System.Configuration.ConfigurationManager.GetSection("twitterApi");

            var twitter = new Tweet(config.ConsumerKey.Value,
                                    config.ConsumerSecret.Value,
                                    config.AccessToken.Value,
                                    config.AccessTokenSecret.Value);
            var tweets = twitter.GetTimeline(String.Format(apiUrl, config.UserName, count.ToString()));

            return tweets.ToArray();
        }
    }
}
