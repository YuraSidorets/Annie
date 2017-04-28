using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace BotLogic
{
    public static class RazborPoldcastHelper
    {
        public static string CheckRss()
        {
            string url = "http://feeds.feedburner.com/razbor-podcast";

            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            string output = string.Empty;
            if (feed != null)
            {
                var last = feed.Items.FirstOrDefault();
                output = last.Title.Text + "\n" + last.PublishDate.Date.ToShortDateString() + "\n" + last.Id;
            }
            return output;
        }
    }
}