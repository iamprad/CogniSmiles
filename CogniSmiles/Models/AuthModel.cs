
namespace CogniSmiles.Models
{
    public class AuthModel
    {
        private readonly ISession _currentContext;
        public AuthModel(ISession currentContext)
        {
            _currentContext = currentContext;
        }
        private bool isAuthenticated;

        private int doctorId;
        public bool IsAuthenticated { 
            get {     
                if(!isAuthenticated)
                    isAuthenticated = Convert.ToBoolean(_currentContext.GetString("isAuthenticated"));
                return isAuthenticated; 
            } 
            set {
                isAuthenticated = value;
                _currentContext.SetString("isAuthenticated", value.ToString());
            } 
        }
        public int DoctorId
        {
            get {
                if (doctorId == 0)
                    doctorId = (int)_currentContext.GetInt32("doctorId");
                return doctorId;
            }
            set {
                doctorId = value;
                _currentContext.SetInt32("doctorId", value);
            }
        }
    }
}
