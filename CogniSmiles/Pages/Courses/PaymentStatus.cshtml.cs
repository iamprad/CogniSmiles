using CogniSmiles.Interfaces;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CogniSmiles.Pages.Courses
{
    public class PaymentStatusModel : PageModel
    {
        public CoursePayment? coursePayment { get; set; }
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        public PaymentStatusModel(IPaymentService paymentService, IEmailService emailService)
        {
            _paymentService = paymentService;
            _emailService = emailService;            
        }

        public void OnGet([FromQuery(Name = "PayerID")] string payerId, [FromQuery(Name = "guid")] string guid)
        {   
            if (!string.IsNullOrEmpty(payerId))
            {
                // Execute the order
                var paymentId = PageContext.HttpContext.Session.GetString(guid);
                coursePayment = _paymentService.ExecutePayment(paymentId, payerId);
                //Send Email Notification 
                if (coursePayment != null) {
                    var config = new Dictionary<string, object>()
                    {
                        {"CourseID",coursePayment.Id }
                    };
                    _emailService.SendEmail(Services.EmailType.CourseRegistration, config);
                }
                
            }
        }
        
    }
}
