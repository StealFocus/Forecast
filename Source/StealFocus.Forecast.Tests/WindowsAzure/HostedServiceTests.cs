namespace StealFocus.Forecast.Tests.WindowsAzure
{
    using Forecast.WindowsAzure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class HostedServiceTests
    {
        [TestMethod]
        public void TestDoSomething()
        {
            HostedService.DoSomething();
        }
    }
}
