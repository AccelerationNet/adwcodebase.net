using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Acceleration.Extensions;

namespace Tests.Core {
    class CollectionExtensionsTests : Base {

        
        [Test]
        public void Add() {
            ICollection<int> a = new List<int>();
            Assert.IsEmpty(a);
            a.Add(1);
            Assert.AreEqual(1, a.Count);
            Assert.AreEqual(2, a.Add(2, 3));
            Assert.AreEqual(3, a.Count);
            Assert.AreEqual(3, a.Add(a.ToArray().AsEnumerable()));
            Assert.AreEqual(6, a.Count);
        }
    }
}
