using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace attrService
{
    public class InjectableAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; }
        public InjectableAttribute(ServiceLifetime lifeTime = ServiceLifetime.Transient)
        {
            Lifetime = lifeTime;
        }
    }


    [Injectable(ServiceLifetime.Transient)]
    public class TransientInjectableService : ITransientService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " by Injectable";
        }
    }


    [Injectable(ServiceLifetime.Scoped)]
    public class ScopedInjectableService : IScopedService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " by Injectable";
        }
    }


    [Injectable(ServiceLifetime.Singleton)]
    public class SingletonInjectableService : ISingletonService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " by Injectable";
        }
    }

    public static class ScrutorExtensions
    {
        public static IImplementationTypeSelector InjectableAttributes(this IImplementationTypeSelector selector)
        {
            var lifeTimes = Enum.GetValues(typeof(ServiceLifetime)).Cast<ServiceLifetime>();

            foreach(var item in lifeTimes)
                selector = selector.InjectableAttribute(item);
            
            return selector;
        }

        public static IImplementationTypeSelector InjectableAttribute(this IImplementationTypeSelector selector, ServiceLifetime lifeTime)
        {
            return selector.AddClasses(c => c.WithAttribute<InjectableAttribute>(i => i.Lifetime == lifeTime))
                .AsImplementedInterfaces()
                .WithLifetime(lifeTime);
        }
    }

}