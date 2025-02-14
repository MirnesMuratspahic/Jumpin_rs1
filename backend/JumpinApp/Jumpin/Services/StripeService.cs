using Jumpin.Context;
using Jumpin.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jumpin.Services
{
    public class StripeService
    {
        private readonly StripeClient _stripeClient;

        public StripeService(StripeClient stripeClient)
        {
            _stripeClient = stripeClient;
        }

        public async Task<string> CreateCheckoutSessionAsync(float amount)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(amount * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "VIP Payment",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/succes-page",
                CancelUrl = "http://localhost:4200/stripe",
            };

            var service = new SessionService(_stripeClient);
            var session = await service.CreateAsync(options);

            return session.Id;
        }


    }
}
