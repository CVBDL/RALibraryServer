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

        #region Constructors

        [TestMethod]
        public void WithValidIsbn10Test()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(null);
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
        public void WithInvalidIsbn10Test()
        {
            Isbn isbn;

            try
            {
                isbn = new Isbn("");
                Assert.Fail();
            }
            catch (IsbnFormatException) { }

            try
            {
                isbn = new Isbn("   ");
                Assert.Fail();
            }
            catch (IsbnFormatException) { }

            try
            {
                isbn = new Isbn(string.Empty);
                Assert.Fail();
            }
            catch (IsbnFormatException) { }

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
        public void WithValidIsbn13Test()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(null);

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
        public void WithInvalidIsbn13Test()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn("");
                isbn = new Isbn(string.Empty);
                isbn = new Isbn("978-1-86197-876-8");
                Assert.Fail();
            }
            catch (IsbnFormatException) { }
        }

        #endregion Constructors

        #region NormalizedValue

        [TestMethod]
        public void NormalizedValueTest()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(null);
                Assert.AreEqual(isbn.NormalizedValue, null);

                isbn = new Isbn("097522980x");
                Assert.AreEqual(isbn.NormalizedValue, "097522980X");

                isbn = new Isbn("097522980X");
                Assert.AreEqual(isbn.NormalizedValue, "097522980X");

                isbn = new Isbn("1843560283");
                Assert.AreEqual(isbn.NormalizedValue, "1843560283");
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }

        #endregion NormalizedValue

        #region IsbnType

        [TestMethod]
        public void IsbnTypeTest()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn(null);
                Assert.AreEqual(isbn.Type, IsbnType.None);

                isbn = new Isbn("097522980x");
                Assert.AreEqual(isbn.Type, IsbnType.Ten);

                isbn = new Isbn("097522980X");
                Assert.AreEqual(isbn.Type, IsbnType.Ten);

                isbn = new Isbn("9780306406157");
                Assert.AreEqual(isbn.Type, IsbnType.Thirteen);
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }

        #endregion IsbnType
    }
}
