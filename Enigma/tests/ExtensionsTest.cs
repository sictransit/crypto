using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.SicTransit.Crypto.Enigma.Extensions;
using System;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma.Tests
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void TestChunkByFive()
        {
            const string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var chunked = s.ChunkedByFive();

            Assert.AreEqual((int)Math.Ceiling(s.Length / 5d) - 1, chunked.Count(x => x == ' '));
        }
    }
}