using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payments.Application.Common.Behaviours;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.Commands.CreatePayment;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Common.Behaviours
{
    public class LoggingBehaviour
    {
        private readonly Mock<ILogger<CreatePaymentCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;


        public LoggingBehaviour()
        {
            _logger = new Mock<ILogger<CreatePaymentCommand>>();

            _currentUserService = new Mock<ICurrentUserService>();

            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new RequestLogger<CreatePaymentCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreatePaymentCommand { CardHolder= "Name Vinay" }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new RequestLogger<CreatePaymentCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreatePaymentCommand { CardHolder = "Another Name" }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
        }
    }
}