using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z_Market.Models;
using Z_Market.ViewModels;

namespace Z_Market.Controllers
{
    public class OrdersController : Controller
    {

        private Z_MarketContext db = new Z_MarketContext();
        // GET: Orders
        public ActionResult NewOrder()
        {
            var orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();
            Session["OrderView"] = orderView;
            var list = db.Customers.ToList();
            list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
            list = list.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
            return View(orderView);
        }

        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {
            orderView = Session["OrderView"] as OrderView;

            var customerID = int.Parse(Request["CustomerID"]);

            if (customerID == 0)
            {
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Debe seleccionar un cliente";
                return View(orderView);
            }

            var customer = db.Customers.Find(customerID);
            if (customer == null)
            {
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Cliente no existe";
                return View(orderView);
            }

            if(orderView.Products.Count==0)
            {
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Debe ingresar detalle";
                return View(orderView);
            }

            int orderID = 0;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        CustomerID = customerID,
                        DateOrder = DateTime.Now,
                        OrderStatus = OrderStatus.Created
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderID = order.OrderID;

                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ID = item.ID,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            OrderID = orderID,

                        };
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "ERROR: " + ex.Message;
                    var list2 = db.Customers.ToList();
                    list2.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
                    list2 = list2.OrderBy(c => c.FullName).ToList();
                    return View(orderView);
                }
                
            }

            ViewBag.Message = string.Format("La orden: {0}, grabada ok", orderID);
            var list1 = db.Customers.ToList();
            list1.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
            list1 = list1.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(list1, "CustomerID", "FullName");

            orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();
            Session["orderView"] = orderView;
            return View(orderView);
        }

        public ActionResult AddProduct()
        {
            var list = db.Products.ToList();
            list.Add(new ProductOrder { ID = 0, Description = "[Seleccione un producto]" });
            list = list.OrderBy(c => c.Description).ToList();
            ViewBag.ID = new SelectList(list, "ID", "Description");
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            var orderView = Session["OrderView"] as OrderView;

            var productID = int.Parse(Request["ID"]);

            if (productID == 0)
            {
                var list = db.Products.ToList();
                list.Add(new ProductOrder { ID = 0, Description = "[Seleccione un producto]" });
                list = list.OrderBy(c => c.Description).ToList();
                ViewBag.ID = new SelectList(list, "ID", "Description");
                ViewBag.Error = "Debe seleccionar un producto";
                return View(productOrder);
            }

            var product = db.Products.Find(productID);
            if (product == null)
            {
                var list = db.Products.ToList();
                list.Add(new ProductOrder { ID = 0, Description = "[Seleccione un producto]" });
                list = list.OrderBy(c => c.Description).ToList();
                ViewBag.ID = new SelectList(list, "ID", "Description");
                ViewBag.Error = "Producto no existe";
                return View(productOrder);
            }
            productOrder = orderView.Products.Find(p => p.ID == productID);

            if(productOrder==null)
            {
                productOrder = new ProductOrder
                {
                    Description = product.Description,
                    Price = product.Price,
                    ID = product.ID,
                    Quantity = float.Parse(Request["Quantity"])
                };
                orderView.Products.Add(productOrder);
            }
            else
            {
                productOrder.Quantity += float.Parse(Request["Quantity"]);
            }


            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
            listC = listC.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            return View("NewOrder", orderView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}