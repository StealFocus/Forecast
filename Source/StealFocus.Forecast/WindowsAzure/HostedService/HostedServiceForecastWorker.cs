namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Globalization;
    using System.Threading;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal abstract class HostedServiceForecastWorker : ForecastWorker
    {
        private const int FiveSecondsInMilliseconds = 5000;

        protected HostedServiceForecastWorker(string id)
            : base(id)
        {
        }

        protected HostedServiceForecastWorker(string id, int sleepTimeInMilliseconds)
            : base(id, sleepTimeInMilliseconds)
        {
        }

        protected void WaitForResultOfRequest(ILog logger, string workerTypeName, IOperation operation, Guid subscriptionId, string certificateThumbprint, string requestId)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.Status = OperationStatus.InProgress;
            bool done = false;
            while (!done)
            {
                operationResult = operation.StatusCheck(subscriptionId, certificateThumbprint, requestId);
                if (operationResult.Status == OperationStatus.InProgress)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' submitted a deployment request with ID '{2}', the operation was found to be in process, waiting for '{3}' seconds.", workerTypeName, this.Id, requestId, FiveSecondsInMilliseconds / 1000);
                    logger.Info(logMessage);
                    Thread.Sleep(FiveSecondsInMilliseconds);
                }
                else
                {
                    done = true;
                }
            }

            if (operationResult.Status == OperationStatus.Failed)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' submitted a deployment request with ID '{2}' and it failed. The code was '{3}' and message '{4}'.", workerTypeName, this.Id, requestId, operationResult.Code, operationResult.Message);
                logger.Error(logMessage);
            }
            else if (operationResult.Status == OperationStatus.Succeeded)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' submitted a deployment request with ID '{2}' and it succeeded. The code was '{3}' and message '{4}'.", workerTypeName, this.Id, requestId, operationResult.Code, operationResult.Message);
                logger.Info(logMessage);
            }
        }
    }
}
