using NUnit.Framework;

namespace BankVue.Question2
{
    [TestFixture]
    public class string_extension_tests
    {
        [Test]
        public void reverse_flips_the_string_around()
        {
            var theString = "ABCDEFG";
            var expectedValue = "GFEDCBA";

            var actual = theString.Reverse();
            
            Assert.AreEqual(expectedValue,actual);
        }
    }
}