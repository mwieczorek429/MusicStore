using Microsoft.AspNetCore.Mvc;
using MusicStore.Enums;
using MusicStore.Models;
using Newtonsoft.Json;

namespace MusicStore.ApiControllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PayPalWebhookController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public PayPalWebhookController(IOrderRepository orderRepository) 
        {
            _orderRepository = orderRepository;
        }
        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                var jsonBody = JsonConvert.DeserializeObject<dynamic>(body);

                string eventType = jsonBody["event_type"];
                string paymentId = jsonBody["resource"]["id"];

                switch (eventType)
                {
                    case "PAYMENT.SALE.COMPLETED":
                        HandlePaymentCompleted(jsonBody);
                        break;
                    case "PAYMENT.SALE.DENIED":
                        HandlePaymentDenied(jsonBody);
                        break;
                    case "PAYMENT.SALE.REFUNDED":
                        HandlePaymentRefunded(jsonBody);
                        break;
                }
                return Ok();
            }
        }
        private void HandlePaymentCompleted(dynamic jsonBody)
        {
            string orderId = jsonBody["resource"]["invoice_number"];
            var order = _orderRepository.Orders.FirstOrDefault(o => o.Id == int.Parse(orderId));
            if (order != null)
            {
                order.PaymentStatus = PaymentStatus.Completed;
                _orderRepository.SaveOrder(order);
            }
        }

        private void HandlePaymentDenied(dynamic jsonBody)
        {
            string orderId = jsonBody["resource"]["invoice_number"]; 
            var order = _orderRepository.Orders.FirstOrDefault(o => o.Id == int.Parse(orderId));
            if (order != null)
            {
                order.PaymentStatus = PaymentStatus.Failed;
                _orderRepository.SaveOrder(order);
            }
        }

        private void HandlePaymentRefunded(dynamic jsonBody)
        {
            string orderId = jsonBody["resource"]["invoice_number"]; 
            var order = _orderRepository.Orders.FirstOrDefault(o => o.Id == int.Parse(orderId));
            if (order != null)
            {
                order.PaymentStatus = PaymentStatus.Refunded;
                _orderRepository.SaveOrder(order);
            }
        }
    }
}
