using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System.Data;
using Acceleration.Extensions;

namespace Tests.Core {
    class DataExtensionsTests : Base {

        Mock<IDataReader> MockReader;
        IDataReader Reader { get { return MockReader.Object; } }

        [SetUp]
        public void SetupMocks() {
            MockReader = new Mock<IDataReader>();
            MockReader.SetupGet(x => x.FieldCount).Returns(3);
            MockReader.Setup(x => x.GetName(0)).Returns("A");
            MockReader.Setup(x => x.GetName(1)).Returns("B");
            MockReader.Setup(x => x.GetName(2)).Returns("C");

            MockReader.Setup(x => x.IsDBNull(0)).Returns(true);
            MockReader.Setup(x => x.IsDBNull(1)).Returns(false);
            MockReader.Setup(x => x.GetString(1)).Returns(TEST_STRING);
            MockReader.Setup(x => x.GetDecimal(1)).Returns(TEST_DECIMAL);
            MockReader.Setup(x => x.GetDouble(1)).Returns(TEST_DOUBLE);
            MockReader.Setup(x => x.GetBoolean(1)).Returns(true);
            MockReader.Setup(x => x.GetInt32(1)).Returns(TEST_INT);
            MockReader.Setup(x => x.GetDateTime(1)).Returns(TEST_DATETIME);
        }

        [Test]
        public void ColumnOrdinalMap() {
            var map = Reader.ColumnOrdinalMap();

            Assert.AreEqual(3, map.Count);
            Assert.AreEqual(1, map["B"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnOrdinalMapWithDupes() {
            // duplicate the column name
            MockReader.Setup(x => x.GetName(2)).Returns("A");
            Reader.ColumnOrdinalMap();
        }

        [Test]
        public void GetAsWhenDBNull() {
            Assert.AreEqual(0, Reader.GetAs<int>(0), "ask for a normal type, get default");
            Assert.IsNull(Reader.GetAs<int?>(0), "ask for a nullable type, get null");
            Assert.IsNull(Reader.GetAs<DateTime?>(0));

            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime>(1));
            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime?>(1), "correctly handles nullable values");

        }

        [Test]
        public void GetAs() {
            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime>(1));
            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime?>(1), "correctly handles nullable values");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAsNoMapping() {
            Reader.GetAs<IDataReader>(1);       
        }
    }
}
