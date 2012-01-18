namespace StealFocus.Forecast.WindowsAzure.Net
{
    using System;
    using System.IO;
    using System.Net;
    using System.Xml;
    using System.Xml.Linq;

    public static class WebResponseExtensions
    {
        public static XDocument GetResponseBody(this WebResponse webResponse)
        {
            if (webResponse == null)
            {
                throw new ArgumentNullException("webResponse");
            }

            XDocument responseBody = null;
            if (webResponse.ContentLength > 0)
            {
                Stream responseStream = webResponse.GetResponseStream();
                if (responseStream != null)
                {
                    using (XmlReader reader = XmlReader.Create(responseStream))
                    {
                        responseBody = XDocument.Load(reader);
                    }
                }
            }

            return responseBody;
        }
    }
}
