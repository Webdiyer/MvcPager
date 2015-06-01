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
    ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/Classes/Class[@name="MvcAjaxOptionsBuilder"]/*'/>
    public class MvcAjaxOptionsBuilder
    {
        private readonly MvcAjaxOptions _ajaxOptions;

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Constructor[@name="MvcAjaxOptionsBuilder"]/*'/>
        public MvcAjaxOptionsBuilder(MvcAjaxOptions ajaxOptions)
        {
            _ajaxOptions = ajaxOptions;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetUpdateTargetId"]/*'/>
        public MvcAjaxOptionsBuilder SetUpdateTargetId(string targetId)
        {
            _ajaxOptions.UpdateTargetId = targetId;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetHttpMethod"]/*'/>
        public MvcAjaxOptionsBuilder SetHttpMethod(string method)
        {
            _ajaxOptions.HttpMethod = method;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetOnBegin"]/*'/>
        public MvcAjaxOptionsBuilder SetOnBegin(string name)
        {
            _ajaxOptions.OnBegin = name;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetOnSuccess"]/*'/>
        public MvcAjaxOptionsBuilder SetOnSuccess(string name)
        {
            _ajaxOptions.OnSuccess = name;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetOnComplete"]/*'/>
        public MvcAjaxOptionsBuilder SetOnComplete(string name)
        {
            _ajaxOptions.OnComplete = name;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetOnFailure"]/*'/>
        public MvcAjaxOptionsBuilder SetOnFailure(string name)
        {
            _ajaxOptions.OnFailure = name;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetLoadingElementId"]/*'/>
        public MvcAjaxOptionsBuilder SetLoadingElementId(string id)
        {
            _ajaxOptions.LoadingElementId = id;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetLoadingElementDuration"]/*'/>
        public MvcAjaxOptionsBuilder SetLoadingElementDuration(int duration)
        {
            _ajaxOptions.LoadingElementDuration = duration;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetConfirm"]/*'/>
        public MvcAjaxOptionsBuilder SetConfirm(string confirm)
        {
            _ajaxOptions.Confirm = confirm;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="EnablePartialLoading"]/*'/>
        public MvcAjaxOptionsBuilder EnablePartialLoading()
        {
            _ajaxOptions.EnablePartialLoading = true;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="SetDataFormId"]/*'/>
        public MvcAjaxOptionsBuilder SetDataFormId(string id)
        {
            _ajaxOptions.DataFormId = id;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="DisallowCache"]/*'/>
        public MvcAjaxOptionsBuilder DisallowCache()
        {
            _ajaxOptions.AllowCache = false;
            return this;
        }

        ///<include file='MvcPagerDocs.xml' path='MvcPagerDocs/MvcAjaxOptionsBuilder/Method[@name="DisableHistorySupport"]/*'/>
        public MvcAjaxOptionsBuilder DisableHistorySupport()
        {
            _ajaxOptions.EnableHistorySupport = false;
            return this;
        }
    }
}
