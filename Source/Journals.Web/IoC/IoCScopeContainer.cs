using Microsoft.Practices.Unity;
using System.Web.Http.Dependencies;

namespace Journals.Web.IoC
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