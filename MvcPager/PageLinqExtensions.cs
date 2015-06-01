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
using System.Linq;

namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="PageLinqExtensions"]/*'/>
    public static class PageLinqExtensions
    {
        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PageLinqExtensions/Method[@name="ToPagedList1"]/*'/>
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex-1) * pageSize;
            var totalItemCount = allItems.Count();
            while (totalItemCount <= itemIndex&&pageIndex>1)
            {
                itemIndex = (--pageIndex - 1) * pageSize;
            }
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PageLinqExtensions/Method[@name="ToPagedList2"]/*'/>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
        {
            return allItems.AsQueryable().ToPagedList(pageIndex, pageSize);
        } 
    }
}
