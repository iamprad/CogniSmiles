using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CogniSmiles.Models
{
    public class AuthModel : PageModel
    {       
        private bool isAuthenticated;
        private int doctorId;
        public bool IsAuthenticated { 
            get {     
                
                if(!isAuthenticated)
                    isAuthenticated = Convert.ToBoolean(PageContext.HttpContext.Session.GetString("isAuthenticated"));
                return isAuthenticated; 
            } 
            set {
                isAuthenticated = value;
                PageContext.HttpContext.Session.SetString("isAuthenticated", value.ToString());
            } 
        }
        public int DoctorId
        {
            get {
                if (doctorId == 0)
                    doctorId = (int)PageContext.HttpContext.Session.GetInt32("doctorId");
                return doctorId;
            }
            set {
                doctorId = value;
                PageContext.HttpContext.Session.SetInt32("doctorId", value);
            }
        }
        public bool ClearSession()
        {
            IsAuthenticated = false;
            DoctorId = 0;
            PageContext.HttpContext.Session.Clear();
            return true;
        }
    }
}
