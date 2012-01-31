namespace StealFocus.Forecast.WindowsAzure.Net
{
    using System;
    using System.Net;
    using System.Threading;

    public static class WebRequestExtensions
    {
        private static readonly object syncRoot = new object();

        public static WebResponse GetResponse(this WebRequest webRequest, int throttleTimeInMilliseconds)
        {
            if (webRequest == null)
            {
                throw new ArgumentNullException("webRequest");
            }

            WebResponse webResponse;
            lock (syncRoot)
            {
                webResponse = webRequest.GetResponse();
                Thread.Sleep(throttleTimeInMilliseconds);
            }

            return webResponse;
        }
    }
}
