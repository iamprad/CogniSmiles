using CogniSmiles.Data;
using CogniSmiles.Interfaces;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PayPal.Api;
using System;
using System.Configuration;

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
            var guid = Convert.ToString((new Random()).Next(100000));

            var paymentReg = _paymentService.RegisterPayment(guid);

            PageContext.HttpContext.Session.SetString(guid, paymentReg.Item1);

            return Redirect(paymentReg.Item2);
            

        }
    }
}
