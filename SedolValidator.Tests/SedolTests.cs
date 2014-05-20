using System.Globalization;
using NUnit.Framework;

namespace SedolValidator.Tests
{
    [TestFixture]
    public class SedolTests
    {
        [Test]
        public void CharacterCodeForBIs11()
        {
            var actual = Sedol.Code('B');
            Assert.AreEqual(11, actual);
        }

        [Test]
        public void CharacterCodeForZIs35()
        {
            var actual = Sedol.Code('z');
            Assert.AreEqual(35, actual);
        }

        [Test]
        public void CharacterCodeFor0Is0()
        {
            var actual = Sedol.Code('0');
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Some random VALID sedols from the sedol wiki, including a user-defined one.
        /// Note, no validation/sanitisation of input, fewer than 6 characters in the input will throw
        /// an IndexOutOfRangeException.
        /// http://en.wikipedia.org/wiki/SEDOL
        /// </summary>
        /// <param name="input"></param>
        [TestCase("0709954")]
        [TestCase("7108899")]
        [TestCase("B0YBKJ7")]
        [TestCase("4065663")]
        [TestCase("B0YBLH2")]
        [TestCase("2282765")]
        [TestCase("B0YBKL9")]
        [TestCase("5579107")]
        [TestCase("B0YBKR5")]
        [TestCase("5852842")]
        [TestCase("B0YBKT7")]
        [TestCase("B000300")]
        [TestCase("9aBcDe1")]
        public void CheckDigitTest(string input)
        {
            Sedol sedol = new Sedol(input);
            var actual = sedol.CheckDigit;
            Assert.AreEqual(input[6].ToString(CultureInfo.InvariantCulture), actual.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("123456789")]
        [TestCase("12")]
        public void ValidLengthChecks(string input)
        {
            Sedol sedol = new Sedol(input);
            Assert.IsFalse(sedol.IsValidLength);
        }

        [TestCase("éz-^&**")]
        [TestCase("éz-^&*ó")]
        public void SedolsContainingNonAlphanumericCharacters(string input)
        {
            Sedol sedol = new Sedol(input);
            Assert.IsFalse(sedol.IsAlphaNumeric);
        }

        [TestCase("9123458")]
        [TestCase("9aBcDe1")]
        public void UserDefinedSedols(string input)
        {
            Sedol sedol = new Sedol(input);
            Assert.IsTrue(sedol.IsUserDefined);
        }

        [TestCase("9123457")]
        [TestCase("9aBcDe6")]
        public void UserDefinedSedolsWithIncorrectChecksum(string input)
        {
            Sedol sedol = new Sedol(input);
            Assert.IsFalse(sedol.HasValidCheckDigit);
        }

        [TestCase("1234567")]
        [TestCase("1234566")]
        public void SedolsWithIncorrectChecksum(string input)
        {
            Sedol sedol = new Sedol(input);
            Assert.IsFalse(sedol.HasValidCheckDigit);
        }

        [TestCase("aeuioA7")]
        [TestCase("AEIOUAE")]
        public void StringContainingVowelsWillReturnExpectedValidationDetails(string input)
        {
            Sedol sedol = new Sedol(input);
            Assert.IsTrue(sedol.ContainsVowel);
        }
    }
}
