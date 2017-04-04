using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

namespace PMBot.Helpers
{
    public class RazborPoldcastHelper
    {
        public string CheckRss()
        {
            string url = "http://feeds.feedburner.com/razbor-podcast";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            string output = string.Empty;
            if (feed != null)
            {
                var last = feed.Items.FirstOrDefault();
                output = last.Title.Text + "\n"+ last.PublishDate.Date.ToShortDateString() + "\n" + last.Id;
            }
            return output;
        }
    }
}