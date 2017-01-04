using Medico.Model.Helpers;

namespace Medico.Web.Tests.Controllers
{
    public abstract class BaseTests
    {
        public BaseTests()
        {
            AutoMapperHelper.Initialize();

        }
    }
}
