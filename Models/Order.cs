using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using MusicStore.Enums;

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
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal TotalValue { get; set; }
		[BindNever]
		public PaymentStatus PaymentStatus { get; set; }
	}
}
