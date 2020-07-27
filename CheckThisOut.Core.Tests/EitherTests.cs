using System;
using FluentAssertions;
using JonBates.CheckThisOut.Core;
using Xunit;

namespace CheckThisOut.Core.Tests
{
    public class EitherTests
    {
        [Fact]
        public void Either_supports_left()
        {
            var sut = Either<string, string>.Left("hello");
            sut.IsLeft.Should().BeTrue();
            sut.IsRight.Should().BeFalse();
            sut.LeftValue.Should().Be("hello");

            Assert.Throws<InvalidOperationException>(() => sut.RightValue);
        }

        [Fact]
        public void Either_does_not_allow_null_as_a_left_value()
        {
            Assert.Throws<ArgumentNullException>(() => Either<string, string>.Left(null));
        }

        [Fact]
        public void Either_supports_right()
        {
            var sut = Either<string, string>.Right("hello");
            sut.IsRight.Should().BeTrue();
            sut.IsLeft.Should().BeFalse();
            sut.RightValue.Should().Be("hello");
            Assert.Throws<InvalidOperationException>(() => sut.LeftValue);
        }

        [Fact]
        public void Either_does_not_allow_null_as_a_right_value()
        {
            Assert.Throws<ArgumentNullException>(() => Either<string, string>.Right(null));
        }

        [Fact]
        public void Two_instances_with_the_same_payload_are_equivalent_left()
        {
            var first = Either<string, string>.Left("hello");
            var second = Either<string, string>.Left("hello");

            first.Should().BeEquivalentTo(second);
        }
        [Fact]
        public void Two_instances_with_the_same_payload_are_equivalent_right()
        {
            var first = Either<string, string>.Right("hello");
            var second = Either<string, string>.Right("hello");

            first.Should().BeEquivalentTo(second);
        }
    }
}
