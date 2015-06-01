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
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="PagerOptions"]/*'/>
    public class PagerOptions
    {
        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Constructor[@name="PagerOptions"]/*'/>
        public PagerOptions()
        {
            AutoHide = true;
            PageIndexParameterName = "pageindex";
            NumericPagerItemCount = 10;
            AlwaysShowFirstLastPageNumber = false;
            ShowPrevNext = true;
            PrevPageText = MvcPagerResources.PrevPageText;
            NextPageText = MvcPagerResources.NextPageText;
            ShowNumericPagerItems = true;
            ShowFirstLast = true;
            FirstPageText = MvcPagerResources.FirstPageText;
            LastPageText = MvcPagerResources.LastPageText;
            ShowMorePagerItems = true;
            MorePageText = "...";
            ShowDisabledPagerItems = true;
            //PagerItemsSeparator = "&nbsp;&nbsp;";
            MaximumPageIndexItems = 20;
            ContainerTagName = "div";
            InvalidPageIndexErrorMessage = MvcPagerResources.InvalidPageIndexErrorMessage;
            PageIndexOutOfRangeErrorMessage = MvcPagerResources.PageIndexOutOfRangeErrorMessage;
            MaximumPageNumber = 0;
            FirstPageRouteName = null;
        }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ActionName"]/*'/>
        public string ActionName { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ControllerName"]/*'/>
        public string ControllerName { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="RouteName"]/*'/>
        public string RouteName { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="RouteValues"]/*'/>
        public RouteValueDictionary RouteValues { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="HtmlAttributes"]/*'/>
        public IDictionary<string,object> HtmlAttributes { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="FirstPageRouteName"]/*'/>
        public string FirstPageRouteName { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="AutoHide"]/*'/>
        public bool AutoHide { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="PageIndexOutOfRangeErrorMessage"]/*'/>
        public string PageIndexOutOfRangeErrorMessage { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="InvalidPageIndexErrorMessage"]/*'/>
        public string InvalidPageIndexErrorMessage { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="PageIndexParameterName"]/*'/>
        public string PageIndexParameterName { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="PageIndexBoxId"]/*'/>
        public string PageIndexBoxId { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="GoToButtonId"]/*'/>
        public string GoToButtonId { get; set; }
        
        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="MaximumPageIndexItems"]/*'/>
        public int MaximumPageIndexItems { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="PageNumberFormatString"]/*'/>
        public string PageNumberFormatString { get; set; }
       

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="CurrentPageNumberFormatString"]/*'/>
        public string CurrentPageNumberFormatString { get; set; }

        private string _containerTagName;
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ContainerTagName"]/*'/>
        public string ContainerTagName
        {
            get
            {
                return _containerTagName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new System.ArgumentException(MvcPagerResources.ContainerTagNameCannotBeNull);
                _containerTagName = value;
            }
        }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="PagerItemTemplate"]/*'/>
        public string PagerItemTemplate { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="NumericPagerItemTemplate"]/*'/>
        public string NumericPagerItemTemplate { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="CurrentPagerItemTemplate"]/*'/>
        public string CurrentPagerItemTemplate { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="NavigationPagerItemTemplate"]/*'/>
        public string NavigationPagerItemTemplate { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="MorePagerItemTemplate"]/*'/>
        public string MorePagerItemTemplate { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="DisabledPagerItemTemplate"]/*'/>
        public string DisabledPagerItemTemplate { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="AlwaysShowFirstLastPageNumber"]/*'/>
        public bool AlwaysShowFirstLastPageNumber { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="NumericPagerItemCount"]/*'/>
        public int NumericPagerItemCount { get; set; }


        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ShowPrevNext"]'/>
        public bool ShowPrevNext { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="PrevPageText"]/*'/>
        public string PrevPageText { get; set; }
       

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="NextPageText"]/*'/>
        public string NextPageText { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ShowNumericPagerItems"]'/>
        public bool ShowNumericPagerItems { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ShowFirstLast"]'/>
        public bool ShowFirstLast { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="FirstPageText"]/*'/>
        public string FirstPageText { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="LastPageText"]/*'/>
        public string LastPageText { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ShowMorePagerItems"]/*'/>
        public bool ShowMorePagerItems { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="MorePageText"]/*'/>
        public string MorePageText { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="Id"]/*'/>
        public string Id { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="HorizontalAlign"]/*'/>
        public string HorizontalAlign { get; set; }
      

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="CssClass"]/*'/>
        public string CssClass { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="ShowDisabledPagerItems"]/*'/>
        public bool ShowDisabledPagerItems { get; set; }
        

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="MaximumPageNumber"]/*'/>
        public int MaximumPageNumber { get; set; }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="HidePagerItems"]/*'/>
        public bool HidePagerItems { get; set; }

        private PagerItemsPosition _navPagerItemsPosition = PagerItemsPosition.BothSide;

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="NavigationPagerItemsPosition"]/*'/>
        public PagerItemsPosition NavigationPagerItemsPosition { get { return _navPagerItemsPosition; } set{_navPagerItemsPosition = value;} }

        /// <include file='MvcPagerDocs.xml' path='MvcPagerDocs/PagerOptions/Property[@name="OnPageIndexError"]/*'/>
        public string OnPageIndexError { get; set; }
    }
}