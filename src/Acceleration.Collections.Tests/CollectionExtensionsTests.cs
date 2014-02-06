using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Acceleration.Collections.Tests.Core {
    [TestClass]
    public class CollectionExtensionsTests {
        
        [TestMethod]
        public void Add() {
            ICollection<int> a = new List<int>();
            Assert.AreEqual(0, a.Count);
            a.Add(1);
            Assert.AreEqual(1, a.Count);
            Assert.AreEqual(2, a.Add(2, 3));
            Assert.AreEqual(3, a.Count);
            Assert.AreEqual(3, a.Add(a.ToArray().AsEnumerable()));
            Assert.AreEqual(6, a.Count);
        }
        [TestMethod]
        public void AddNull() {
            ICollection<int> a = new List<int>();
            Assert.AreEqual(0, a.Count);
            Assert.AreEqual(0, a.Add((IEnumerable<int>)null));
            Assert.AreEqual(0, a.Count);            
        }
    }
}
