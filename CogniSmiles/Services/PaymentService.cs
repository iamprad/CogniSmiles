using CogniSmiles.Interfaces;
using CogniSmiles.Data;
using CogniSmiles.Models;
using PayPal.Api;

namespace CogniSmiles.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly CogniSmilesContext _context;
        private readonly IConfiguration _config;
        private APIContext _apiContext;
        public PaymentService(CogniSmilesContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            var pconfig = _config.GetSection("PayPalConfig");
            // Get a reference to the config
            var paypalconfig = new Dictionary<string, string>()
            {
                {"mode", pconfig.GetValue<string>("Mode") },
                {"clientId",pconfig.GetValue<string>("ClientID") },
                {"clientSecret",pconfig.GetValue<string>("ClientSecret") }
            };

            // Use OAuthTokenCredential to request an access token from PayPal
            var accessToken = new OAuthTokenCredential(paypalconfig).GetAccessToken();

            _apiContext = new APIContext(accessToken)
            {
                Config = paypalconfig
            };

        }

        public Tuple<string,string> RegisterPayment(string? guid)
        {
            var coursePayment = new CoursePayment()
            {
                CourseName = "Payment for Sinus Lift Course at London on 19th & 20th May 2023",
                CourseFee = (decimal?)1650.00,
            };
            var payer = new Payer() { payment_method = "paypal" };

            var redirectUrl = "https://" + _config.GetValue<string>("DomainName") + "/Courses/PaymentStatus" + "?guid=" + guid;
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
                    amount = amount,
                    payment_options = new PaymentOptions()
                    {
                        allowed_payment_method="INSTANT_FUNDING_SOURCE"
                    },
                    item_list = new ItemList()
                    {
                        items = new List<Item>(){
                            new Item()
                            {
                                name = coursePayment.CourseName,
                                description = coursePayment.CourseName,
                                quantity = "1",
                                price = coursePayment.CourseFee.ToString(),
                                currency = "GBP",
                            }
                        } 
                    }
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
            return Tuple.Create(coursePayment.PaymentID, approveUrl);
        }
        public CoursePayment? ExecutePayment(string? paymentId,string payerId)
        {
            var payment = new Payment() { id = paymentId };

            var coursePayment = _context.CoursePayment.FirstOrDefault(cp => cp.PaymentID == paymentId);
            try
            {
                var paymentExecution = new PaymentExecution() { payer_id = payerId };

                // Execute the order payment.
                var executedPayment = payment.Execute(_apiContext, paymentExecution);

                // Get the information about the executed order from the returned payment object.
                var order = executedPayment.transactions[0].related_resources[0].order;
                var amount = executedPayment.transactions[0].amount;

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
                order.Authorize(_apiContext);
                if (coursePayment != null)
                    coursePayment.PaymentStatus = executedPayment.state;

                var capture = new Capture
                {
                    amount = amount,
                    is_final_capture = true
                };
                order.Capture(_apiContext, capture);

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
            return coursePayment;
        }
    }
}
