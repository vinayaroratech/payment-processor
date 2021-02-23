using FluentAssertions;
using NUnit.Framework;
using Payments.Domain.Common.Exceptions;
using Payments.Domain.ValueObjects;

namespace Payments.Domain.UnitTests.ValueObjects
{
    public class StatusTests
    {
        [Test]
        public void ShouldReturnCorrectStatusCode()
        {
            var code = 0;

            var status = Status.From(code);

            status.Code.Should().Be(code);
        }

        [Test]
        public void ToStringReturnsCode()
        {
            var status = Status.Pending;

            status.ToString().Should().Be(status.Code.ToString());
        }

        [Test]
        public void ShouldPerformImplicitConversionToStatusCodeString()
        {
            string code = Status.Completed;

            code.Should().Be("3");
        }

        [Test]
        public void ShouldPerformExplicitConversionGivenSupportedStatusCode()
        {
            var status = (Status)4;

            status.Should().Be(Status.OnHold);
        }

        [Test]
        public void ShouldThrowUnsupportedStatusExceptionGivenNotSupportedStatusCode()
        {
            FluentActions.Invoking(() => Status.From(34))
                .Should().Throw<UnsupportedStatusException>();
        }
    }
}