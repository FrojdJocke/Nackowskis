using System;
using Nackowskis.Infrastructure;
using Xunit;

namespace UnitTests
{
    public class UnitTests
    {
        [Theory]
        [InlineData(8,6,-2)]
        [InlineData(2,4,2)]
        public void NumberDifferenceTest(int expectedValue, int value1, int value2)
        {
            var actualValue = Calculate.Difference(value1, value2);
            Assert.Equal(expectedValue,actualValue);
        }
        [Theory]
        [InlineData(true,1,2)]
        [InlineData(true,4900,4901)]
        [InlineData(false,500,150)]
        public void GreaterValueTest(bool expectedValue, int currentValue, int newValue)
        {
            var result = Calculate.GreaterValueControl(currentValue, newValue);
            Assert.Equal(expectedValue,result);
        }


    }
}
