using System.Data.Entity;
using Medico.Repository;
using Medico.Repository.DataContext;
using Medico.Web.Controllers;
using Microsoft.Practices.Unity;

namespace Medico.Web.IoC
{
    public static class IoCMappingContainer
    {
        private static IUnityContainer _Instance = new UnityContainer();

        static IoCMappingContainer()
        {
        }

        public static IUnityContainer GetInstance()
        {
            _Instance.RegisterType<DbContext, JournalsContext>();

            var context = _Instance.Resolve<JournalsContext>();

            _Instance.RegisterType<IJournalRepository, JournalRepository>(new HierarchicalLifetimeManager(), new InjectionConstructor(context));
            _Instance.RegisterType<ISubscriptionRepository, SubscriptionRepository>(new HierarchicalLifetimeManager(), new InjectionConstructor(context));
            _Instance.RegisterType<IStaticMembershipService, StaticMembershipService>(new HierarchicalLifetimeManager());

            _Instance.RegisterType<HomeController>();
            _Instance.RegisterType<PublisherController>();
            _Instance.RegisterType<SubscriberController>();

            return _Instance;
        }
    }
}