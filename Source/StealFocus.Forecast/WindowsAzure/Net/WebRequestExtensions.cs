namespace StealFocus.Forecast.WindowsAzure.Net
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using log4net;

    public static class WebRequestExtensions
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object syncRoot = new object();

        private const int DefaultThrottleTimeInMilliseconds = 1000;

        /// <remarks>
        /// The Windows Azure Management REST API does not allow a rapid succession of requests. Calls via this 
        /// mechanism will throttle the calls.
        /// </remarks>
        public static WebResponse GetResponseThrottled(this WebRequest webRequest)
        {
            return webRequest.GetResponseThrottled(DefaultThrottleTimeInMilliseconds);
        }

        /// <remarks>
        /// The Windows Azure Management REST API does not allow a rapid succession of requests. Calls via this 
        /// mechanism will throttle the calls.
        /// </remarks>
        public static WebResponse GetResponseThrottled(this WebRequest webRequest, int throttleTimeInMilliseconds)
        {
            if (webRequest == null)
            {
                throw new ArgumentNullException("webRequest");
            }

            WebResponse webResponse;
            lock (syncRoot)
            {
                logger.Debug("Sending request to Windows Azure Management REST API...");
                webResponse = webRequest.GetResponse();
                logger.Debug("...completed request to Windows Azure Management REST API.");
                Thread.Sleep(throttleTimeInMilliseconds);
            }

            return webResponse;
        }
    }
}
