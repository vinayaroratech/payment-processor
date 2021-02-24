using AutoMapper;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Mappings;
using System;

namespace Payments.Application.UnitTests.Common
{
    public class CommandTestBase : IDisposable
    {
        public IConfigurationProvider Configuration { get; }
        public IMapper Mapper { get; }

        public CommandTestBase()
        {
            Configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = Configuration.CreateMapper();

            Context = ApplicationDbContextFactory.Create();
        }

        public IApplicationDbContext Context { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }
}