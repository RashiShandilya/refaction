using refactor_me.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.WebApi;

namespace refactor_me
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            container.RegisterType<IProductService, ProductService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);           
        }
    }
}