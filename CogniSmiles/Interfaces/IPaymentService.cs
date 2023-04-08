using CogniSmiles.Models;
using CogniSmiles.Services;

namespace CogniSmiles.Interfaces
{
    public interface IPaymentService
    {
        Tuple<string, string> RegisterPayment(string? guid);
        CoursePayment? ExecutePayment(string? paymentId, string payerId);
    }
}

