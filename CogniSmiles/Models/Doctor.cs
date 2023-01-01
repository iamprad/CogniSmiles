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
        public string FullName { get; set; }
        public string PracticeName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public PostalAddress BillingAddress { get; set; }
        public PostalAddress DeliveryAddress { get; set; }
        public DateTime? AddedDate { get; set;}
        public DateTime? UpdatedDate { get; set; }
        public string UpdateComment { get; set; }
    }

    public class PostalAddress
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string City { get; set; }
        public string? Region { get; set; }
        public string PostalCode { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
