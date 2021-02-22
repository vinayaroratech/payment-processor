using Payments.Domain.Common.Exceptions;
using Payments.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace Payments.Domain.UnitTests.ValueObjects
{
    public class AdAccountTests
    {
        [Fact]
        public void ShouldHaveCorrectDomainAndName()
        {
            const string accountString = "VA\\Vinay";

            var account = AdAccount.For(accountString);

            account.Domain.ShouldBe("VA");
            account.Name.ShouldBe("Vinay");
        }

        [Fact]
        public void ToStringReturnsCorrectFormat()
        {
            const string accountString = "VA\\Vinay";

            var account = AdAccount.For(accountString);

            var result = account.ToString();

            result.ShouldBe(accountString);
        }

        [Fact]
        public void ImplicitConversionToStringResultsInCorrectString()
        {
            const string accountString = "VA\\Vinay";

            var account = AdAccount.For(accountString);

            string result = account;

            result.ShouldBe(accountString);
        }

        [Fact]
        public void ExplicitConversionFromStringSetsDomainAndName()
        {
            const string accountString = "VA\\Vinay";

            var account = (AdAccount)accountString;

            account.Domain.ShouldBe("VA");
            account.Name.ShouldBe("Vinay");
        }

        [Fact]
        public void ShouldThrowAdAccountInvalidExceptionForInvalidAdAccount()
        {
            Assert.Throws<AdAccountInvalidException>(() => (AdAccount)"VAVinay");
        }
    }
}