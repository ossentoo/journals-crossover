using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace Medico.Web.IoC
{
    public class IoCScopeContainer : ScopeContainer, System.Web.Mvc.IDependencyResolver
    {
        public IoCScopeContainer(IUnityContainer container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new ScopeContainer(child);
        }
    }
}