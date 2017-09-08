using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void RejectNullに非null値を渡しても例外を投げない()
        {
            object param1 = new object();

            try
            {
                Utilities.RejectNull(param1, nameof(param1));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RejectNullにnullを渡すとArgumentNullExceptionを投げる()
        {
            object param1 = null;

            try
            {
                Utilities.RejectNull(param1, nameof(param1));
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("param1", ex.ParamName);
            }
        }

    }
}
