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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="DisplayNameExtensions"]/*'/>
    public static class DisplayNameExtensions
    {
        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/DisplayNameExtensions/Method[@name="DisplayNameFor1"]/*'/>
        public static IHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return GetDisplayName(expression);
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/DisplayNameExtensions/Method[@name="DisplayNameFor2"]/*'/>
        public static IHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<PagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return GetDisplayName(expression);
        }

        private static IHtmlString GetDisplayName<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string resolvedDisplayName = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            return new MvcHtmlString(HttpUtility.HtmlEncode(resolvedDisplayName));
        }
    }
}
