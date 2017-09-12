using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaLibrary.Utils;

namespace RaLibrary.Tests.Utils
{
    /// <summary>
    /// Summary description for IsbnTest
    /// </summary>
    [TestClass]
    public class IsbnAttributeTests
    {
        public IsbnAttributeTests()
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
        public void Isbn10AttributeTest()
        {

            var validation = new IsbnTenAttribute();

            Assert.IsTrue(validation.IsValid(null));
            Assert.IsTrue(validation.IsValid("097522980X"));
            Assert.IsTrue(validation.IsValid("097522980x"));
            Assert.IsTrue(validation.IsValid("1843560283"));

            Assert.IsFalse(validation.IsValid(""));
            Assert.IsFalse(validation.IsValid("   "));
            Assert.IsFalse(validation.IsValid("9780306406157"));
        }

        [TestMethod]
        public void Isbn13AttributeTest()
        {

            var validation = new IsbnThirteenAttribute();

            Assert.IsTrue(validation.IsValid(null));
            Assert.IsTrue(validation.IsValid("9780306406157"));
            Assert.IsTrue(validation.IsValid("9781861978769"));
            Assert.IsTrue(validation.IsValid("9787550263284"));

            Assert.IsFalse(validation.IsValid(""));
            Assert.IsFalse(validation.IsValid("   "));
            Assert.IsFalse(validation.IsValid("097522980X"));
        }
    }
}

