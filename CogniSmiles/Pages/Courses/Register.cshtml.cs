using CogniSmiles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;

namespace CogniSmiles.Pages.Courses
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _config;
        public RegisterModel(IConfiguration config)
        {
            _config = config;            
        }

        public void OnGet()
        {           

        }
        public IActionResult OnPost() 
        {
            return BookYourCourse();
        }
        public IActionResult OnGetBookCourse()
        {
            return BookYourCourse();
        }
        public IActionResult BookYourCourse()
        {
            var paymentConfig = _config.GetSection("StripeConfig");
            // This is your test secret API key.
            StripeConfiguration.ApiKey = paymentConfig.GetValue<string>("Publishable_Key"); 
           
            var domain = "https://"+ _config.GetValue<string>("DomainName"); 

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                      // Price ID of the Product Created in Stripe Dashboard  - TEST
                    Price = paymentConfig.GetValue<string>("Product_PriceID"),
                    Quantity = 1
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/Courses/PaymentStatus?handler=Success",
                CancelUrl = domain + "/Courses/PaymentStatus?handler=Failure",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
