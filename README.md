# MvcPager
MvcPager is a free and open source paging component for ASP.NET MVC, it expose a series of extension methods for using in ASP.NET MVC applications, the implementation was inspired by Scott Guthrie's PagedList<T> idea, it was first published in 2009 and had been steadily improved and updated since then. The latest version 3.0 is one the most powerful ASP.NET MVC paging controls.

Project home page: http://en.webdiyer.com/mvcpager/

Online demos: http://en.webdiyer.com/mvcpager/demos/basic/

Online documentation: http://en.webdiyer.com/mvcpager/docs/

##MvcPager features:

* Basic url route paging;
* Support Ajax paging by using jQuery plugin;
* Support manual enter or select page index from textbox or dropdownlist;
* Support searching functionality in Ajax paging mode;
* Browser history support in Ajax paging mode (except for Opera and IE<=7);
* Degradated gracefully to basic url route paging if there's Javascript error on page or Javascript is disabled;
* Search engine friendly, search engine can index all pages in both url route paging or Ajax paging modes;
* Support ASP.NET MVC 4.0 and higher versions;
* Support all major browsers include IE, Firefox, Opera, Chrome and Safari;
* Support creation of customized user interface and jump to specified page using Javascript API(added in version 3.0);

##What's new in MvcPager 3.0

* Fixed a bug where MvcPager failed to generate link url for page index box if the current route contains constraint that limits page index to be ingeter;
* Fixed a bug where MvcPager failed to initialize in Ajax paging mode if total page count is 1 and PagerOptions.AuthoHide is true;
* Fixed a bug where multiple MvcPagers within a single partial view with the same value of PagerOptions.PageIndexParameterName property will trigger multiple http requests in Ajax paging mode;
* Added EnableHistorySupport property to MvcAjaxOptions class which is used to switch browser history support in Ajax paging mode;
* Added AllowCache property to MvcAjaxOptions class which is used to indicate whether client cache is allowed in Ajax paging mode (default value is true, only works with GET request, ref to the jQuery docs);
* Added DisabledPagerItemTemplate property to PagerOptions class which is used to set html template for the disabled pager items;
* Added OnPageIndexError property to PagerOptions class which is used to set the Javascript function to call when page index errors occured;
* Added HidePagerItems property to PagerOptions class which is used to hide all pager items in case you need to create your own pager UI;
* Added PageIndexBoxId and GoToButtonId proerties to PagerOptions class;
* Added ActionName, ControllerName, RouteName, RouteValues and HtmlAttributes properties to PagerOptions class, merged actionName, controllerName, routeName, routeValues and htmlAttributes parameters into pagerOptions parameter for HtmlHelper.Pager() and AjaxHelper.Pager() extension methods and adjusted method overloadings accordingly;
* Renamed PagerItemWrapperFormatString, NumericPagerItemWrapperFormatString, CurrentPagerItemWrapperFormatString, NavigationPagerItemWrapperFormatString, MorePagerItemWrapperFormatString properties to PagerItemTemplate, NumericPagerItemTemplate, CurrentPagerItemTemplate, NavigationPagerItemTemplate, MorePagerItemTemplate accordingly;
* Renamed PagerOptions.MaxPageIndex property name to PagerOptions.MaximumPageNumber;
* Removed PageIndexBoxWrapperFormatString and GoToPageSectionWrapperFormatString properties from PagerOptions class;
* Removed ShowPageIndexBox, ShowGoButton, PageIndexBoxType and GoButtonText properties from PagerOptions class;
* Removed PagerItemsSeperator property from PagerOptions class;
* Renamed PagedList.StartRecordIndex to PagedList.StartItemIndex;
* Renamed PagedList.EndRecordIndex to PagedList.EndItemIndex;
* Disabled pager items will not be hyperlinked (removed <a disabled="disabled"></a>);
* Added Options method to HtmlPager class for setting PagerOptions property;
* Added Options and AjaxOptions methods to AjaxPager class for setting PagerOptions and MvcAjaxOptions properties;
* Added HtmlHelper.LoadMvcPagerScript method which is used to load and initialize MvcPager jQuery plugin via Ajax dynamically;
* Refactored and improved code quality of the MvcPager jQuery plugin;
* Multi language support (English, Simplified Chinese and Tranditional Chinese);
* Added Javascript API for getting properties of MvcPager and jump to specified page;
* Added more samples to demo project that covers almost all functionalities of MvcPager;
* Added more unit tests;
* Support ASP.NET MVC 4.0 and higher versions;

##System requirements:

* jQuery 1.7 or higher versions;
* ASP.NET MVC 4.0 or higher versions;

##Known issues:

* If a single partial view contains multiple AjaxPagers with the same value of PagerOptions.PageIndexParameterName property, the Ajax events of all AjaxPagers except the first one will not be triggered in order to prevent duplicate http requests, page index boxes will also not work for all AjaxPagers except the first one;
* Browser history support in Ajax paging mode is not supported for Opera and IE7- browsers;
* Parameters with empty value in the url will be removed after paging;
* When using search functionality within AjaxForm, UnobtrusiveJavaScriptEnabled app setting in the web.config file must be set to true(default), otherwise search result may be loaded more that one times;
