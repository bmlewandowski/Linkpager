using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace Linkpager
{
    public class FetchTitle
    {
        public static string FetchHTML(string sUrl)
        {

            try
            {
                System.Net.WebClient oClient = new System.Net.WebClient();
                return oClient.DownloadString(sUrl);
            }
            catch (WebException)
            {
                string notURL = "<title></title>";
                return notURL;
            }

        }

        public static string FetchTitleFromHTML(string sHtml)
        {
            // Regular expression for an HTML title
            string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return rex.Match(sHtml).Value.Trim();
        }
    }
}