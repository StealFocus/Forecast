namespace StealFocus.Forecast.WindowsAzure.Net
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using log4net;

    public static class WebRequestExtensions
    {
        private const int DefaultThrottleTimeInMilliseconds = 1000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

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
            lock (SyncRoot)
            {
                string sleepMessage = string.Format(CultureInfo.CurrentCulture, "Sleeping for {0} milliseconds before sending request to Windows Azure Management REST API (the Windows Azure Management REST API is throttled).", throttleTimeInMilliseconds);
                Logger.Debug(sleepMessage);
                Thread.Sleep(throttleTimeInMilliseconds);
                Logger.Debug("Sending request to Windows Azure Management REST API...");
                webResponse = webRequest.GetResponse();
                Logger.Debug("...completed request to Windows Azure Management REST API.");
            }

            return webResponse;
        }
    }
}
