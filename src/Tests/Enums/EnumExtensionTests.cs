using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acceleration.Enums;
using System.Collections.Generic;
namespace Tests.Enums {
    [TestClass]
    public class EnumExtensionTests : Base {

        struct NotAnEnum {}

        [Flags]
        enum Testing {
            A = 0x1,
            B = 0x2,
            C = 0x4,
            FooBar = 0x8
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFlagsThrowsWithBadType() {
            var f = new NotAnEnum();
            var res = f.GetFlags().ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseToEnumThrowsWithBadType() {
            "foo".ParseToEnum<NotAnEnum>();            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryParseToEnumThrowsWithBadType() {
            NotAnEnum? x;
            "foo".TryParseToEnum<NotAnEnum>(out x);
        }

        [TestMethod]
        public void GetFlags() {
            var combo = Testing.A | Testing.C;
            var f = combo.GetFlags().ToList();
            Assert.IsNotNull(f);
            Assert.AreEqual(2, f.Count);
            Assert.IsTrue(f.Contains(Testing.A));
            Assert.IsTrue(f.Contains(Testing.C));

            f = Testing.A.GetFlags().ToList();
            Assert.AreEqual(1, f.Count);
            Assert.IsTrue(f.Contains(Testing.A));
        }

        [TestMethod]
        public void GetFlagsNullable() {
            Testing? combo = Testing.A | Testing.C;
            var f = combo.GetFlags().ToList();
            Assert.AreEqual(2, f.Count);            
        }

        [TestMethod]
        public void GetFlagsNull() {
            Testing? combo = null;
            Assert.IsNull(combo.GetFlags());
        }

        [TestMethod]
        public void ParseToEnum() {
            Assert.AreEqual(Testing.A, "A".ParseToEnum<Testing>());
            Assert.AreEqual(Testing.A, "a".ParseToEnum<Testing>());
            Assert.AreEqual(Testing.FooBar, "foobar".ParseToEnum<Testing>());
            Assert.AreEqual(Testing.FooBar, "foo bar".ParseToEnum<Testing>());
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ParseToEnumNullThrows() {
            string val = null;
            val.ParseToEnum<Testing>();;
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ParseToEnumEmptyThrows() {
            "".ParseToEnum<Testing>(); ;
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public void ParseToEnumNotInEnumThrows() {
            "asdf".ParseToEnum<Testing>();
        }

        void AssertTryParse(string input, Testing? expected) {
            Testing? x = null;
            var res = input.TryParseToEnum<Testing>(out x);
            Assert.AreEqual(expected != null, res);
            Assert.AreEqual(expected, x);

        }

        [TestMethod]
        public void TryParseToEnum() {
            foreach (var input in new[]{"A", "a", " a "}) {
                AssertTryParse(input, Testing.A);
            }

            foreach (var input in new[] { "foobar", "foo bar", "FooBar" }) {
                AssertTryParse(input, Testing.FooBar);
            }
        }
        [TestMethod]
        public void TryParseToEnumFailures() {
            foreach (var input in new[] { "", null, "  ", "asdf" }) {
                AssertTryParse(input, null);
            }
        }
    }
}
