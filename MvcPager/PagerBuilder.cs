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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

namespace Webdiyer.WebControls.Mvc
{
    internal class PagerBuilder
    {
        private readonly HtmlHelper _html;
        private readonly AjaxHelper _ajax;
        private readonly int _totalPageCount = 1;
        private readonly int _pageIndex;
        private readonly PagerOptions _pagerOptions;
        private readonly int _startPageIndex = 1;
        private readonly int _endPageIndex = 1;
        private readonly bool _ajaxPagingEnabled;
        private readonly MvcAjaxOptions _ajaxOptions;
        private readonly string _copyrightText = "\r\n" + MvcPagerResources.CopyrightText + "\r\n";
        

        //html pager builder
        internal PagerBuilder(HtmlHelper htmlHelper, int totalPageCount, int pageIndex, PagerOptions pagerOptions)
        {
            _ajaxPagingEnabled = false;
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();
            _html = htmlHelper;
            if (pagerOptions.MaximumPageNumber == 0 || pagerOptions.MaximumPageNumber > totalPageCount)
                _totalPageCount = totalPageCount;
            else
                _totalPageCount = pagerOptions.MaximumPageNumber;
            _pageIndex = pageIndex;
            _pagerOptions = pagerOptions;

            // start page index
            _startPageIndex = pageIndex - (pagerOptions.NumericPagerItemCount / 2);
            if (_startPageIndex + pagerOptions.NumericPagerItemCount > _totalPageCount)
                _startPageIndex = _totalPageCount + 1 - pagerOptions.NumericPagerItemCount;
            if (_startPageIndex < 1)
                _startPageIndex = 1;

            // end page index
            _endPageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount - 1;
            if (_endPageIndex > _totalPageCount)
                _endPageIndex = _totalPageCount;
        }
        //Ajax pager builder
        internal PagerBuilder(AjaxHelper ajaxHelper, int totalPageCount, int pageIndex, PagerOptions pagerOptions,
            MvcAjaxOptions ajaxOptions)
        {
            _ajaxPagingEnabled =true;
            if (ajaxHelper == null)
            {
                throw new ArgumentNullException("ajaxHelper");
            }
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();
            _ajax = ajaxHelper;
            if (pagerOptions.MaximumPageNumber == 0 || pagerOptions.MaximumPageNumber > totalPageCount)
                _totalPageCount = totalPageCount;
            else
                _totalPageCount = pagerOptions.MaximumPageNumber;
            _pageIndex = pageIndex;
            _pagerOptions = pagerOptions;
            _ajaxOptions = ajaxOptions ?? new MvcAjaxOptions();

            // start page index
            _startPageIndex = pageIndex - (pagerOptions.NumericPagerItemCount / 2);
            if (_startPageIndex + pagerOptions.NumericPagerItemCount > _totalPageCount)
                _startPageIndex = _totalPageCount + 1 - pagerOptions.NumericPagerItemCount;
            if (_startPageIndex < 1)
                _startPageIndex = 1;

            // end page index
            _endPageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount - 1;
            if (_endPageIndex > _totalPageCount)
                _endPageIndex = _totalPageCount;
        }
        

        private void AddPrevious(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.PrevPageText, _pageIndex - 1, _pageIndex == 1, PagerItemType.PrevPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }
        private void AddFirst(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.FirstPageText, 1, _pageIndex == 1, PagerItemType.FirstPage);
            //Add pager item when PagerItem is not disabled or PagerItem is disabled but PagerOptions.ShowDisabledPagerItems is true
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }


        private void AddMoreBefore(ICollection<PagerItem> results)
        {
            if (_startPageIndex > 1 && _pagerOptions.ShowMorePagerItems)
            {
                var index = _startPageIndex - 1;
                if (index < 1) index = 1;
                var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        private void AddPageNumbers(ICollection<PagerItem> results)
        {
            for (var pageIndex = _startPageIndex; pageIndex <= _endPageIndex; pageIndex++)
            {
                var text = pageIndex.ToString(CultureInfo.InvariantCulture);
                if (pageIndex == _pageIndex && !string.IsNullOrEmpty(_pagerOptions.CurrentPageNumberFormatString))
                    text = String.Format(_pagerOptions.CurrentPageNumberFormatString, text);
                else if (!string.IsNullOrEmpty(_pagerOptions.PageNumberFormatString))
                    text = String.Format(_pagerOptions.PageNumberFormatString, text);
                var item = new PagerItem(text, pageIndex, false, PagerItemType.NumericPage);
                results.Add(item);
            }
        }

        private void AddMoreAfter(ICollection<PagerItem> results)
        {
            if (_endPageIndex < _totalPageCount)
            {
                var index = _startPageIndex + _pagerOptions.NumericPagerItemCount;
                if (index > _totalPageCount) { index = _totalPageCount; }
                var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        private void AddNext(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.NextPageText, _pageIndex + 1, _pageIndex >= _totalPageCount, PagerItemType.NextPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }

        private void AddLast(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.LastPageText, _totalPageCount, _pageIndex >= _totalPageCount, PagerItemType.LastPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }


        /// <summary>
        /// Generate pagination url using page index
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <returns>Pagination url</returns>
        private string GenerateUrl(int pageIndex)
        {
            ViewContext viewContext = _ajax == null ? _html.ViewContext : _ajax.ViewContext;
            //Return null if page index is larger than total page count or page index equals current page index
            if (pageIndex > _totalPageCount || pageIndex == _pageIndex)
                return null;
            var routeValues = new RouteValueDictionary(viewContext.RouteData.Values);
            AddQueryStringToRouteValues(routeValues, viewContext);
            if (_pagerOptions.RouteValues != null && _pagerOptions.RouteValues.Count > 0)
            {
                foreach (var de in _pagerOptions.RouteValues)
                {
                    if (!routeValues.ContainsKey(de.Key))
                    {
                        routeValues.Add(de.Key, de.Value);
                    }
                    else
                    {
                        routeValues[de.Key] = de.Value; //RouteValues that added manually have higher privilege
                    }
                }
            }
            var pageValue = viewContext.RouteData.Values[_pagerOptions.PageIndexParameterName];
            string routeName =_pagerOptions.RouteName;

            object constraintValue = null;
            //Get route name for MvcPager, use current route if route name is not specified
            RouteCollection routeCollection = _html == null ? (_ajax==null?RouteTable.Routes:_ajax.RouteCollection) : _html.RouteCollection;
            var route = routeCollection[routeName] as Route ?? viewContext.RouteData.Route as Route; 
            //Generate url format string when pageIndex is 0
            if (pageIndex == 0)
            {
                //Check if constraint is applied to page index parameter in route definition 
                if (route != null && route.Constraints.ContainsKey(_pagerOptions.PageIndexParameterName))
                {
                    //Remove constraint applied to page index parameter in route definition temporarily in order to generate paging url format string, otherwise it maybe failed because page index in paging url format is a string value
                    constraintValue = route.Constraints[_pagerOptions.PageIndexParameterName];
                    route.Constraints.Remove(_pagerOptions.PageIndexParameterName);
                }
                routeValues[_pagerOptions.PageIndexParameterName] = "__" + _pagerOptions.PageIndexParameterName + "__";
            }
            else
            {
                if (pageIndex == 1)
                {
                    if (!string.IsNullOrWhiteSpace(_pagerOptions.FirstPageRouteName))
                        //Apply FirstPageRouteName to first page item if it is specified
                    {
                        routeName = _pagerOptions.FirstPageRouteName;
                        routeValues.Remove(_pagerOptions.PageIndexParameterName); //Remove page index parameter from route value
                        viewContext.RouteData.Values.Remove(_pagerOptions.PageIndexParameterName);
                    }
                    else
                    {
                        var curRoute = viewContext.RouteData.Route as Route;

                        //Remove page index parameter from route values when page index parameter is optional and no constraint is applied to it in route definition
                        if (curRoute != null &&
                            (curRoute.Defaults[_pagerOptions.PageIndexParameterName] == UrlParameter.Optional ||
                             !curRoute.Url.Contains("{" + _pagerOptions.PageIndexParameterName + "}")))
                        {
                            routeValues.Remove(_pagerOptions.PageIndexParameterName); //Remove page index parameter from route value
                            viewContext.RouteData.Values.Remove(_pagerOptions.PageIndexParameterName);
                        }
                        else
                        {
                            routeValues[_pagerOptions.PageIndexParameterName] = pageIndex;
                        }
                    }
                }
                else
                {
                    routeValues[_pagerOptions.PageIndexParameterName] = pageIndex;
                }
            }
            var routes = _ajax == null ? (_html==null?RouteTable.Routes:_html.RouteCollection) : _ajax.RouteCollection;

            string url;
            if (!string.IsNullOrEmpty(routeName))
                url = UrlHelper.GenerateUrl(routeName,_pagerOptions.ActionName, _pagerOptions.ControllerName, routeValues, routes,
                    viewContext.RequestContext, false);
            else
                url = UrlHelper.GenerateUrl(null, _pagerOptions.ActionName, _pagerOptions.ControllerName, routeValues, routes,
                    viewContext.RequestContext, false);
            if (pageValue != null)
                viewContext.RouteData.Values[_pagerOptions.PageIndexParameterName] = pageValue;
            if (constraintValue != null && !route.Constraints.ContainsKey(_pagerOptions.PageIndexParameterName)) //Add constraint back
                route.Constraints.Add(_pagerOptions.PageIndexParameterName, constraintValue);
            return url;
        }

        /// <summary>
        /// Generate final html code
        /// </summary>
        /// <returns></returns>
        public string GenerateHtml()
        {
            var htmlAttributes = _pagerOptions.HtmlAttributes;
            if ((_pageIndex > _totalPageCount && _totalPageCount > 0) || _pageIndex < 1)
            {
                if (_ajaxPagingEnabled)
                {
                    return string.Format("{0}<div data-ajax=\"true\" data-ajax-update=\"{1}\" data-invalidpageerrmsg=\"{2}\" data-outrangeerrmsg=\"{3}\" data-pagerid=\"Webdiyer.MvcPager\" style=\"color:red;font-weight:bold\">{3}</div>{0}",
                                             _copyrightText, EscapeIdSelector(_ajaxOptions.UpdateTargetId), _pagerOptions.InvalidPageIndexErrorMessage, _pagerOptions.PageIndexOutOfRangeErrorMessage);
                    
                }
                return string.Format("{0}<div data-invalidpageerrmsg=\"{1}\" data-outrangeerrmsg=\"{2}\" data-pagerid=\"Webdiyer.MvcPager\" style=\"color:red;font-weight:bold\">{2}</div>{0}",
                                         _copyrightText,_pagerOptions.InvalidPageIndexErrorMessage, _pagerOptions.PageIndexOutOfRangeErrorMessage);
            }

            var tb = new TagBuilder(_pagerOptions.ContainerTagName);
            if (!string.IsNullOrEmpty(_pagerOptions.Id))
                tb.GenerateId(_pagerOptions.Id);
            if (!string.IsNullOrEmpty(_pagerOptions.HorizontalAlign))
            {
                string strAlign = "text-align:" + _pagerOptions.HorizontalAlign.ToLower();
                MergeStyleAttribute(ref htmlAttributes,strAlign);
            }
            if (_pagerOptions.HidePagerItems)
            {
                MergeStyleAttribute(ref htmlAttributes,"display:none");
            }
            tb.MergeAttributes(htmlAttributes, true);
            if (!string.IsNullOrEmpty(_pagerOptions.CssClass))
                tb.AddCssClass(_pagerOptions.CssClass);
            IDictionary<string, object> attrs = null;
            if (_ajaxPagingEnabled)
            {
                attrs=_ajaxOptions.ToUnobtrusiveHtmlAttributes();
                attrs.Remove("data-ajax-url");
                attrs.Remove("data-ajax-mode");
                if (_ajaxOptions.EnablePartialLoading)
                    attrs.Add("data-ajax-partialloading", "true");
                if (!string.IsNullOrWhiteSpace(_ajaxOptions.DataFormId))
                    attrs.Add("data-ajax-dataformid", EscapeIdSelector(_ajaxOptions.DataFormId));
                if (!_ajaxOptions.AllowCache)
                    attrs.Add("data-ajax-allowcache", "false");
                if(!_ajaxOptions.EnableHistorySupport)
                    attrs.Add("data-ajax-enablehistorysupport","false");
                //AddDataAttributes(attrs);
            }
            if (attrs == null)
            {
                attrs = new Dictionary<string, object>();
            }
            AddDataAttributes(attrs);
            tb.MergeAttributes(attrs, true);
            if ((_totalPageCount > 1 || (!_pagerOptions.AutoHide)) && ! _pagerOptions.HidePagerItems)
            {
                var pagerItems = new List<PagerItem>();
                if (_pagerOptions.NavigationPagerItemsPosition == PagerItemsPosition.Left ||
                    _pagerOptions.NavigationPagerItemsPosition == PagerItemsPosition.BothSide)
                {
                    //First page
                    if (_pagerOptions.ShowFirstLast)
                        AddFirst(pagerItems);

                    // Prev page
                    if (_pagerOptions.ShowPrevNext)
                        AddPrevious(pagerItems);
                    if (_pagerOptions.NavigationPagerItemsPosition == PagerItemsPosition.Left)
                    {
                        // Next page
                        if (_pagerOptions.ShowPrevNext)
                            AddNext(pagerItems);

                        //Last page
                        if (_pagerOptions.ShowFirstLast)
                            AddLast(pagerItems);
                    }
                }

                if (_pagerOptions.ShowNumericPagerItems)
                {
                    if (_pagerOptions.AlwaysShowFirstLastPageNumber && _startPageIndex > 1)
                        pagerItems.Add(new PagerItem("1", 1, false, PagerItemType.NumericPage));

                    // more page before numeric page buttons
                    if (_pagerOptions.ShowMorePagerItems &&
                        ((!_pagerOptions.AlwaysShowFirstLastPageNumber && _startPageIndex > 1) ||
                         (_pagerOptions.AlwaysShowFirstLastPageNumber && _startPageIndex > 2)))
                        AddMoreBefore(pagerItems);

                    // numeric page
                    AddPageNumbers(pagerItems);

                    // more page after numeric page buttons
                    if (_pagerOptions.ShowMorePagerItems &&
                        ((!_pagerOptions.AlwaysShowFirstLastPageNumber && _endPageIndex < _totalPageCount) ||
                         (_pagerOptions.AlwaysShowFirstLastPageNumber && _totalPageCount > _endPageIndex + 1)))
                        AddMoreAfter(pagerItems);

                    if (_pagerOptions.AlwaysShowFirstLastPageNumber && _endPageIndex < _totalPageCount)
                        pagerItems.Add(new PagerItem(_totalPageCount.ToString(CultureInfo.InvariantCulture),
                            _totalPageCount, false,
                            PagerItemType.NumericPage));
                }
                if (_pagerOptions.NavigationPagerItemsPosition == PagerItemsPosition.Right ||
                    _pagerOptions.NavigationPagerItemsPosition == PagerItemsPosition.BothSide)
                {
                    if (_pagerOptions.NavigationPagerItemsPosition == PagerItemsPosition.Right)
                    {
                        //First page
                        if (_pagerOptions.ShowFirstLast)
                            AddFirst(pagerItems);

                        // Prev page
                        if (_pagerOptions.ShowPrevNext)
                            AddPrevious(pagerItems);
                    }
                    // Next page
                    if (_pagerOptions.ShowPrevNext)
                        AddNext(pagerItems);

                    //Last page
                    if (_pagerOptions.ShowFirstLast)
                        AddLast(pagerItems);
                }

                var sb = new StringBuilder();
                if (_ajaxPagingEnabled)
                {
                    foreach (PagerItem item in pagerItems)
                    {
                        sb.Append(GenerateAjaxPagerElement(item));
                    }
                }
                else
                {
                    foreach (PagerItem item in pagerItems)
                    {
                        sb.Append(GeneratePagerElement(item));
                    }
                }
                tb.InnerHtml = sb.ToString();
            }
            return _copyrightText +  tb.ToString(TagRenderMode.Normal)  + _copyrightText;
        }

        private static string EscapeIdSelector(string id)
        {
            var reg=new Regex(@"[.:[\]]");
            return '#' + reg.Replace(id, @"\$&");
        }

        private static void MergeStyleAttribute(ref IDictionary<string, object> attributes, string style)
        {
            if (attributes == null)
            {
                attributes = new Dictionary<string, object> {{"style", style}};
            }
            else
            {
                if (attributes.ContainsKey("style"))
                {
                    attributes["style"] += ";" + style;
                }
                else
                {
                    attributes.Add("style", style);
                }
            }
        }

        private void AddDataAttributes(IDictionary<string, object> attrs)
        {
            attrs.Add("data-urlformat", GenerateUrl(0));
            attrs.Add("data-pagerid", "Webdiyer.MvcPager");
            if (_pageIndex > 1)
                attrs.Add("data-currentpage", _pageIndex);
            if (_pageIndex > 1)
                attrs.Add("data-firstpage", GenerateUrl(1));
            if(!string.IsNullOrWhiteSpace(_pagerOptions.OnPageIndexError))
                attrs.Add("data-onerror",_pagerOptions.OnPageIndexError);
            attrs.Add("data-pageparameter", _pagerOptions.PageIndexParameterName);
            attrs.Add("data-pagecount", _totalPageCount);

            if (!string.IsNullOrWhiteSpace(_pagerOptions.PageIndexBoxId))
            {
                attrs.Add("data-pageindexbox", EscapeIdSelector(_pagerOptions.PageIndexBoxId));
                if (!string.IsNullOrWhiteSpace(_pagerOptions.GoToButtonId))
                {
                    attrs.Add("data-gotobutton", EscapeIdSelector(_pagerOptions.GoToButtonId));
                }
                if (_pagerOptions.MaximumPageIndexItems != 20)
                {
                    attrs.Add("data-maxitems", _pagerOptions.MaximumPageIndexItems);
                }
            }
            attrs.Add("data-outrangeerrmsg", _pagerOptions.PageIndexOutOfRangeErrorMessage);
            attrs.Add("data-invalidpageerrmsg", _pagerOptions.InvalidPageIndexErrorMessage);
        }

        private string GenerateAjaxAnchor(PagerItem item)
        {
            string url = GenerateUrl(item.PageIndex);
            if (string.IsNullOrWhiteSpace(url))
                return HttpUtility.HtmlEncode(item.Text);
            var tag = new TagBuilder("a") {InnerHtml = item.Text};
            tag.MergeAttribute("href", url);
            tag.MergeAttribute("data-pageindex", item.PageIndex.ToString(CultureInfo.InvariantCulture));
            return tag.ToString(TagRenderMode.Normal);
        }

        private IHtmlString GeneratePagerElement(PagerItem item)
        {
            //pager item link
            string url = GenerateUrl(item.PageIndex);
            if (item.Disabled) //first,last,next or previous page
                return CreateWrappedPagerElement(item, item.Text);
            return CreateWrappedPagerElement(item,
                                             string.IsNullOrEmpty(url)
                                                 ? HttpUtility.HtmlEncode(item.Text)
                                                 : String.Format("<a href=\"{0}\">{1}</a>", url, item.Text));
        }

        private IHtmlString GenerateAjaxPagerElement(PagerItem item)
        {
            if (item.Disabled)
                return CreateWrappedPagerElement(item,item.Text);
            return CreateWrappedPagerElement(item, GenerateAjaxAnchor(item));
        }


        private IHtmlString CreateWrappedPagerElement(PagerItem item, string el)
        {
            if (item.Disabled)
            {
                if ((!string.IsNullOrEmpty(_pagerOptions.DisabledPagerItemTemplate) ||!string.IsNullOrEmpty(_pagerOptions.NavigationPagerItemTemplate)||
                     !string.IsNullOrEmpty(_pagerOptions.PagerItemTemplate)))
                {
                    return
                        MvcHtmlString.Create(
                            string.Format(
                                _pagerOptions.DisabledPagerItemTemplate ?? (_pagerOptions.NavigationPagerItemTemplate ??
                                                                            _pagerOptions.PagerItemTemplate), el));
                }
                return MvcHtmlString.Create(el);
            }
            string navStr = el;
            switch (item.Type)
            {
                case PagerItemType.FirstPage:
                case PagerItemType.LastPage:
                case PagerItemType.NextPage:
                case PagerItemType.PrevPage:
                    if ((!string.IsNullOrEmpty(_pagerOptions.NavigationPagerItemTemplate) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemTemplate)))
                        navStr =
                            string.Format(
                                _pagerOptions.NavigationPagerItemTemplate ??
                                _pagerOptions.PagerItemTemplate, el);
                    break;
                case PagerItemType.MorePage:
                    if ((!string.IsNullOrEmpty(_pagerOptions.MorePagerItemTemplate) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemTemplate)))
                        navStr =
                            string.Format(
                                _pagerOptions.MorePagerItemTemplate ??
                                _pagerOptions.PagerItemTemplate, el);
                    break;
                case PagerItemType.NumericPage:
                    if (item.PageIndex == _pageIndex &&
                        (!string.IsNullOrEmpty(_pagerOptions.CurrentPagerItemTemplate) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemTemplate))) //current page
                        navStr =
                            string.Format(
                                _pagerOptions.CurrentPagerItemTemplate ??
                                _pagerOptions.PagerItemTemplate, el);
                    else if (!string.IsNullOrEmpty(_pagerOptions.NumericPagerItemTemplate) ||
                             !string.IsNullOrEmpty(_pagerOptions.PagerItemTemplate))
                        navStr =
                            string.Format(
                                _pagerOptions.NumericPagerItemTemplate ??
                                _pagerOptions.PagerItemTemplate, el);
                    break;
            }
            return MvcHtmlString.Create(navStr);
        }
                
        private void AddQueryStringToRouteValues(RouteValueDictionary routeValues,ViewContext viewContext)
        {
            if(routeValues==null)
                routeValues=new RouteValueDictionary();
            var rq = viewContext.HttpContext.Request.QueryString;
            if (rq != null && rq.Count > 0)
            {
                var invalidParams = new[] { "x-requested-with", "xmlhttprequest", _pagerOptions.PageIndexParameterName.ToLower() };
                foreach (string key in rq.Keys)
                {
                    //Add url parameter to route values
                    if (!string.IsNullOrEmpty(key) && Array.IndexOf(invalidParams, key.ToLower()) < 0)
                    {
                        var kv = rq[key];
                        routeValues[key] = kv;
                    }
                }
            }
        }
    }
}