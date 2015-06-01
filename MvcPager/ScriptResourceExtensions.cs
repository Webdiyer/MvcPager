/* MvcPager source code
This file is part of MvcPager.
Copyright 2009-2015 Webdiyer(http://en.webdiyer.com)
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Webdiyer.WebControls.Mvc
{

    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="ScriptResourceExtensions"]/*'/>
    public static class ScriptResourceExtensions
    {
        private static string GetMvcPagerScriptUrl(HttpContextBase context)
        {
            var page = context.CurrentHandler as Page;
            return (page ?? new Page()).ClientScript.GetWebResourceUrl(typeof(PagerExtensions), "Webdiyer.WebControls.Mvc.MvcPager.min.js");
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/ScriptResourceExtensions/Method[@name="RegisterMvcPagerScriptResource"]/*'/>
        public static void RegisterMvcPagerScriptResource(this HtmlHelper html)
        {
#if DEBUG
            html.ViewContext.Writer.Write("<script type=\"text/javascript\" src=\"/scripts/MvcPager.js\"></script>");
#else
            html.ViewContext.Writer.Write("<script type=\"text/javascript\" src=\"" + GetMvcPagerScriptUrl(html.ViewContext.HttpContext) + "\"></script>");
#endif
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/ScriptResourceExtensions/Method[@name="LoadMvcPagerScript"]/*'/>
        public static void LoadMvcPagerScript(this AjaxHelper ajax)
        {
            ajax.ViewContext.Writer.Write("if(!$.fn.initMvcPagers){$.getScript(\""+GetMvcPagerScriptUrl(ajax.ViewContext.HttpContext)+"\");}");
        }

    }
}
