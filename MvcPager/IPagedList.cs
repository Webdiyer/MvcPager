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
using System.Collections;
using System.Collections.Generic;

namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Interfaces/Interface[@name="IPagedList"]/*'/>
    public interface IPagedList:IEnumerable
    {
        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/IPagedList/Property[@name="CurrentPageIndex"]/*'/>
        int CurrentPageIndex { get; set; }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/IPagedList/Property[@name="PageSize"]/*'/>
        int PageSize { get; set; }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/IPagedList/Property[@name="TotalItemCount"]/*'/>
        int TotalItemCount { get; set; }
    }

    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Interfaces/Interface[@name="IPagedList2"]/*'/>
    public interface IPagedList<T>:IEnumerable<T>,IPagedList{}
}
