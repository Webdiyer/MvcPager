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
using System.IO;
using System.Web.Mvc;

[assembly: System.Web.UI.WebResource("Webdiyer.WebControls.Mvc.MvcPager.min.js", "text/javascript")]
namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="PagerExtensions"]/*'/>
    public static class PagerExtensions
    {
        #region Html Pager

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="HtmlPager1"]/*'/>
        public static HtmlPager Pager(this HtmlHelper helper, int totalItemCount, int pageSize, int pageIndex, PagerOptions pagerOptions)
        {
            return new HtmlPager
                (
                    helper,
                    totalItemCount,pageSize,
                    pageIndex,
                    pagerOptions
                );
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="HtmlPager2"]/*'/>
        public static HtmlPager Pager(this HtmlHelper helper, int totalItemCount, int pageSize, int pageIndex)
        {
            return new HtmlPager
                (
                    helper,
                    totalItemCount, pageSize,
                    pageIndex
                );
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="HtmlPager3"]/*'/>
        public static HtmlPager Pager(this HtmlHelper helper, IPagedList pagedList)
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }
            return new HtmlPager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null);
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="HtmlPager4"]/*'/>
        public static HtmlPager Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions)
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, pagerOptions);
        }


        #endregion
        
        #region Ajax Pager

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="AjaxPager1"]/*'/>
        public static AjaxPager Pager(this AjaxHelper ajax, int totalItemCount, int pageSize, int pageIndex, PagerOptions pagerOptions, MvcAjaxOptions ajaxOptions)
        {
            return new AjaxPager(ajax, totalItemCount, pageSize, pageIndex, pagerOptions, ajaxOptions);
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="AjaxPager2"]/*'/>
        public static AjaxPager Pager(this AjaxHelper ajax, IPagedList pagedList)
        {
            return new AjaxPager(ajax, pagedList, null,null);
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="AjaxPager3"]/*'/>
        public static AjaxPager Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions)
        {
            return Pager(ajax, pagedList, pagerOptions, null);
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerExtensions/Method[@name="AjaxPager4"]/*'/>
        public static AjaxPager Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions, MvcAjaxOptions ajaxOptions)
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex,pagerOptions, ajaxOptions);
        }

        #endregion
    }
}