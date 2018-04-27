using Microsoft.AspNetCore.Mvc;
using NetCoreCQRS;
using NetCoreDI;

namespace NetCoreIdentity.Controllers
{
    public class BaseApiController : Controller
    {
        private IExecutor _executor;
        protected IExecutor Executor => _executor ?? (_executor = AmbientContext.Current.Resolver.ResolveObject<IExecutor>());
    }
}