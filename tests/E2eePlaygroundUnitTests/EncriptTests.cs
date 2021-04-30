using FluentAssertions;
using NUnit.Framework;

namespace E2eePlayegroundUnitTests
{
    public class EncriptTests
    {

        //Some examples:
        //titanic-> 3(3 - ("
        //minecraft-> , (-$"1 %3
        //javascript-> ) 5 2"1(/3
        [TestCase("titanic", "3(3 - (\"")]
        [TestCase("minecraft", ", (-$\"1 % 3")]
        [TestCase("", ") 5 2\"1(/ 3")]
        public void TestAlgo(string input, string expected)
        {

            // arrange
            // act
            var sut = Encript(input);

            // assert
            sut.Should().Be(expected);
        }

        private string Encript(string input)
        {
            return input;
        }
    }
}