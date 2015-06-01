using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Webdiyer.WebControls.Mvc
{

    internal class DataLoaderBuilder
    {
        public MvcHtmlString RenderLoader()
        {
            return new MvcHtmlString(string.Empty);
        }
    }

    public static class DataLoaderHelper
    {
        public static MvcHtmlString DataLoader(this AjaxHelper ajax, int totalItemCount, int pageSize, int pageIndex,
            string actionName, string controllerName,
            string routeName, DataLoaderOptions pagerOptions, object routeValues, DataLoadType loadType,
            object htmlAttributes)
        {
            
        }
    }

    public class DataLoaderOptions
    {
        public string ButtonText { get; set; }

        public string PullToRefreshText { get; set; }

    }

    public enum DataLoadType
    {
        ClickToLoad,
        PullToRefresh,
        Auto
    }
}
