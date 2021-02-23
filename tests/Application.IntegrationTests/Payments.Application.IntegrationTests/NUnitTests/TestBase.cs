using NUnit.Framework;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.NUnitTests
{
    using static Testing;
    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}
