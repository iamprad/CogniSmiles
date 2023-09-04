using CogniSmiles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CogniSmiles.Pages.Courses
{
    public class PaymentStatusModel : PageModel
    {
        public bool PaymentStatus { get; set; }
        private readonly IEmailService _emailService;
        public bool paymentCancelled;
        public PaymentStatusModel(IEmailService emailService)
        {
            _emailService = emailService;   
        }

        public IActionResult OnGetSuccess()
        {

            var config = new Dictionary<string, object>()
                    {
                        {"PaymentStatus","Success" }
                    };
            _emailService.SendEmail(Services.EmailType.CourseRegistration, config);
            PaymentStatus = true;
            return Page();
        }
        public IActionResult OnGetFailure()
        {
            var config = new Dictionary<string, object>()
                    {
                        {"PaymentStatus","Failed" }
                    };
            _emailService.SendEmail(Services.EmailType.CourseRegistration, config);
            PaymentStatus = false;
            return Page();
        }
    }
}
