using System.Web.Mvc;
using System.Web.Routing;

namespace Webdiyer.MvcPagerDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("MvcPager_PageSize", "{controller}/{action}/{pagesize}/{pageindex}", new { controller = "Demo", action = "PageSize", pageindex = UrlParameter.Optional }, new { action = "PageSize", pagesize = @"\d*", pageindex = @"\d*" });
            routes.MapRoute("MvcPager_Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new { id = @"\d*" });
            routes.MapRoute("MvcPager_SEO", "{controller}/{action}/page_{id}", new { controller = "Demo", action = "FirstPageUrl", id = 1 }, new { action = "FirstPageUrl", id = @"\d*" });
            routes.MapRoute("MvcPager_Pager1", "{controller}/{action}/page_{pageindex}", new { controller = "Demo", action = "CustomRouting", pageindex = 1 }, new { action = "CustomRouting", pageindex = @"\d*" });
            routes.MapRoute("MvcPager_Pager2", "{controller}/{action}/pageindex-{pageindex}", new { controller = "Demo", action = "CustomRouting", pageindex = 1 }, new { action = "CustomRouting", pageindex = @"\d*" });
            routes.MapRoute("MvcPager_Pager3", "{controller}/{action}/p-{pageindex}", new { controller = "Demo", action = "CustomRouting", pageindex = 1}, new { action = "CustomRouting", pageindex = @"\d*" });

        }
    }
}
