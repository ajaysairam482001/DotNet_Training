using Question1_DB_First.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Question1_DB_First.Controllers
{
    public class CodeController : Controller
    {
        private northwindEntities db = new northwindEntities();

        public  ActionResult costumersInGermany()
        {
            var Customers = db.Customers.Where(c => c.Country == "Germany").ToList();
            return View(Customers);
        }

        public ActionResult CustomerDetails(int orderId)
        {
            var customer = db.Orders.Where(o => o.OrderID == orderId).Select(o => o.Customer).FirstOrDefault();
            return View(customer);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}