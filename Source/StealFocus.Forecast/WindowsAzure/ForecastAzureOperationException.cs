namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Linq;

    /// <summary>
    /// ForecastException Class.
    /// </summary>
    [Serializable]
    public class ForecastAzureOperationException : ForecastException, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastAzureOperationException"/> class.
        /// </summary>
        public ForecastAzureOperationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastAzureOperationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ForecastAzureOperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastAzureOperationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner Exception.</param>
        public ForecastAzureOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastAzureOperationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ForecastAzureOperationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public XDocument ResponseBody { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ResponseBody", this.ResponseBody);
        }
    }
}
