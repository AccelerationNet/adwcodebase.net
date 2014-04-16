using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceleration.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Collections {
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

        [TestMethod]
        public void RandomElement(){
            var c = new[] { 1, 2, 3 };

            for (var i = 0; i < 100; i++) {
                Assert.IsTrue(c.Contains(c.RandomElement()));
            }

            var r = new Random();
            var g = Enumerable.Range(0, 100)
                .Select(x => c.RandomElement(r))
                .Distinct();
            Assert.AreEqual(c.Count(), g.Count());
                        

            var v = c.RandomElement(new Random(42));
            var v2 = c.RandomElement(new Random(42));
            Assert.AreEqual(v, v2);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RandomElement_ThrowsWhenNull() {
            ICollection<int> c = null;
            c.RandomElement();
        }
    }
}
