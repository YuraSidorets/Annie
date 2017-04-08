using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
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
                byte[] bytes = Encoding.Default.GetBytes(last.Title.Text);
                var text = Encoding.GetEncoding("windows-1251").GetString(bytes);

                output = text + "\n" + last.PublishDate.Date.ToShortDateString() + "\n" + last.Id;
            }
            return output;
        }
    }
}