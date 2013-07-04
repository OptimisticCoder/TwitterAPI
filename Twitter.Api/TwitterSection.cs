using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Twitter.Api
{
    public class TwitterSection : ConfigurationSection
    {
        [ConfigurationProperty("userName", DefaultValue = "[YourTwitterUsername]", IsRequired = true)]
        public String UserName
        {
            get
            {
                return (String)this["userName"];
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("consumerKey", DefaultValue = "[YourConsumerKey]")]
        public ConfigValueElement ConsumerKey
        {
            get
            {
                return (ConfigValueElement)this["consumerKey"];
            }
            set
            { this["consumerKey"] = value; }
        }

        [ConfigurationProperty("consumerSecret", DefaultValue="[YourConsumerSecret]")]
        public ConfigValueElement ConsumerSecret
        {
            get
            {
                return (ConfigValueElement)this["consumerSecret"];
            }
            set
            { this["consumerSecret"] = value; }
        }

        [ConfigurationProperty("accessToken", DefaultValue = "[YourAccessToken]")]
        public ConfigValueElement AccessToken
        {
            get
            {
                return (ConfigValueElement)this["accessToken"];
            }
            set
            { this["accessToken"] = value; }
        }

        [ConfigurationProperty("accessTokenSecret", DefaultValue = "[YourAccessTokenSecret]")]
        public ConfigValueElement AccessTokenSecret
        {
            get
            {
                return (ConfigValueElement)this["accessTokenSecret"];
            }
            set
            { this["accessTokenSecret"] = value; }
        }
    }
}
