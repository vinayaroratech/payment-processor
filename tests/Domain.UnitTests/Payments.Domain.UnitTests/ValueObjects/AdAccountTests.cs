using FluentAssertions;
using NUnit.Framework;
using Payments.Domain.Common.Exceptions;
using Payments.Domain.ValueObjects;

namespace Payments.Domain.UnitTests.ValueObjects
{
    public class AdAccountTests
    {
        [Test]
        public void ShouldHaveCorrectDomainAndName()
        {
            const string accountString = "VA\\Vinay";

            var account = AdAccount.For(accountString);

            account.Domain.Should().Be("VA");
            account.Name.Should().Be("Vinay");
        }

        [Test]
        public void ToStringReturnsCorrectFormat()
        {
            const string accountString = "VA\\Vinay";

            var account = AdAccount.For(accountString);

            var result = account.ToString();

            result.Should().Be(accountString);
        }

        [Test]
        public void ImplicitConversionToStringResultsInCorrectString()
        {
            const string accountString = "VA\\Vinay";

            var account = AdAccount.For(accountString);

            string result = account;

            result.Should().Be(accountString);
        }

        [Test]
        public void ExplicitConversionFromStringSetsDomainAndName()
        {
            const string accountString = "VA\\Vinay";

            var account = (AdAccount)accountString;

            account.Domain.Should().Be("VA");
            account.Name.Should().Be("Vinay");
        }

        [Test]
        public void ShouldThrowAdAccountInvalidExceptionForInvalidAdAccount()
        {
            Assert.Throws<AdAccountInvalidException>(() =>
            {
                var account = (AdAccount)"VAVinay";
            });
        }
    }
}