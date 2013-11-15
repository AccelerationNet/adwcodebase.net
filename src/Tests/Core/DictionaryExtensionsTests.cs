using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Extensions;
using NUnit.Framework;
using Moq;

namespace Tests.Core {
    class DictionaryExtensionsTests : Base {

        IDictionary<int, int> dict;

        [SetUp]
        public void Setup() {
            dict = new Dictionary<int, int>();
        }

        [Test]
        public void GetWithDefault() {
            Assert.IsFalse(dict.ContainsKey(0));
            Assert.AreEqual(10, dict.Get(0, 10));
            Assert.IsFalse(dict.ContainsKey(0));
        }
        [Test]
        public void Get() {
            Assert.IsFalse(dict.ContainsKey(0));
            Assert.AreEqual(default(int), dict.Get(0));
            Assert.IsFalse(dict.ContainsKey(0));
        }

        [Test]
        public void Ensure() {
            var fetcher = new Mock<IExpensive>();
            fetcher.Setup(f => f.Run(0)).Returns(10);

            Assert.IsFalse(dict.ContainsKey(0));
            Assert.AreEqual(10, dict.Ensure(0, fetcher.Object.Run));
            Assert.AreEqual(10, dict[0]);
            fetcher.Verify(f => f.Run(0), Times.Once);
            
            Assert.AreEqual(10, dict.Ensure(0, fetcher.Object.Run));
            fetcher.Verify(f => f.Run(0), Times.Once, "fn only runs once for repeated calls to ensure");
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void EnsureErrorInValueFn() {
            var fetcher = new Mock<IExpensive>();
            fetcher.Setup(f => f.Run(0)).Throws<IndexOutOfRangeException>();
            dict.Ensure(0, fetcher.Object.Run);
        }
    }

    /// <summary>
    /// dummy interface to mock up
    /// </summary>
    public interface IExpensive {
        int Run(int i);
    }
}
