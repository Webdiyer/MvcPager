using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Webdiyer.MvcPagerDemo.Models;
using Webdiyer.WebControls.Mvc;

namespace Webdiyer.MvcPagerDemo.Controllers
{
    public class XmlDemoController : Controller
    {
        public ActionResult Index()
        {
            return View("XmlIndex");
        }


        #region Html Pager

        public ViewResult Basic(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ViewResult QueryString(int pageIndex = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(pageIndex, 5));
        }

        public ActionResult CustomRouting(int pageindex = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(pageindex, 5));
        }
        

        public ActionResult UrlParams(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ActionResult PageIndexBox(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }


        public ActionResult ApplyCSS(int id = 1)
        {
                return View(new PagedList<string>(new string[0], id, 1, 80));
        }


        public ActionResult CustomInfo(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }


        public ActionResult MultipleMvcPagers(int id = 1,int pageIndex=1)
        {
            const int pageSize = 5;
            var model =
                new Tuple<PagedList<Order>, PagedList<Order>>(
                    DemoData.AllOrders.OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize),
                    DemoData.AllOrders.OrderBy(o => o.OrderId).ToPagedList(id, pageSize));
        return View(model);
        }

        public ActionResult Search(int id = 1, string kword = null)
        {
            var query = DemoData.AllOrders.AsQueryable();
                if (!string.IsNullOrWhiteSpace(kword))
                    query = query.Where(a => a.EmployeeName.Contains(kword));
                var model = query.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                return View(model);
        }
        
        public ActionResult FirstPageUrl(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ActionResult PageSize(int pagesize = 10, int pageindex = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(pageindex, pagesize));
        }

        public ViewResult CombinedMode(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ActionResult NavButtonsPosition(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 80));
        }

        public ActionResult NumericPagerItemsFormat(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 80));
        }

        public ActionResult PagerItemsTemplate(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 80));
        }


        public ActionResult PageIndexError(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 6));
        }
        #endregion
        
        #region Ajax Pager

        public ActionResult AjaxPaging(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_OrderList", model);
                return View(model);
        }

        public ActionResult LoadByAjax(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_LoadByAjax", model);
            }
            return View();
        }

        public ActionResult AjaxPageIndexBox(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxPageIndexBox", model);
            return View(model);
        }

        public ActionResult AjaxEvents(int id = 1)
        {
            if (id == 2)
            {
                Response.StatusCode = 500;
                return Content("Test error text sent from server");
            }
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxEvents", model);
            return View(model);
        }

        public ActionResult AjaxInitData(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_OrderList", model);
                return View(model);
        }


        public ActionResult AjaxSearchPartialGet(string emp, int id = 1)
        {
            var qry = DemoData.AllOrders.AsQueryable();
            if (!string.IsNullOrWhiteSpace(emp))
                qry = qry.Where(a => a.EmployeeName.Contains(emp));
            var model = qry.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            return View(model);
        }

        public ActionResult AjaxSearchPartialPost(string emp, int id = 1)
        {
            var qry = DemoData.AllOrders.AsQueryable();
            if (!string.IsNullOrWhiteSpace(emp))
                qry = qry.Where(a => a.EmployeeName.Contains(emp));
            var model = qry.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            return View(model);
        }

        public ActionResult AjaxLoading(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                {
                    Thread.Sleep(2000);
                    return PartialView("_AjaxLoading", model);
                }
                return View(model);
        }

        public ActionResult AjaxNoHistory(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ActionResult AjaxPartialLoading(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ActionResult MultipleAjaxPagers(int id = 1,int pageIndex=1)
        {
            const int pageSize = 5;
            var model =
                new Tuple<PagedList<Order>, PagedList<Order>>(
                    DemoData.AllOrders.OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize),
                    DemoData.AllOrders.OrderBy(o => o.OrderId).ToPagedList(id, pageSize));
            return View(model);
        }

        public ActionResult AjaxPagers(int id = 1, int pageIndex = 1,int page=1)
        {
            const int pageSize = 5;
            if (Request.IsAjaxRequest())
            {
                var target = Request.QueryString["target"];
                if (target == "orders")
                {
                    return PartialView("_AjaxData1",
                        DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, pageSize));
                }
                if (target == "orders2")
                {
                    return PartialView("_AjaxData2",
                        DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(pageIndex, pageSize));
                }
                return PartialView("_AjaxData3",
                    DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(page, pageSize));
            }
            var model =
                new Tuple<PagedList<Order>, PagedList<Order>, PagedList<Order>>(
                    DemoData.AllOrders.OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize),
                    DemoData.AllOrders.OrderBy(o => o.OrderId).ToPagedList(id, pageSize),DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(page, pageSize));
            return View(model);
        }

        public ActionResult AjaxSearchGet(string emp, int id = 1)
        {
            return ajaxSearchGetResult(emp, id);
        }

        public ActionResult AjaxSearchHtmlGet(string emp, int id = 1)
        {
            return ajaxSearchGetResult(emp,  id);
        }

        private ActionResult ajaxSearchGetResult(string emp,  int id = 1)
        {
            var qry = DemoData.AllOrders.AsQueryable();
            if (!string.IsNullOrWhiteSpace(emp))
                qry = qry.Where(a => a.EmployeeName.Contains(emp));
                var model = qry.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchGet", model);
                return View(model);
        }

        public ActionResult AjaxSearchPost(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchPost", model);
                return View(model);
        }

        [HttpPost]
        public ActionResult AjaxSearchPost(string emp, int id = 1)
        {
            return ajaxSearchPostResult(emp, id);
        }

        public ActionResult AjaxSearchHtmlPost(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchPost", model);
                return View(model);
        }
        [HttpPost]
        public ActionResult AjaxSearchHtmlPost(string emp, int id = 1)
        {
            return ajaxSearchPostResult(emp, id);
        }

        private ActionResult ajaxSearchPostResult(string emp,  int id = 1)
        {
            var qry = DemoData.AllOrders.AsQueryable();
            if (!string.IsNullOrWhiteSpace(emp))
                qry = qry.Where(a => a.EmployeeName.Contains(emp));
            var model = qry.OrderBy(o => o.OrderId).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchPost", model);
                return View(model);
        }
        

        public ActionResult AjaxDegradation(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_Degradation", model);
            return View(model);
        }
        #endregion

        #region Javascript API
        public ActionResult JavascriptApiHtml(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        public ActionResult JavascriptApiAjax(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_OrderList",model);

            }
            return View(model);
        }



        public ActionResult CustomizeAjaxPager(int id = 1)
        {
            var model = DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxPagerHidden", model);
            return View(model);
        }
        
        public ActionResult CustomizeHtmlPager(int id = 1)
        {
            return View(DemoData.AllOrders.OrderBy(o=>o.OrderId).ToPagedList(id, 5));
        }

        #endregion

        #region PagedList and ToPagedList method samples

        public ActionResult Delete(int id = 1)
        {
            List<int> list = HttpContext.Cache["TestList"] as List<int>;
            if (list == null)
            {
                list = Enumerable.Range(1, 12).ToList();
                HttpContext.Cache["TestList"] = list;
            }
            return View(list.ToPagedList(id, 5));
        }

        [HttpPost]
        public ActionResult Delete(FormCollection values, int id = 1)
        {
            if (values["resetList"] != null)
            {
                HttpContext.Cache.Remove("TestList");
                return RedirectToAction("Delete");
            }
            var list = HttpContext.Cache["TestList"] as List<int>;
            if (list == null)
                throw new ApplicationException("Can not get test data from cache, please retry");
            int[] delItems = Array.ConvertAll(values["val"].Split(','), i => int.Parse(i));
            list.RemoveAll(delItems.Contains);
            HttpContext.Cache["TestList"] = list;
            return View(list.ToPagedList(id, 5));
        }


        public ActionResult IPagedList(int id=1)
        {
            MyPagedList<int> list=new MyPagedList<int>(Enumerable.Range(1,88),id,5);
            return View(list);
        }

        #endregion
    }
}
