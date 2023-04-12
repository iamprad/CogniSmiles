using CogniSmiles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CogniSmiles.Pages.Courses
{
    public class RegisterModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        public RegisterModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;            
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
            var guid = Convert.ToString((new Random()).Next(100000));

            var paymentReg = _paymentService.RegisterPayment(guid);

            PageContext.HttpContext.Session.SetString(guid, paymentReg.Item1);

            return Redirect(paymentReg.Item2);
        }
    }
}
