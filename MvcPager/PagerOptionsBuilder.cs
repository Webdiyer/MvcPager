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

using System.Collections.Generic;
using System.Web.Routing;

namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="PagerOptionsBuilder"]/*'/>
    public class PagerOptionsBuilder
    {
        private readonly PagerOptions _pagerOptions;

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Constrctor[@name="PagerOptionsBuilder"]/*'/>
        public PagerOptionsBuilder(PagerOptions pagerOptions)
        {
            _pagerOptions = pagerOptions;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetActionName"]/*'/>
        public PagerOptionsBuilder SetActionName(string actionName)
        {
            _pagerOptions.ActionName = actionName;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetControllerName"]/*'/>
        public PagerOptionsBuilder SetControllerName(string controllerName)
        {
            _pagerOptions.ControllerName = controllerName;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="AddHtmlAttribute"]/*'/>
        public PagerOptionsBuilder AddHtmlAttribute(string key, object value)
        {
            if (_pagerOptions.HtmlAttributes == null)
            {
                _pagerOptions.HtmlAttributes=new Dictionary<string, object>();
            }
            _pagerOptions.HtmlAttributes[key] = value;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetOnPageIndexError"]/*'/>
        public PagerOptionsBuilder SetOnPageIndexError(string handler)
        {
            _pagerOptions.OnPageIndexError = handler;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="AddRouteValue"]/*'/>
        public PagerOptionsBuilder AddRouteValue(string key, object value)
        {
            if (_pagerOptions.RouteValues == null)
            {
                _pagerOptions.RouteValues=new RouteValueDictionary();
            }
            _pagerOptions.RouteValues[key] = value;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetRouteName"]/*'/>
        public PagerOptionsBuilder SetRouteName(string routeName)
        {
            _pagerOptions.RouteName = routeName;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetFirstPageRouteName"]/*'/>
        public PagerOptionsBuilder SetFirstPageRouteName(string routeName)
        {
            _pagerOptions.FirstPageRouteName = routeName;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="DisableAutoHide"]/*'/>
        public PagerOptionsBuilder DisableAutoHide()
        {
            _pagerOptions.AutoHide = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetPageIndexOutOfRangeErrorMessage"]/*'/>
        public PagerOptionsBuilder SetPageIndexOutOfRangeErrorMessage(string msg)
        {
            _pagerOptions.PageIndexOutOfRangeErrorMessage = msg;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetHorizontalAlign"]/*'/>
        public PagerOptionsBuilder SetHorizontalAlign(string alignment)
        {
            _pagerOptions.HorizontalAlign = alignment;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetInvalidPageIndexErrorMessage"]/*'/>
        public PagerOptionsBuilder SetInvalidPageIndexErrorMessage(string msg)
        {
            _pagerOptions.InvalidPageIndexErrorMessage = msg;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetPageIndexParameterName"]/*'/>
        public PagerOptionsBuilder SetPageIndexParameterName(string prmName)
        {
            _pagerOptions.PageIndexParameterName = prmName;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetPageIndexBoxId"]/*'/>
        public PagerOptionsBuilder SetPageIndexBoxId(string id)
        {
            _pagerOptions.PageIndexBoxId = id;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetMaximumPageIndexItems"]/*'/>
        public PagerOptionsBuilder SetMaximumPageIndexItems(int itmes)
        {
            _pagerOptions.MaximumPageIndexItems = itmes;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetGoToButtonId"]/*'/>
        public PagerOptionsBuilder SetGoToButtonId(string id)
        {
            _pagerOptions.GoToButtonId = id;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetPageNumberFormatString"]/*'/>
        public PagerOptionsBuilder SetPageNumberFormatString(string format)
        {
            _pagerOptions.PageNumberFormatString = format;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetCurrentPageNumberFormatString"]/*'/>
        public PagerOptionsBuilder SetCurrentPageNumberFormatString(string format)
        {
            _pagerOptions.CurrentPageNumberFormatString = format;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetContainerTagName"]/*'/>
        public PagerOptionsBuilder SetContainerTagName(string tagName)
        {
            _pagerOptions.ContainerTagName = tagName;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetPagerItemTemplate"]/*'/>
        public PagerOptionsBuilder SetPagerItemTemplate(string template)
        {
            _pagerOptions.PagerItemTemplate = template;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetNumericPagerItemTemplate"]/*'/>
        public PagerOptionsBuilder SetNumericPagerItemTemplate(string template)
        {
            _pagerOptions.NumericPagerItemTemplate = template;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetCurrentPagerItemTemplate"]/*'/>
        public PagerOptionsBuilder SetCurrentPagerItemTemplate(string template)
        {
            _pagerOptions.CurrentPagerItemTemplate = template;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetNavigationPagerItemTemplate"]/*'/>
        public PagerOptionsBuilder SetNavigationPagerItemTemplate(string template)
        {
            _pagerOptions.NavigationPagerItemTemplate = template;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetMorePagerItemTemplate"]/*'/>
        public PagerOptionsBuilder SetMorePagerItemTemplate(string template)
        {
            _pagerOptions.MorePagerItemTemplate = template;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetDisabledPagerItemTemplate"]/*'/>
        public PagerOptionsBuilder SetDisabledPagerItemTemplate(string template)
        {
            _pagerOptions.DisabledPagerItemTemplate = template;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="AlwaysShowFirstLastPageNumber"]/*'/>
        public PagerOptionsBuilder AlwaysShowFirstLastPageNumber()
        {
            _pagerOptions.AlwaysShowFirstLastPageNumber = true;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetNumericPagerItemCount"]/*'/>
        public PagerOptionsBuilder SetNumericPagerItemCount(int itemCount)
        {
            _pagerOptions.NumericPagerItemCount = itemCount;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="HidePrevNext"]/*'/>
        public PagerOptionsBuilder HidePrevNext()
        {
            _pagerOptions.ShowPrevNext = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetPrevPageText"]/*'/>
        public PagerOptionsBuilder SetPrevPageText(string text)
        {
            _pagerOptions.PrevPageText = text;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetNextPageText"]/*'/>
        public PagerOptionsBuilder SetNextPageText(string text)
        {
            _pagerOptions.NextPageText = text;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="HideNumericPagerItems"]/*'/>
        public PagerOptionsBuilder HideNumericPagerItems()
        {
            _pagerOptions.ShowNumericPagerItems = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="HideFirstLast"]/*'/>
        public PagerOptionsBuilder HideFirstLast()
        {
            _pagerOptions.ShowFirstLast = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetFirstPageText"]/*'/>
        public PagerOptionsBuilder SetFirstPageText(string text)
        {
            _pagerOptions.FirstPageText = text;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetLastPageText"]/*'/>
        public PagerOptionsBuilder SetLastPageText(string text)
        {
            _pagerOptions.LastPageText = text;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="HideMorePagerItems"]/*'/>
        public PagerOptionsBuilder HideMorePagerItems()
        {
            _pagerOptions.ShowMorePagerItems = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetMorePageText"]/*'/>
        public PagerOptionsBuilder SetMorePageText(string text)
        {
            _pagerOptions.MorePageText = text;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetId"]/*'/>
        public PagerOptionsBuilder SetId(string id)
        {
            _pagerOptions.Id = id;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetCssClass"]/*'/>
        public PagerOptionsBuilder SetCssClass(string cssClass)
        {
            _pagerOptions.CssClass = cssClass;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="HideDisabledPagerItems"]/*'/>
        public PagerOptionsBuilder HideDisabledPagerItems()
        {
            _pagerOptions.ShowDisabledPagerItems = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetMaximumPageNumber"]/*'/>
        public PagerOptionsBuilder SetMaximumPageNumber(int number)
        {
            _pagerOptions.MaximumPageNumber = number;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="HidePagerItems"]/*'/>
        public PagerOptionsBuilder HidePagerItems()
        {
            _pagerOptions.HidePagerItems = true;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetNavigationPagerItemsPosition"]/*'/>
        public PagerOptionsBuilder SetNavigationPagerItemsPosition(PagerItemsPosition position)
        {
            _pagerOptions.NavigationPagerItemsPosition = position;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetRouteValues"]/*'/>
        public PagerOptionsBuilder SetRouteValues(RouteValueDictionary values)
        {
            _pagerOptions.RouteValues = values;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptionsBuilder/Method[@name="SetHtmlAttributes"]/*'/>
        public PagerOptionsBuilder SetHtmlAttributes(IDictionary<string, object> attributes)
        {
            _pagerOptions.HtmlAttributes = attributes;
            return this;
        }
    }
}
