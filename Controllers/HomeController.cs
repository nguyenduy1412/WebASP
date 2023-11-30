using QLHS.App_Start;
using QLHS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.Security;
using System.Web.UI;

namespace QLHS.Controllers
{
    public class HomeController : Controller
    {
        QLHS.Models.HieuSachEntities db = new QLHS.Models.HieuSachEntities();
        public QLHS.Models.users us = QLHS.App_Start.SessionConfig.GetUser();
        public ActionResult Index()
        {
           
            return View();
        }
        [ChildActionOnly]
        public ActionResult Category()
        {
            
            List<QLHS.Models.categories> data = db.categories.ToList();
            return View("_Category", data);
        }
        [ChildActionOnly]
        public ActionResult CategoryChild1()
        {
            var data = new List<List<QLHS.Models.book>>();
            for (int i = 0; i<3; i++)
            {
                var bookChild = db.book
            .Where(book => book.category_id == 1)
            .OrderBy(book => book.id) // hoặc sắp xếp theo một trường khác
            .Skip(i * 3)
            .Take(3)
            .ToList();
                data.Add(bookChild);
            }
            return View("_CategoryChild", data);
        }
        [ChildActionOnly]
        public ActionResult CategoryChild2()
        {
            var data = new List<List<QLHS.Models.book>>();
            for (int i = 0; i<3; i++)
            {
                var bookChild = db.book
            .Where(book => book.category_id == 1)
            .OrderBy(book => book.id) // hoặc sắp xếp theo một trường khác
            .Skip(i * 3)
            .Take(3)
            .ToList();
                data.Add(bookChild);
            }
            return View("_CategoryChild", data);
        }
        public ActionResult CategoryChild3()
        {
            var data = new List<List<QLHS.Models.book>>();
            for (int i = 0; i<3; i++)
            {
                var bookChild = db.book
            .Where(book => book.category_id == 1)
            .OrderBy(book => book.id) // hoặc sắp xếp theo một trường khác
            .Skip(i * 3)
            .Take(3)
            .ToList();
                data.Add(bookChild);
            }
            return View("_CategoryChild", data);
        }
        [ChildActionOnly]
        public ActionResult Banner()
        {
            List<QLHS.Models.banner> data = db.banner.OrderBy(b=>b.position).ToList();
            return View("_Banner", data);
        }
        [ChildActionOnly]
        public ActionResult BookSale()
        {
            List<QLHS.Models.book> data = db.book.Where(book => book.sale > 0).Take(6).ToList();
            return View("_BookSale", data);
        }
        [ChildActionOnly]
        public ActionResult BookNew()
        {
            var data = new List<List<QLHS.Models.book>>();
            for(int i = 0; i<5; i++)
            {
                var bookChild = db.book.OrderByDescending(book => book.id).Skip(i*2).Take(2).ToList();
                data.Add(bookChild);
            }
            return View("_BookNew", data);
        }
        [ChildActionOnly]
        public ActionResult BookTrend()
        {
            return null;
        }
        [ChildActionOnly]
        public ActionResult Order()
        {
            
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            List<QLHS.Models.orders> data = db.orders.Where(o => o.user_id ==userId).ToList();
            return View("_Order", data);
        }
        public ActionResult Cart()
        {
            
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            long sum = 0;
            var cart=db.cart.FirstOrDefault(c => c.users.id == userId);
            foreach(var item in cart.cart_item)
            {
               sum += (long)(item.book.price_sale * item.quantity);
            }
            cart.total = sum;
            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();
            return View(cart);
        }
        [HttpPost]
        public ActionResult AddCart(int bookId, int quantity)
        {
            long userId = (long)us.id;
            var cart = db.cart.FirstOrDefault(c => c.users.id == userId);
            if (cart != null)
            {
                // Kiểm tra xem sản phẩm đã có trong giỏ hàng hay chưa
                var cartItem = db.cart_item.FirstOrDefault(item => item.cart_id == cart.id && item.book_id == bookId);

                if (cartItem != null)
                {
                    // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
                    cartItem.quantity += quantity;
                }
                else
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới cartItem
                    var newCartItem = new cart_item()
                    {
                        cart_id = cart.id,
                        book_id = bookId,
                        quantity = quantity // Đặt giá trị mặc định hoặc theo nhu cầu của bạn
                    };

                    db.cart_item.Add(newCartItem);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }
            return RedirectToAction("Cart", "Home");
        }
        [ChildActionOnly]
        public ActionResult DetailCheckout()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            var cart = db.cart.FirstOrDefault(c => c.users.id == userId);
            return View("_DetailCheckout", cart);
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(QLHS.Models.orders obj)
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            var cart = db.cart.FirstOrDefault(c => c.users.id == userId);
            List<QLHS.Models.cart_item> list = db.cart_item.Where(c => c.cart_id == cart.id).ToList();
            obj.sum_money=(long)cart.total;
            obj.user_id = userId;
            obj.date_order = DateTime.Now;
            obj.status=0;
            db.orders.Add(obj);
            db.SaveChanges();
            try
            {
                foreach (var cartItem in list)
                {
                    var orderDetail = new order_detail()
                    {
                        order_id = obj.id, // Gán order_id của đơn hàng mới tạo
                        book_id = cartItem.book_id,
                        quantity = cartItem.quantity,
                        price=cartItem.book.price_sale,
                        status_rate=0,
                    };

                    // Thêm chi tiết đơn hàng vào bảng orderDetail và xóa các sản phẩm trong giỏ hàng
                    db.order_detail.Add(orderDetail);
                    db.cart_item.Remove(cartItem);
                }
                db.SaveChanges();
            }
            catch
            {
                return View();
            }
           
            return RedirectToAction("Index");
        }
        public ActionResult BookDetail(int id)
        {
            var book = db.book.Find(id);
            return View(book);
        }
        public ActionResult FindCategory(int id,int page=1)
        {
            List<QLHS.Models.book> data = db.book.Where(b => b.category_id == id).OrderBy(b => b.id).Skip((page-1)*12).Take(12).ToList();
            int sum = db.book.Where(b => b.category_id == id).Count();

            int total = sum/12 +1;
            ViewBag.total = total;
            ViewBag.page = page;
            ViewBag.id=id;
            return View("BookByCategory", data);
        }
        [ChildActionOnly]
        public ActionResult CategoryBook()
        {
            List<QLHS.Models.categories> data = db.categories.ToList();
            return View("_CategoryBook", data);
        }
        [ChildActionOnly]
        public ActionResult AuthorBook()
        {
            List<QLHS.Models.author> data = db.author.ToList();
            return View("_AuthorList", data);
        }
        [ChildActionOnly]
        public ActionResult PublicsherBook()
        {
            List<QLHS.Models.publicsher> data = db.publicsher.ToList();
            return View("_Publicsher", data);
        }
        public ActionResult Book(int page=1)
        {
            int sum=db.book.Count();
            List<QLHS.Models.book> data = db.book.OrderByDescending(book => book.id).Skip((page-1)*12).Take(12).ToList();
            int total = sum/12 +1;
            ViewBag.total = total;
            ViewBag.page = page;
            return View(data);
        }
        [HttpPost]
        public ActionResult Search(string keyword)
        {
            ViewBag.keyword = keyword;
            List<QLHS.Models.book> data=db.book.Where(b => b.book_name.Contains(keyword)).ToList();
            return View("Book",data);
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(QLHS.Models.users obj)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword;
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashedPasswordBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(obj.pass_word));
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashedPasswordBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    hashedPassword = sb.ToString();
                }
                Models.users check = db.users.FirstOrDefault(m => m.user_name == obj.user_name && m.pass_word == hashedPassword);
                if (check == null)
                {
                    //Đăng nhập thất bại
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng!");
                }
                else
                {
                    SessionConfig.SetUser(check);
                    var us = SessionConfig.GetUser();
                    //FormsAuthentication.SetAuthCookie(check.user_name, false);
                    if (Request.QueryString["ReturnUrl"]==null || Request.QueryString["ReturnUrl"] == "")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    }
                }
            }
            return View(obj);
        }
        public ActionResult SignUp()
        {

            return View();
        }
        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult AddWishList(int bookId)
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            var favourite = db.favourite.FirstOrDefault(c => c.users.id == userId);
            if (favourite != null)
            {
                // Kiểm tra xem sản phẩm đã có hay chưa
                var favouriteItem = db.favourite_item.FirstOrDefault(item => item.favourite_id == favourite.id && item.book_id == bookId);

                if (favouriteItem != null)
                {
                    // Nếu sản phẩm đã có không thêm
                }
                else
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới 
                    var newFavouriteItem = new favourite_item()
                    {
                        favourite_id = favourite.id,
                        book_id = bookId,
                       
                    };

                    db.favourite_item.Add(newFavouriteItem);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }
            return RedirectToAction("Wishlist", "Home");
        }
        public ActionResult Wishlist()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            var wish = db.favourite.FirstOrDefault(c => c.users.id == userId);
            List<QLHS.Models.favourite_item> data = db.favourite_item.Where(c => c.favourite_id == wish.id).ToList();
            return View(data);
        }
        public ActionResult MyAccount()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Login","Home");
            }
            long userId = (long)us.id;
            var user = db.users.Find(userId);
            return View(user);
        }
        [HttpPost]
        public ActionResult MyAccount(QLHS.Models.users obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fImage = Request.Files["fImage"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fName = fImage.FileName;
                        string foder = Server.MapPath("~/Assets/Upload/" + fName);
                        fImage.SaveAs(foder);
                        obj.img =  fName;
                    }
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyAccount","Home");
                }
                catch { }
            }
            return View();
        }
        public ActionResult OrderDetail(int id)
        {
            var order = db.orders.Find(id);
            return View(order);
        }
        public ActionResult CancelOrder(int id)
        {
            var order = db.orders.Find(id);
            List<QLHS.Models.order_detail> lst = db.order_detail.Where(o=>o.order_id == id).ToList();
            foreach (var item in lst)
            {
                db.order_detail.Remove(item);
            }
            db.orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("MyAccount", "Home");
        }
    }
}