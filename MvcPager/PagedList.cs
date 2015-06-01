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
using System.Collections.Generic;
using System.Linq;

namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="PagedList"]/*'/>
    public class PagedList<T> : List<T>,IPagedList<T>
    {
        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Constructor[@name="PagedList1"]/*'/>
        public PagedList(IEnumerable<T> allItems, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            var items = allItems as IList<T> ?? allItems.ToList();
            TotalItemCount = items.Count();
            CurrentPageIndex = pageIndex;
            AddRange(items.Skip(StartItemIndex - 1).Take(pageSize));
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Constructor[@name="PagedList2"]/*'/>
        public PagedList(IEnumerable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(currentPageItems);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Constructor[@name="PagedList3"]/*'/>
        public PagedList(IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            int startIndex = (pageIndex - 1)*pageSize;
            AddRange(allItems.Skip(startIndex).Take(pageSize));
            TotalItemCount = allItems.Count();
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Constructor[@name="PagedList4"]/*'/>
        public PagedList(IQueryable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(currentPageItems);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Property[@name="CurrentPageIndex"]/*'/>
        public int CurrentPageIndex { get; set; }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Property[@name="PageSize"]/*'/>
        public int PageSize { get; set; }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Property[@name="TotalItemCount"]/*'/>
        public int TotalItemCount { get; set; }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Property[@name="TotalPageCount"]/*'/>
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Property[@name="StartItemIndex"]/*'/>
        public int StartItemIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagedList/Property[@name="EndItemIndex"]/*'/>
        public int EndItemIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
    }
}
