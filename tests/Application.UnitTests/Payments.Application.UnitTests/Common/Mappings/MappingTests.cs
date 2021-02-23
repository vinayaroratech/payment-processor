using AutoMapper;
using NUnit.Framework;
using Payments.Application.Common.Mappings;
using Payments.Application.Payments.Queries.GetPayment;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Domain.Entities;
using System;

namespace Payments.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Payment), typeof(PaymentDto))]
        [TestCase(typeof(Payment), typeof(PaymentVm))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            object instance;
            try
            {
                instance = Activator.CreateInstance(source);
            }
            catch (MissingMethodException)
            {
                instance = System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(source);
            }

            _mapper.Map(instance, source, destination);
        }
    }
}