namespace StealFocus.Forecast
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// ForecastException Class.
    /// </summary>
    [Serializable]
    public class ForecastException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastException"/> class.
        /// </summary>
        public ForecastException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ForecastException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner Exception.</param>
        public ForecastException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ForecastException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
