using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Common.Behaviours;
using System.Reflection;

namespace Payments.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            return services;
        }
    }
}