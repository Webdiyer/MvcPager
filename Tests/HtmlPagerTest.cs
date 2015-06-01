using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webdiyer.WebControls.Mvc;

namespace MvcPager.Tests
{
    [TestClass]
    public class HtmlPagerTest
    {
        private const string AppPathModifier = TestHelper.AppPathModifier;
        private PagedList<int> _testList;
        private HtmlHelper _htmlHelper;
        const string BaseLink = AppPathModifier + "/mvcpager/test";

        [TestInitialize]
        public void Init()
        {
            _testList = new PagedList<int>(Enumerable.Range(1, 88).ToList(), 1, 8);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
        }


        [TestMethod]
        public void PagerOptions_MaximumPageNumberIs8_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       MaximumPageNumber = 8,RouteName = "Default"
                                                   });
            string htmlStr = html.ToHtmlString();
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"8\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("FirstPrev1");
            for (int i = 2; i <= 8; i++)
            {
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 8, "Last");
            sb.Append("</div>");
            sb.Append(TestHelper.CopyrightText);
            Assert.AreEqual(htmlStr,sb.ToString());
        }


        [TestMethod]
        public void PagerOptions_AlwaysShowFirstLastPageNumberWithCurrentPageIndexIs12_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 2;
            _testList.CurrentPageIndex = 12;
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", AlwaysShowFirstLastPageNumber = true});
            string htmlStr = html.ToHtmlString();
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"12\"", "data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"44\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a href=\"").Append(BaseLink).Append("\">First</a>");
            sb.AppendFormat(linkFormat, 11, "Prev");
            sb.Append("<a href=\"").Append(BaseLink).Append("\">1</a>");
            sb.AppendFormat(linkFormat, 6, "...");
            for (int i = 7; i <= 16; i++)
            {
                if (i == 12)
                    sb.Append(i);
                else
                {
                    sb.AppendFormat(linkFormat, i, i);
                }
            }
            sb.AppendFormat(linkFormat, 17, "...");
            sb.AppendFormat(linkFormat, 44, "44");
            sb.AppendFormat(linkFormat, 13, "Next");
            sb.AppendFormat(linkFormat, 44, "Last");
            sb.Append("</div>");
            sb.Append(TestHelper.CopyrightText);
            Assert.AreEqual(htmlStr, sb.ToString());
        }

        [TestMethod]
        public void PagerOptions_MaximumPageIndexItemsIs6_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetPageIndexBoxId("pib").SetMaximumPageIndexItems(6));
            string htmlStr = html.ToHtmlString();
            var attrs = new[]
            {
                "data-maxitems=\"6\"","data-pageindexbox=\"#pib\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"18\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            var sb = new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            StringAssert.Contains(htmlStr,sb.ToString());
        }
        

        [TestMethod]
        public void PagerOptions_PageIndexErrorMessage_ShouldReturnCorrectString()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetPageIndexOutOfRangeErrorMessage("Out of range").SetInvalidPageIndexErrorMessage("Invalid page index"));
            string htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, " data-outrangeerrmsg=\"Out of range\" ");
            StringAssert.Contains(htmlStr, " data-invalidpageerrmsg=\"Invalid page index\"");
        }
        
        
        [TestMethod]
        public void CustomRoute_CurrentPageIndexIs1_ShouldGenerateCorrectFirstPageUrlAndUrlFormatAttributes()
        {
            _htmlHelper = TestHelper.GetHtmlHelper(
                                                  new RouteValueDictionary
                                                      {
                                                          {"controller", "Controls"},
                                                          {"action", "MvcPager"}
                                                      });
            var pagingRoute = _htmlHelper.RouteCollection.MapRoute("Paging", "{controller}/{action}/page_{id}",
                                                                   new {controller = "Home", action = "Index", id = 1});
            _htmlHelper.ViewContext.RouteData.Route = pagingRoute;
            _testList.CurrentPageIndex = 8;
            IHtmlString html =
                _htmlHelper.Pager(_testList).Options(o => o.SetPageIndexParameterName("id").SetRouteName("Paging"));
            string htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr," data-firstpage=\"" + AppPathModifier + "/Controls/MvcPager/page_1\"",
                          "Url of the first page is incorrect");
            StringAssert.Contains(htmlStr," data-urlformat=\"" + AppPathModifier + "/Controls/MvcPager/page___id__\"",
                "Paging url format is incorrect");
        }

        [TestMethod]
        public void DefaultRoute_DefaultPageIndexParameterNameWithExtrIdRouteData_ShouldGenerateCorrectLinks()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 4;
            IHtmlString html = _htmlHelper.Pager(_testList);
            string htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, " data-firstpage=\"" + BaseLink + "/2\"",
                          "first page url is incorrect");
            StringAssert.Contains(htmlStr, "<a href=\"" + BaseLink + "/2?pageindex=3\">3</a>",
                          "paging link is incorrect");
            StringAssert.Contains(htmlStr, " data-urlformat=\"" + BaseLink + "/2?pageindex=__pageindex__\"",
                "url format is incorrect");
        }

        [TestMethod]
        public void MultipleMvcPager_ShouldGenerateCorrectUrlForDataAttributesAndLinks()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                       {
                                                           PageIndexParameterName = "id"
                                                       });
            var htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, "<a href=\"" + BaseLink + "/4\">4</a>",
                          "Paging url of the first MvcPager is incorrect");
            StringAssert.Contains(htmlStr, " data-firstpage=\"" + BaseLink + "\"",
                          "Value of firstpage data attribute of the first MvcPager is incorrent");
            StringAssert.Contains(htmlStr, " data-urlformat=\"" + BaseLink + "/__id__",
                          "Value of urlformat data attribute of the first MvcPager is incorrect");

            var plist=new PagedList<int>(Enumerable.Range(1,33),3,5);
            html = _htmlHelper.Pager(plist, new PagerOptions { RouteName = "Default" });
            htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, " data-firstpage=\"" + BaseLink + "/2\"",
                          "Value of firstpage data attribute of the second MvcPager is incorrent");
            StringAssert.Contains(htmlStr, " data-urlformat=\"" + BaseLink + "/2?pageindex=__pageindex__",
                "Value of urlformat data attribute of the second MvcPager is incorrect");
        }


        [TestMethod]
        public void RouteWithConstraintOnPageIndexParameter_ShouldGenerateCorrectUrlFormat()
        {
            _htmlHelper = TestHelper.GetHtmlHelper(new RouteValueDictionary(new{controller="articles",action="list"}));
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _htmlHelper.Pager(_testList,
                new PagerOptions
                {
                    PageIndexParameterName = "id",
                    RouteName = "constraintroute"
                });
            var htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, " data-urlformat=\"" + AppPathModifier + "/articles/list/__id__",
                "Value of urlformat data attribute is incorrect");
        }

        [TestMethod]
        public void DefaultrouteWithExtraRouteValues_ShouldGenerateCorrectUrlWithParameters()
        {
            _htmlHelper =
                TestHelper.GetHtmlHelper(new RouteValueDictionary { { "author", "Webdiyer" }, { "city", "Wuqi" }, { "id", 6 } });
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id"
                                                   });
            var htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, " data-firstpage=\"" + BaseLink + "?author=Webdiyer&amp;city=Wuqi\"",
                "Value of firstpage data attribute of the first MvcPager is incorrent");
            StringAssert.Contains(htmlStr, " data-urlformat=\"" + BaseLink + "/__id__?author=Webdiyer&amp;city=Wuqi\"","Value of urlformat data attribute is incorrent");
        }


        [TestMethod]
        public void MultipleMvcPagersWithExtraRouteValues_ShouldGenerateCorrectUrlWithParameters()
        {
            _htmlHelper =
                TestHelper.GetHtmlHelper(new RouteValueDictionary { { "author", "Webdiyer" }, { "city", "Wuqi" }, { "id", 6 } });
            _htmlHelper.Pager(_testList, new PagerOptions
            {
                PageIndexParameterName = "id"
            });
            _testList.CurrentPageIndex = 5;
            IHtmlString html = _htmlHelper.Pager(_testList);
            var htmlStr = html.ToHtmlString();
            StringAssert.Contains(htmlStr, " data-firstpage=\"" + BaseLink + "/6?author=Webdiyer&amp;city=Wuqi\"",
                "Value of firstpage data attribute of the second MvcPager is incorrent");
            StringAssert.Contains(htmlStr, " data-urlformat=\"" + BaseLink + "/6?author=Webdiyer&amp;city=Wuqi&amp;pageindex=__pageindex__",
                "Value of urlformat data attribute of the second MvcPager is incorrect");
        }


        [TestMethod]
        public void CurrentUICultureIsEnUs_ShouldGetCorrectPropertyValuesFromResourceFile()
        {
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 13;
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id"
                                                   });
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"13\"", "data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"18\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a href=\"").Append(BaseLink).Append("\">First</a>");
            sb.AppendFormat(linkFormat, 12, "Prev");
            sb.AppendFormat(linkFormat, 7, "...");
            for (int i = 8; i < 18; i++)
            {
                if (i == 13)
                    sb.Append(i);
                else
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 18, "...");
            sb.AppendFormat(linkFormat, 14, "Next");
            sb.AppendFormat(linkFormat, 18, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void CurrentUICultureIsZhCN_ShouldGetCorrectPropertyValuesFromResourceFile()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 13;
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id"
                                                   });
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightTextCn);
            var attrs = new[]
            {
                "data-currentpage=\"13\"", "data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessageCn + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessageCn + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"18\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a href=\"").Append(BaseLink).Append("\">首页</a>");
            sb.AppendFormat(linkFormat, 12, "上页");
            sb.AppendFormat(linkFormat, 7, "...");
            for (int i = 8; i < 18; i++)
            {
                if (i == 13)
                    sb.Append(i);
                else
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 18, "..."); 
            sb.AppendFormat(linkFormat, 14, "下页");
            sb.AppendFormat(linkFormat, 18, "尾页");
            sb.Append("</div>").Append(TestHelper.CopyrightTextCn);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void AlwaysShowFirstLastPageNumberWithPageIndexIs7_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 7;
            var po = new PagerOptions
            {
                PageIndexParameterName = "id",
                AlwaysShowFirstLastPageNumber = true,
                ShowFirstLast = false,
                ShowPrevNext = false
            };


            IHtmlString html = _htmlHelper.Pager(_testList, po);
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"7\"", "data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"18\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a href=\"" + BaseLink + "\">1</a>");
            for (int i = 2; i <= 11; i++)
            {
                if (i == 7)
                    sb.Append(i);
                else
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 12, "...");
            sb.AppendFormat(linkFormat, 18, 18);
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void AlwaysShowFirstLastPageNumber_PageIndexIs13AndPageSizeIs5_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 13;
            var po = new PagerOptions
            {
                PageIndexParameterName = "id",
                AlwaysShowFirstLastPageNumber = true,
                ShowFirstLast = false,
                ShowPrevNext = false
            };

            _htmlHelper = TestHelper.GetHtmlHelper();

            IHtmlString html = _htmlHelper.Pager(_testList, po);
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"13\"", "data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"18\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a href=\"" + BaseLink + "\">1</a>");
            sb.Append("<a href=\"" + BaseLink + "/7\">...</a>");
            for (int i = 8; i <= 18; i++)
            {
                if (i == 13)
                    sb.Append(i);
                else
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetAlwaysShowFirstLastPageNumber_PageIndexIs13AndPageSizeIs5_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 13;

            _htmlHelper = TestHelper.GetHtmlHelper();

            IHtmlString html = _htmlHelper.Pager(_testList).Options(o => o.SetPageIndexParameterName("id").AlwaysShowFirstLastPageNumber().HideFirstLast().HidePrevNext());
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"13\"","data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"18\"",
                "data-urlformat=\""+BaseLink+"/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a href=\"" + BaseLink + "\">1</a>");
            sb.Append("<a href=\"" + BaseLink + "/7\">...</a>");
            for (int i = 8; i <= 18; i++)
            {
                if (i == 13)
                    sb.Append(i);
                else
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }


        [TestMethod]
        public void HtmlPagerWithAllParameters_ShouldGenerateCorrectHtml()
        {
            const string rootLink = AppPathModifier + "/MyController/MyAction";
            const string linkFormat = "<a href=\"" + rootLink + "/{0}?city=Wuqi&nick=Webdiyer\">{1}</a>";
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(88, 10, 1,
                                                   new PagerOptions
                                                       {
                                                           ActionName = "MyAction",
                                                           ControllerName="MyController",
                                                           PageIndexParameterName = "id",
                                                           FirstPageText = "First Page",
                                                           PrevPageText = "Prev Page",
                                                           NextPageText = "Next Page",
                                                           LastPageText = "Last Page",RouteValues = new RouteValueDictionary { { "Controller", "MyController" }, { "Action", "MyAction" }, { "city", "Wuqi" }, { "nick", "Webdiyer" } },
                                                           HtmlAttributes = new Dictionary<string, object>{{"id","mypager"}}
                                                       });
            var sb = new StringBuilder(TestHelper.CopyrightText); 
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"9\"",
                "data-urlformat=\""+rootLink+"/__id__?city=Wuqi&amp;nick=Webdiyer\"","id=\"mypager\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First PagePrev Page1");
            for (int i = 2; i <= 9; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, "2", "Next Page");
            sb.AppendFormat(linkFormat, "9", "Last Page");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetAllParameters_ShouldGenerateCorrectHtml()
        {
            const string rootLink = AppPathModifier + "/MyController/MyAction";
            const string linkFormat = "<a href=\"" + rootLink + "/{0}?city=Wuqi&nick=Webdiyer\">{1}</a>";
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(88, 10, 1).Options(o=>o.SetActionName("MyAction").SetControllerName("MyController").
                                                       SetPageIndexParameterName("id").
                                                       SetFirstPageText("First Page").SetPrevPageText("Prev Page").
                                                       SetNextPageText("Next Page").SetLastPageText("Last Page").
                                                       SetRouteValues(new RouteValueDictionary { { "Controller", "MyController" }, { "Action", "MyAction" }, { "city", "Wuqi" }, { "nick", "Webdiyer" } }).
                                                       SetHtmlAttributes(new Dictionary<string, object> { { "id", "mypager" } }));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"9\"",
                "data-urlformat=\""+rootLink+"/__id__?city=Wuqi&amp;nick=Webdiyer\"","id=\"mypager\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First PagePrev Page1");
            for (int i = 2; i <= 9; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, "2", "Next Page");
            sb.AppendFormat(linkFormat, "9", "Last Page");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void CustomHtmlAttributesValue_ShouldGenerateCorrectHtmlAttributes()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", HtmlAttributes = new Dictionary<string, object> { { "aaa", "98%" }, { "bbb", "26px" }, { "ccc", "both" } } });
            var attrs = new[]
            {
                "aaa=\"98%\"", "bbb=\"26px\"","ccc=\"both\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            var sb=new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            StringAssert.Contains(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void HtmlAttributesWithStyleValue_ShouldGenerateCorrectStyleAttribute()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id",HorizontalAlign = "right",HidePagerItems = true,  HtmlAttributes = new Dictionary<string, object> { { "style", "width:95%;height:28px" }}});
            StringAssert.Contains(html.ToHtmlString(), " style=\"width:95%;height:28px;text-align:right;display:none\"");
        }

        [TestMethod]
        public void SetHtmlAttributesWithStyleValue_ShouldGenerateCorrectStyleAttribute()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetHorizontalAlign("right").HidePagerItems().SetHtmlAttributes(new Dictionary<string, object> { { "style", "width:95%;height:28px" } } ));
            StringAssert.Contains(html.ToHtmlString(), " style=\"width:95%;height:28px;text-align:right;display:none\"");
        }

        [TestMethod]
        public void HidePagerItemsAndCurrentPageIndexIs1_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", HidePagerItems = true });
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\"","style=\"display:none\""
            };
            var sb=new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append("></div>");
            Assert.AreEqual(html.ToHtmlString(), TestHelper.CopyrightText + sb + TestHelper.CopyrightText);
        }

        [TestMethod]
        public void HidePagerItemsAndCurrentPageIndexIs6_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", HidePagerItems = true });
            var attrs = new[]
            {
                "data-currentpage=\"6\"","data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\"","style=\"display:none\""
            };
            var sb = new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append("></div>");
            Assert.AreEqual(html.ToHtmlString(), TestHelper.CopyrightText + sb + TestHelper.CopyrightText);
        }

        [TestMethod]
        public void CurrentPagerItemTemplate_ShouldGenerateCorrentHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 2;
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                       {
                                                           PageIndexParameterName = "id",
                                                           CurrentPagerItemTemplate =
                                                               "<span class=\"current\">{0}</span>"
                                                       });
            StringAssert.Contains(html.ToHtmlString(),"<span class=\"current\">2</span>");
        }

        [TestMethod]
        public void CurrentPageNumberFormatStringAndPagerItemTemplate_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                       {
                                                           PageIndexParameterName = "id",
                                                           CurrentPagerItemTemplate =
                                                               "<span class=\"current\">{0}</span>",
                                                           CurrentPageNumberFormatString = "-{0}-"
                                                       });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"6\"", "data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.AppendFormat("<a href=\"{0}\">First</a>", BaseLink);
            sb.AppendFormat(linkFormat,5,"Prev");
            sb.AppendFormat("<a href=\"{0}\">1</a>",BaseLink);
            for (int i = 2; i <= 10; i++)
            {
                if (i == 6)
                    sb.Append("<span class=\"current\">-6-</span>");
                else
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 7, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetCurrentPageNumberFormatStringAndPagerItemTemplate_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetCurrentPagerItemTemplate("<span class=\"current\">{0}</span>").SetCurrentPageNumberFormatString("-{0}-"));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"6\"","data-firstpage=\"" + BaseLink + "\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.AppendFormat("<a href=\"{0}\">First</a>", BaseLink);
            sb.AppendFormat(linkFormat, 5, "Prev");
            sb.AppendFormat("<a href=\"{0}\">1</a>", BaseLink);
            for (int i = 2; i <= 10; i++)
            {
                if (i == 6)
                    sb.Append("<span class=\"current\">-6-</span>");
                else
                    sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 7, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void PageNumberFormatString_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">[{1}]</a>";
            const string txtLinkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       PageNumberFormatString = "[{0}]"
                                                   });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First")
                .Append("Prev")
                .Append("[1]");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(txtLinkFormat, 11, "...");
            sb.AppendFormat(txtLinkFormat, 2, "Next");
            sb.AppendFormat(txtLinkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void ContainerTagNameAndPagerItemTemplate_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<li><a href=\"" + BaseLink + "/{0}\">{1}</a></li>";
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       ContainerTagName = "ul",
                                                       PageIndexParameterName = "id",
                                                       PagerItemTemplate = "<li>{0}</li>"});
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<ul");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<li>First</li>")
                .Append("<li>Prev</li>")
                .Append("<li>1</li>");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</ul>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(),sb.ToString());
        }

        [TestMethod]
        public void SetContainerTagNameAndPagerItemTemplate_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<li><a href=\"" + BaseLink + "/{0}\">{1}</a></li>";
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetContainerTagName("ul").SetPageIndexParameterName("id")
                                                       .SetPagerItemTemplate("<li>{0}</li>"));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<ul");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<li>First</li>")
                .Append("<li>Prev</li>")
                .Append("<li>1</li>");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</ul>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void DisabledPagerItemTemplate_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       DisabledPagerItemTemplate = "<span class=\"disabled\">{0}</span>"
                                                   });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<span class=\"disabled\">First</span>")
                .Append("<span class=\"disabled\">Prev</span>")
                .Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void DisabledPagerItemTemplate_ShouldOverridePagerItemTemplate()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<li><a href=\"" + BaseLink + "/{0}\">{1}</a></li>";
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       ContainerTagName = "ul",
                                                       PageIndexParameterName = "id",
                                                       DisabledPagerItemTemplate = "<li class=\"disabled\">{0}</li>",
                                                       CurrentPagerItemTemplate = "<li class=\"current\">{0}</li>",
                                                       PagerItemTemplate = "<li>{0}</li>"
                                                   });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<ul");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<li class=\"disabled\">First</li>")
                .Append("<li class=\"disabled\">Prev</li>")
                .Append("<li class=\"current\">1</li>");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</ul>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetDisabledPagerItemTemplate_ShouldOverridePagerItemTemplate()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<li><a href=\"" + BaseLink + "/{0}\">{1}</a></li>";
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetContainerTagName("ul").SetPageIndexParameterName("id").
                                                       SetDisabledPagerItemTemplate("<li class=\"disabled\">{0}</li>").
                                                       SetCurrentPagerItemTemplate("<li class=\"current\">{0}</li>").
                                                       SetPagerItemTemplate("<li>{0}</li>"));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<ul");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<li class=\"disabled\">First</li>")
                .Append("<li class=\"disabled\">Prev</li>")
                .Append("<li class=\"current\">1</li>");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</ul>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }
        
        [TestMethod]
        public void CurrentPagerItemTemplate_ShouldInheritFromNumericPagerItemTemplate()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 2;
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       NumericPagerItemTemplate ="<span class=\"number\">{0}</span>"
                                                   });
            StringAssert.Contains(html.ToHtmlString(), "<span class=\"number\">2</span>");
        }

        [TestMethod]
        public void DisabledPagerItemTemplate_ShouldInheritFromNavigationPagerItemTemplate()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       NavigationPagerItemTemplate = "<span class=\"navbtn\">{0}</span>"
                                                   });
            StringAssert.Contains(html.ToHtmlString(), "<span class=\"navbtn\">First</span>");
        }

        [TestMethod]
        public void NumericPagerItemTemplate_ShouldOverridePagerItemTemplate()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<span><a href=\"" + BaseLink + "/{0}\">{1}</a></span>";
            const string numericLinkFormat = "<span class=\"number\"><a href=\"" + BaseLink + "/{0}\">{1}</a></span>";
            IHtmlString html = _htmlHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       ContainerTagName = "ul",
                                                       PageIndexParameterName = "id",
                                                       PagerItemTemplate = "<span>{0}</span>",
                                                       NumericPagerItemTemplate = "<span class=\"number\">{0}</span>"
                                                   });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\""+BaseLink+"/__id__\""
            };
            sb.Append("<ul");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<span>First</span>")
                .Append("<span>Prev</span>")
                .Append("<span>1</span>");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(numericLinkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</ul>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetNumericPagerItemTemplate_ShouldOverridePagerItemTemplate()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            const string linkFormat = "<span><a href=\"" + BaseLink + "/{0}\">{1}</a></span>";
            const string numericLinkFormat = "<span class=\"number\"><a href=\"" + BaseLink + "/{0}\">{1}</a></span>";
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetContainerTagName("ul").SetPageIndexParameterName("id").SetPagerItemTemplate("<span>{0}</span>")
                                                       .SetNumericPagerItemTemplate("<span class=\"number\">{0}</span>"));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<ul");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<span>First</span>")
                .Append("<span>Prev</span>")
                .Append("<span>1</span>");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(numericLinkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</ul>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void DefaultRoute_NullPagerOptions_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a href=\"" + BaseLink + "/2?pageindex={0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList,null);
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"pageindex\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\""+BaseLink+"/2?pageindex=__pageindex__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First").Append("Prev").Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void CurrentPageIndexIsLargerThanTotalPageCount_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 88;
            IHtmlString html = _htmlHelper.Pager(_testList, null);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"", "data-pagerid=\"Webdiyer.MvcPager\"",
                "style=\"color:red;font-weight:bold\""
            };
            var sb=new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">").Append(TestHelper.PageIndexOutOfRangeErrorMessage).Append("</div>");
            Assert.AreEqual(html.ToHtmlString(), TestHelper.CopyrightText + sb + TestHelper.CopyrightText);
        }

        [TestMethod]
        public void NavigationPagerItemsPositionIsLeft_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", NavigationPagerItemsPosition = PagerItemsPosition.Left });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\""+BaseLink+"/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First").Append("Prev");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last").Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void NavigationPagerItemsPositionIsRight_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", NavigationPagerItemsPosition = PagerItemsPosition.Right });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\""+BaseLink+"/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            //sb.Append("<div data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\" data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\" data-pageparameter=\"id\" data-pagerid=\"Webdiyer.MvcPager\" data-pagecount=\"11\" data-urlformat=\"").Append(BaseLink).Append("/__id__\">");
            sb.Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...").Append("First").Append("Prev");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetNavigationPagerItemsPositionToRight_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetNavigationPagerItemsPosition(PagerItemsPosition.Right));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...").Append("First").Append("Prev");
            sb.AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void OnPageIndexError_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", OnPageIndexError="alert('page index error:'+err)" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\""+BaseLink+"/__id__\"","data-onerror=\""+HttpUtility.HtmlEncode("alert('page index error:'+err)")+"\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First").Append("Prev").Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...").AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void SetOnPageIndexError_ShouldGenerateCorrectHtml()
        {
            _htmlHelper = TestHelper.GetHtmlHelper();
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _htmlHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetOnPageIndexError("alert('page index error:'+err)"));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\"","data-onerror=\"" + HttpUtility.HtmlEncode("alert('page index error:'+err)") + "\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("First").Append("Prev").Append("1");
            for (int i = 2; i <= 10; i++)
            {
                sb.AppendFormat(linkFormat, i, i);
            }
            sb.AppendFormat(linkFormat, 11, "...").AppendFormat(linkFormat, 2, "Next");
            sb.AppendFormat(linkFormat, 11, "Last");
            sb.Append("</div>").Append(TestHelper.CopyrightText);
            Assert.AreEqual(html.ToHtmlString(), sb.ToString());
        }
    }
}
