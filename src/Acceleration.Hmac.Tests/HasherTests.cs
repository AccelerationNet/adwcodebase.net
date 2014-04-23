using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acceleration.Hmac;

namespace Acceleration.Hmac.Tests {
    [TestClass]
    public class HasherTests {

        class ViewModel : Hmac {
            [Hmac]
            public int Id { get; set; }
            [Hmac]
            public decimal Amount { get; set; }
            public string Input { get; set; }
        }

        class ViewModelChild : ViewModel { }

        ViewModel testObj;

        [TestInitialize]
        public void Setup() {
            testObj = new ViewModel() {
                Id = 1,
                Amount = 5.5m,
                Input = "A"
            };
        }

        [TestMethod]
        public void FieldsMatter() {
            var h1 = Hasher.ComputeHash(null, 1);
            Assert.IsNotNull(h1);
            Assert.AreNotEqual(h1, Hasher.ComputeHash(null, 2), "data matters");
        }

        [TestMethod]
        public void SaltMatters() {
            var h1 = Hasher.ComputeHash(null, 1);
            Assert.IsNotNull(h1);
            var h2 = Hasher.ComputeHash("salt", 1);
            Assert.IsNotNull(h2);
            Assert.AreNotEqual(h1, h2, "salt matters");
        }

        [TestMethod]
        public void NonHashedFieldsDontMatter() {
            var h1 = testObj.ComputeHash();
            Assert.IsNotNull(h1);
            testObj.Input += "test";
            Assert.AreEqual(h1, testObj.ComputeHash());
        }

        [TestMethod]
        public void HasherUpdatesObjectHash() {
            var h1 = testObj.ComputeHash();
            Assert.IsNotNull(h1);
            Assert.AreEqual(h1, testObj.Hash);
        }

        [TestMethod]
        public void ObjectIdentityDoesntMatter() {
            var h1 = testObj.ComputeHash();
            Assert.IsNotNull(h1);
            var copy = new ViewModel() {
                Id = testObj.Id,
                Amount = testObj.Amount,
                Input = testObj.Input
            };
            Assert.AreEqual(h1, copy.ComputeHash());
        }

        [TestMethod]
        public void TypeIsImplicitSalt() {
            var other = new ViewModelChild() {
                Id = testObj.Id,
                Amount = testObj.Amount,
                Input = testObj.Input
            };
            Assert.AreNotEqual(testObj.ComputeHash(), other.ComputeHash());
        }

        [TestMethod]
        public void HasherDoesntOverride() {
            Assert.IsNull(testObj.Hash);
            var h1 = testObj.ComputeHash("salt a");
            var h2 = testObj.ComputeHash("salt b");
            Assert.AreNotEqual(h1, h2);
            Assert.AreEqual(h1, testObj.Hash);
        }

        class VMContainer : Hmac {
            [Hmac]
            public ViewModel Inner { get; set; }
        }

        [TestMethod]
        public void NestedHmacSupport() {
            Assert.IsNull(testObj.Hash);
            var container = new VMContainer() {
                Inner = testObj
            };
            container.ComputeHash();
            Assert.IsNotNull(container.Hash, "container has hash");
            Assert.IsNotNull(testObj.Hash, "child has hash");
            Assert.AreNotEqual(container.Hash, testObj.Hash);
        }

        [TestMethod]
        public void Validate() {
            Assert.IsNull(testObj.Hash);
            testObj.ComputeHash();
            Assert.IsTrue(testObj.ValidateHash());

            Assert.IsFalse(testObj.ValidateHash("asdf"), "different salt");
            testObj.Id++;
            Assert.IsFalse(testObj.ValidateHash(), "different data");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidateNeedsHash() {
            Assert.IsNull(testObj.Hash);
            testObj.ValidateHash();
        }
    }
}
