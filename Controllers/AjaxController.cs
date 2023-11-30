using QLHS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHS.Controllers
{
    public class AjaxController : Controller
    {
        public QLHS.Models.users us = QLHS.App_Start.SessionConfig.GetUser();
        QLHS.Models.HieuSachEntities db = new QLHS.Models.HieuSachEntities();
        // GET: Ajax
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public bool DeleteItem(int id)
        {
            var item = db.cart_item.Find(id);
            db.cart_item.Remove(item);
            db.SaveChanges();
            return true;

        }
        [HttpPost]
        public bool DeleteFavourite(int id)
        {
            var item = db.favourite_item.Find(id);
            db.favourite_item.Remove(item);
            db.SaveChanges();
            return true;
        }
        [HttpPost]
        public bool AddItem(int id)
        {

            long userId = (long)us.id;
            var cart = db.cart.FirstOrDefault(c => c.users.id == userId);
            if (cart != null)
            {
                // Kiểm tra xem sản phẩm đã có trong giỏ hàng hay chưa
                var cartItem = db.cart_item.FirstOrDefault(item => item.cart_id == cart.id && item.book_id == id);

                if (cartItem != null)
                {
                    // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
                    cartItem.quantity += 1;
                }
                else
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới cartItem
                    var newCartItem = new cart_item()
                    {
                        cart_id = cart.id,
                        book_id = id,
                        quantity = 1 // Đặt giá trị mặc định hoặc theo nhu cầu của bạn
                    };

                    db.cart_item.Add(newCartItem);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                return true;
            }
            return false;
    
        }

        [HttpPost]
        public ActionResult UpdateCart(int id ,int quantity)
        {
            var cartItem=db.cart_item.Find(id);
            cartItem.quantity = quantity;
            db.SaveChanges();
            return Json(new { success = true, message = "Cập nhật giỏ hàng thành công" });
        }
        [HttpPost]
        public bool UpdateStatus(int id, int status)
        {
            try
            {
                var item = db.orders.Find(id);
                item.status = status;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {

            }
            return true;
        }
    }
    
}