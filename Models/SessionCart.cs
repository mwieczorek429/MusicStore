using MusicStore.Extensions;
using Newtonsoft.Json;

namespace MusicStore.Models
{
	public class SessionCart : Cart
	{
		public static Cart GetCart(IServiceProvider services)
		{
			ISession session = services.GetRequiredService<IHttpContextAccessor>()?
				.HttpContext.Session;
			SessionCart cart = session?.GetJson<SessionCart>("Cart")
				?? new SessionCart();
			cart.Session = session;
			return cart;
		}

		[JsonIgnore]
		public ISession Session { get; set; }

		public override void AddItem(Album album, int quantity)
		{
			base.AddItem(album, quantity);
			Session.SetJson("Cart", this);
		}

		public override void RemoveLine(Album album)
		{
			base.RemoveLine(album);
			Session.SetJson("Cart", this);
		}

		public override void Clear()
		{
			base.Clear();
			Session.Remove("Cart");
		}
	}
}
