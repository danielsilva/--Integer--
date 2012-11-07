using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Integer.Web.Helpers
{
    public static class UriHelperExtensions
    {
        public static string FormatAbsoluteUrl(this Uri url, string path)
        {
            return string.Format(
               "{0}/{1}", url.FormatUrlStart(), path.TrimStart('/'));
        }

        public static string FormatUrlStart(this Uri url)
        {
            return string.Format("{0}://{1}{2}", url.Scheme,
               url.Host, url.Port == 80 ? string.Empty : ":" + url.Port);
        }
    }
}