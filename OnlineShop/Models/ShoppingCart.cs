using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Models
{
    public partial class ShoppingCart
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product, int qty = 1)
        {
            var cart = storeDB.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.Id);

            if(cart == null)
            {
                cart = new Cart
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = qty,
                    DateCreated = DateTime.Now
                };

                storeDB.Carts.Add(cart);
            }
            else
            {
                cart.Count += qty;
            }
            storeDB.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cart = storeDB.Carts.Single(c => c.CartId == ShoppingCartId && c.RecordId == id);
            int itemCount = 0;

            if(cart != null)
            {
                if(cart.Count > 1)
                {
                    cart.Count--;
                    itemCount = cart.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cart);
                }
                storeDB.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach(var cartItem in cart)
            {
                storeDB.Carts.Remove(cartItem);
            }
            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(c => c.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count * cartItems.Product.price).Sum();
            return count ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            foreach(Cart item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.price,
                    Quantity = item.Count
                };
                orderTotal += (item.Count * item.Product.price);

                storeDB.OrderDetails.Add(orderDetail);
            }

            order.Total = orderTotal;
            storeDB.SaveChanges();
            EmptyCart();
            return order.OrderId;
        }

        public string GetCartId(HttpContextBase context)
        {
            if(context.Session[CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach(Cart cart in shoppingCart)
            {
                cart.CartId = userName;
            }
            storeDB.SaveChanges();
        }
    }
}