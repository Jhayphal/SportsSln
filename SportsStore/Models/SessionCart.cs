using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;
using System;
using System.Text.Json.Serialization;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        private const string SessionCartName = "Cart";

        public static Cart GetCart(IServiceProvider serviceProvider)
        {
            var session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext
                .Session;

            var cart = session?.GetJson<SessionCart>(SessionCartName) ?? new SessionCart();
            cart.Session = session;

            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);

            Session.SetJson(SessionCartName, this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);

            Session.SetJson(SessionCartName, this);
        }

        public override void Clear()
        {
            base.Clear();

            Session.Remove(SessionCartName);
        }
    }
}
