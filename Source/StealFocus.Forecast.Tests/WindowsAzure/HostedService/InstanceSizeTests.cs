namespace StealFocus.Forecast.Tests.WindowsAzure.HostedService
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class InstanceSizeTests
    {
        [TestMethod]
        public void UnitTestLessThanOperator()
        {
            Assert.IsTrue(InstanceSize.ExtraSmall < InstanceSize.Small);
            Assert.IsTrue(InstanceSize.Small < InstanceSize.Medium);
            Assert.IsTrue(InstanceSize.Medium < InstanceSize.Large);
            Assert.IsTrue(InstanceSize.Large < InstanceSize.ExtraLarge);
            Assert.IsFalse(InstanceSize.ExtraSmall > InstanceSize.Small);
            Assert.IsFalse(InstanceSize.Small > InstanceSize.Medium);
            Assert.IsFalse(InstanceSize.Medium > InstanceSize.Large);
            Assert.IsFalse(InstanceSize.Large > InstanceSize.ExtraLarge);
        }

        [TestMethod]
        public void UnitTestGreaterThanOperator()
        {
            Assert.IsTrue(InstanceSize.Small > InstanceSize.ExtraSmall);
            Assert.IsTrue(InstanceSize.Medium > InstanceSize.Small);
            Assert.IsTrue(InstanceSize.Large > InstanceSize.Medium);
            Assert.IsTrue(InstanceSize.ExtraLarge > InstanceSize.Large);
            Assert.IsFalse(InstanceSize.Small < InstanceSize.ExtraSmall);
            Assert.IsFalse(InstanceSize.Medium < InstanceSize.Small);
            Assert.IsFalse(InstanceSize.Large < InstanceSize.Medium);
            Assert.IsFalse(InstanceSize.ExtraLarge < InstanceSize.Large);
        }
    }
}
