namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Net;

    public struct OperationResult
    {
        public Guid Id { get; set; }

        public OperationStatus Status { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
