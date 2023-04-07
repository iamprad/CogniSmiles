using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CogniSmiles.Models
{
    public class CoursePayment
    {
        public int Id { get; set; }
        [Display(Name = "Course Name")]
        public string? CourseName { get; set; }
        [Display(Name = "Course Fee")]
        public decimal? CourseFee { get; set; }
        [Display(Name = "Payer ID")]
        public string? PayerID { get; set; }
        [Display(Name = "Payment ID")]
        public string? PaymentID { get; set; }
        [Display(Name = "Payment Status")]
        public string? PaymentStatus { get; set; }
        [Display(Name = "Payer Name")]
        public string? PayerName { get; set;}
        [Display(Name = "Payer Email")]
        public string? PayerEmail { get; set;}
        [Display(Name = "Payment Error Message")]
        public string? PaymentError { get; set; }
        [Display(Name = "Payment Date")]
        public string? PaymentDate { get; set; }
    }
}