using System;
using System.Collections.Generic;
using System.Data;
using Acceleration.Core.Data;
using Acceleration.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Core.Data {
    [TestClass]
    public class MappedReaderTests : Base {
        Mock<IDataReader> MockReader;
        IDataReader Reader { get { return MockReader.Object; } }
        IMappedReader Map;


        [TestInitialize]
        public void Setup() {
            MockReader = new Mock<IDataReader>();
            MockReader.SetupGet(x => x.FieldCount).Returns(3);
            MockReader.Setup(x => x.GetName(0)).Returns("TestStr");
            MockReader.Setup(x => x.GetName(1)).Returns("TestInt");
            MockReader.Setup(x => x.GetName(2)).Returns("TestNullInt");

            MockReader.Setup(x => x.IsDBNull(0)).Returns(false);
            MockReader.Setup(x => x.IsDBNull(1)).Returns(false);
            MockReader.Setup(x => x.IsDBNull(2)).Returns(false);

            MockReader.Setup(x => x.GetString(0)).Returns(TEST_STRING);
            MockReader.Setup(x => x.GetInt32(1)).Returns(TEST_INT);
            MockReader.Setup(x => x.GetInt32(2)).Returns(TEST_INT + 1);
            Map = MockReader.Object.MappedReader();
        }

        class TestObjLessPropsThanQuery {
            public string TestStr { get; set; }            
        }

        class TestObj : TestObjLessPropsThanQuery {            
            public int TestInt { get; set; }
            public int? TestNullInt { get; set; }
        }

        class TestObjMorePropsThanQuery : TestObj {
            public DateTime? TestDate { get; set; }
        }

        [TestMethod]
        public void Parse() {
            var obj = Map.Parse<TestObj>();
            Assert.AreEqual(TEST_STRING, obj.TestStr);
            Assert.AreEqual(TEST_INT, obj.TestInt);
            Assert.AreEqual(TEST_INT+1, obj.TestNullInt);
        }

        [TestMethod]
        public void ParseNull() {
            MockReader.Setup(x => x.IsDBNull(0)).Returns(true);
            MockReader.Setup(x => x.IsDBNull(2)).Returns(true);

            var obj = Map.Parse<TestObj>();
            Assert.IsNull(obj.TestStr);
            Assert.AreEqual(TEST_INT, obj.TestInt);
            Assert.IsNull(obj.TestNullInt);
        }

        [TestMethod]
        public void ParsePartial() {           
            var obj = Map.Parse<TestObjLessPropsThanQuery>();
            Assert.AreEqual(TEST_STRING, obj.TestStr);
        }
        [TestMethod]
        public void ParseWithTooManyProps() {
            var obj = Map.Parse<TestObjMorePropsThanQuery>();
            Assert.AreEqual(TEST_STRING, obj.TestStr);
            Assert.AreEqual(TEST_INT, obj.TestInt);
            Assert.AreEqual(TEST_INT + 1, obj.TestNullInt);
            Assert.IsNull(obj.TestDate);
        }

        [TestMethod]
        public void ParseWithRenames() {
            var obj = Map.Parse<TestObj>(new Dictionary<string, string>() {
                {"TestNullInt" , "TestInt"}
            });
            Assert.AreEqual(TEST_STRING, obj.TestStr);
            Assert.AreEqual(TEST_INT, obj.TestInt);
            Assert.AreEqual(TEST_INT, obj.TestNullInt);            
        }
    }
}
