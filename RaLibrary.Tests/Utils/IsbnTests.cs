using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaLibrary.Utils;

namespace RaLibrary.Tests.Utils
{
    /// <summary>
    /// Summary description for IsbnTest
    /// </summary>
    [TestClass]
    public class IsbnTests
    {
        public IsbnTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ValidIsbn10Test()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(null);
                isbn = new Isbn("");
                isbn = new Isbn(string.Empty);
                isbn = new Isbn("097522980x");
                isbn = new Isbn("097522980X");
                isbn = new Isbn("1843560283");
                isbn = new Isbn("0198526636");
                isbn = new Isbn("1861972717");
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InvalidIsbn10Test()
        {
            Isbn isbn;

            try
            {
                isbn = new Isbn("ISBN0-9752298-0-4");
                Assert.Fail();
            }
            catch (IsbnFormatException) { }

            try
            {
                isbn = new Isbn("ISBN 4 86197 271 7");
                Assert.Fail();
            }
            catch (IsbnFormatException) { }
        }

        [TestMethod]
        public void ValidIsbn13Test()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(null);
                isbn = new Isbn("");
                isbn = new Isbn(string.Empty);

                isbn = new Isbn("9780306406157");
                isbn = new Isbn("9781861978769");
                isbn = new Isbn("9787550263284");
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InvalidIsbn13Test()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn("978-1-86197-876-8");
                Assert.Fail();
            }
            catch (IsbnFormatException) { }
        }

        [TestMethod]
        public void NormalizedIsbnTest()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn("097522980x");
                Assert.AreEqual("097522980X", isbn.NormalizedIsbn);

                isbn = new Isbn("097522980X");
                Assert.AreEqual("097522980X", isbn.NormalizedIsbn);

                isbn = new Isbn("1843560283");
                Assert.AreEqual("1843560283", isbn.NormalizedIsbn);
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ValidIsbnTenTest()
        {
            // Valid ISBN 10
            Assert.IsTrue(Isbn.IsValidIsbnTen(null));
            Assert.IsTrue(Isbn.IsValidIsbnTen(""));
            Assert.IsTrue(Isbn.IsValidIsbnTen(string.Empty));

            Assert.IsTrue(Isbn.IsValidIsbnTen("097522980x"));
            Assert.IsTrue(Isbn.IsValidIsbnTen("097522980X"));
            Assert.IsTrue(Isbn.IsValidIsbnTen("1843560283"));
            Assert.IsTrue(Isbn.IsValidIsbnTen("0198526636"));
            Assert.IsTrue(Isbn.IsValidIsbnTen("1861972717"));
        }

        [TestMethod]
        public void InvalidIsbnTenTest()
        {
            // Invalid ISBN 10
            Assert.IsFalse(Isbn.IsValidIsbnTen("0975229804"));
            Assert.IsFalse(Isbn.IsValidIsbnTen("4861972717"));
            Assert.IsFalse(Isbn.IsValidIsbnTen("0 - 9752298 - 0 - 4"));
            Assert.IsFalse(Isbn.IsValidIsbnTen("A 0975229804"));
        }

        [TestMethod]
        public void ValidIsbnThirteenTest()
        {
            // Valid ISBN 13
            Assert.IsTrue(Isbn.IsValidIsbnThirteen(null));
            Assert.IsTrue(Isbn.IsValidIsbnThirteen(""));
            Assert.IsTrue(Isbn.IsValidIsbnThirteen(string.Empty));

            Assert.IsTrue(Isbn.IsValidIsbnThirteen("9780306406157"));
            Assert.IsTrue(Isbn.IsValidIsbnThirteen("9781861978769"));
            Assert.IsTrue(Isbn.IsValidIsbnThirteen("9787550263284"));
        }

        [TestMethod]
        public void InvalidIsbnThirteenTest()
        {
            // Invalid ISBN 13
            Assert.IsFalse(Isbn.IsValidIsbnThirteen("978-1-86197-876-8"));
            Assert.IsFalse(Isbn.IsValidIsbnThirteen("9781861978768"));
        }

        [TestMethod]
        public void ValidIsbnTest()
        {
            Assert.IsTrue(Isbn.IsValidIsbn(null));
            Assert.IsTrue(Isbn.IsValidIsbn(""));
            Assert.IsTrue(Isbn.IsValidIsbn(string.Empty));

            Assert.IsTrue(Isbn.IsValidIsbn("097522980x"));
            Assert.IsTrue(Isbn.IsValidIsbn("097522980X"));
            Assert.IsTrue(Isbn.IsValidIsbn("1861972717"));

            Assert.IsTrue(Isbn.IsValidIsbn("9780306406157"));
            Assert.IsTrue(Isbn.IsValidIsbn("9781861978769"));
            Assert.IsTrue(Isbn.IsValidIsbn("9787550263284"));

            Assert.IsFalse(Isbn.IsValidIsbn("0-9752298-0-4"));
            Assert.IsFalse(Isbn.IsValidIsbn("4 86197 271 7"));

            Assert.IsFalse(Isbn.IsValidIsbn("978-1-86197-876-8"));
        }
    }
}
