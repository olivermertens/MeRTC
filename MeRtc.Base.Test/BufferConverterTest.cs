using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeRtc.Base.Test
{
    [TestClass]
    public class BufferConverterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            byte[] buffer = new byte[4];

            BufferConverter.WriteBytes((int)1, buffer, 0, true);

            Assert.IsTrue(buffer[3] == 1);

            int value = System.BitConverter.ToInt32(buffer, 0);
        }

        [TestMethod]
        public void DoubleToAndFromBuffer()
        {
            double value = -573.994635621;

            var buffer = new byte[8];
            BufferConverter.WriteBytes(value, buffer, 0, true);

            double value2 = BufferConverter.ToDouble(buffer, 0, true);

            Assert.AreEqual(value, value2);
        }
    }
}
