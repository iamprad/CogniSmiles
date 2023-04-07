using CogniSmiles.Data;
using CogniSmiles.Interfaces;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPal.Api;

namespace CogniSmiles.Pages.Courses
{
    public class PaymentStatusModel : PageModel
    {
        private Order order;
        private Amount amount;
        private APIContext _apiContext;
        public CoursePayment coursePayment { get; set; }
        private readonly CogniSmilesContext _context;
        private readonly IEmailService _emailService;
        public PaymentStatusModel(CogniSmilesContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            // Get a reference to the config
            var config = new Dictionary<string, string>()
            {
                {"mode","sandbox" },
                {"clientId","ASs9uNRDK_KA3lQYz0QdY8mu94fRA_DJ_yo3I9enmKcwDfbx5pu2F_gMfQuRTvecgLqNUWrJdC-Sr_CU" },
                {"clientSecret","EGJABizgZ1B5o_RfY-AyoYvsAQ3LCmWHuAmd2fRZCtU0BhT4OVq4uZJJAOIqCzEnmhN4bNskifDO66MU" }
            };

            // Use OAuthTokenCredential to request an access token from PayPal
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();

            _apiContext = new APIContext(accessToken);
        }

        public void OnGet([FromQuery(Name = "PayerID")] string payerId, [FromQuery(Name = "guid")] string guid)
        {   
            if (!string.IsNullOrEmpty(payerId))
            {
                // Execute the order
                var paymentId = PageContext.HttpContext.Session.GetString(guid);

                var payment = new Payment() { id = paymentId };

                coursePayment = _context.CoursePayment.FirstOrDefault(cp => cp.PaymentID == paymentId);
                try
                {                      
                    var paymentExecution = new PaymentExecution() { payer_id = payerId };

                    // Execute the order payment.
                    var executedPayment = payment.Execute(_apiContext, paymentExecution);

                    // Get the information about the executed order from the returned payment object.
                    order = executedPayment.transactions[0].related_resources[0].order;
                    amount = executedPayment.transactions[0].amount;
                    
                    // Update CoursePayment Table
                    if (coursePayment != null)
                    {
                        coursePayment.PayerID = payerId;
                        coursePayment.PayerName = executedPayment.payer.payer_info.first_name + " " + executedPayment.payer.payer_info.last_name;
                        coursePayment.PayerEmail = executedPayment.payer.payer_info.email;
                        coursePayment.PaymentDate = executedPayment.create_time;
                        coursePayment.PaymentStatus = executedPayment.state;
                    }

                    // Once the order has been executed, an order ID is returned that can be used
                    // to do one of the following:
                    AuthorizeOrder(_apiContext);
                    if (coursePayment != null)
                        coursePayment.PaymentStatus = executedPayment.state;
                    CaptureOrder(_apiContext);
                    if (coursePayment != null)
                        coursePayment.PaymentStatus = executedPayment.state;
                }
                catch (Exception ex)
                {
                    if (coursePayment != null)
                        coursePayment.PaymentError = ex.Message;
                }
                if (coursePayment != null)
                { 
                    _context.CoursePayment.Update(coursePayment); 
                    _context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Authorizes an order. This begins the process of confirming that
        /// funds are available until it is time to complete the payment
        /// transaction.
        /// 
        /// More Information:
        /// https://developer.paypal.com/webapps/developer/docs/integration/direct/create-process-order/#authorize-an-order
        /// </summary>
        private void AuthorizeOrder(APIContext apiContext)
        {
            this.order.Authorize(apiContext);
        }

        /// <summary>
        /// Captures an order. For a partial capture, you can provide a lower
        /// amount than the total payment. Additionally, you can explicitly
        /// indicate a final capture (complete the transaction and prevent
        /// future captures) by setting the is_final_capture value to true.
        /// 
        /// More Information:
        /// https://developer.paypal.com/webapps/developer/docs/integration/direct/create-process-order/#capture-an-order
        /// </summary>
        private void CaptureOrder(APIContext apiContext)
        {
            var capture = new Capture();
            capture.amount = this.amount;
            capture.is_final_capture = true;
            this.order.Capture(apiContext, capture);
        }
    }
}
