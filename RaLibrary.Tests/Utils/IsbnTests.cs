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
                isbn = new Isbn("ISBN0-9752298-0-X");
                isbn = new Isbn("ISBN1-84356-028-3");
                isbn = new Isbn("ISBN0-19-852663-6");
                isbn = new Isbn("ISBN 1 86197 271 7");
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
                isbn = new Isbn("978-0-306-40615-7");
                isbn = new Isbn("978-1-86197-876-9");
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
        public void NormalizedValueTest()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn("ISBN0-9752298-0-X");
                Assert.AreEqual("097522980X", isbn.NormalizedValue);

                isbn = new Isbn("ISBN1-84356-028-3");
                Assert.AreEqual("1843560283", isbn.NormalizedValue);

                isbn = new Isbn("ISBN 1 86197 271 7");
                Assert.AreEqual("1861972717", isbn.NormalizedValue);

                isbn = new Isbn("ISBN978-0-306-40615-7");
                Assert.AreEqual("9780306406157", isbn.NormalizedValue);

                isbn = new Isbn("9787550263284");
                Assert.AreEqual("9787550263284", isbn.NormalizedValue);
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void NormalizedValueUppercaseTest()
        {
            Isbn isbn;
            try
            {
                isbn = new Isbn("ISBN0-9752298-0-x");
                Assert.AreEqual("097522980X", isbn.NormalizedValue);
            }
            catch (IsbnFormatException)
            {
                Assert.Fail();
            }
        }
    }
}
