using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Collections;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Collections {
    [TestClass]
    public class DictionaryExtensionsTests {

        IDictionary<int, int> dict;

        [TestInitialize]
        public void Setup() {
            dict = new Dictionary<int, int>();
        }

        [TestMethod]
        public void GetWithDefault() {
            Assert.IsFalse(dict.ContainsKey(0));
            Assert.AreEqual(10, dict.Get(0, 10));
            Assert.IsFalse(dict.ContainsKey(0));
        }
        [TestMethod]
        public void Get() {
            Assert.IsFalse(dict.ContainsKey(0));
            Assert.AreEqual(default(int), dict.Get(0));
            Assert.IsFalse(dict.ContainsKey(0));
        }

        [TestMethod]
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

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void EnsureErrorInValueFn() {
            var fetcher = new Mock<IExpensive>();
            fetcher.Setup(f => f.Run(0)).Throws<IndexOutOfRangeException>();
            dict.Ensure(0, fetcher.Object.Run);
        }

        [TestMethod]
        public void Update() {
            Assert.AreEqual(0, dict.Count);
            var d = dict.Update(new Dictionary<int, int> { { 0, 0 }, {1,1} });
            Assert.AreSame(dict, d);
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(0, dict[0]);
            Assert.AreEqual(1, dict[1]);

            dict.Update(new Dictionary<int, int>());
            Assert.AreEqual(2, dict.Count);
            
        }

        [TestMethod]
        public void Update_Overwrites() {
            Assert.AreEqual(0, dict.Count);
            dict.Add(0, 0);
            Assert.AreEqual(0, dict[0]);
            dict.Update(new Dictionary<int, int> {{0, 1}});
            Assert.AreEqual(1, dict[0]);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_ThrowIfSourceIsNull() {            
            dict.Update(null);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_ThrowIfDictIsNull() {
            IDictionary<int, int> d = null;
            d.Update(dict);
        }
    }

    /// <summary>
    /// dummy interface to mock up
    /// </summary>
    public interface IExpensive {
        int Run(int i);
    }
}
