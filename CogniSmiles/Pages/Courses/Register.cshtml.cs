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
        private Order order;
        private Amount amount;
        private APIContext _apiContext;
        public CoursePayment coursePayment { get; set; }
        private readonly CogniSmilesContext _context;
        public RegisterModel(CogniSmilesContext context)
        {
            _context = context;
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
            coursePayment = new CoursePayment()
            {
                CourseName = "Payment for Sinus Lift Course at London on 19th & 20th May 2023",
                CourseFee = (decimal?)1650.00,
            };
        }

        public void OnGet()
        {           

        }
        public IActionResult OnPost() 
        {
            var payer = new Payer() { payment_method = "paypal" };

            var guid = Convert.ToString((new Random()).Next(100000));
            var redirectUrl ="https://"+Request.Host.Value + "/Courses/PaymentStatus" + "?guid=" + guid;
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&cancel=true",
                return_url = redirectUrl
            };

            // ###Amount
            // Lets you specify a payment amount.
            var amount = new Amount()
            {
                currency = "GBP",
                total = coursePayment.CourseFee.ToString()
            };

            // ###Transaction
            // A transaction defines the contract of a
            // payment - what is the payment for and who
            // is fulfilling it. 
            var transactionList = new List<Transaction>
            {
                // The Payment creation API requires a list of
                // Transaction; add the created `Transaction`
                // to a List
                new Transaction()
                {
                    description = coursePayment.CourseName,
                    amount = amount
                }
            };

            // ###Payment
            // Create a payment with the intent set to 'order'
            var payment = new Payment()
            {
                intent = "order",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            var createdPayment = payment.Create(_apiContext);

            coursePayment.PaymentID = createdPayment.id;
            coursePayment.PaymentStatus = createdPayment.state;
            coursePayment.PaymentDate = createdPayment.create_time;
            PageContext.HttpContext.Session.SetString(guid, createdPayment.id);

            var links = createdPayment.links.GetEnumerator();
            string approveUrl = string.Empty;
            while (links.MoveNext())
            {
                var link = links.Current;
                if (link.rel.ToLower().Trim().Equals("approval_url"))
                {
                    approveUrl = link.href;
                    break;
                }
            }
            _context.CoursePayment.Add(coursePayment);
            _context.SaveChanges();

            return Redirect(approveUrl);
            

        }
    }
}
