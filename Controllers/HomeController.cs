using QLHS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace QLHS.Controllers
{
    public class HomeController : Controller
    {
        QLHS.Models.TestDBEntities db = new QLHS.Models.TestDBEntities();
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
            int userId = 1;
            List<QLHS.Models.orders> data = db.orders.Where(o => o.user_id ==userId).ToList();
            return View("_Order", data);
        }
        public ActionResult Cart()
        {
            int userId = 1;
            var cart=db.cart.FirstOrDefault(c => c.users.id == userId);
           
            return View(cart);
        }
        [HttpPost]
        public ActionResult AddCart(int bookId, int quantity)
        {
            int userId = 1;
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
            int userId = 1;
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

            return RedirectToAction("Index");
        }
        public ActionResult BookDetail(int id)
        {
            var book = db.book.Find(id);
            return View(book);
        }
        public ActionResult FindCategory(int id)
        {
            List<QLHS.Models.book> data = db.book.Where(b => b.category_id == id).ToList();
            return View("Book",data);
        }
        [ChildActionOnly]
        public ActionResult CategoryBook()
        {
            List<QLHS.Models.categories> data = db.categories.ToList();
            return View("_CategoryBook", data);
        }
        public ActionResult Book()
        {
            List<QLHS.Models.book> data = db.book.ToList();
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
        public ActionResult SignUp()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddWishList(int bookId)
        {
            int userId = 1;
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
            int userId = 1;
            var wish = db.favourite.FirstOrDefault(c => c.users.id == userId);
            List<QLHS.Models.favourite_item> data = db.favourite_item.Where(c => c.favourite_id == wish.id).ToList();
            return View(data);
        }
        public ActionResult MyAccount()
        {
            var user = db.users.Find(1);
            return View(user);
        }
        [HttpPost]
        public ActionResult MyAccount(QLHS.Models.users obj)
        {

            return View();
        }
        public ActionResult OrderDetail(int id)
        {
            var order = db.orders.Find(id);
            return View(order);
        }
        
    }
}