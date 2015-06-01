/*
 MvcPager for ASP.NET MVC - test.js
 Copyright:2009-2015 Webdiyer(http://en.webdiyer.com) 陕西省延安市吴起县 杨涛<Webdiyer> (http://www.webdiyer.com)
 Source code released under Ms-PL license
 */

test('parameters', function() {
    equal(Webdiyer.MvcPagers[0].urlFormat, "/NoDbDemo/AjaxPaging/__id__");
    equal(Webdiyer.MvcPagers.getById("mypager").isAjaxPager, true);
    equal(Webdiyer.MvcPagers.getById("mypager").firstPageUrl,null);
    equal(Webdiyer.MvcPagers.getById("mypager").pageCount, 5);
    equal(Webdiyer.MvcPagers.getById("mypager").pageIndexName, "id");
    equal(Webdiyer.MvcPagers.getById("mypager").updateTarget, "#articles");
    equal(Webdiyer.MvcPagers.getById("mypager").currentPageIndex, 1);
});