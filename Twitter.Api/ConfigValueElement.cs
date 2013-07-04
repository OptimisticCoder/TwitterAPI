using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Twitter.Api
{
    public class ConfigValueElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public String Value
        {
            get
            {
                return (String)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}
