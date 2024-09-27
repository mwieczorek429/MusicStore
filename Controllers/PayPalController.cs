using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.Configurations;
using MusicStore.Data;
using MusicStore.Models;
using PayPal.Api;

namespace MusicStore.Controllers
{
    public class PayPalController : Controller
    {
        private readonly PayPalConfiguration _payPalConfiguration;
        private Cart _cart;
        private IOrderRepository _orderRepository;

        public PayPalController(PayPalConfiguration payPalConfiguration, Cart cart, IOrderRepository orderRepository)
        {
            _payPalConfiguration = payPalConfiguration;
            _cart = cart;
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> CreatePayment(int OrderID)
        {
            _cart.Clear();

            var order = await _orderRepository.Orders.
                FirstOrDefaultAsync(o => o.Id == OrderID);

            if (order == null)
            {
                return NotFound();
            }

            var apiContext = _payPalConfiguration.GetAPIContext();

            var payment = new Payment()
            {
                intent = "sale", 
                payer = new Payer() { payment_method = "paypal" },
                transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    description = "Zakup testowy",
                    invoice_number = OrderID.ToString(), 
                    amount = new Amount()
                    {
                        currency = "PLN",
                        total = order.TotalValue.ToString(), 
                    }
                }
            },
                redirect_urls = new RedirectUrls()
                {
                    return_url = Url.Action("PaymentSuccess", "PayPal", null, Request.Scheme),
                    cancel_url = Url.Action("PaymentCancel", "PayPal", null, Request.Scheme)
                }
            };

            var createdPayment = payment.Create(apiContext);

            var approvalUrl = createdPayment.links.GetEnumerator();
            while (approvalUrl.MoveNext())
            {
                var link = approvalUrl.Current;
                if (link.rel.ToLower().Trim().Equals("approval_url"))
                {
                    return Redirect(link.href);
                }
            }

            return RedirectToAction("Error");
        }

        public IActionResult PaymentSuccess(string paymentId, string token, string PayerID)
        {
            var apiContext = _payPalConfiguration.GetAPIContext();

            var paymentExecution = new PaymentExecution() { payer_id = PayerID };
            var payment = new Payment() { id = paymentId };

            var executedPayment = payment.Execute(apiContext, paymentExecution);

            if (executedPayment.state.ToLower() != "approved")
            {
                return RedirectToAction("Error");
            }

            return View("Success");
        }

        public IActionResult PaymentCancel()
        {
            return View("Cancel");
        }

        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
