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
namespace Webdiyer.WebControls.Mvc
{
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="MvcAjaxOptions"]/*'/>
    public class MvcAjaxOptions : System.Web.Mvc.Ajax.AjaxOptions
    {
        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptions/Property[@name="EnablePartialLoading"]/*'/>
        public bool EnablePartialLoading { get; set; }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptions/Property[@name="DataFormId"]/*'/>
        public string DataFormId { get; set; }

        private bool _allowCache = true;

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptions/Property[@name="AllowCache"]/*'/>
        public bool AllowCache { get { return _allowCache; } set { _allowCache = value; } }

        private bool _enableHistorySupport = true;

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptions/Property[@name="EnableHistorySupport"]/*'/>
        public bool EnableHistorySupport { get { return _enableHistorySupport; } set { _enableHistorySupport = value; } }

    }
}
