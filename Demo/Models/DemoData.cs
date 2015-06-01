using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Webdiyer.MvcPagerDemo.Models
{
    public static class DemoData
    {
        public static List<Order> AllOrders
        {
            get
            {
                const string cacheKey = "DemoOrders";
                var orders = HttpContext.Current.Cache.Get(cacheKey) as IEnumerable<Order>;
                if (orders == null)
                {
                    XDocument xdoc = XDocument.Load(HttpContext.Current.Server.MapPath("/app_data/orders.xml"));
                    orders = from order in xdoc.Descendants("order")
                        select new Order
                        {
                            OrderId = int.Parse(order.Element("OrderId").Value),
                            CustomerId = order.Element("CustomerId").Value,
                            CompanyName = order.Element("CompanyName").Value,
                            OrderDate = DateTime.Parse(order.Element("OrderDate").Value),
                            EmployeeName = order.Element("EmployeeName").Value
                        };
                    HttpContext.Current.Cache[cacheKey] = orders;
                }
                return orders.ToList();
            }

        }

    }
}