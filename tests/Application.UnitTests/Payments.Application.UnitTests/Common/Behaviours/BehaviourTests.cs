using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payments.Application.Common.Behaviours;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.Commands.CreatePayment;
using System.Threading;

namespace Payments.Application.UnitTests.Common.Behaviours
{
    public class BehaviourTests
    {
        private const string UserId = "vinay";

        [Test]
        public void RequestLogger_Should_Call_GetUserNameAsync_Once_If_Authenticated()
        {
            var logger = new Mock<ILogger<CreatePaymentCommand>>();
            var currentUserService = new Mock<ICurrentUserService>();
            var identityService = new Mock<IIdentityService>();

            currentUserService.Setup(x => x.UserId).Returns(UserId);

            IRequestPreProcessor<CreatePaymentCommand> requestLogger = new RequestLogger<CreatePaymentCommand>(logger.Object, currentUserService.Object, identityService.Object);

            requestLogger.Process(new CreatePaymentCommand { Name = "Some Name" }, new CancellationToken());

            identityService.Verify(i => i.GetUserNameAsync(UserId), Times.Once);
        }


        [Test]
        public void RequestLogger_Should_Not_Call_GetUserNameAsync_Once_If_Unauthenticated()
        {
            var logger = new Mock<ILogger<CreatePaymentCommand>>();
            var currentUserService = new Mock<ICurrentUserService>();
            var identityService = new Mock<IIdentityService>();

            currentUserService.Setup(x => x.UserId).Returns((string)null);

            IRequestPreProcessor<CreatePaymentCommand> requestLogger = new RequestLogger<CreatePaymentCommand>(logger.Object, currentUserService.Object, identityService.Object);

            requestLogger.Process(new CreatePaymentCommand { Name = "Other Name" }, new CancellationToken());

            identityService.Verify(i => i.GetUserNameAsync(null), Times.Once);
        }
    }
}