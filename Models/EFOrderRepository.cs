
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;

namespace MusicStore.Models
{
	public class EFOrderRepository : IOrderRepository
	{
		private readonly MusicStoreDbContext _context;
		public EFOrderRepository(MusicStoreDbContext context) 
		{
			_context = context;
		}
		public IQueryable<Order> Orders => _context.Orders
			.Include(o => o.Lines)
			.ThenInclude(l => l.Album);

		public void SaveOrder(Order order)
		{
			_context.AttachRange(order.Lines.Select(l => l.Album));
			if(order.Id == 0) 
			{
				_context.Orders.Add(order);	
			}
			_context.SaveChanges();
		}
	}
}
