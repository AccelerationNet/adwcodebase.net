using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data;
using Acceleration.MappedReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.MappedReader {
    [TestClass]
    public class DataExtensionsTests {

        Mock<IDataReader> MockReader;
        IDataReader Reader { get { return MockReader.Object; } }

        const string TEST_STRING = "foo";
        const int TEST_INT = 42;
        const decimal TEST_DECIMAL = 42.0m;
        const double TEST_DOUBLE = 3.14;
        DateTime TEST_DATETIME = DateTime.Now;


        [TestInitialize]
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

        [TestMethod]
        public void ColumnOrdinalMap() {
            var map = Reader.ColumnOrdinalMap();

            Assert.AreEqual(3, map.Count);
            Assert.AreEqual(1, map["B"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnOrdinalMapWithDupes() {
            // duplicate the column name
            MockReader.Setup(x => x.GetName(2)).Returns("A");
            Reader.ColumnOrdinalMap();
        }

        [TestMethod]
        public void GetAsWhenDBNull() {
            Assert.AreEqual(0, Reader.GetAs<int>(0), "ask for a normal type, get default");
            Assert.IsNull(Reader.GetAs<int?>(0), "ask for a nullable type, get null");
            Assert.IsNull(Reader.GetAs<DateTime?>(0));

            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime>(1));
            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime?>(1), "correctly handles nullable values");

        }

        [TestMethod]
        public void GetAs() {
            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime>(1));
            Assert.AreEqual(TEST_DATETIME, Reader.GetAs<DateTime?>(1), "correctly handles nullable values");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAsNoMapping() {
            Reader.GetAs<IDataReader>(1);       
        }

        const int ERROR_DECIMAL = 5;
        [TestMethod]
        public void BadMappingExceptionHasDetails() {
            MockReader.Setup(x => x.GetDecimal(ERROR_DECIMAL))
                .Throws<InvalidProgramException>();
            MockReader.Setup(x => x.GetName(ERROR_DECIMAL)).Returns("error");
            MockReader.Setup(x => x.GetDataTypeName(ERROR_DECIMAL))
                .Returns("type");
            MockReader.Setup(x => x.GetValue(ERROR_DECIMAL)).Returns("bad");
            try {
                Reader.GetAs<decimal>(ERROR_DECIMAL);
                Assert.Fail();
            }
            catch (BadMappingException bme) {
                Assert.AreEqual("bad", bme.Value);
                Assert.IsInstanceOfType(bme.Value, typeof(string));
                Assert.AreEqual(typeof(decimal), bme.RequestedType);
                Assert.AreEqual("error", bme.ColumnName);
                Assert.AreEqual("Failed to convert type column `error` from System.String `bad` to System.Decimal", bme.Message);
            }
        }

        [TestMethod]
        public void BadMappingExceptionWhenValueThrows() {
            MockReader.Setup(x => x.GetDecimal(ERROR_DECIMAL))
                .Throws<InvalidProgramException>();            
            MockReader.Setup(x => x.GetValue(ERROR_DECIMAL))
                .Throws<InvalidProgramException>();
            try {
                Reader.GetAs<decimal>(ERROR_DECIMAL);
                Assert.Fail();
            }
            catch (BadMappingException bme) {
                Assert.IsNull(bme.Value);
            }
        }
    }
}