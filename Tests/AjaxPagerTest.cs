using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webdiyer.WebControls.Mvc;

namespace MvcPager.Tests
{
    [TestClass]
    public class AjaxPagerTest
    {
        private PagedList<int> _testList;
        private AjaxHelper _ajaxHelper;
        private const string AppPath = "/webdiyer";
        private const string BaseLink = TestHelper.AppPathModifier + AppPath + "/mvcpager/test";

        [TestInitialize]
        public void Init()
        {
            _testList = new PagedList<int>(Enumerable.Range(1, 88).ToList(), 1, 8);
            _ajaxHelper = TestHelper.GetAjaxHelper(AppPath);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
        }

        [TestCleanup]
        public void CleanUp()
        {
            _ajaxHelper = null;
        }

        [TestMethod]
        public void MaximumPageNumberIs8_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            IHtmlString html = _ajaxHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").SetMaximumPageNumber(8)).AjaxOptions(a=>a.SetUpdateTargetId("test"));
            string htmlStr = html.ToHtmlString();
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#test\"","data-pagecount=\"8\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"","data-urlformat=\"" + BaseLink + "/__id__\""
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
            Assert.AreEqual(htmlStr, sb.ToString());
        }

        [TestMethod]
        public void AlwaysShowFirstLastPageNumberWithCurrentPageIndexIs12_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 2;
            _testList.CurrentPageIndex = 12;
            IHtmlString html = _ajaxHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").AlwaysShowFirstLastPageNumber()).AjaxOptions(a=>a.SetUpdateTargetId("test"));
            string htmlStr = html.ToHtmlString();
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-currentpage=\"12\"","data-firstpage=\"" + BaseLink + "\"",
                "data-ajax=\"true\"", "data-ajax-update=\"#test\"","data-pagecount=\"44\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"","data-urlformat=\"" + BaseLink + "/__id__\""
            };
            sb.Append("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            sb.Append("<a data-pageindex=\"1\" href=\"").Append(BaseLink).Append("\">First</a>");
            sb.AppendFormat(linkFormat, 11, "Prev");
            sb.Append("<a data-pageindex=\"1\" href=\"").Append(BaseLink).Append("\">1</a>");
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
        public void MaximumPageIndexItemsIs6_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            IHtmlString html = _ajaxHelper.Pager(_testList).Options(o => o.SetPageIndexParameterName("id").SetPageIndexBoxId("pib").SetMaximumPageIndexItems(6)).AjaxOptions(a=>a.SetUpdateTargetId("test"));
            string htmlStr = html.ToHtmlString();
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#test\"","data-maxitems=\"6\"","data-pageindexbox=\"#pib\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"","data-pagecount=\"18\"","data-urlformat=\"" + BaseLink + "/__id__\""
            };
            var sb=new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            StringAssert.Contains(htmlStr, sb.ToString());
        }


        [TestMethod]
        public void CurrentPageIndexIs3_CustomErrorMessage_ShouldGenerateCorrectDataAttributes()
        {
            _testList.CurrentPageIndex = 3;
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id",  PageIndexOutOfRangeErrorMessage = "out of range", InvalidPageIndexErrorMessage = "invalid page index" }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"3\"",
                "data-invalidpageerrmsg=\"invalid page index\"","data-firstpage=\""+BaseLink+"\"",
                "data-outrangeerrmsg=\"out of range\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"","data-pagecount=\"11\"","data-urlformat=\"" + BaseLink + "/__id__\""
            };
            var sb = new StringBuilder("<div");
            foreach (var att in attrs.OrderBy(a => a))
            {
                sb.Append(" ").Append(att);
            }
            sb.Append(">");
            StringAssert.Contains(html.ToHtmlString(), sb.ToString());
        }

        [TestMethod]
        public void CurrentPageIndexIs3_ShouldGenerateCorrectPaginationLink()
        {
            _testList.CurrentPageIndex = 3;
            IHtmlString html = _ajaxHelper.Pager(_testList).AjaxOptions(a=>a.SetUpdateTargetId("test")).Options(o=>o.SetPageIndexParameterName("id"));
            StringAssert.Contains(html.ToHtmlString(), "<a data-pageindex=\"4\" href=\"" + BaseLink+ "/4\">4</a>");
        }

        [TestMethod]
        public void MvcAjaxOptions_DisallowCache_ShouldGenerateCorrectAttributes()
        {
            _testList.CurrentPageIndex = 3;
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id" }, new MvcAjaxOptions { UpdateTargetId = "uptarget", AllowCache = false });
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"3\"","data-firstpage=\""+BaseLink+"\"",
                "data-ajax-allowcache=\"false\"",
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
            StringAssert.Contains(html.ToHtmlString(), sb.ToString());}

        [TestMethod]
        public void CurrentUICultureIsEnUs_ShouldGetCorrectPropertyValuesFromResourceFile()
        {
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 13;
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id" }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + TestHelper.AppPathModifier + AppPath + "/mvcpager/test/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"13\"","data-firstpage=\""+BaseLink+"\"",
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
            sb.Append("<a data-pageindex=\"1\" href=\"").Append(BaseLink).Append("\">First</a>");
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

            IHtmlString html = _ajaxHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id")).AjaxOptions(a=>a.SetUpdateTargetId("uptarget"));
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightTextCn);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"13\"","data-firstpage=\""+BaseLink+"\"",
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
            sb.Append("<a data-pageindex=\"1\" href=\"").Append(BaseLink).Append("\">首页</a>");
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
        public void AlwaysShowFirstLastPageNumber_PageIndexIs7_ShouldGenerateCorrectHtml()
        {
            _testList.PageSize = 5;
            _testList.CurrentPageIndex = 7;
            var po = new PagerOptions
            {
                PageIndexParameterName = "id",
                AlwaysShowFirstLastPageNumber = true,
                ShowFirstLast = false,
                ShowPrevNext = false
            };
            IHtmlString html = _ajaxHelper.Pager(_testList, po, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"7\"","data-firstpage=\""+BaseLink+"\"",
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
            sb.Append("<a data-pageindex=\"1\" href=\"" + BaseLink + "\">1</a>");
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
        public void AlwaysShowFirstLastPageNumber_PageIndexIs13_ShouldGenerateCorrectHtml()
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

            IHtmlString html = _ajaxHelper.Pager(_testList, po, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"13\"","data-firstpage=\""+BaseLink+"\"",
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
            sb.AppendFormat("<a data-pageindex=\"{0}\" href=\"" + BaseLink + "\">{1}</a>", 1, "1");
            sb.AppendFormat(linkFormat,7,"...");
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
        public void CustomRouteValuesWithControllerNameAndActionName_ShouldGenerateCorrectLink()
        {
            IHtmlString html = _ajaxHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id").AddRouteValue("Controller","MyController").AddRouteValue("Action","MyAction")).AjaxOptions(a=>a.SetUpdateTargetId("uptarget"));
            StringAssert.Contains(html.ToHtmlString(), "<a data-pageindex=\"3\" href=\"" + TestHelper.AppPathModifier +AppPath+ "/MyController/MyAction/3\">3</a>");
        }

        [TestMethod]
        public void PagerOptionsWithCustomRouteValues_ShouldGenerateGenerateCorrectUrl()
        {
            IHtmlString html = _ajaxHelper.Pager(_testList).AjaxOptions(a=>a.SetUpdateTargetId("uptarget")).Options(o=>o.SetPageIndexParameterName("id").AddRouteValue("Controller","MyController").AddRouteValue("Action","MyAction").AddRouteValue("city","Wuqi").AddRouteValue("nick","Webdiyer"));
            StringAssert.Contains(html.ToHtmlString(), "<a data-pageindex=\"3\" href=\"" + TestHelper.AppPathModifier + AppPath + "/MyController/MyAction/3?city=Wuqi&amp;nick=Webdiyer\">3</a>");
        }

        [TestMethod]
        public void PagerOptionsWithCustomHtmlAttributesValue_ShouldGenerateCorrectAttributes()
        {
            IHtmlString html = _ajaxHelper.Pager(_testList).AjaxOptions(a=>a.SetUpdateTargetId("uptarget")).Options(o=>o.SetPageIndexParameterName("id").AddHtmlAttribute("aaa","98%").AddHtmlAttribute("bbb","28px").AddHtmlAttribute("ccc","both"));
            StringAssert.Contains(html.ToHtmlString(), "<div aaa=\"98%\" bbb=\"28px\" ccc=\"both\" data-ajax=\"true\" ");
        }

        [TestMethod]
        public void HtmlAttributesWithStyleValue_ShouldGenerateCorrectStyleAttribute()
        {
            IHtmlString html = _ajaxHelper.Pager(_testList,
                new PagerOptions { PageIndexParameterName = "id", HorizontalAlign = "right", HidePagerItems = true, HtmlAttributes = new Dictionary<string, object> { { "style", "width:99%;height:28px" } } }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            StringAssert.Contains(html.ToHtmlString(), " style=\"width:99%;height:28px;text-align:right;display:none\"");
        }

        [TestMethod]
        public void HidePagerItemsAndCurrentPageIndexIs1_ShouldGenerateCorrectHtml()
        {
            IHtmlString html = _ajaxHelper.Pager(_testList,
                new PagerOptions { PageIndexParameterName = "id", HorizontalAlign = "right", HidePagerItems = true,  HtmlAttributes = new Dictionary<string, object> { { "style", "width:99%;height:28px" } } }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\"","style=\"width:99%;height:28px;text-align:right;display:none\""
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
            _testList.CurrentPageIndex = 6;
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", HidePagerItems = true }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"6\"","data-firstpage=\"" + BaseLink + "\"",
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
            _testList.CurrentPageIndex = 2;
            IHtmlString html = _ajaxHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       CurrentPagerItemTemplate =
                                                           "<span class=\"current\">{0}</span>"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            StringAssert.Contains(html.ToHtmlString(), "<span class=\"current\">2</span>");
        }

        [TestMethod]
        public void CurrentPageNumberFormatString_PagerItemTemplate_ShouldGenerateCorrectHtml()
        {
            _testList.CurrentPageIndex = 6;
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList, 
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       CurrentPagerItemTemplate =
                                                           "<span class=\"current\">{0}</span>",
                                                       CurrentPageNumberFormatString = "-{0}-"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" }); 
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","data-currentpage=\"6\"","data-firstpage=\"" + BaseLink + "\"",
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
            sb.AppendFormat("<a data-pageindex=\"1\" href=\"{0}\">First</a>", BaseLink);
            sb.AppendFormat(linkFormat, 5, "Prev");
            sb.AppendFormat("<a data-pageindex=\"1\" href=\"{0}\">1</a>", BaseLink);
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
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">[{1}]</a>";
            const string txtLinkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       PageNumberFormatString = "[{0}]"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
            const string linkFormat = "<li><a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a></li>";
            IHtmlString html = _ajaxHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       ContainerTagName = "ul",
                                                       PageIndexParameterName = "id",
                                                       PagerItemTemplate = "<li>{0}</li>"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       PageIndexParameterName = "id",
                                                       DisabledPagerItemTemplate = "<span class=\"disabled\">{0}</span>"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
            const string linkFormat = "<li><a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a></li>";
            IHtmlString html = _ajaxHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       ContainerTagName = "ul",
                                                       PageIndexParameterName = "id",
                                                       DisabledPagerItemTemplate = "<li class=\"disabled\">{0}</li>",
                                                       CurrentPagerItemTemplate = "<li class=\"current\">{0}</li>",
                                                       PagerItemTemplate = "<li>{0}</li>"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
            //sb.Append("<ul data-ajax=\"true\" data-ajax-update=\"#uptarget\" data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\" data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\" data-pageparameter=\"id\" data-pagerid=\"Webdiyer.MvcPager\" data-pagecount=\"11\" data-urlformat=\"").Append(BaseLink).Append("/__id__\">");
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
        public void NumericPagerItemTemplate_ShouldOverridePagerItemTemplate()
        {
            const string linkFormat = "<span><a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a></span>";
            const string numericLinkFormat = "<span class=\"number\"><a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a></span>";
            IHtmlString html = _ajaxHelper.Pager(_testList,
                                                   new PagerOptions
                                                   {
                                                       ContainerTagName = "ul",
                                                       PageIndexParameterName = "id",
                                                       PagerItemTemplate = "<span>{0}</span>",
                                                       NumericPagerItemTemplate = "<span class=\"number\">{0}</span>"
                                                   }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
            //sb.Append("<ul data-ajax=\"true\" data-ajax-update=\"#uptarget\" data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\" data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\" data-pageparameter=\"id\" data-pagerid=\"Webdiyer.MvcPager\" data-pagecount=\"11\" data-urlformat=\"").Append(BaseLink).Append("/__id__\">");
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
            _testList.CurrentPageIndex = 1;
            const string linkFormat =  "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "?pageindex={0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList, null, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"pageindex\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "?pageindex=__pageindex__\""
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
            _testList.CurrentPageIndex = 88;
            IHtmlString html = _ajaxHelper.Pager(_testList).AjaxOptions(a=>a.SetUpdateTargetId("uptarget"));
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"","style=\"color:red;font-weight:bold\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pagerid=\"Webdiyer.MvcPager\""
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
            _testList.CurrentPageIndex = 1;
            const string linkFormat =  "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", NavigationPagerItemsPosition = PagerItemsPosition.Left }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
            _testList.CurrentPageIndex = 1;
            const string linkFormat =  "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id", NavigationPagerItemsPosition = PagerItemsPosition.Right }, new MvcAjaxOptions { UpdateTargetId = "uptarget" });
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
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
        public void AjaxEvents_ShouldGenerateCorrectHtml()
        {
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList, new PagerOptions { PageIndexParameterName = "id"}, new MvcAjaxOptions { UpdateTargetId = "uptarget",OnBegin = "function(data){alert(data);}",OnComplete = "alert(\"complete\")",OnFailure = "failureHandler",OnSuccess = "showSuccessMsg"});
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
                "data-invalidpageerrmsg=\"" + TestHelper.InvalidPageIndexErrorMessage + "\"",
                "data-outrangeerrmsg=\"" + TestHelper.PageIndexOutOfRangeErrorMessage + "\"",
                "data-pageparameter=\"id\"", "data-pagerid=\"Webdiyer.MvcPager\"", "data-pagecount=\"11\"",
                "data-urlformat=\"" + BaseLink + "/__id__\"","data-ajax-begin=\""+HttpUtility.HtmlEncode("function(data){alert(data);}")+"\"",
                "data-ajax-complete=\"" + HttpUtility.HtmlEncode("alert(\"complete\")")+"\"","data-ajax-failure=\"failureHandler\"",
                "data-ajax-success=\"showSuccessMsg\""
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
        public void SetAjaxEvents_ShouldGenerateCorrectHtml()
        {
            _testList.CurrentPageIndex = 1;
            const string linkFormat = "<a data-pageindex=\"{0}\" href=\"" + BaseLink + "/{0}\">{1}</a>";
            IHtmlString html = _ajaxHelper.Pager(_testList).Options(o=>o.SetPageIndexParameterName("id")).AjaxOptions(a=>a.SetUpdateTargetId("uptarget").SetOnBegin("function(data){alert(data);}").SetOnComplete("alert(\"complete\")").SetOnFailure("failureHandler").SetOnSuccess("showSuccessMsg"));
            var sb = new StringBuilder(TestHelper.CopyrightText);
            var attrs = new[]
            {
                "data-ajax=\"true\"", "data-ajax-update=\"#uptarget\"",
                "data-ajax-begin=\"" + HttpUtility.HtmlEncode("function(data){alert(data);}") + "\"",
                "data-ajax-complete=\"" + HttpUtility.HtmlEncode("alert(\"complete\")") + "\"",
                "data-ajax-failure=\"failureHandler\"","data-ajax-success=\"showSuccessMsg\"",
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
    }
}
