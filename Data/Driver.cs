using System.ComponentModel.DataAnnotations;

namespace jwt_test.Models{
    public class Driver{
        [Key]
        public int driverId {get;set;}
        public string driverName {get;set;}
        public string registrationId {get;set;}
        
    }//end of driver class
}//end of namespace