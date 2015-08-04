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

'use strict';
var Webdiyer = Webdiyer || {};
Webdiyer.MvcPagers = [];
Webdiyer.MvcPagers.getById = function (id) {
    for (var i = 0; i <= this.length; i++) {
        if (typeof this[i] !== "undefined" && this[i].id === id) {
            return this[i];
        }
    }
    return null;
};
Webdiyer.__ajaxPages = {}; //Ajax page parameters, preventing multiple MvcPager with same UrlPageIndexName trigger multiple http requests

Webdiyer.MvcPager = function (wrapper) {
    this.wrapper = wrapper;
};

Webdiyer.MvcPager.prototype = {
    wrapper: null,
    id: null,
    urlFormat: null,
    pageIndexName: null,
    updateTarget: null,
    onBegin: null,
    onComplete: null,
    onFailure: null,
    onSuccess: null,
    httpMethod: null,
    confirm: null,
    loadingElementId: null,
    loadingDuration: 0,
    partialLoading: null,
    currentPageIndex: null,
    dataFormId: null,
    allowCache: true,
    enableHistorySupport: null,
    //autoSubmitForm: null,
    searchCriteria: null,
    pageCount: null,
    invalidPageErrMsg: null,
    outOfRangeErrMsg: null,
    firstPageUrl: null,
    pageIndexBox: null,
    goToButton: null,
    maxPageIndexItems: 20,
    isAjaxPager: null,
    onError: null,
    isFirstLoading: true,
    allowReload: false, //used by search form to check if reload is needed even current page index is not changed
    init: function () {
        var wrapper = $(this.wrapper);
        var newPageIndex = 1;
        var initialPageIndex; //value of the page index when page is loaded first time, if go back to this page we need to load the correct data by checking this value(it's not necessarily to be 1)
        this.id = wrapper.attr("id");
        this.isAjaxPager = wrapper.data("ajax") || false;
        this.pageCount = wrapper.data("pagecount");
        this.invalidPageErrMsg = wrapper.data("invalidpageerrmsg");
        this.outOfRangeErrMsg = wrapper.data("outrangeerrmsg");
        this.firstPageUrl = wrapper.data("firstpage");
        this.urlFormat = wrapper.data("urlformat");
        this.pageIndexName = wrapper.data("pageparameter");
        this.currentPageIndex = wrapper.data("currentpage") || 1;
        this.pageIndexBox = wrapper.data("pageindexbox");
        this.goToButton = wrapper.data("gotobutton");
        this.maxPageIndexItems = wrapper.data("maxitems") || 20;
        this.onError = wrapper.data("onerror") || "alert(errMsg)";
        var context = this;
        if (this.isAjaxPager) {
            this.updateTarget = wrapper.data("ajax-update");
            this.onBegin = wrapper.data("ajax-begin");
            this.onComplete = wrapper.data("ajax-complete");
            this.onFailure = wrapper.data("ajax-failure");
            this.onSuccess = wrapper.data("ajax-success");
            this.confirm = wrapper.data("ajax-confirm") || undefined;
            this.httpMethod = wrapper.data("ajax-method") || "GET";
            this.loadingElementId = wrapper.data("ajax-loading") || undefined;
            this.dataFormId = wrapper.data("ajax-dataformid") || undefined;
            this.allowCache = wrapper.data("ajax-allowcache") || true;
            var history = wrapper.data("ajax-enablehistorysupport");
            this.enableHistorySupport = (typeof history === "undefined" ? true : history);
            //this.autoSubmitForm = wrapper.data("ajax-autosubmitform") || false;
            this.loadingDuration = wrapper.data("ajax-loading-duration") || 0;
            this.partialLoading = wrapper.data("ajax-partialloading") || false;
            initialPageIndex = this.currentPageIndex;
            var pagerSelector = "[data-pagerid='Webdiyer.MvcPager']";
            var hashIndex = this.__getPageIndex(this.pageIndexName);
            if (hashIndex !== this.currentPageIndex && hashIndex > 0)
            { this.__ajax(hashIndex, { type: this.httpMethod, data: [] }); }
            if (typeof this.dataFormId !== "undefined") {
                var isAjaxForm = $(context.dataFormId).data("ajax") || false;
                $(context.dataFormId).submit(function (event) {
                    context.searchCriteria = $(context.dataFormId).serializeArray();
                    if (isAjaxForm) {
                        if (context.currentPageIndex !== 1) {
                            context.currentPageIndex = 1;
                            if (context.enableHistorySupport) {
                                context.__setPageIndex(context.pageIndexName, -1); //prevent reloading triggered by hashchange event
                                context.allowReload = true;
                            } else {
                                context.__ajax(1, { type: context.httpMethod, data: [] });
                            }
                        } else {
                            if (typeof Webdiyer.__ajaxPages[context.pageIndexName] === "undefined")
                            { context.allowReload = true; }
                        }
                    } else {
                        if (typeof Webdiyer.__ajaxPages[context.pageIndexName] === "undefined")
                        { context.allowReload = true; }
                        if (context.currentPageIndex === 1) {
                            context.__ajax(1, { type: context.httpMethod, data: [] });
                        } else {
                            if (context.enableHistorySupport) {
                                context.__setPageIndex(context.pageIndexName, 1);
                            } else {
                                context.__ajax(1, { type: context.httpMethod, data: [] });
                            }
                            context.currentPageIndex = 1;
                        } context.allowReload = true;
                        event.preventDefault();
                    }
                });
            }
            if (this.enableHistorySupport) {
                this.__initHashChange(initialPageIndex);
            }
            //pagination items
            $(this.updateTarget).on("click", pagerSelector + " a[data-pageindex]", function (e) {
                newPageIndex = $(this).data("pageindex");
                e.preventDefault();
                if (context.enableHistorySupport) {
                    context.__setPageIndex(context.pageIndexName, newPageIndex);
                } else {
                    context.__ajax(newPageIndex, { type: context.httpMethod, data: [] });
                }
            });
        }
        //page index box
        this.__bindPageIndexBox();
        return this;
    },
    __bindPageIndexBox: function () {
        var context = this;
        if (context.isAjaxPager) {
            if ($.trim(context.pageIndexBox) !== "") {
                if ($(context.updateTarget).find(context.pageIndexBox).length > 0) {
                    //page index box is inside of update target area
                    //check if page index box is dropdownlist and fill it with page indices
                    context.__fillPageIndexBox();
                    if ($(context.pageIndexBox).is('input:text')) {
                        $(context.pageIndexBox).val(context.currentPageIndex);
                        $(context.updateTarget).on("keydown", context.pageIndexBox, function () {
                            context.__validateInput(event);
                        });
                    }
                    if ($.trim(context.goToButton) !== "") {
                        $(context.updateTarget).on("click", context.goToButton, function () {
                            var newPageIndex = $(context.pageIndexBox).val();
                            context.goToPage(newPageIndex);
                        });
                    } else {
                        $(context.updateTarget).on("change", context.pageIndexBox, function () {
                            var newPageIndex = $(context.pageIndexBox).val();
                            context.goToPage(newPageIndex);
                        });
                    }
                } else {
                    //page index box is outside of update target area
                    context.__bindBoxEvents();
                }
            }
        } else {
            //page index box
            if ($.trim(context.pageIndexBox) !== "") {
                context.__bindBoxEvents();
            }
        }
    },
    __bindBoxEvents: function () {
        var context = this;
        context.__fillPageIndexBox();
        if ($(context.pageIndexBox).is('input:text')) {
            $(context.pageIndexBox).val(context.currentPageIndex);
            $(context.pageIndexBox).keydown(function () {
                context.__validateInput(event);
            });
        }
        if ($.trim(context.goToButton) !== "") {
            $(context.goToButton).click(function () {
                var newPageIndex = $(context.pageIndexBox).val();
                context.goToPage(newPageIndex);
            });
        } else {
            $(context.pageIndexBox).change(function () {
                var newPageIndex = $(context.pageIndexBox).val();
                context.goToPage(newPageIndex);
            });
        }
    },
    __fillPageIndexBox: function () {
        var se = $(this.pageIndexBox);
        if (se.prop('type') === 'select-one') {
            se.empty();
            var startIndex = this.currentPageIndex - (this.maxPageIndexItems / 2);
            if (startIndex + this.maxPageIndexItems > this.pageCount)
            { startIndex = this.pageCount + 1 - this.maxPageIndexItems; }
            if (startIndex < 1)
            { startIndex = 1; }
            var endIndex = startIndex + this.maxPageIndexItems - 1;
            if (endIndex > this.pageCount)
            { endIndex = this.pageCount; }
            for (var i = startIndex; i <= endIndex; i++) {
                se.append('<option value="' + i + '">' + i + '</option>');
            }
        }
        se.val(this.currentPageIndex);
    },
    __initHashChange: function (initialPageIndex) {
        var context = this;
        var docMode = document.documentMode;
        if ("onhashchange" in window &&
        (docMode === undefined || docMode > 7)) //IE compatable mode
        {
            $(window).bind("hashchange", function () {
                var pageIndex = context.__getPageIndex(context.pageIndexName);
                if (pageIndex === 0)
                { pageIndex = initialPageIndex; } //initial page index in url without hash value,eg. when go back from articles/list/3#id=2 to articles/list/3 initialPageIndex will be 3;
                context.__ajax(pageIndex, { type: context.httpMethod, data: [] });
            });
        } else {
            var currentHash = window.location.hash;
            setInterval(function () {
                var pageIndex = context.__getPageIndex(context.pageIndexName);
                if (window.location.hash !== currentHash) {
                    currentHash = window.location.hash;
                    if (pageIndex === 0)
                    { pageIndex = initialPageIndex; }
                    context.__ajax(pageIndex, { type: context.httpMethod, data: [] });
                }
            }, 200);
        }
    },
    __getPageIndex: function (pname) {
        var hash = window.location.hash.substring(1);
        if ($.trim(hash) !== "") {
            var harr = hash.split('&');
            for (var i = 0; i < harr.length; i++) {
                var hval = harr[i].split("=");
                if (hval[0].toString().toLowerCase() === pname.toString().toLowerCase()) {
                    return parseInt(hval[1]) || 1;
                }
            }
        }
        return 0;
    },
    __setPageIndex: function (pname, pindex) {
        var hash = window.location.hash.substring(1);
        if ($.trim(hash) === "")
        { window.location.hash = pname + "=" + pindex; }
        else {
            var r = new RegExp(pname + "=[^&]*", 'i');
            if (!r.test(hash))
            { window.location.hash += "&" + pname + "=" + pindex; }
            else {
                var index = hash.replace(r, pname + "=" + pindex);
                window.location.hash = index;
            }
        }
    },
    goToPage: function (pageIndex) {
        var r = new RegExp("^\\s*(\\d+)\\s*$");
        if (!r.test(pageIndex)) {
            this.__getFunction(this.onError, ["errType", "errMsg"]).apply(this, [0, this.invalidPageErrMsg]);
            return;
        } else if (RegExp.$1 < 1 || RegExp.$1 > this.pageCount) {
            this.__getFunction(this.onError, ["errType", "errMsg"]).apply(this, [1, this.outOfRangeErrMsg]);
            return;
        }
        if (this.isAjaxPager) {
            if (this.enableHistorySupport) {
                this.__setPageIndex(this.pageIndexName, pageIndex);
            } else {
                this.__ajax(pageIndex, { type: this.httpMethod, data: [] });
            }
        } else {
            if (typeof this.firstPageUrl !== "undefined" && this.firstPageUrl !== false && parseInt(pageIndex) === 1)
            { window.self.location.href = this.firstPageUrl; }
            else
            { window.self.location.href = decodeURI(this.urlFormat).replace("__" + this.pageIndexName + "__", pageIndex); }
        }
    },
    __ajax: function (index, options) {
        var context = this;
        if (typeof Webdiyer.__ajaxPages[context.pageIndexName] === "undefined" || Webdiyer.__ajaxPages[context.pageIndexName] !== index || context.allowReload) { //prevent duplicate ajax requests
            if (index === -1 || (index === 0 && context.isFirstLoading) || (index === context.currentPageIndex && !context.allowReload)) {
                return;
            }
            if (context.confirm && !(window.confirm(context.confirm))) {
                return;
            }
            $.extend(options, {
                url: this.urlFormat.replace("__" + context.pageIndexName + "__", index),
                cache: this.allowCache,
                beforeSend: function (xhr) {
                    var formMethod = options.type.toUpperCase();
                    if (!(formMethod === "GET" || formMethod === "POST")) {
                        xhr.setRequestHeader("X-HTTP-Method-Override", formMethod);
                    }
                    var result = context.__getFunction(context.onBegin, ["xhr"]).apply(this, arguments);
                    if (result !== false && typeof context.loadingElementId !== "undefined") {
                        $(context.loadingElementId).show(context.loadingDuration);
                    }
                    return result; //Ajax request will be cancelled if return false
                },
                complete: function () {
                    if (typeof context.loadingElementId !== "undefined") {
                        $(context.loadingElementId).hide(context.loadingDuration);
                    }
                    context.__fillPageIndexBox();
                    context.__getFunction(context.onComplete, ["xhr", "status"]).apply(this, arguments);
                },
                success: function (data) {
                    if (context.partialLoading)
                    { $(context.updateTarget).html($(context.updateTarget, data).html()); }
                    else
                    { $(context.updateTarget).html(data); }
                    context.currentPageIndex = index;
                    context.isFirstLoading = false;
                    context.__getFunction(context.onSuccess, ["data", "status", "xhr"]).apply(this, arguments);
                },
                error: context.__getFunction(context.onFailure, ["xhr", "status", "error"])
            });
            if (typeof context.dataFormId !== "undefined") {
                context.__pushData(options.data, context.searchCriteria);
            }
            options.data.push({ name: "X-Requested-With", value: "XMLHttpRequest" });
            var method = options.type.toUpperCase();
            if (!(method === "GET" || method === "POST")) {
                options.type = "POST";
                options.data.push({ name: "X-HTTP-Method-Override", value: method });
            }
            $.ajax(options);
            Webdiyer.__ajaxPages[context.pageIndexName] = index;
        }
    },
    __pushData: function (dataArr, dataToPush) {
        if (dataToPush !== null && typeof dataToPush !== "undefined") {
            for (var i = 0; i < dataToPush.length; i++) {
                dataArr.push({ name: dataToPush[i].name, value: dataToPush[i].value });
            }
        }
    },
    __getFunction: function (code, argNames) {
        var fn = window, parts = (code || "").split(".");
        while (fn && parts.length) {
            fn = fn[parts.shift()];
        }
        if (typeof (fn) === "function") {
            return fn;
        } //onSuccess="functionName"
        if ($.trim(code).toLowerCase().indexOf("function") === 0) {
            /*jslint evil: true */
            return new Function("return (" + code + ").apply(this,arguments);");
        } //onSuccess="function(data){alert(data);}"
        argNames.push(code);
        try {
            return Function.constructor.apply(null, argNames); //onSuccess="alert('hello');return false;"
        } catch (e) {
            alert("Error:\r\n" + code + "\r\n is not a valid callback function");
        }
    },
    __validateInput: function (e) {
        var kc;
        if (window.event) {
            kc = e.keyCode;
        }
        else if (e.which) {
            kc = e.which;
        }
        var valideKeys = [8, 37, 39, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
        if (kc !== null && valideKeys.indexOf(kc) < 0) {
            if (e.preventDefault) {
                e.preventDefault();
            }
            else {
                event.returnValue = false;
            }
        }
    }
};
(function ($) {
    $.fn.initMvcPagers = function () {
        return this.each(function () {
            Webdiyer.MvcPagers.push(new Webdiyer.MvcPager(this).init());
        });
    };
})(jQuery);
$(function () { $("[data-pagerid='Webdiyer.MvcPager']").initMvcPagers(); });
