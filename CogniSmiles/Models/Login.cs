using System.ComponentModel.DataAnnotations;

namespace CogniSmiles.Models
{
    public class Login
    {
        public Login()
        {
            AuthType = AuthType.Doctor;
            CreatedOn = DateTime.Now;
            IsActive = false;
            IsLocked = false;
            UserId = Guid.NewGuid();
        }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public AuthType AuthType { get; set; }
        public int DoctorId { get; set; }                
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? ActivatedOn { get; set; }
    }

    public enum AuthType
    {
        Admin = 0,
        Doctor = 1
    }
}
