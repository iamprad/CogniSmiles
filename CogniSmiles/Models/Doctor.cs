using System.ComponentModel.DataAnnotations;

namespace CogniSmiles.Models
{
    public class Doctor
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Doctor()
        {
            BillingAddress = new PostalAddress();
            DeliveryAddress = new PostalAddress();

        }
        public int Id { get; set; }
        [Display(Name ="Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Practice Name")]
        public string PracticeName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Billing Address")]
        public PostalAddress BillingAddress { get; set; }
        [Display(Name = "Delivery Address")]
        public PostalAddress DeliveryAddress { get; set; }
        public DateTime? AddedDate { get; set;}
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "Update Comment")]
        public string? UpdateComment { get; set; }
    }

    public class PostalAddress
    {
        public int Id { get; set; }
        [Display(Name = "Address Line1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line2")]
        public string? AddressLine2 { get; set; }
        [Display(Name = "Address Line3")]
        public string? AddressLine3 { get; set; }
        public string City { get; set; }
        public string? Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
