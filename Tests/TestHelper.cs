using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace MvcPager.Tests
{
    public static class TestHelper
    {
        internal const string AppPathModifier = "/$(SESSION)";
        internal const string CopyrightTextCn = "\r\n<!--MvcPager v3.0 for ASP.NET MVC 4.0+ © 2009-2015 Webdiyer (http://www.webdiyer.com)-->\r\n";
        internal const string CopyrightText = "\r\n<!--MvcPager v3.0 for ASP.NET MVC 4.0+ © 2009-2015 Webdiyer (http://en.webdiyer.com)-->\r\n";
        internal const string InvalidPageIndexErrorMessage = "Page index is invalid";
        internal const string PageIndexOutOfRangeErrorMessage = "Page index is out of range";
        internal const string InvalidPageIndexErrorMessageCn = "页索引无效";
        internal const string PageIndexOutOfRangeErrorMessageCn = "页索引超出范围";

        internal static HtmlHelper<object> GetHtmlHelper()
        {
            return GetHtmlHelper(null);
        }

        internal static HtmlHelper<object> GetHtmlHelper(RouteValueDictionary routeValues)
        {
            HttpContextBase httpcontext = GetHttpContext();
            var rt = new RouteCollection();
            var defaultRoute = rt.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("constraintroute", new Route("{controller}/{action}/{id}", new RouteValueDictionary(new {id = UrlParameter.Optional }),new RouteValueDictionary(new{id="\\d*"}), null));
            var rd = new RouteData {Route = defaultRoute};
            rd.Values.Add("controller", "mvcpager");
            rd.Values.Add("action", "test");
            rd.Values.Add("id", 2);
            if (routeValues != null && routeValues.Count > 0)
            {
                foreach (var de in routeValues)
                {
                    rd.Values[de.Key] = de.Value;
                }
            }
            var vdd =new ViewDataDictionary();
            var viewContext = new ViewContext()
            {
                HttpContext = httpcontext,
                RouteData = rd,
                ViewData = vdd
            };
            var mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

            var htmlHelper = new HtmlHelper<object>(viewContext, mockVdc.Object, rt);
            return htmlHelper;
        }


        internal static AjaxHelper GetAjaxHelper(string appPath)
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(o => o.ApplicationPath).Returns(appPath);

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(o => o.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(o => o.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(o => o.Response).Returns(mockResponse.Object);

            var routes = new RouteCollection();
            var defaultRoute = routes.MapRoute("default", "{controller}/{action}/{id}", new { id = UrlParameter.Optional });

            var routeData = new RouteData();
            routeData.Route = defaultRoute;
            routeData.Values.Add("controller", "mvcpager");
            routeData.Values.Add("action", "test");

            var viewContext = new ViewContext()
            {
                HttpContext = mockHttpContext.Object,
                RouteData = routeData,
                Writer = TextWriter.Null
            };

            return new AjaxHelper(viewContext, new Mock<IViewDataContainer>().Object, routes);
        }

        internal static HttpContextBase GetHttpContext()
        {
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            return mockHttpContext.Object;
        }


    }
}
