namespace TwitterAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Linq;
    using System.Text;
    using System.Net;
    using System.Xml.Linq;
    using System.Security.Cryptography;
    using System.IO;
    using System.Web;

    using Newtonsoft.Json;

    public class Tweet
    {
        #region Public Properties

        public String UserName { get; set; }
        public String DisplayName { get; set; }
        public String ProfileImage { get; set; }
        public String Status { get; set; }
        public String StatusHtml { get; set; }
        public DateTime StatusDate { get; set; }

        #endregion

        #region Private Members

        private String ConsumerKey = String.Empty;
        private String ConsumerSecret = String.Empty;
        private String AccessToken = String.Empty;
        private String AccessTokenSecret = String.Empty;

        #endregion

        #region Constructors

        public Tweet(String consumerKey,
                     String consumerSecret,
                     String accessToken,
                     String accessTokenSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public Tweet()
        {
        }

        #endregion

        #region Public Methods

        public Tweet[] GetTimeline(String apiUrl)
        {
            String response = httpGet(apiUrl);
            dynamic tweets = (dynamic)JsonConvert.DeserializeObject(response);

            if (tweets == null)
                return null;

            List<Tweet> tw = new List<Tweet>();
            foreach (var t in tweets)
            {
                String html = buildHtml(t);
                tw.Add(new Tweet
                {
                    Status = t.text,
                    StatusHtml = html,
                    UserName = t.user.screen_name,
                    DisplayName = t.user.name,
                    ProfileImage = t.user.profile_image_url,
                    StatusDate = parseDateTime(t.created_at.ToString())
                });
            }

            return tw.ToArray();
        }

        #endregion

        #region Private Helpers

        private String buildHtml(dynamic t)
        {
            String html = t.text;
            foreach (var u in t.entities.urls)
            {
                String actual = u.url;
                String display = u.display_url;
                String expanded = u.expanded_url;
                html = html.Replace(actual, "<a href=\"" + expanded + "\" target=\"_blank\">" + display + "</a>");
            }
            foreach (var u in t.entities.hashtags)
            {
                String actual = u.text;
                html = html.Replace("#" + actual, "<a href=\"https://twitter.com/search?q=%23" + actual + "&src=hash\" target=\"_blank\">#" + actual + "</a>");
            }
            foreach (var u in t.entities.user_mentions)
            {
                String actual = u.screen_name;
                html = html.Replace("@" + actual, "<a href=\"https://twitter.com/" + actual + "\" target=\"_blank\">@" + actual + "</a>");
            }

            return html;
        }

        private DateTime parseDateTime(String date)
        {
            string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }

        private String performRequest(string method, string url)
        {
            String nonce = Guid.NewGuid().ToString().Replace("-", String.Empty);

            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            String timestamp = ((Int32)t.TotalSeconds).ToString();

            Int32 indexA = url.IndexOf("?");
            String strippedUrl = url;
            String countParam = String.Empty;
            String restParams = String.Empty;
            if (indexA > -1)
            {
                strippedUrl = url.Substring(0, indexA);

                String[] parts = url.Substring(indexA + 1).Split('&');
                for(Int32 i = parts.Length - 1; i> -1; i--)
                {
                    String[] values = parts[i].Split('=');
                    if (values[0] == "count")
                        countParam = parts[i];
                    else
                    {
                        restParams += lowercaseEncodings("&" + parts[i]);
                    }
                }
            }

            String signatureBaseString = "GET&" + lowercaseEncodings(strippedUrl) + "&" +
                                         (String.IsNullOrEmpty(countParam) ? String.Empty : lowercaseEncodings(countParam) + "%26") +
                                         "oauth_consumer_key%3D" + lowercaseEncodings(ApiKey) + "%26" +
                                         "oauth_nonce%3D" + lowercaseEncodings(nonce) + "%26" +
                                         "oauth_signature_method%3DHMAC-SHA1%26" +
                                         "oauth_timestamp%3D" + timestamp + "%26" +
                                         "oauth_token%3D" + lowercaseEncodings(ApiToken) + "%26" +
                                         "oauth_version%3D1.0" + restParams;

            String sig = encode(signatureBaseString, Encoding.Default.GetBytes(ConsumerSecret + "&" + TokenSecret)); 

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;
            request.Headers.Add("Authorization", @"OAuth oauth_consumer_key=""" + ApiKey + @""", 
                                                         oauth_nonce=""" + nonce + @""", 
                                                         oauth_signature=""" + lowercaseEncodings(sig) + @""", 
                                                         oauth_signature_method=""HMAC-SHA1"", 
                                                         oauth_timestamp=""" + timestamp + @""", 
                                                         oauth_token=""" + ApiToken + @""", 
                                                         oauth_version=""1.0""");
            String responseString = String.Empty;
            try
            {
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return responseString;
        }

        private static String encode(string input, byte[] key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.Default.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            return Convert.ToBase64String(myhmacsha1.ComputeHash(stream));
        }

        private String httpGet(string url)
        {
            return performRequest("GET", url);
        }

        private String lowercaseEncodings(String src)
        {
            Char[] temp = HttpUtility.UrlEncode(src).ToCharArray();
            for (Int32 i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = Char.ToUpper(temp[i + 1]);
                    temp[i + 2] = Char.ToUpper(temp[i + 2]);
                }
            }
            return new String(temp);
        }

        #endregion
    }
}
// Twitter: @OptimisticCoder
