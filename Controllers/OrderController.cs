﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
	public class OrderController : Controller
	{
		private IOrderRepository _orderRepository;
		private Cart _cart;
		public OrderController(IOrderRepository orderRepository, Cart cart)
		{
			_orderRepository = orderRepository;
			_cart = cart;
		}
		public IActionResult Checkout()
		{
			Console.WriteLine();
			return View(new Order());
		}

		[HttpPost]
		public IActionResult Checkout(Order order)
		{
			if (_cart.Lines.Count() == 0)
			{
				ModelState.AddModelError("", "Koszyk jest pusty");
			}

			if (ModelState.IsValid)
			{
				order.Lines = _cart.Lines.ToArray();
				_orderRepository.SaveOrder(order);
				return RedirectToAction(nameof(Completed));
			}
			else
			{
				return View(order);
			}
		}

		public IActionResult Completed()
		{
			_cart.Clear();
			return View();
		}

		[Authorize(Roles = "Administrator")]
		public IActionResult List() =>
			View(_orderRepository.Orders.Where(o => !o.Shipped));

		[HttpPost]
		[Authorize(Roles = "Administrator")]
		public IActionResult MarkShipped(int orderID) 
		{
			var order = _orderRepository.Orders.FirstOrDefault(o => o.Id == orderID);

			if(order != null) 
			{
				order.Shipped = true;
				_orderRepository.SaveOrder(order);
				return RedirectToAction(nameof(List));
			}
			else 
			{
				return NotFound();
			}
		}
	}
}
