using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MusicStore.Models
{
	public class Order
	{
		//TO DO: Add the remaining fields
		[BindNever]
		public int Id { get; set; }
		[BindNever]
		[ValidateNever] //TO DO: Think of another solution
		public ICollection<CartLine> Lines { get; set; }
		[BindNever]
		public bool Shipped { get; set; }
		public string? Name { get; set; }
	}
}
